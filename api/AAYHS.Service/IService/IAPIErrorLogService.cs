using AAYHS.Core.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AAYHS.Service.IService
{
    public interface IAPIErrorLogService
    {
        Task<int> InserAPILogToDB(APILogRequest apiLogRequest);
        Task UpdateAPILogToDB(UpdateAPILogRequest updateAPILogRequest);
    }
}
