using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Core.Shared.Static;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AAYHS.Service.IService;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
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
        private IGlobalCodeRepository _globalCodeRepository;
        private IExhibitorClassRepository _exhibitorClassRepository;
        private IClassRepository _classRepository;
        private ISponsorExhibitorRepository _sponsorExhibitorRepository;
        private ISponsorRepository _sponsorRepository;
        private IScanRepository _scanRepository;
        private IExhibitorPaymentDetailRepository _exhibitorPaymentDetailRepository;
        private IExhibitorHorseRepository _exhibitorHorseRepository;
        private IHorseRepository _horseRepository;
        #endregion

        public ExhibitorService(IExhibitorRepository exhibitorRepository,IAddressRepository addressRepository,
                                 IExhibitorHorseRepository exhibitorHorseRepository,IHorseRepository horseRepository, 
                                 IGroupExhibitorRepository groupExhibitorRepository,IGlobalCodeRepository globalCodeRepository,
                                 IExhibitorClassRepository exhibitorClassRepository, IClassRepository classRepository,
                                 ISponsorExhibitorRepository sponsorExhibitorRepository,ISponsorRepository sponsorRepository,
                                 IScanRepository scanRepository,IExhibitorPaymentDetailRepository exhibitorPaymentDetailRepository,IMapper mapper)
        {
            _exhibitorRepository = exhibitorRepository;
            _addressRepository = addressRepository;
            _exhibitorHorseRepository = exhibitorHorseRepository;
            _horseRepository = horseRepository;
            _groupExhibitorRepository = groupExhibitorRepository;
            _globalCodeRepository = globalCodeRepository;
            _exhibitorClassRepository = exhibitorClassRepository;
            _classRepository = classRepository;
            _sponsorExhibitorRepository = sponsorExhibitorRepository;
            _sponsorRepository = sponsorRepository;
            _scanRepository = scanRepository;
            _exhibitorPaymentDetailRepository = exhibitorPaymentDetailRepository;
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
                    ZipCodeId = request.ZipCodeId,
                    CreatedBy = actionBy,
                    CreatedDate = DateTime.Now
                };
                var _address = _addressRepository.Add(address);
                var exhibitor = new Exhibitors
                {                   
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
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false
                };
               
                var _exhibitor= _exhibitorRepository.Add(exhibitor);
                if (request.GroupId > 0)
                {
                    var groupExhibitor = new GroupExhibitors
                    {
                        ExhibitorId = _exhibitor.ExhibitorId,
                        GroupId = request.GroupId,
                        CreatedBy = actionBy,
                        CreatedDate= DateTime.Now,
                        IsActive=true,
                        IsDeleted=false
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
                        address.ZipCodeId = request.ZipCodeId;
                        address.ModifiedBy = actionBy;
                        address.ModifiedDate = DateTime.Now;
                        _addressRepository.Update(address);
                    }

                    if (request.GroupId > 0)
                    {
                        var groupExhibitor = _groupExhibitorRepository.GetSingle(x => x.ExhibitorId == exhibitor.ExhibitorId);
                        if (groupExhibitor != null && groupExhibitor.GroupExhibitorId > 0)
                        {
                            groupExhibitor.GroupId = request.GroupId;
                            groupExhibitor.ModifiedBy = actionBy;
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

        public MainResponse GetAllHorses(int exhibitorId)
        {
            var allHorses = _horseRepository.GetAll(x => x.IsActive == true && x.IsDeleted == false);
            var exhibitorHorses = _exhibitorHorseRepository.GetAll(x => x.ExhibitorId == exhibitorId && x.IsActive == true && x.IsDeleted == false);

            if (allHorses.Count>0)
            {
                var horses = allHorses.Where(x => exhibitorHorses.All(y => y.HorseId != x.HorseId)).ToList();
                var _allHorses = _mapper.Map<List<GetHorses>>(horses);
                GetExhibitorHorsesList getExhibitorHorsesList = new GetExhibitorHorsesList();
                getExhibitorHorsesList.getHorses = _allHorses;
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
                var horseType= _globalCodeRepository.GetSingle(x => x.GlobalCodeId == horse.HorseTypeId);
                var horseDetail = _mapper.Map<GetHorses>(horse);
                horseDetail.HorseType = horseType.CodeName;
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

        public MainResponse GetAllClassesOfExhibitor(int exhibitorId)
        {
            var exhibitorClasses = _exhibitorRepository.GetAllClassesOfExhibitor(exhibitorId);

            if (exhibitorClasses != null && exhibitorClasses.TotalRecords!=0)
            {
                _mainResponse.GetAllClassesOfExhibitor = exhibitorClasses;
                _mainResponse.GetAllClassesOfExhibitor.TotalRecords = exhibitorClasses.TotalRecords;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse RemoveExhibitorFromClass(int exhibitorClassId, string actionBy)
        {
            var exhibitor = _exhibitorClassRepository.GetSingle(x => x.ExhibitorClassId == exhibitorClassId);

            if (exhibitor != null)
            {
                exhibitor.IsDeleted = true;
                exhibitor.DeletedBy = actionBy;
                exhibitor.DeletedDate = DateTime.Now;
                _exhibitorClassRepository.Update(exhibitor);

                _mainResponse.Message = Constants.CLASS_REMOVED;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse GetAllClasses(int exhibitorId)
        {
            var allClasses = _classRepository.GetAll(x => x.IsActive == true && x.IsDeleted == false);
            var exhibitorInClass = _exhibitorClassRepository.GetAll(x => x.ExhibitorId == exhibitorId && x.IsActive == true && x.IsDeleted == false);

            if (allClasses.Count > 0)
            {
                var classes = allClasses.Where(x => exhibitorInClass.All(y => y.ClassId != x.ClassId)).ToList();
                var _allClasses = _mapper.Map<List<GetClassesForExhibitor>>(classes);
                GetAllClassesForExhibitor getAllClassesForExhibitor = new GetAllClassesForExhibitor();
                getAllClassesForExhibitor.getClassesForExhibitor = _allClasses;
                _mainResponse.GetAllClassesForExhibitor = getAllClassesForExhibitor;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse GetClassDetail(int classId, int exhibitorId)
        {
            var classDetail = _classRepository.GetSingle(x => x.ClassId == classId && x.IsActive == true && x.IsDeleted == false);
            if (classDetail != null && classDetail.ClassId > 0)
            {
                var entries = _exhibitorClassRepository.GetAll(x => x.ClassId == classId && x.IsActive == true && x.IsDeleted == false);               
                var _class = _mapper.Map<GetClassesForExhibitor>(classDetail);
                _class.Entries = entries.Count();
                _class.IsScratch = false;
                _mainResponse.GetClassesForExhibitor = _class;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse UpdateScratch(UpdateScratch updateScratch,string actionBy)
        {
            var exhibitorClass = _exhibitorClassRepository.GetSingle(x => x.ExhibitorClassId == updateScratch.exhibitorClassId && x.IsActive == true
                                                          &&  x.IsDeleted == false);

            if (exhibitorClass!=null && exhibitorClass.ExhibitorClassId>0)
            {
                exhibitorClass.IsScratch = updateScratch.IsScratch;
                exhibitorClass.ModifiedBy = actionBy;
                exhibitorClass.ModifiedDate = DateTime.Now;
                _exhibitorClassRepository.Update(exhibitorClass);

                _mainResponse.Message = Constants.CLASS_EXHIBITOR_SCRATCH;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse AddExhibitorToClass(AddExhibitorToClass addExhibitorToClass, string actionBy)
        {
            var exhibitor = new ExhibitorClass
            {
                ExhibitorId = addExhibitorToClass.ExhibitorId,
                ClassId = addExhibitorToClass.ClassId,
                CreatedBy = actionBy,
                CreatedDate = DateTime.Now
            };

            _exhibitorClassRepository.Add(exhibitor);
            _mainResponse.Message = Constants.CLASS_EXHIBITOR;
            _mainResponse.Success = true;
            return _mainResponse;
        }

        public MainResponse GetAllSponsorsOfExhibitor(int exhibitorId)
        {
            var allsponsor = _exhibitorRepository.GetAllSponsorsOfExhibitor(exhibitorId);

            if (allsponsor.getSponsorsOfExhibitors!=null && allsponsor.TotalRecords>0 )
            {
                _mainResponse.GetAllSponsorsOfExhibitor = allsponsor;
                _mainResponse.GetAllSponsorsOfExhibitor.TotalRecords = allsponsor.TotalRecords;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse RemoveSponsorFromExhibitor(int sponsorExhibitorId,string actionBy)
        {
            var sponsor = _sponsorExhibitorRepository.GetSingle(x => x.SponsorExhibitorId == sponsorExhibitorId && x.IsActive==true && x.IsDeleted==false);

            if (sponsor!=null)
            {
                sponsor.IsDeleted = true;
                sponsor.DeletedBy = actionBy;
                sponsor.DeletedDate = DateTime.Now;

                _sponsorExhibitorRepository.Update(sponsor);
                _mainResponse.Message = Constants.EXHIBITOR_SPONSOR_REMOVED;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse GetAllSponsor(int exhibitorId)
        {
            var allSponsors = _sponsorRepository.GetAll(x => x.IsActive == true && x.IsDeleted == false);
            var sponsors = _sponsorExhibitorRepository.GetAll(x=>x.ExhibitorId==exhibitorId && x.IsActive==true && x.IsDeleted==false);

            if (sponsors.Count>0)
            {
                var _sponsors = allSponsors.Where(x => sponsors.All(y => y.SponsorId != x.SponsorId)).ToList();
                var _allSponsors = _mapper.Map<List<GetSponsorForExhibitor>>(_sponsors);
                GetAllSponsorForExhibitor getAllSponsorForExhibitor = new GetAllSponsorForExhibitor();
                getAllSponsorForExhibitor.getSponsorForExhibitors = _allSponsors;
                _mainResponse.GetAllSponsorForExhibitor = getAllSponsorForExhibitor;
                _mainResponse.Success = true;

            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse GetSponsorDetail(int sponsorId)
        {
            var sponsor= _exhibitorRepository.GetSponsorDetail(sponsorId);

            if (sponsor!=null && sponsor.SponsorId>0)
            {              
                _mainResponse.GetSponsorForExhibitor = sponsor;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_EXIST_WITH_ID;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }
      
        public MainResponse AddSponsorForExhibitor(AddSponsorForExhibitor addSponsorForExhibitor, string actionBy)
        {
            if (addSponsorForExhibitor.SponsorExhibitorId==0)
            {
                var sponsor = new SponsorExhibitor
                {
                    ExhibitorId = addSponsorForExhibitor.ExhibitorId,
                    SponsorId = addSponsorForExhibitor.SponsorId,
                    SponsorTypeId = addSponsorForExhibitor.SponsorTypeId,
                    TypeId = addSponsorForExhibitor.TypeId,
                    CreatedBy = actionBy,
                    CreatedDate = DateTime.Now
                };
                _sponsorExhibitorRepository.Add(sponsor);
                _mainResponse.Message = Constants.EXHIBITOR_SPONSOR_ADDED;
                _mainResponse.Success = true;
            }
            else
            {
                var sponsor = _sponsorExhibitorRepository.GetSingle(x => x.SponsorExhibitorId == addSponsorForExhibitor.SponsorExhibitorId && x.IsActive == true
                                                                    && x.IsDeleted == false);
                if (sponsor!=null && sponsor.SponsorExhibitorId>0)
                {
                    sponsor.SponsorId = addSponsorForExhibitor.SponsorId;
                    sponsor.ExhibitorId = addSponsorForExhibitor.ExhibitorId;
                    sponsor.SponsorTypeId = addSponsorForExhibitor.SponsorTypeId;
                    sponsor.TypeId = addSponsorForExhibitor.TypeId;
                    sponsor.ModifiedDate = DateTime.Now;
                    _sponsorExhibitorRepository.Update(sponsor);
                    _mainResponse.Message = Constants.EXHIBITOR_SPONSOR_UPDATED;
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

        public MainResponse GetExhibitorFinancials(int exhibitorId)
        {
            var exhibitorFinancials = _exhibitorRepository.GetExhibitorFinancials(exhibitorId);
            _mainResponse.GetExhibitorFinancials = exhibitorFinancials;
            return _mainResponse;
        }

        public MainResponse UploadDocumentFile(DocumentUploadRequest documentUploadRequest,string actionBy)
        {
            string uniqueFileName = null;
            string path = null;
            if (documentUploadRequest.Documents!=null)
            {
                foreach (IFormFile file in documentUploadRequest.Documents)
                {

                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

                    
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                    var FilePath = Path.Combine(uploadsFolder, "Resources", "Documents");
                    path = Path.Combine(FilePath, uniqueFileName);

                    string filePath = Path.Combine(FilePath, uniqueFileName);


                    file.CopyTo(new FileStream(filePath, FileMode.Create));


                    path = path.Replace(uploadsFolder, "").Replace("\\", "/");

                    var scans = new Scans
                    {
                        ExhibitorId = documentUploadRequest.Exhibitor,
                        DocumentType = documentUploadRequest.DocumentType,
                        DocumentPath = path,
                        CreatedBy = actionBy,
                        CreatedDate = DateTime.Now
                    };
                    _scanRepository.Add(scans);
                }
               
                _mainResponse.Message = Constants.DOCUMENT_UPLOAD;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_DOCUMENT_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse GetUploadedDocuments(int exhibitorId)
        {
            var documents = _exhibitorRepository.GetUploadedDocuments(exhibitorId);

            if (documents.getUploadedDocuments!=null && documents.getUploadedDocuments.Count()>0)
            {
                _mainResponse.GetAllUploadedDocuments = documents;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_DOCUMENT_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse GetFees()
        {
            var fees = _exhibitorRepository.GetAllFees();

            if (fees.getFees!=null && fees.getFees.Count()>0)
            {
                _mainResponse.GetAllFees = fees;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse RemoveExhibitorTransaction(int exhibitorPaymentId,string actionBy)
        {
            var exhibitorPayment = _exhibitorPaymentDetailRepository.GetSingle(x => x.ExhibitorPaymentId == exhibitorPaymentId && x.IsActive == true
                                    && x.IsDeleted == false);

            if (exhibitorPayment!=null && exhibitorPayment.ExhibitorPaymentId>0)
            {
                exhibitorPayment.IsDeleted = true;
                exhibitorPayment.DeletedBy = actionBy;
                exhibitorPayment.DeletedDate = DateTime.Now;
                _exhibitorPaymentDetailRepository.Update(exhibitorPayment);

                _mainResponse.Message = Constants.FINANCIAL_TRANSACTION_DELETED;
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
