using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AAYHS.Service.IService;
using AutoMapper;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AAYHS.Service.Service
{
    public class APIErrorLogService : IAPIErrorLogService
    {
        #region readonly
        private readonly IMapper _Mapper;
        #endregion

        #region private
        private MainResponse _MainResponse;
        private IAPIErrorLogRepository _APIErrorLogRepository;
        #endregion

        public APIErrorLogService(IAPIErrorLogRepository APIErrorLogRepository, IMapper Mapper)
        {
            _APIErrorLogRepository = APIErrorLogRepository;
            _Mapper = Mapper;
            _MainResponse = new MainResponse();
        }

        public async Task<int> InserAPILogToDB(APILogRequest apiLogRequest)
        {
            var apilogs = _Mapper.Map<Apilogs>(apiLogRequest);
            apilogs.StartDateTime = DateTime.UtcNow;
            await _APIErrorLogRepository.AddAsync(apilogs);
            return apilogs.ApilogId;
        }

        public async Task UpdateAPILogToDB(UpdateAPILogRequest updateAPILogRequest)
        {
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@apilogid", updateAPILogRequest.APILogId);
            dynamicParameters.Add("@success", updateAPILogRequest.Success);
            dynamicParameters.Add("@exceptionmsg", updateAPILogRequest.ExceptionMsg);
            dynamicParameters.Add("@exceptiontype", updateAPILogRequest.ExceptionType);
            dynamicParameters.Add("@exceptionsource", updateAPILogRequest.ExceptionSource);
            await _APIErrorLogRepository.SPExecuteNonQueryAsync(dynamicParameters, "sp_UpdateAPILog");
        }
    }
}
