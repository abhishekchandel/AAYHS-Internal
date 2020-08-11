using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.Shared.Static;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AAYHS.Repository.Repository;
using AAYHS.Service.IService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AAYHS.Service.Service
{
   public class SponsorService:ISponsorService
    {
        #region readonly
        private readonly IMapper _Mapper;
        #endregion

        #region private
        private MainResponse _mainResponse;
        private ISponsorRepository _SponsorRepository;
        private IAddressRepository _AddressRepository;
        #endregion

        public SponsorService(ISponsorRepository SponsorRepository, IAddressRepository AddressRepository, IMapper Mapper)
        {
            _SponsorRepository = SponsorRepository;
            _AddressRepository = AddressRepository;
            _Mapper = Mapper;
            _mainResponse = new MainResponse();
        }

        public MainResponse AddUpdateSponsor(SponsorRequest request)
        {
            if (request.SponsorId==null || request.SponsorId <= 0)
            {
                var checkexist = _SponsorRepository.GetSingle(x => x.SponsorName == request.SponsorName
                && x.IsActive==true && x.IsDeleted==false);
                if (checkexist != null && checkexist.SponsorId > 0)
                {
                    _mainResponse.Message = Constants.NAME_ALREADY_EXIST;
                    _mainResponse.Success = false;
                    return _mainResponse;
                }

                var addressEntity = new Addresses
                {
                    Address = request.Address,
                    CityId = request.CityId,
                    ZipCode = request.ZipCode,
                    CreatedDate = DateTime.Now
                };
                var address = _AddressRepository.Add(addressEntity);
                var sponsor = new Sponsors
                {
                    SponsorName = request.SponsorName,
                    ContactName = request.ContactName,
                    Phone = request.Phone,
                    Email = request.Email,
                    AmountReceived = request.AmountReceived,
                    AddressId = address!=null? address.AddressId:0,
                    CreatedDate = DateTime.Now
                };
                _SponsorRepository.Add(sponsor);
                _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
                _mainResponse.Success = true;
            }
            else
            {
                var sponsor = _SponsorRepository.GetSingle(x => x.SponsorId == request.SponsorId);
                if (sponsor != null && sponsor.SponsorId>0)
                {
                    sponsor.SponsorName = request.SponsorName;
                    sponsor.ContactName = request.ContactName;
                    sponsor.Phone = request.Phone;
                    sponsor.Email = request.Email;
                    sponsor.AmountReceived = request.AmountReceived;
                    sponsor.ModifiedDate = DateTime.Now;
                    _SponsorRepository.Update(sponsor);

                    var address = _AddressRepository.GetSingle(x => x.AddressId == sponsor.AddressId);
                    if (address != null && address.AddressId > 0)
                    {
                        address.Address = request.Address;
                        address.CityId = request.CityId;
                        address.ZipCode = request.ZipCode;
                        address.ModifiedDate = DateTime.Now;
                        _AddressRepository.Update(address);
                    }
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

        public MainResponse GetAllSponsorsWithFilter(BaseRecordFilterRequest request)
        {
            _mainResponse = _SponsorRepository.GetAllSponsorsWithFilter(request);
            if (_mainResponse.SponsorListResponse.sponsorResponses != null && _mainResponse.SponsorListResponse.sponsorResponses.Count() > 0)
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

        public MainResponse GetAllSponsors()
        {
            _mainResponse = _SponsorRepository.GetAllSponsor();
            if (_mainResponse.SponsorListResponse.sponsorResponses != null && _mainResponse.SponsorListResponse.sponsorResponses.Count() > 0)
            {
                _mainResponse.Message = Constants.RECORD_FOUND;
                _mainResponse.Success = true;
                _mainResponse.SponsorListResponse.TotalRecords = _mainResponse.SponsorListResponse.sponsorResponses.Count();
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse GetSponsorById(int SponsorId)
        {
            _mainResponse = _SponsorRepository.GetSponsorById(SponsorId);
            if (_mainResponse.SponsorResponse != null&& _mainResponse.SponsorResponse.SponsorId>0)
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

      
        public MainResponse DeleteSponsor(int SponsorId)
        {
            var sponsor = _SponsorRepository.GetSingle(x => x.SponsorId == SponsorId);
            if (sponsor != null&& sponsor.SponsorId>0)
            {
                sponsor.IsDeleted = true;
                sponsor.IsActive = false;
                sponsor.DeletedDate = DateTime.Now;
                _SponsorRepository.Update(sponsor);
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

        public MainResponse SearchSponsor(SearchRequest searchRequest)
        {
            var search = _SponsorRepository.SearchSponsor(searchRequest);
            if (search!=null && search.TotalRecords!=0)
            {
                _mainResponse.SponsorListResponse = search;
                _mainResponse.Success = true;
                _mainResponse.SponsorListResponse.TotalRecords = search.TotalRecords;
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
