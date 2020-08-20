using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.Shared.Static;
using AAYHS.Repository.IRepository;
using AAYHS.Service.IService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AAYHS.Service.Service
{
   public class AdvertisementService: IAdvertisementService
    {

        #region readonly
       
       
        private IAdvertisementRepository _advertisementRepository;
        private IMapper _mapper;
        #endregion

        #region private
        private MainResponse _mainResponse;
        #endregion

        public AdvertisementService(IAdvertisementRepository advertisementRepository  , IMapper Mapper)
        {
            _advertisementRepository = advertisementRepository;
             _mapper = Mapper;
            _mainResponse = new MainResponse();
        }

        public MainResponse GetAllAdvertisements(BaseRecordFilterRequest request)
        {
            _mainResponse = _advertisementRepository.GetAllAdvertisements(request);
            if (_mainResponse.AdvertisementListResponse.advertisementResponses != null && _mainResponse.AdvertisementListResponse.advertisementResponses.Count() > 0)
            {
                _mainResponse.Message = Constants.RECORD_FOUND;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

    }
}
