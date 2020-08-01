﻿using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.Shared.Static;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AAYHS.Service.IService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private MainResponse _mainResponse;
        private ISponsorExhibitorRepository _SponsorExhibitorRepository;
        #endregion

        public SponsorExhibitorService(ISponsorExhibitorRepository SponsorExhibitorRepository, IMapper Mapper)
        {
            _SponsorExhibitorRepository = SponsorExhibitorRepository;
            _Mapper = Mapper;
            _mainResponse = new MainResponse();
        }
        public MainResponse AddUpdateSponsorExhibitor(SponsorExhibitorRequest request)
        {
            if (request.SponsorExhibitorId <= 0)
            {
                var checkexist = _SponsorExhibitorRepository.GetSingle(x => x.SponsorId == request.SponsorId && x.ExhibitorId == request.ExhibitorId);
                if (checkexist != null && checkexist.SponsorExhibitorId > 0)
                {
                    _mainResponse.Message = Constants.RECORD_AlREADY_EXIST;
                    _mainResponse.Success = false;
                    return _mainResponse;
                }
                var SponsorExhibitor = _Mapper.Map<SponsorExhibitor>(request);
                _SponsorExhibitorRepository.Add(SponsorExhibitor);
                _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
                _mainResponse.Success = true;
            }
            else
            {
                var SponsorExhibitor = _SponsorExhibitorRepository.GetSingle(x => x.SponsorExhibitorId == request.SponsorExhibitorId);
                if (SponsorExhibitor != null && SponsorExhibitor.SponsorExhibitorId>0)
                {
                    SponsorExhibitor.SponsorId = request.SponsorId;
                    SponsorExhibitor.ExhibitorId = request.ExhibitorId;
                    SponsorExhibitor.SponsorTypeId = request.SponsorTypeId;
                    SponsorExhibitor.TypeId = request.TypeId;
                    _SponsorExhibitorRepository.Update(SponsorExhibitor);
                    _mainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;
                    _mainResponse.Success = true;
                }
                else
                {
                    _mainResponse.Message = Constants.NO_RECORD_EXIST_WITH_ID;
                    _mainResponse.Success = false;
                }
            }
            return _mainResponse;
        }

        public MainResponse DeleteSponsorExhibitor(DeleteSponsorExhibitorRequest request)
        {
            var SponsorExhibitor = _SponsorExhibitorRepository.GetSingle(x => x.SponsorExhibitorId == request.SponsorExhibitorId);
            if (SponsorExhibitor != null && SponsorExhibitor.SponsorExhibitorId>0)
            {
                SponsorExhibitor.IsDeleted = true;
                SponsorExhibitor.IsActive = false;
                SponsorExhibitor.DeletedDate = DateTime.Now;
                _SponsorExhibitorRepository.Update(SponsorExhibitor);
                _mainResponse.Message = Constants.RECORD_DELETE_SUCCESS;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_EXIST_WITH_ID;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse GetSponsorExhibitorBySponsorId(GetSponsorExhibitorRequest request)
        {
            _mainResponse = _SponsorExhibitorRepository.GetSponsorExhibitorBySponsorId(request);
            if (_mainResponse.SponsorExhibitorListResponse.SponsorExhibitorResponses != null && _mainResponse.SponsorExhibitorListResponse.SponsorExhibitorResponses.Count>0)
            {
                _mainResponse.SponsorExhibitorListResponse.TotalRecords = _mainResponse.SponsorExhibitorListResponse.SponsorExhibitorResponses.Count();
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
