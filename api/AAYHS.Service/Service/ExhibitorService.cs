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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
namespace AAYHS.Service.Service
{
  public  class ExhibitorService: IExhibitorService
    {
        #region readonly
        private readonly IMapper _Mapper;
        #endregion

        #region private
        private MainResponse _mainResponse;
        private IExhibitorRepository _ExhibitorRepository;
        private IAddressRepository _addressRepository;
        #endregion

        public ExhibitorService(IExhibitorRepository ExhibitorRepository,IAddressRepository addressRepository, IMapper Mapper)
        {
            _ExhibitorRepository = ExhibitorRepository;
            _addressRepository = addressRepository;
            _Mapper = Mapper;
            _mainResponse = new MainResponse();
        }

        public MainResponse AddUpdateExhibitor(ExhibitorRequest request, string actionBy)
        {
            if (request.ExhibitorId <= 0)
            {
                var exhibitorBackNumberExist = _ExhibitorRepository.GetSingle(x => x.BackNumber == request.BackNumber && x.IsActive == true && x.IsDeleted == false);
                if (exhibitorBackNumberExist != null && exhibitorBackNumberExist.ExhibitorId>0)
                {
                    _mainResponse.Message = Constants.BACKNUMBER_AlREADY_EXIST;
                    _mainResponse.Success = false;
                    return _mainResponse;
                }
                var address = new Addresses
                {
                    Address = request.Address,
                    CityId = request.CityId,
                    ZipCode = request.ZipCode,
                    CreatedBy = actionBy,
                    CreatedDate = DateTime.Now
                };
                var _address = _addressRepository.Add(address);
                var exhibitor = new Exhibitors
                {
                    GroupId=request.GroupId,
                    AddressId= _address.AddressId,
                    FirstName=request.FirstName,
                    LastName=request.LastName,
                    BackNumber=request.BackNumber,
                    BirthYear=request.BirthYear,
                    IsNSBAMember=request.IsNSBAMember,
                    IsDoctorNote=request.IsDoctorNote,
                    QTYProgram=request.QTYProgram,
                    PrimaryEmail=request.PrimaryEmail,
                    SecondaryEmail=request.SecondaryEmail,
                    Phone=request.Phone,
                    CreatedBy = actionBy,
                    CreatedDate = DateTime.Now
                };
               
                var _exhibitor= _ExhibitorRepository.Add(exhibitor);
             
                _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
                _mainResponse.NewId = _exhibitor.ExhibitorId;
                _mainResponse.Success = true;
            }
            else
            {               
                var exhibitor = _ExhibitorRepository.GetSingle(x => x.ExhibitorId == request.ExhibitorId && x.IsActive == true && x.IsDeleted == false);
               
                if (exhibitor!=null && exhibitor.ExhibitorId>0)
                {
                    exhibitor.GroupId = request.GroupId;
                    exhibitor.FirstName = request.FirstName;
                    exhibitor.LastName = request.LastName;
                    exhibitor.BackNumber = request.BackNumber;
                    exhibitor.BirthYear = request.BirthYear;
                    exhibitor.IsNSBAMember = request.IsNSBAMember;
                    exhibitor.IsDoctorNote = request.IsDoctorNote;
                    exhibitor.QTYProgram = request.QTYProgram;
                    exhibitor.PrimaryEmail = request.PrimaryEmail;
                    exhibitor.SecondaryEmail = request.SecondaryEmail;
                    exhibitor.Phone = request.Phone;
                    exhibitor.ModifiedDate = DateTime.Now;
                    exhibitor.ModifiedBy = actionBy;
                    _ExhibitorRepository.Update(exhibitor);
                    _mainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;
                    _mainResponse.NewId = request.ExhibitorId;
                    _mainResponse.Success = true;

                    var address = _addressRepository.GetSingle(x => x.AddressId == request.AddressId && x.IsActive == true && x.IsDeleted == false);
                    if (address!=null && address.AddressId>0)
                    {
                        address.Address = request.Address;
                        address.CityId = request.CityId;
                        address.ZipCode = request.ZipCode;
                        address.ModifiedBy = actionBy;
                        address.ModifiedDate = DateTime.Now;
                        _addressRepository.Update(address);
                    }
                }
                else
                {
                    _mainResponse.Message = Constants.NO_RECORD_EXIST_WITH_ID;
                    _mainResponse.Success = false;
                }
               
               
            }
            return _mainResponse;
        }
    
        public MainResponse GetAllExhibitors(BaseRecordFilterRequest filterRequest)
        {
            _mainResponse = _ExhibitorRepository.GetAllExhibitors(filterRequest);
            if (_mainResponse.ExhibitorListResponse.exhibitorResponses != null && _mainResponse.ExhibitorListResponse.TotalRecords > 0)
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

        public MainResponse GetExhibitorById(int ExhibitorId)
        {
            var data = _ExhibitorRepository.GetExhibitorById(ExhibitorId);
            if (data.exhibitorResponses != null && data.TotalRecords > 0)
            {               
                _mainResponse.ExhibitorListResponse =data;
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

        public MainResponse DeleteExhibitor(int ExhibitorId,string actionBy)
        {
            var Exhibitor = _ExhibitorRepository.GetSingle(x => x.ExhibitorId == ExhibitorId);
            if (Exhibitor != null && Exhibitor.ExhibitorId>0)
            {
                Exhibitor.IsDeleted = true;
                Exhibitor.IsActive = false;
                Exhibitor.DeletedDate = DateTime.Now;
                Exhibitor.DeletedBy = actionBy;
                _ExhibitorRepository.Update(Exhibitor);
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
    }
}
