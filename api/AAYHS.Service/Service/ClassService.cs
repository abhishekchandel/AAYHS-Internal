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
using System.Threading.Tasks;

namespace AAYHS.Service.Service
{
    public class ClassService : IClassService
    {
        #region readonly
        private readonly IClassRepository _classRepository;
        private IScheduleDateRepository _scheduleDateRepository;
        private readonly IExhibitorClassRepository _exhibitorClassRepositor;
        private ISplitClassRepository _splitClassRepository;
        private IMapper _mapper;
        #endregion

        #region private
        private MainResponse _mainResponse;
        #endregion

        public ClassService(IClassRepository classRepository,IScheduleDateRepository scheduleDateRepository,IExhibitorClassRepository exhibitorClassRepository,
                            ISplitClassRepository splitClassRepository,IMapper Mapper)
        {
            _classRepository = classRepository;
            _scheduleDateRepository = scheduleDateRepository;
            _exhibitorClassRepositor = exhibitorClassRepository;
            _splitClassRepository = splitClassRepository;
            _mapper = Mapper;
            _mainResponse = new MainResponse();
        }

        public MainResponse GetAllClasses(ClassRequest classRequest)
        {
            var allClasses = _classRepository.GetAllClasses(classRequest);
            if (allClasses.GetAllClasses != null)
            {
                _mainResponse.GetAllClasses = allClasses.GetAllClasses;
                _mainResponse.TotalRecords = _mainResponse.GetAllClasses.classResponses.Count();
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }
        public MainResponse GetClass(ClassRequest classRequest)
        {
            var getClass = _classRepository.GetClass(classRequest);
            if (getClass.GetClass!=null)
            {
                _mainResponse.GetClass = getClass.GetClass;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }
        public async Task<MainResponse> CreateUpdateClass(AddClassRequest addClassRequest,string actionBy)
        {
            if (addClassRequest.ClassId == 0)
            {
                var classes = new Classes
                {
                    ClassNumber = addClassRequest.ClassNumber,
                    Name = addClassRequest.Name,
                    Location = addClassRequest.Location,
                    AgeGroup = addClassRequest.AgeGroup,
                    IsActive = true,
                    CreatedBy = actionBy,
                    CreatedDate = DateTime.Now
                };
                var _class = await _classRepository.AddAsync(classes);

                var schedule = new ScheduleDates
                {
                    ClassId = _class.ClassId,
                    Date = addClassRequest.ScheduleDate,
                    Time = addClassRequest.ScheduleTime,
                    IsActive = true,
                    CreatedBy = actionBy,
                    CreatedDate = DateTime.Now,
                };
                await _scheduleDateRepository.AddAsync(schedule);
                _mainResponse.Message = Constants.CLASS_CREATED;
                _mainResponse.Success = true;
                return _mainResponse;
            }
            else
            {
                var updateClass = _classRepository.GetSingle(x => x.ClassId == addClassRequest.ClassId);
                if (updateClass!=null)
                {
                    updateClass.ClassNumber = addClassRequest.ClassNumber;
                    updateClass.Name = addClassRequest.Name;
                    updateClass.Location = addClassRequest.Location;
                    updateClass.AgeGroup = addClassRequest.AgeGroup;
                    updateClass.ModifiedBy = actionBy;
                    updateClass.ModifiedDate = DateTime.Now;
                    await _classRepository.UpdateAsync(updateClass);
                }
                var updateClassSchedule = _scheduleDateRepository.GetSingle(x => x.ClassId == addClassRequest.ClassId);
                if (updateClassSchedule!=null)
                {
                    updateClassSchedule.Date = addClassRequest.ScheduleDate;
                    updateClassSchedule.Time = addClassRequest.ScheduleTime;
                    updateClassSchedule.ModifiedBy = actionBy;
                    updateClassSchedule.ModifiedDate = DateTime.Now;
                    await _scheduleDateRepository.UpdateAsync(updateClassSchedule);
                }
                _mainResponse.Message = Constants.CLASS_UPDATED;
                _mainResponse.Success = true;
                return _mainResponse;
            }
           
        }
        public async Task<MainResponse> AddExhibitorToClass(AddClassExhibitor addClassExhibitor, string actionBy)
        {
            var addExhibitor = new ExhibitorClass
            {
                ExhibitorId = addClassExhibitor.ExhibitorId,
                ClassId = addClassExhibitor.ClassId,
                HorseId = addClassExhibitor.HorseId,
                IsScratch=addClassExhibitor.Scratch,
                IsActive = true,
                CreatedBy = actionBy,
                CreatedDate = DateTime.Now
            };
            await _exhibitorClassRepositor.AddAsync(addExhibitor);
            _mainResponse.Message = Constants.CLASS_EXHIBITOR;
            _mainResponse.Success = true;
            return _mainResponse;
           
        }
        public MainResponse GetClassExhibitors(ClassRequest classRequest)
        {
            var allExhibitor = _classRepository.GetClassExhibitors(classRequest);
            if (allExhibitor.GetAllClassExhibitor!=null)
            {
                _mainResponse.GetAllClassExhibitor.getClassExhibitors = allExhibitor.GetAllClassExhibitor.getClassExhibitors;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }
        public async Task<MainResponse> RemoveClass(RemoveClass removeClass,string actionBy)
        {
            var _class = _classRepository.GetSingle(x => x.ClassId == removeClass.ClassId);
            if (_class != null)
            {
                _class.IsDeleted = true;
                _class.DeletedBy = actionBy;
                _class.DeletedDate = DateTime.Now;
                await _classRepository.UpdateAsync(_class);

                _mainResponse.Success = true;
                _mainResponse.Message = Constants.CLASS_REMOVED;
            }
            else
            {
                _mainResponse.Success = true;
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
            }

            return _mainResponse;
        }
        public async Task<MainResponse> AddUpdateSplitClass(List<SplitRequest> splitRequest,string actionBy)
        {
            _splitClassRepository.DeleteSplitsByClassId(splitRequest);
                foreach(var split in splitRequest)
                {
                    var splitClass = new ClassSplits
                    {
                        ClassId = split.ClassId,
                        SplitNumber = split.SplitNumber,
                        Entries = split.Entries,
                        IsActive = true,
                        CreatedBy = actionBy,
                        CreatedDate = DateTime.Now
                    };
                   await _splitClassRepository.AddAsync(splitClass);
                };
                                 
            _mainResponse.Message = Constants.SPLIT_CREATED;
            _mainResponse.Success = true;           
            return _mainResponse;
        }
        public MainResponse GetBackNumberForAllExhibitor(BackNumberRequest backNumberRequest)
        {
            GetAllBackNumber allBackNumber = new GetAllBackNumber();
            var backNumber = _classRepository.GetBackNumberForAllExhibitor(backNumberRequest);
            if ( backNumber.Count!=0)
            {
                allBackNumber.getBackNumbers = backNumber.ToList();
                _mainResponse.GetAllBackNumber = allBackNumber;
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
        public MainResponse GetResultExhibitorDetails(ResultExhibitorRequest resultExhibitorRequest)
        {
            var exhibitorDetails = _classRepository.GetResultExhibitorDetails(resultExhibitorRequest);
            if (exhibitorDetails!=null)
            {
                _mainResponse.ResultExhibitorDetails = exhibitorDetails;
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
