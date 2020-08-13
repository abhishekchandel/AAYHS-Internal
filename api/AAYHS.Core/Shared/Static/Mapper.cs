using AAYHS.Core.DTOs.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AAYHS.Core.Shared.Static
{
    public static class Mapper
    {
        public static string Convert<T>(MainResponse mainReponse)
        {

            Response<T> response = new Response<T>();
            response.Message = mainReponse.Message;
            response.Success = mainReponse.Success;
            response.NewId = mainReponse.NewId;

            if (mainReponse.Success == true)
            {
                string genericClassName = typeof(T).Name;
                PropertyInfo propertyData = mainReponse.GetType().GetProperty(genericClassName);
                T data = (T)(propertyData.GetValue(mainReponse, null));
                response.Data = data;
            }

            return JsonConvert.SerializeObject(response);
        }
    }
}
