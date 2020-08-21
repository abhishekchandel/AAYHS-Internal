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
        #endregion

        public ExhibitorService(IExhibitorRepository ExhibitorRepository, IMapper Mapper)
        {
            _ExhibitorRepository = ExhibitorRepository;
            _Mapper = Mapper;
            _mainResponse = new MainResponse();
        }

        public MainResponse AddUpdateExhibitor(ExhibitorRequest request, string actionBy)
        {
            if (request.ExhibitorId <= 0)
            {
                var exhibitor = new Exhibitors
                {
                    GroupId=request.GroupId,
                    AddressId=request.AddressId,
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
                exhibitor.GroupId = request.GroupId;
                exhibitor.AddressId = request.AddressId;
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
            var data = _ExhibitorRepository.GetSingle(x => x.ExhibitorId == ExhibitorId && x.IsActive == true && x.IsDeleted == false);
            if (data != null && data.ExhibitorId > 0)
            {
                _mainResponse.ExhibitorResponse = _Mapper.Map<ExhibitorResponse>(data);
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
