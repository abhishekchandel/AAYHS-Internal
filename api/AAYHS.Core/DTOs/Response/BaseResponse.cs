using AAYHS.Core.DTOs.Response.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response
{
    /// <summary>
    /// This class will contain common property that is used by all API response in the application.
    /// </summary>
    public class BaseResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; } = true;
    }

    public class MainResponse : BaseResponse
    {
        public BaseResponse BaseResponse { get; set; }
        public ClassResponse ClassResponse { get; set; }
        public GetAllClasses GetAllClasses { get; set; }
    }
    public class Response<T> : BaseResponse
    {
        public T Data { get; set; }

    }
}
