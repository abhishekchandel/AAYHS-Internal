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
        private readonly IResultRepository _resultRepository;
        private IExhibitorRepository _exhibitorRepository;
        private IMapper _mapper;
        #endregion

        #region private
        private MainResponse _mainResponse;
        #endregion

        public ClassService(IClassRepository classRepository,IScheduleDateRepository scheduleDateRepository,IExhibitorClassRepository exhibitorClassRepository,
                            ISplitClassRepository splitClassRepository,IResultRepository resultRepository,IExhibitorRepository exhibitorRepository, IMapper Mapper)
        {
            _classRepository = classRepository;
            _scheduleDateRepository = scheduleDateRepository;
            _exhibitorClassRepositor = exhibitorClassRepository;
            _splitClassRepository = splitClassRepository;
            _resultRepository = resultRepository;
            _exhibitorRepository = exhibitorRepository;
            _mapper = Mapper;
            _mainResponse = new MainResponse();
        }

        public MainResponse GetAllClasses(ClassRequest classRequest)
        {
            var allClasses = _classRepository.GetAllClasses(classRequest);
            if (allClasses.GetAllClasses != null && allClasses.GetAllClasses.TotalRecords != 0 )
            {
                _mainResponse.GetAllClasses = allClasses.GetAllClasses;
                _mainResponse.GetAllClasses.TotalRecords = allClasses.GetAllClasses.TotalRecords;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }
        public MainResponse GetClass(int ClassId)
        {
            var getClass = _classRepository.GetClass(ClassId);
            if (getClass.GetClass!=null && getClass.GetClass.TotalRecords != 0)
            {
                _mainResponse.GetClass = getClass.GetClass;
                _mainResponse.GetClass.TotalRecords = getClass.GetClass.TotalRecords;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }
        public MainResponse GetClassExhibitors(int ClassId)
        {
            GetClassAllExhibitors getClassAllExhibitors = new GetClassAllExhibitors();
            List < GetClassExhibitors> getClassListExhibitors = new List<GetClassExhibitors>();
            var allExhibitor = _exhibitorRepository.GetAll(x=>x.IsActive==true && x.IsDeleted==false);
            if (allExhibitor!=null)
            {
                var exhibitorClasses = _exhibitorClassRepositor.GetAll(x => x.ClassId == ClassId && x.IsActive == true && x.IsDeleted == false);
                if (exhibitorClasses!=null)
                {
                    for (int i = 0; i < allExhibitor.Count(); i++)
                    {
                        GetClassExhibitors getClassExhibitors = new GetClassExhibitors();
                        if (i<=exhibitorClasses.Count()-1)
                        {
                            if (allExhibitor[i].ExhibitorId != exhibitorClasses[i].ExhibitorId)
                            {
                                getClassExhibitors.ExhibitorId = allExhibitor[i].ExhibitorId;
                                getClassExhibitors.Exhibitor = allExhibitor[i].ExhibitorId + " " + allExhibitor[i].FirstName + " " + allExhibitor[i].LastName;
                                getClassListExhibitors.Add(getClassExhibitors);
                            }
                        }
                        else
                        {
                            getClassExhibitors.ExhibitorId = allExhibitor[i].ExhibitorId;
                            getClassExhibitors.Exhibitor = allExhibitor[i].ExhibitorId + " " + allExhibitor[i].FirstName + " " + allExhibitor[i].LastName;
                            getClassListExhibitors.Add(getClassExhibitors);
                        }
                        
                    }
                    getClassAllExhibitors.getClassExhibitors = getClassListExhibitors;
                    _mainResponse.GetClassAllExhibitors = getClassAllExhibitors;
                    _mainResponse.Success = true;
                }
                else
                {
                    for (int i = 0; i < allExhibitor.Count(); i++)
                    {
                        GetClassExhibitors getClassExhibitors = new GetClassExhibitors();

                        getClassExhibitors.ExhibitorId = allExhibitor[i].ExhibitorId;
                        getClassExhibitors.Exhibitor = allExhibitor[i].ExhibitorId + " " + allExhibitor[i].FirstName + " " + allExhibitor[i].LastName;
                        getClassListExhibitors.Add(getClassExhibitors);
                    }
                    getClassAllExhibitors.getClassExhibitors = getClassListExhibitors;
                    _mainResponse.GetClassAllExhibitors = getClassAllExhibitors;
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
        public MainResponse GetExhibitorHorses(int ExhibitorId)
        {
            var exhibotorHorses = _classRepository.GetExhibitorHorses(ExhibitorId);
            if (exhibotorHorses.GetExhibitorAllHorses != null && exhibotorHorses.GetExhibitorAllHorses.TotalRecords != 0)
            {
                _mainResponse.GetExhibitorAllHorses = exhibotorHorses.GetExhibitorAllHorses;
                _mainResponse.GetExhibitorAllHorses.TotalRecords = exhibotorHorses.GetExhibitorAllHorses.TotalRecords;
                _mainResponse.Success = true;

            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }
        public MainResponse GetClassExhibitorsAndHorses(int ClassId)
        {
            _mainResponse = _classRepository.GetClassExhibitorsAndHorses(ClassId);
            if (_mainResponse.ClassExhibitorHorses.ClassExhibitorHorse != null && _mainResponse.ClassExhibitorHorses.ClassExhibitorHorse.Count > 0)
            {
                _mainResponse.ClassExhibitorHorses.TotalRecords = _mainResponse.ClassExhibitorHorses.ClassExhibitorHorse.Count();
                _mainResponse.Message = Constants.RECORD_FOUND;
                _mainResponse.Success = true;
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
                    ClassHeader=addClassRequest.ClassHeader,
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
                    updateClass.ClassHeader = addClassRequest.ClassHeader;
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
        public MainResponse GetClassEntries(ClassRequest classRequest)
        {
            var allExhibitor = _classRepository.GetClassEntries(classRequest);
            if (allExhibitor.GetAllClassEntries!=null && allExhibitor.GetAllClassEntries.TotalRecords != 0)
            {
                _mainResponse.GetAllClassEntries = allExhibitor.GetAllClassEntries;
                _mainResponse.GetAllClassEntries.TotalRecords = allExhibitor.GetAllClassEntries.TotalRecords;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }
        public async Task<MainResponse>DeleteClassExhibitor(int ExhibitorClassId, string actionBy)
        {
            var classExhibitor = _exhibitorClassRepositor.GetSingle(x => x.ExhibitorClassId == ExhibitorClassId);
            if (classExhibitor!=null)
            {
                classExhibitor.IsDeleted = true;
                classExhibitor.DeletedBy = actionBy;
                classExhibitor.DeletedDate = DateTime.Now;
                await _exhibitorClassRepositor.UpdateAsync(classExhibitor);

                _mainResponse.Success = true;
                _mainResponse.Message = Constants.CLASS_EXHIBITOR_DELETED;
            }
            else
            {
                _mainResponse.Success = true;
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
            }
            return _mainResponse;
        }
        public async Task<MainResponse> RemoveClass(int ClassId, string actionBy)
        {
            var _class = _classRepository.GetSingle(x => x.ClassId == ClassId);
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
        public async Task<MainResponse> AddUpdateSplitClass(SplitRequest splitRequest,string actionBy)
        {
            _splitClassRepository.DeleteSplitsByClassId(splitRequest);

                foreach(var split in splitRequest.splitEntries)
                {
                    var splitClass = new ClassSplits
                    {
                        ClassId = splitRequest.ClassId,
                        SplitNumber = splitRequest.SplitNumber,
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
        public MainResponse GetBackNumberForAllExhibitor(int ClassId)
        {
            GetAllBackNumber allBackNumber = new GetAllBackNumber();
            var backNumber = _classRepository.GetBackNumberForAllExhibitor(ClassId);
            if ( backNumber.Count!=0)
            {
                allBackNumber.getBackNumbers = backNumber.ToList();
                _mainResponse.GetAllBackNumber = allBackNumber;
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
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }
        public async Task<MainResponse> AddClassResult(AddClassResultRequest addClassResultRequest,string actionBy)
        {
            var ageGroup = _classRepository.GetSingle(x => x.ClassId == addClassResultRequest.ClassId);

            foreach (var addRequest in addClassResultRequest.addClassResults)
            {
                var addResult = new Result
                {
                    ClassId = addClassResultRequest.ClassId,
                    AgeGroup = ageGroup.AgeGroup,
                    ExhibitorId = addRequest.ExhibitorId,
                    Placement = addRequest.Place,
                    IsActive = true,
                    CreatedBy = actionBy,
                    CreatedDate = DateTime.Now
                };

                await _resultRepository.AddAsync(addResult);

            }
            _mainResponse.Message = Constants.CLASS_RESULT_ADDED;
            _mainResponse.Success = true;
            return _mainResponse;
        }
        public MainResponse GetResultOfClass(ClassRequest classRequest)
        {
            var getResult = _classRepository.GetResultOfClass(classRequest);
            if (getResult.GetResult!=null && getResult.GetResult.TotalRecords!=0)
            {
                _mainResponse.GetResult = getResult.GetResult;
                _mainResponse.GetResult.TotalRecords = getResult.GetResult.TotalRecords;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }
        public MainResponse SearchClass(SearchRequest searchRequest)
        {
            var search = _classRepository.SearchClass(searchRequest);
            if (search.GetAllClasses != null && search.GetAllClasses.TotalRecords != 0)
            {
                _mainResponse.GetAllClasses = search.GetAllClasses;
                _mainResponse.GetAllClasses.TotalRecords = search.GetAllClasses.TotalRecords;
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
