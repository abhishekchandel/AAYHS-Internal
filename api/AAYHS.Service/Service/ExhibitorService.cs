using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
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
        private readonly IMapper _mapper;
        #endregion

        #region private
        private MainResponse _mainResponse;
        private IExhibitorRepository _exhibitorRepository;
        private IAddressRepository _addressRepository;
        private IGroupExhibitorRepository _groupExhibitorRepository;
        private IExhibitorHorseRepository _exhibitorHorseRepository;
        private IHorseRepository _horseRepository;
        #endregion

        public ExhibitorService(IExhibitorRepository exhibitorRepository,IAddressRepository addressRepository,
                                 IExhibitorHorseRepository exhibitorHorseRepository,IHorseRepository horseRepository, IGroupExhibitorRepository groupExhibitorRepository, IMapper mapper)
        {
            _exhibitorRepository = exhibitorRepository;
            _addressRepository = addressRepository;
            _exhibitorHorseRepository = exhibitorHorseRepository;
            _horseRepository = horseRepository;
            _groupExhibitorRepository = groupExhibitorRepository;
            _mapper = mapper;
            _mainResponse = new MainResponse();
        }

        public MainResponse AddUpdateExhibitor(ExhibitorRequest request, string actionBy)
        {
            if (request.ExhibitorId <= 0)
            {
                var exhibitorBackNumberExist = _exhibitorRepository.GetSingle(x => x.BackNumber == request.BackNumber && x.IsActive == true && x.IsDeleted == false);
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
               
                var _exhibitor= _exhibitorRepository.Add(exhibitor);
                if (request.GroupId != null && request.GroupId > 0)
                {
                    var groupExhibitor = new GroupExhibitors
                    {
                        ExhibitorId = _exhibitor.ExhibitorId,
                        GroupId = request.GroupId,
                         CreatedDate= DateTime.Now
                };
                    var _groupExhibitor = _groupExhibitorRepository.Add(groupExhibitor);
                }
                _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
                _mainResponse.NewId = _exhibitor.ExhibitorId;
                _mainResponse.Success = true;
            }
            else
            {               
                var exhibitor = _exhibitorRepository.GetSingle(x => x.ExhibitorId == request.ExhibitorId && x.IsActive == true && x.IsDeleted == false);
               
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
                    _exhibitorRepository.Update(exhibitor);
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

                    if (request.GroupId != null && request.GroupId > 0)
                    {
                        var groupExhibitor = _groupExhibitorRepository.GetSingle(x => x.ExhibitorId == exhibitor.ExhibitorId);
                        if (groupExhibitor != null && groupExhibitor.GroupExhibitorId > 0)
                        {
                            groupExhibitor.GroupId = request.GroupId;
                            groupExhibitor.ModifiedDate = DateTime.Now;
                            _groupExhibitorRepository.Update(groupExhibitor);
                        }
                    }
                    else
                    {
                        var groupExhibitor = _groupExhibitorRepository.GetSingle(x => x.ExhibitorId == exhibitor.ExhibitorId);
                        if (groupExhibitor != null && groupExhibitor.GroupExhibitorId > 0)
                        {
                            groupExhibitor.IsActive = false;
                            groupExhibitor.IsDeleted = true;
                            _groupExhibitorRepository.Update(groupExhibitor);
                        }
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
            var exhibitorList = _exhibitorRepository.GetAllExhibitors(filterRequest);
            if (exhibitorList.exhibitorResponses != null && exhibitorList.TotalRecords > 0)
            {
                _mainResponse.ExhibitorListResponse = exhibitorList;
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

        public MainResponse GetExhibitorById(int exhibitorId)
        {
            var data = _exhibitorRepository.GetExhibitorById(exhibitorId);
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

        public MainResponse DeleteExhibitor(int exhibitorId,string actionBy)
        {
            var Exhibitor = _exhibitorRepository.GetSingle(x => x.ExhibitorId == exhibitorId);
            if (Exhibitor != null && Exhibitor.ExhibitorId>0)
            {
                Exhibitor.IsDeleted = true;
                Exhibitor.IsActive = false;
                Exhibitor.DeletedDate = DateTime.Now;
                Exhibitor.DeletedBy = actionBy;
                _exhibitorRepository.Update(Exhibitor);
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

        public MainResponse SearchExhibitor(SearchRequest searchRequest)
        {
            var exhibitorList = _exhibitorRepository.SearchExhibitor(searchRequest);
            if (exhibitorList.exhibitorResponses != null && exhibitorList.TotalRecords > 0)
            {
                _mainResponse.ExhibitorListResponse = exhibitorList;
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

        public MainResponse GetExhibitorHorses(int exhibitorId)
        {
            var exhibitorHorses = _exhibitorRepository.GetExhibitorHorses(exhibitorId);
            if (exhibitorHorses.exhibitorHorses!=null && exhibitorHorses.TotalRecords>0)
            {
                _mainResponse.ExhibitorHorsesResponse = exhibitorHorses;
                _mainResponse.ExhibitorHorsesResponse.TotalRecords = exhibitorHorses.TotalRecords;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse DeleteExhibitorHorse(int exhibitorHorseId,string actionBy)
        {
            var exhibitorHorse = _exhibitorHorseRepository.GetSingle(x => x.ExhibitorHorseId == exhibitorHorseId && x.IsActive == true && x.IsDeleted == false);
            if (exhibitorHorse!=null && exhibitorHorse.ExhibitorId>0)
            {              
                _exhibitorHorseRepository.Delete(exhibitorHorse);
                _mainResponse.Message = Constants.EXHIBITOR_HORSE_DELETED;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse GetAllHorses()
        {
            var horses = _horseRepository.GetAll(x => x.IsActive == true && x.IsDeleted == false);
            if (horses.Count>0)
            {
                var allHorses=  _mapper.Map<List<GetHorses>>(horses);
                GetExhibitorHorsesList getExhibitorHorsesList = new GetExhibitorHorsesList();
                getExhibitorHorsesList.getHorses = allHorses;
                _mainResponse.GetExhibitorHorsesList = getExhibitorHorsesList;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse GetHorseDetail(int horseId)
        {
            var horse = _horseRepository.GetSingle(x => x.HorseId == horseId && x.IsActive == true && x.IsDeleted == false);
            if (horse != null && horse.HorseId>0)
            {
                var horseDetail = _mapper.Map<GetHorses>(horse);
                _mainResponse.GetHorses = horseDetail;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse AddExhibitorHorse(AddExhibitorHorseRequest addExhibitorHorseRequest,string actionBy)
        {
            var exhibitorHorse = new ExhibitorHorse
            {
                ExhibitorId = addExhibitorHorseRequest.ExhibitorId,
                HorseId=addExhibitorHorseRequest.HorseId,
                BackNumber=addExhibitorHorseRequest.BackNumber,
                CreatedDate=DateTime.Now,
                CreatedBy= actionBy
            };

            _exhibitorHorseRepository.Add(exhibitorHorse);
            _mainResponse.Message = Constants.EXHIBITOR_HORSE_ADDED;
            _mainResponse.Success = true;
            return _mainResponse;
        }
  }
}
