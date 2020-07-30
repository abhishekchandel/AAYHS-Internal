using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.Shared.Static;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AAYHS.Service.IService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AAYHS.Service.Service
{
    public class SponsorExhibitorService : ISponsorExhibitorService
    {
        #region readonly
        private readonly IMapper _Mapper;
        #endregion

        #region private
        private MainResponse _MainResponse;
        private ISponsorExhibitorRepository _SponsorExhibitorRepository;
        #endregion

        public SponsorExhibitorService(ISponsorExhibitorRepository SponsorExhibitorRepository, IMapper Mapper)
        {
            _SponsorExhibitorRepository = SponsorExhibitorRepository;
            _Mapper = Mapper;
            _MainResponse = new MainResponse();
        }
        public MainResponse AddSponsorExhibitor(SponsorExhibitorRequest request)
        {
            var SponsorExhibitor = _Mapper.Map<SponsorExhibitor>(request);
            _SponsorExhibitorRepository.Add(SponsorExhibitor);
            _MainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
            _MainResponse.Success = true;
            return _MainResponse;
        }

        public MainResponse DeleteSponsorExhibitor(DeleteSponsorExhibitorRequest request)
        {
            var SponsorExhibitor = _SponsorExhibitorRepository.GetSingle(x => x.SponsorExhibitorId == request.SponsorExhibitorId);
            if (SponsorExhibitor != null)
            {
                SponsorExhibitor.IsDeleted = true;
                SponsorExhibitor.IsActive = false;
                SponsorExhibitor.DeletedDate = DateTime.Now;
                _SponsorExhibitorRepository.Update(SponsorExhibitor);
                _MainResponse.Message = Constants.RECORD_DELETE_SUCCESS;
                _MainResponse.Success = true;
            }
            else
            {
                _MainResponse.Message = Constants.NO_RECORD_Exist_WITH_ID;
                _MainResponse.Success = false;
            }
            return _MainResponse;
        }

        public MainResponse GetSponsorExhibitorBySponsorId(GetSponsorExhibitorRequest request)
        {
            var data = _SponsorExhibitorRepository.GetSponsorExhibitorBySponsorId(request);
            if (data != null && data.Count>0)
            {
                _MainResponse.Data.SponsorExhibitorListResponses = data;
                _MainResponse.Message = Constants.RECORD_FOUND;
                _MainResponse.Success = true;
            }
            else
            {
                _MainResponse.Message = Constants.NO_RECORD_FOUND;
                _MainResponse.Success = false;
            }
            
            return _MainResponse;
        }

        public MainResponse UpdateSponsorExhibitor(SponsorExhibitorRequest request)
        {
            var SponsorExhibitor = _SponsorExhibitorRepository.GetSingle(x=>x.SponsorExhibitorId== request.SponsorExhibitorId);
            if(SponsorExhibitor!=null)
            {
                SponsorExhibitor.SponsorId = request.SponsorId;
                SponsorExhibitor.ExhibitorId = request.ExhibitorId;
                SponsorExhibitor.SponsorTypeId = request.SponsorTypeId;
                SponsorExhibitor.TypeId = request.TypeId;
                _SponsorExhibitorRepository.Update(SponsorExhibitor);
                _MainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;
                _MainResponse.Success = true;
            }
            else
            {
                _MainResponse.Message = Constants.RECORD_UPDATE_FAILED;
                _MainResponse.Success = false;
            }
           
            return _MainResponse;
        }
    }
}
