using AAYHS.Core.DTOs.Request;
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
    public class SponsorDistributionService : ISponsorDistributionService
    {
        #region readonly
        private readonly IMapper _Mapper;
        #endregion

        #region private
        private MainResponse _mainResponse;
        private ISponsorExhibitorRepository _SponsorExhibitorRepository;
        private ISponsorDistributionRepository _SponsorDistributionRepository;
        private ISponsorRepository _SponsorRepository;
        private IGlobalCodeRepository _GlobalCodeRepository;
        #endregion

        public SponsorDistributionService(ISponsorExhibitorRepository SponsorExhibitorRepository, ISponsorDistributionRepository 
                                         SponsorDistributionRepository, ISponsorRepository SponsorRepository,
                                         IGlobalCodeRepository GlobalCodeRepository,IMapper Mapper)
        {
            _SponsorDistributionRepository = SponsorDistributionRepository;
            _SponsorExhibitorRepository = SponsorExhibitorRepository;
            _SponsorRepository = SponsorRepository;
            _GlobalCodeRepository = GlobalCodeRepository;
            _Mapper = Mapper;
            _mainResponse = new MainResponse();
        }
        public MainResponse AddUpdateSponsorDistribution(SponsorDistributionRequest request)
        {
            if (request.SponsorDistributionId <= 0)
            {
                decimal sponsorAmount = Convert.ToDecimal(_SponsorRepository.GetSingle(x => x.SponsorId == request.SponsorId).AmountReceived);

                decimal sponsorexhibitorPaid = _SponsorExhibitorRepository.GetAll(x => x.SponsorId == request.SponsorId && x.IsActive == true).Select(x => x.SponsorAmount).Sum();
                decimal sponsordistributionPaid = _SponsorDistributionRepository.GetAll(x => x.SponsorId == request.SponsorId && x.IsActive == true).Select(x => x.TotalDistribute).Sum();

                decimal checkSponsorAmount = sponsorAmount - (sponsorexhibitorPaid + sponsordistributionPaid);
                if (checkSponsorAmount <= 0)
                {
                    _mainResponse.Message = Constants.SPONSOR_NO_FUND;
                    _mainResponse.Success = false;
                    return _mainResponse;
                }
                decimal checkAddedAmount = checkSponsorAmount - request.TotalDistribute;
                if (checkAddedAmount < 0)
                {
                    _mainResponse.Message = Constants.SPONSOR_NO_FUND;
                    _mainResponse.Success = false;
                    return _mainResponse;
                }
                var sponsorType = _GlobalCodeRepository.GetSingle(x => x.GlobalCodeId == request.SponsorTypeId);

                if (sponsorType.CodeName == "Ad")
                {
                    var sponsorAdExist = _SponsorDistributionRepository.GetSingle(x => x.TypeId == request.TypeId &&
                     x.IsActive == true && x.IsDeleted == false);

                    if (sponsorAdExist != null)
                    {
                        _mainResponse.Message = Constants.AD_NUMBER_EXIST;
                        _mainResponse.Success = false;
                        return _mainResponse;
                    }
                }

                var sponsorDistribution = _Mapper.Map<SponsorDistributions>(request);
                sponsorDistribution.IsActive = true;
                sponsorDistribution.IsDeleted = false;
                sponsorDistribution.CreatedDate = DateTime.Now;
                _SponsorDistributionRepository.Add(sponsorDistribution);
                _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
                _mainResponse.Success = true;
            }
            else
            {
                var sponsorDistribution = _SponsorDistributionRepository.GetSingle(x => x.SponsorDistributionId == request.SponsorDistributionId);
                if (sponsorDistribution != null && sponsorDistribution.SponsorDistributionId > 0)
                {
                    sponsorDistribution.SponsorId = request.SponsorId;
                    sponsorDistribution.SponsorTypeId = request.SponsorTypeId;
                    sponsorDistribution.TotalDistribute = request.TotalDistribute;
                    sponsorDistribution.AdTypeId = Convert.ToInt32(request.AdTypeId);
                    sponsorDistribution.TypeId = request.TypeId;
                    sponsorDistribution.ModifiedDate = DateTime.Now;

                    _SponsorDistributionRepository.Update(sponsorDistribution);
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

        public MainResponse DeleteSponsorDistribution(int SponsorDistributionId)
        {
            var sponsorDistribution = _SponsorDistributionRepository.GetSingle(x => x.SponsorDistributionId == SponsorDistributionId);
            if (sponsorDistribution != null && sponsorDistribution.SponsorDistributionId > 0)
            {
                sponsorDistribution.IsDeleted = true;
                sponsorDistribution.IsActive = false;
                sponsorDistribution.DeletedDate = DateTime.Now;
                _SponsorDistributionRepository.Update(sponsorDistribution);
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

        public MainResponse GetSponsorDistributionBySponsorId(int SponsorId)
        {
            _mainResponse = _SponsorDistributionRepository.GetSponsorDistributionBySponsorId(SponsorId);
            if (_mainResponse.SponsorDistributionListResponse.SponsorDistributionResponses != null)
            {
                _mainResponse.SponsorDistributionListResponse.TotalRecords = _mainResponse.SponsorDistributionListResponse.SponsorDistributionResponses.Count();
                if (_mainResponse.SponsorDistributionListResponse.TotalRecords > 0)
                {
                    _mainResponse.Message = Constants.RECORD_FOUND;
                    _mainResponse.Success = true;
                }
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
