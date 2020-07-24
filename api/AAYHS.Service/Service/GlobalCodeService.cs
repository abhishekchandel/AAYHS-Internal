using AAYHS.Core.DTOs.Response;
using AAYHS.Core.Shared.Static;
using AAYHS.Repository.IRepository;
using AAYHS.Service.IService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AAYHS.Service.Service
{
    public class GlobalCodeService: IGlobalCodeService
    {
        #region readonly
        private readonly IGlobalCodeRepository _globalCodeRepository;      
        private readonly IMapper _mapper;
        #endregion

        #region Private
        private MainResponse _mainResponse;
        #endregion

        public GlobalCodeService(IGlobalCodeRepository GlobalCodeRepository, IMapper Mapper)
        {
            _globalCodeRepository = GlobalCodeRepository;
            _mapper = Mapper;
            _mainResponse = new MainResponse();
        }

        public async Task<MainResponse> GetHorseType()
        {
            var globalCodeResponse = await _globalCodeRepository.GetCodes("HorseType");
            if (globalCodeResponse.totalRecords != 0)
            {

                _mainResponse.Success = true;
                _mainResponse.GlobalCodeMainResponse = globalCodeResponse;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
            }
            return _mainResponse;
        }
        public async Task<MainResponse> GetSponsorType()
        {
            var globalCodeResponse = await _globalCodeRepository.GetCodes("SponsorType");
            if (globalCodeResponse.totalRecords != 0)
            {

                _mainResponse.Success = true;
                _mainResponse.GlobalCodeMainResponse = globalCodeResponse;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
            }
            return _mainResponse;
        }
    }
}
