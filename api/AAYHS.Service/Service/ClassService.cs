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
            if (allClasses != null && allClasses.TotalRecords != 0 )
            {
                _mainResponse.GetAllClasses = allClasses;
                _mainResponse.GetAllClasses.TotalRecords = allClasses.TotalRecords;
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
            if (getClass!=null && getClass.TotalRecords!=0)
            {
                _mainResponse.GetClass = getClass;
                _mainResponse.GetClass.TotalRecords = getClass.TotalRecords;
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
                var exhibitor = allExhibitor.Where(x => exhibitorClasses.All(y => y.ExhibitorId != x.ExhibitorId)).ToList(); 
                if (exhibitor.Count()!=0)
                {
                    for (int i = 0; i < exhibitor.Count(); i++)
                    {
                      GetClassExhibitors getClassExhibitors = new GetClassExhibitors();
                                                                       
                      getClassExhibitors.ExhibitorId = exhibitor[i].ExhibitorId;
                      getClassExhibitors.Exhibitor = exhibitor[i].ExhibitorId + " " + exhibitor[i].FirstName + " " + exhibitor[i].LastName;
                      getClassListExhibitors.Add(getClassExhibitors);
                                                                    
                    }
                    getClassAllExhibitors.getClassExhibitors = getClassListExhibitors;
                    _mainResponse.GetClassAllExhibitors = getClassAllExhibitors;
                    _mainResponse.Success = true;
                }
                else
                {
                    _mainResponse.Message = Constants.NO_RECORD_FOUND;
                    _mainResponse.Success = false;
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
            if (exhibotorHorses != null && exhibotorHorses.TotalRecords != 0)
            {
                _mainResponse.GetExhibitorAllHorses = exhibotorHorses;
                _mainResponse.GetExhibitorAllHorses.TotalRecords = exhibotorHorses.TotalRecords;
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
                    ClassHeaderId=addClassRequest.ClassHeaderId,
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

                if (addClassRequest.getClassSplit != null)
                {
                    foreach (var split in addClassRequest.getClassSplit)
                    {
                        
                        var classSplit = new ClassSplits
                        {
                            ClassId = _class.ClassId,
                            SplitNumber = addClassRequest.SplitNumber,
                            ChampionShipIndicator = addClassRequest.ChampionShipIndicator,
                            Entries = split.Entries,
                            IsActive = true,
                            CreatedBy = actionBy,
                            CreatedDate = DateTime.Now,
                        };
                        await _splitClassRepository.AddAsync(classSplit);
                    }
                }
               
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
                    updateClass.ClassHeaderId = addClassRequest.ClassHeaderId;
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

                if (addClassRequest.getClassSplit != null)
                {
                    _splitClassRepository.DeleteSplitsByClassId(addClassRequest);

                    foreach (var split in addClassRequest.getClassSplit)
                    {
                        var classSplit = new ClassSplits
                        {
                            ClassId = addClassRequest.ClassId,
                            SplitNumber = addClassRequest.SplitNumber,
                            ChampionShipIndicator = addClassRequest.ChampionShipIndicator,
                            Entries = split.Entries,
                            IsActive = true,
                            CreatedBy = actionBy,
                            CreatedDate = DateTime.Now
                        };
                        await _splitClassRepository.AddAsync(classSplit);
                    };
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
            if (allExhibitor!=null && allExhibitor.TotalRecords != 0)
            {
                _mainResponse.GetAllClassEntries = allExhibitor;
                _mainResponse.GetAllClassEntries.TotalRecords = allExhibitor.TotalRecords;
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
                _mainResponse.Success = false;
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
            
                var addResult = new Result
                {
                    ClassId = addClassResultRequest.ClassId,
                    AgeGroup = ageGroup.AgeGroup,
                    ExhibitorId = addClassResultRequest.ExhibitorId,
                    Placement = addClassResultRequest.Place,
                    IsActive = true,
                    CreatedBy = actionBy,
                    CreatedDate = DateTime.Now
                };

             await _resultRepository.AddAsync(addResult);
         
            _mainResponse.Message = Constants.CLASS_RESULT_ADDED;
            _mainResponse.Success = true;
            return _mainResponse;
        }
        public MainResponse GetResultOfClass(ClassRequest classRequest)
        {
            var getResult = _classRepository.GetResultOfClass(classRequest);
            if (getResult!=null && getResult.TotalRecords!=0)
            {
                _mainResponse.GetResult = getResult;
                _mainResponse.GetResult.TotalRecords = getResult.TotalRecords;
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
            if (search != null && search.TotalRecords != 0)
            {
                _mainResponse.GetAllClasses = search;
                _mainResponse.GetAllClasses.TotalRecords = search.TotalRecords;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }
        public MainResponse UpdateClassExhibitorScratch(ClassExhibitorScratch classExhibitorScratch, string actionBy)
        {
            var classExhibitor = _exhibitorClassRepositor.GetSingle(x => x.ExhibitorClassId == classExhibitorScratch.ExhibitorClassId);
            if (classExhibitor!=null)
            {
                classExhibitor.IsScratch = classExhibitorScratch.IsScratch;
                classExhibitor.ModifiedBy = actionBy;
                classExhibitor.ModifiedDate = DateTime.Now;
                _exhibitorClassRepositor.Update(classExhibitor);

                _mainResponse.Success = true;
                _mainResponse.Message = Constants.CLASS_EXHIBITOR_SCRATCH;
            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
            }
            return _mainResponse;
        }
    }
}
