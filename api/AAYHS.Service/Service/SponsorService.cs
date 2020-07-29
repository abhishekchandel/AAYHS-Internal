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
        private MainResponse _MainResponse;
        private ISponsorRepository _SponsorRepository;
        private IAddressRepository _AddressRepository;
        #endregion

        public SponsorService(ISponsorRepository SponsorRepository, IAddressRepository AddressRepository, IMapper Mapper)
        {
            _SponsorRepository = SponsorRepository;
            _AddressRepository = AddressRepository;
            _Mapper = Mapper;
            _MainResponse = new MainResponse();
        }

        public MainResponse AddSponsor(SponsorRequest request)
        {
            var addressEntity = new Addresses
            {
                Address = request.Address,
                CityId = request.CityId,
                ZipCode = request.ZipCode,
                CreatedDate=DateTime.Now
            };
            var address = _AddressRepository.Add(addressEntity);
            var sponsor = new Sponsors {
                SponsorName = request.SponsorName,
                ContactName = request.ContactName,
                Phone = request.Phone,
                Email =request.Email,
                AmountReceived =request.AmountReceived,
                AddressId = address.AddressId,
                CreatedDate = DateTime.Now
            };
                 _SponsorRepository.Add(sponsor);
                 _MainResponse.Data = null;
                _MainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
                _MainResponse.Success = true;
                return _MainResponse;
        }

        public MainResponse GetAllSponsorsWithFilter(BaseRecordFilterRequest request)
        {
                var data = _SponsorRepository.GetAllSponsorsWithFilter(request);
                _MainResponse.TotalRecords = data.Count();
            if (_MainResponse.TotalRecords != 0)
            {
                _MainResponse.Data.SponsorListResponse = data;
                _MainResponse.Message = Constants.RECORD_FOUND;
                _MainResponse.Success = true;
                _MainResponse.TotalRecords = data.Count();
            }
            else
            {
                _MainResponse.Message = Constants.NO_RECORD_FOUND;
                _MainResponse.Success = false;
            }
              
           
            return _MainResponse;
        }

        public MainResponse GetAllSponsors()
        {
            var data = _SponsorRepository.GetAllSponsor();
            _MainResponse.TotalRecords = data.Count();
            if (_MainResponse.TotalRecords != 0)
            {
                _MainResponse.Data.SponsorListResponse = data; 
                _MainResponse.Message = Constants.RECORD_FOUND;
                _MainResponse.Success = true;
                _MainResponse.TotalRecords = data.Count();
            }
            else
            {
                _MainResponse.Message = Constants.NO_RECORD_FOUND;
                _MainResponse.Success = false;
            }
            return _MainResponse;
        }

        public MainResponse GetSponsorById(int sponsorId)
        {
            var data = _SponsorRepository.GetSponsorById(sponsorId);
            if (data != null)
            {
                _MainResponse.Data.SponsorResponse = data;
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

        public MainResponse UpdateSponsor(SponsorRequest request)
        {
            var sponsor = _SponsorRepository.GetSingle(x => x.SponsorId == request.SponsorId);
                sponsor.SponsorName = request.SponsorName;
                sponsor.ContactName = request.ContactName;
                sponsor.Phone = request.Phone;
                sponsor.Email = request.Email;
                sponsor.AmountReceived = request.AmountReceived;
                sponsor.ModifiedDate = DateTime.Now;
                _SponsorRepository.Update(sponsor);

            var address = _AddressRepository.GetSingle(x=>x.AddressId== sponsor.AddressId);
            address.Address = request.Address;
            address.CityId = request.CityId;
            address.ZipCode = request.ZipCode;
            address.ModifiedDate = DateTime.Now;
            _AddressRepository.Update(address);

            _MainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;
            _MainResponse.Data = null;
                _MainResponse.Success = true;
            return _MainResponse;
        }

        public MainResponse DeleteSponsor(int sponsorId)
        {
            var sponsor = _SponsorRepository.GetSingle(x => x.SponsorId == sponsorId);
            if (sponsor != null)
            {
                sponsor.IsDeleted = true;
                sponsor.IsActive = false;
                sponsor.DeletedDate = DateTime.Now;
                _SponsorRepository.Update(sponsor);
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
    }
}
