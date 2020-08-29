using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Data.DBContext;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AutoMapper;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace AAYHS.Repository.Repository
{
    public class ClassRepository : GenericRepository<Classes>, IClassRepository
    {

        #region readonly
        private readonly AAYHSDBContext _ObjContext;
        private IMapper _Mapper;
        #endregion

        #region private 
        private MainResponse _MainResponse;
        #endregion

        public ClassRepository(AAYHSDBContext ObjContext, IMapper Mapper) :base(ObjContext)
        {
            _ObjContext = ObjContext;
            _Mapper = Mapper;
            _MainResponse = new MainResponse();
        }

        public GetAllClasses GetAllClasses(ClassRequest classRequest)
        {
            IEnumerable<ClassResponse> data;
            GetAllClasses getAllClasses = new GetAllClasses();

            data=(from classes in _ObjContext.Classes                  
                  where classes.IsDeleted == false && classes.IsActive==true                          
                  select new ClassResponse
                  { 
                      ClassId= classes.ClassId,
                      ClassNumber= classes.ClassNumber,
                      Name= classes.Name,
                      Entries= classes != null ? _ObjContext.ExhibitorClass.Where(x=>x.ClassId== classes.ClassId && x.IsActive==true && x.IsDeleted==false).Select(x=>x.ExhibitorClassId).Count():0,
                      AgeGroup= classes.AgeGroup,
                                                  
                  });
            if (data.Count()!=0)
            {
                if (classRequest.SearchTerm!=null && classRequest.SearchTerm!="")
                {
                    data = data.Where(x => x.ClassNumber.Contains(classRequest.SearchTerm) || x.Name.ToLower().Contains(classRequest.SearchTerm.ToLower()) ||
                                     Convert.ToString(x.Entries).Contains(classRequest.SearchTerm));
                }
                if (classRequest.OrderByDescending == true)
                {
                    data = data.OrderByDescending(x => x.GetType().GetProperty(classRequest.OrderBy).GetValue(x));
                }
                else
                {
                    data = data.OrderBy(x => x.GetType().GetProperty(classRequest.OrderBy).GetValue(x));
                }
                getAllClasses.TotalRecords = data.Count();
                if (classRequest.AllRecords)
                {
                    getAllClasses.classesResponse = data.ToList();
                }
                else
                {
                    getAllClasses.classesResponse = data.Skip((classRequest.Page - 1) * classRequest.Limit).Take(classRequest.Limit).ToList();

                }
                               
            }
            return getAllClasses;
        }
        public GetClass GetClass(int ClassId)
        {
            IEnumerable<ClassResponse> data;
            GetClass getClass = new GetClass();

            data = (from classes in _ObjContext.Classes
                    join scheduleDate in _ObjContext.ScheduleDates on classes.ClassId equals scheduleDate.ClassId into scheduleDate1
                    from scheduleDate2 in scheduleDate1.DefaultIfEmpty()
                    join splitClass in _ObjContext.ClassSplits on classes.ClassId equals splitClass.ClassId into splitClass1
                    from splitClass2 in splitClass1.DefaultIfEmpty()                                    
                    where classes.IsActive == true && classes.IsDeleted == false 
                    && classes.ClassId == ClassId
                    select new ClassResponse
                    {
                        ClassId= classes.ClassId,
                        ClassHeaderId=classes.ClassHeaderId,
                        ClassNumber= classes.ClassNumber,
                        Name= classes.Name,
                        AgeGroup= classes.AgeGroup,
                        Location= classes!=null?classes.Location:"",
                        ScheduleDate= scheduleDate2!=null? scheduleDate2.Date:null,
                        SchedulTime= scheduleDate2 != null ? scheduleDate2.Time:null,
                        SplitNumber= splitClass2!=null?splitClass2.SplitNumber:0,
                        ChampionShipIndicator= splitClass2 != null ? splitClass2.ChampionShipIndicator:false,
                        getClassSplit = (from splitClass in _ObjContext.ClassSplits                                     
                                        where  splitClass.IsActive==true && splitClass.IsDeleted==false
                                        && splitClass.ClassId == ClassId
                                        select new GetClassSplit
                                        {                                           
                                           Entries= splitClass.Entries       
                                        }).ToList()
                    });

            if (data.Count() != 0)
            {
                getClass.classResponse = data.ToList();
                getClass.TotalRecords = data.Count();
            }
            return getClass;
        }
        public GetExhibitorAllHorses GetExhibitorHorses(int ExhibitorId)
        {
            IEnumerable<GetExhibitorHorses> data;
            GetExhibitorAllHorses getExhibitorAllHorses = new GetExhibitorAllHorses();

            data = (from exhibitorHorses in _ObjContext.ExhibitorHorse
                    join horses in _ObjContext.Horses on exhibitorHorses.HorseId equals horses.HorseId into horses1
                    from horses2 in horses1.DefaultIfEmpty()
                    where exhibitorHorses.IsActive == true && exhibitorHorses.IsDeleted == false
                    && horses2.IsActive == true && horses2.IsDeleted == false
                    && exhibitorHorses.ExhibitorId == ExhibitorId
                    select new GetExhibitorHorses
                    {
                        HorseId = exhibitorHorses.HorseId,
                        Horse = horses2.Name
                    });

            getExhibitorAllHorses.getExhibitorHorses = data.ToList();
            getExhibitorAllHorses.TotalRecords = data.Count();
            return getExhibitorAllHorses;
        }
        public MainResponse GetClassExhibitorsAndHorses(int ClassId)
        {
            ClassExhibitorHorses classExhibitorHorses = new ClassExhibitorHorses();
            List<string> list = new List<string>();
            var exhibitorClass = (from ce in _ObjContext.ExhibitorClass
                                  where ce.ClassId == ClassId
                                  select ce).ToList();

            foreach (var data in exhibitorClass)
            {
                var exhibitor = (from ex in _ObjContext.Exhibitors where ex.ExhibitorId == data.ExhibitorId && ex.IsDeleted==false select ex).FirstOrDefault();
                if (exhibitor != null)
                {
                    var horses = (from hr in _ObjContext.Horses where  hr.IsDeleted == false select hr).ToList();
                    if (horses != null && horses.Count > 0)
                    {
                        foreach (var horse in horses)
                        {
                            var name = exhibitor.FirstName + ' ' + exhibitor.LastName + '/' + horse.Name;
                            if (!list.Contains(name))
                                list.Add(name);
                        }
                    }
                }

            }

            classExhibitorHorses.ClassExhibitorHorse = list;
            _MainResponse.ClassExhibitorHorses = classExhibitorHorses;
            return _MainResponse;
        }
        public GetAllClassEntries GetClassEntries(ClassRequest classRequest)
        {
            IEnumerable<GetClassEntries> data;
            GetAllClassEntries getAllClassEntries = new GetAllClassEntries(); 

            data = (from exhibitorclasses in _ObjContext.ExhibitorClass
                    join exhibitors in _ObjContext.Exhibitors on exhibitorclasses.ExhibitorId equals exhibitors.ExhibitorId into exhibitors1
                    from exhibitors2 in exhibitors1.DefaultIfEmpty()
                    join horses in _ObjContext.Horses on exhibitorclasses.HorseId equals horses.HorseId into horses1
                    from horses2 in horses1.DefaultIfEmpty()
                    join paymentdetails in _ObjContext.ExhibitorPaymentDetails on exhibitorclasses.ExhibitorId equals paymentdetails.ExhibitorId into paymentdetails1
                    from paymentdetails2 in paymentdetails1.DefaultIfEmpty()
                    join f in _ObjContext.Fees on paymentdetails2.FeeId equals f.FeeId into f1
                    from f2 in f1.DefaultIfEmpty()
                    where exhibitorclasses.IsDeleted == false && exhibitors2.IsDeleted == false && exhibitorclasses.IsActive == true && exhibitors2.IsActive == true &&
                    horses2.IsDeleted==false && horses2.IsActive==true &&
                    exhibitorclasses.ClassId== classRequest.ClassId
                    select new GetClassEntries
                    {
                        ExhibitorClassId= exhibitorclasses.ExhibitorClassId,
                        Exhibitor= exhibitors2.ExhibitorId+ " " + exhibitors2.FirstName + " " + exhibitors2.LastName,
                        Horse= horses2.Name,
                        BirthYear= exhibitors2.BirthYear,
                        AmountPaid= paymentdetails2.Amount,
                        AmountDue= ((int)(Convert.ToDecimal(f2.FeeAmount)- paymentdetails2.Amount)),
                        Scratch=exhibitorclasses.IsScratch
                    });
            if (data.Count() != 0)
            {
                if (classRequest.OrderByDescending == true)
                {   
                    data = data.OrderByDescending(x => x.GetType().GetProperty(classRequest.OrderBy).GetValue(x));
                }
                else
                {
                    data = data.OrderBy(x => x.GetType().GetProperty(classRequest.OrderBy).GetValue(x));
                }
                getAllClassEntries.TotalRecords= data.Count();
                if (classRequest.AllRecords)
                {
                    getAllClassEntries.getClassEntries = data.ToList();
                }
                else
                {
                    getAllClassEntries.getClassEntries = data.Skip((classRequest.Page - 1) * classRequest.Limit).Take(classRequest.Limit).ToList();

                }
                             
            }
            return getAllClassEntries;
        }      
        public List< GetBackNumber> GetBackNumberForAllExhibitor(int ClassId)
        {
            var backNumber = (from exhibitorsClass in _ObjContext.ExhibitorClass
                              join exhibitors in _ObjContext.Exhibitors on exhibitorsClass.ExhibitorId equals exhibitors.ExhibitorId into exhibitors1
                              from exhibitors2 in exhibitors1.DefaultIfEmpty()
                              where exhibitorsClass.IsActive == true && exhibitorsClass.IsDeleted == false
                              && exhibitors2.IsActive==true && exhibitors2.IsDeleted==false
                              && exhibitorsClass.ClassId == ClassId
                              select new GetBackNumber
                              {
                                  BackNumber = exhibitors2.BackNumber
                              }).ToList();
            return backNumber;
        }
        public ResultExhibitorDetails GetResultExhibitorDetails(ResultExhibitorRequest resultExhibitorRequest)
        {
            var exhibitor = (from exhibitors in _ObjContext.Exhibitors
                             join addresses in _ObjContext.Addresses on exhibitors.AddressId equals addresses.AddressId into addresses1
                             from addresses2 in addresses1.DefaultIfEmpty()
                             join city in _ObjContext.Cities on addresses2.CityId equals city.CityId into city1
                             from city2 in city1.DefaultIfEmpty()
                             join state in _ObjContext.States on city2.StateId equals state.StateId into state1
                             from state2 in state1.DefaultIfEmpty()
                             join exhibitorsClass in _ObjContext.ExhibitorClass on exhibitors.ExhibitorId equals exhibitorsClass.ExhibitorId into exhibitorsClass1
                             from exhibitorsClass2 in exhibitorsClass1.DefaultIfEmpty()
                             join paymentdetails in _ObjContext.ExhibitorPaymentDetails on exhibitorsClass2.ExhibitorId equals paymentdetails.ExhibitorId into paymentdetails1
                             from paymentdetails2 in paymentdetails1.DefaultIfEmpty()
                             join f in _ObjContext.Fees on paymentdetails2.FeeId equals f.FeeId into f1
                             from f2 in f1.DefaultIfEmpty()
                             where exhibitors.IsActive == true && exhibitors.IsDeleted == false &&
                             exhibitors.BackNumber == resultExhibitorRequest.BackNumber && exhibitorsClass2.ClassId == resultExhibitorRequest.ClassId
                             && exhibitorsClass2.IsActive == true && exhibitorsClass2.IsDeleted == false
                             && addresses2.IsDeleted==false && city2.IsDeleted==false && state2.IsDeleted==false
                             select new ResultExhibitorDetails
                             {
                                 ExhibitorId=exhibitors.ExhibitorId,
                                 ExhibitorName = exhibitors.FirstName + " " + exhibitors.LastName,
                                 BirthYear = exhibitors.BirthYear,
                                 HorseName = _ObjContext.Horses.Where(x => x.HorseId == exhibitorsClass2.HorseId && x.IsActive==true && x.IsDeleted==false).Select(x => x.Name).FirstOrDefault(),
                                 Address= city2.Name+", "+ state2.Code,
                                 AmountPaid= paymentdetails2.Amount,
                                 AmountDue= ((int)(Convert.ToDecimal(f2.FeeAmount) - paymentdetails2.Amount))

                             }).FirstOrDefault();

            return exhibitor;
        }
        public GetResult GetResultOfClass(ClassRequest classRequest)
        {
            IEnumerable<GetResultOfClass> data;
            GetResult getResult = new GetResult();

            data = (from result in _ObjContext.Result
                    join exhibitor in _ObjContext.Exhibitors on result.ExhibitorId equals exhibitor.ExhibitorId into exhibitor1
                    from exhibitor2 in exhibitor1.DefaultIfEmpty()
                    join exhibitorsClass in _ObjContext.ExhibitorClass on exhibitor2.ExhibitorId equals exhibitorsClass.ExhibitorId into exhibitorsClass1
                    from exhibitorsClass2 in exhibitorsClass1.DefaultIfEmpty()
                    join addresses in _ObjContext.Addresses on exhibitor2.AddressId equals addresses.AddressId into addresses1
                    from addresses2 in addresses1.DefaultIfEmpty()
                    join city in _ObjContext.Cities on addresses2.CityId equals city.CityId into city1
                    from city2 in city1.DefaultIfEmpty()
                    join state in _ObjContext.States on city2.StateId equals state.StateId into state1
                    from state2 in state1.DefaultIfEmpty()                       
                    join paymentdetails in _ObjContext.ExhibitorPaymentDetails on exhibitor2.ExhibitorId equals paymentdetails.ExhibitorId into paymentdetails1
                    from paymentdetails2 in paymentdetails1.DefaultIfEmpty()
                    join f in _ObjContext.Fees on paymentdetails2.FeeId equals f.FeeId into f1
                    from f2  in f1.DefaultIfEmpty()
                    where result.IsActive == true && result.IsDeleted == false && exhibitor2.IsActive == true && exhibitor2.IsDeleted == false
                    && exhibitorsClass2.IsDeleted==false && addresses2.IsDeleted == false && city2.IsDeleted == false && state2.IsDeleted == false                   
                    && result.ClassId == classRequest.ClassId
                    select new GetResultOfClass
                    {
                        ResultId=result.ResultId,
                        ExhibitorId = exhibitor2.ExhibitorId,
                        Place = result.Placement,
                        BackNumber= exhibitor2.BackNumber,
                        ExhibitorName= exhibitor2.FirstName+" "+ exhibitor2.LastName,
                        BirthYear= exhibitor2.BirthYear,
                        HorseName= _ObjContext.Horses.Where(x => x.HorseId == exhibitorsClass2.HorseId && x.IsDeleted==false).Select(x => x.Name).FirstOrDefault(),
                        Address= city2.Name + ", " + state2.Code,
                        AmountPaid = paymentdetails2.Amount,
                        AmountDue = ((int)(Convert.ToDecimal(f2.FeeAmount) - paymentdetails2.Amount))
                    }).Distinct().ToList();

            if (data.Count() != 0)
            {
                               
                if (classRequest.OrderByDescending == true)
                {
                    data = data.OrderByDescending(x => x.GetType().GetProperty(classRequest.OrderBy).GetValue(x));
                }
                else
                {
                    data = data.OrderBy(x => x.GetType().GetProperty(classRequest.OrderBy).GetValue(x));
                }
                getResult.TotalRecords = data.Count();
                if (classRequest.AllRecords)
                {
                    getResult.getResultOfClass = data.ToList();
                }
                else
                {
                    getResult.getResultOfClass = data.Skip((classRequest.Page - 1) * classRequest.Limit).Take(classRequest.Limit).ToList();

                }
                               
            }
            return getResult;
        }      
    }
}
