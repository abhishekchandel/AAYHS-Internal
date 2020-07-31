using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Core.Shared.Static;
using AAYHS.Service.IService;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AAYHS.API.Logging
{
    public class ApplicationMiddleware
    {
        private readonly RequestDelegate _next;
        private int APILoggedId = -1;
        public ApplicationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IMapper _mapper, IAPIErrorLogService _APIErrorLogService)
        {
            var apiLogRequest = new APILogRequest();

            //Copy a pointer to the original response body stream
            var originalBodyStream = httpContext.Response.Body;

            try
            {
                using (var responseBodyStream = new MemoryStream())
                {
                    if (AppSettingConfigurations.AppSettings.EnableAPILog)
                    {
                        httpContext.Request.EnableBuffering();
                        //First, get the incoming request
                        apiLogRequest.APIParams = await FormatRequest.FormatRequestBody(httpContext.Request);
                        httpContext.Request.Body.Seek(0, SeekOrigin.Begin);

                        //Read API URL
                        apiLogRequest.APIUrl = httpContext.Request.Scheme + "://" + httpContext.Request.Host + httpContext.Request.Path;

                        //Read Authorization Header
                        apiLogRequest.Headers = httpContext.Request.Headers["Authorization"];

                        // Read Method
                        apiLogRequest.Method = httpContext.Request.Method;
                        APILoggedId = await _APIErrorLogService.InserAPILogToDB(apiLogRequest);
                    }

                    httpContext.Response.Body = responseBodyStream;
                    long length = 0;
                    httpContext.Response.OnStarting(() =>
                    {
                        httpContext.Response.Headers.ContentLength = length;
                        return Task.CompletedTask;
                    });
                    await _next(httpContext);
                    length = httpContext.Response.Body.Length;
                    //Format the response from the server
                    var response = FormatResponse(httpContext.Response, httpContext);

                    //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                    await responseBodyStream.CopyToAsync(originalBodyStream);

                    await _APIErrorLogService.UpdateAPILogToDB(new UpdateAPILogRequest() { APILogId = APILoggedId, Success = true });
                }
            }
            catch (Exception ex)
            {
                httpContext.Response.Body = originalBodyStream;
                await HandleExceptionAsync(httpContext, ex, apiLogRequest, _APIErrorLogService, _mapper);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, APILogRequest apiLogRequest, IAPIErrorLogService _APIErrorLogService, IMapper _mapper)
        {
            HttpStatusCode statusCode = (exception as WebException != null &&
                        ((HttpWebResponse)(exception as WebException).Response) != null) ?
                         ((HttpWebResponse)(exception as WebException).Response).StatusCode
                         : GetErrorCodes.GetErrorCode(exception.GetType());
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            switch (AppSettingConfigurations.AppSettings.ErrorLoggingType.ToLower())
            {
                case "both":
                    {
                        LogExceptionInDB(exception, apiLogRequest, _APIErrorLogService, _mapper);
                        break;
                    }
                case "db":
                    {
                        LogExceptionInDB(exception, apiLogRequest, _APIErrorLogService, _mapper);
                        break;
                    }

                default:
                    break;
            }

            return context.Response.WriteAsync(new ErrorDetailResponse()
            {
                StatusCode = context.Response.StatusCode,
                Message = Constants.INTERNAL_SERVER_ERROR,
                Success = false
            }.ToString());
        }

        private void LogExceptionInDB(Exception exception, APILogRequest APILogRequest, IAPIErrorLogService _APIErrorLogService, IMapper _mapper)
        {
            var updateAPILogRequest = new UpdateAPILogRequest();
            updateAPILogRequest.ExceptionMsg = exception.Message;
            updateAPILogRequest.ExceptionType = exception.GetType().Name;
            updateAPILogRequest.ExceptionSource = GenerateExceptionMessage(exception, APILogRequest);
            updateAPILogRequest.APILogId = APILoggedId;
            _APIErrorLogService.UpdateAPILogToDB(updateAPILogRequest);
        }

        public string GenerateExceptionMessage(Exception exception, APILogRequest APILogRequest)
        {
            return "APIURL : " + APILogRequest.APIUrl + System.Environment.NewLine + "APIParams : " + APILogRequest.APIParams + System.Environment.NewLine + "Method : " + APILogRequest.Method + System.Environment.NewLine +
           "Error Message : " + exception.Message + System.Environment.NewLine + "Inner Exception : " + exception.InnerException + System.Environment.NewLine +
           "Source : " + exception.Source + System.Environment.NewLine + "StackTrace : " + exception.StackTrace;
        }
        private async Task<string> FormatResponse(HttpResponse response, HttpContext httpContext)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            BaseResponse baseResponse = JsonConvert.DeserializeObject<BaseResponse>(text);
            if (baseResponse != null && baseResponse.Success != true)
            {
                response.StatusCode = 500;
            }

            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return $"{response.StatusCode}: {text}";
        }
    }
}
