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

        public  MainResponse GetAllClasses(ClassRequest classRequest)
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
                      Entries= classes != null ? _ObjContext.ExhibitorClass.Where(x=>x.ClassId== classes.ClassId).Select(x=>x.ExhibitorClassId).Count():0,
                      AgeGroup= classes.AgeGroup,
                                                  
                  });
            if (data.Count()!=0)
            {
                if (classRequest.OrderByDescending == true)
                {
                    data = data.OrderByDescending(x => x.GetType().GetProperty(classRequest.OrderBy).GetValue(x));
                }
                else
                {
                    data = data.OrderBy(x => x.GetType().GetProperty(classRequest.OrderBy).GetValue(x));
                }

                if (classRequest.AllRecords)
                {
                    getAllClasses.classResponses = data.ToList();
                }
                else
                {
                    getAllClasses.classResponses = data.Skip((classRequest.Page - 1) * classRequest.Limit).Take(classRequest.Limit).ToList();

                }
                _MainResponse.GetAllClasses = getAllClasses;
               
            }
            return _MainResponse;
        }
        public MainResponse GetClass(ClassRequest classRequest)
        {
            IEnumerable<GetClass> data;
            GetClass getClass = new GetClass();

            data = (from classes in _ObjContext.Classes
                    join scheduleDate in _ObjContext.ScheduleDates on classes.ClassId equals scheduleDate.ClassId into scheduleDate1
                    from scheduleDate2 in scheduleDate1.DefaultIfEmpty()
                    where classes.IsActive == true && classes.IsDeleted == false && classes.ClassId == classRequest.ClassId
                    select new GetClass 
                    {
                        ClassId= classes.ClassId,
                        ClassNumber= classes.ClassNumber,
                        Name= classes.Name,
                        AgeGroup= classes.AgeGroup,
                        Location= classes!=null?classes.Location:"",
                        ScheduleDate= scheduleDate2.Date,
                        SchedulTime= scheduleDate2.Time
                    });
            if (data.Count()!=0)
            {
                 if (classRequest.OrderByDescending == true)
                {
                    data = data.OrderByDescending(x => x.GetType().GetProperty(classRequest.OrderBy).GetValue(x));
                }
                else
                {
                    data = data.OrderBy(x => x.GetType().GetProperty(classRequest.OrderBy).GetValue(x));
                }

                if (classRequest.AllRecords)
                {
                    getClass = data.FirstOrDefault();
                }
                else
                {
                    getClass = data.Skip((classRequest.Page - 1) * classRequest.Limit).Take(classRequest.Limit).FirstOrDefault();

                }
                _MainResponse.GetClass = getClass;
               
            }
            return _MainResponse;
        }               
        public MainResponse GetClassExhibitors(ClassRequest classRequest)
        {
            IEnumerable<GetClassExhibitor> data;
            GetAllClassExhibitor getAllClassExhibitor = new GetAllClassExhibitor();

            data = (from exhibitorclasses in _ObjContext.ExhibitorClass
                    join exhibitors in _ObjContext.Exhibitors on exhibitorclasses.ExhibitorId equals exhibitors.ExhibitorId
                    join horses in _ObjContext.Horses on exhibitorclasses.HorseId equals horses.HorseId
                    join paymentdetails in _ObjContext.ExhibitorPaymentDetails on exhibitorclasses.ExhibitorId equals paymentdetails.ExhibitorId
                    join f in _ObjContext.Fees on paymentdetails.FeeId equals f.FeeId
                    where exhibitorclasses.IsDeleted == false && exhibitors.IsDeleted == false && exhibitorclasses.IsActive == true && exhibitors.IsActive == true &&
                    exhibitorclasses.ClassId== classRequest.ClassId
                    select new GetClassExhibitor
                    {
                        ExhibitorClassId= exhibitorclasses.ExhibitorClassId,
                        Exhibitor= exhibitors.BackNumber+ " " + exhibitors.FirstName + " " + exhibitors.LastName,
                        Horse= horses.Name,
                        BirthYear= exhibitors.BirthYear,
                        AmountPaid= paymentdetails.Amount,
                        AmountDue= ((int)(Convert.ToDecimal(f.FeeAmount)- paymentdetails.Amount)),
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

                if (classRequest.AllRecords)
                {
                    getAllClassExhibitor.getClassExhibitors = data.ToList();
                }
                else
                {
                    getAllClassExhibitor.getClassExhibitors = data.Skip((classRequest.Page - 1) * classRequest.Limit).Take(classRequest.Limit).ToList();

                }
                _MainResponse.GetAllClassExhibitor = getAllClassExhibitor;

            }
            return _MainResponse;
        }      
        public List< GetBackNumber> GetBackNumberForAllExhibitor(int ClassId)
        {
            var backNumber = (from exhibitorsClass in _ObjContext.ExhibitorClass
                              join exhibitors in _ObjContext.Exhibitors on exhibitorsClass.ExhibitorId equals exhibitors.ExhibitorId
                              where exhibitorsClass.IsActive == true && exhibitorsClass.IsDeleted == false
                              && exhibitorsClass.ClassId == ClassId
                              select new GetBackNumber
                              {
                                  BackNumber = exhibitors.BackNumber
                              }).ToList();
            return backNumber;
        }
        public ResultExhibitorDetails GetResultExhibitorDetails(ResultExhibitorRequest resultExhibitorRequest)
        {
            var exhibitor = (from exhibitors in _ObjContext.Exhibitors
                             join addresses in _ObjContext.Addresses on exhibitors.AddressId equals addresses.AddressId
                             join city in _ObjContext.Cities on addresses.CityId equals city.CityId
                             join state in _ObjContext.States on city.StateId equals state.StateId
                             join exhibitorsClass in _ObjContext.ExhibitorClass on exhibitors.ExhibitorId equals exhibitorsClass.ExhibitorId
                             join paymentdetails in _ObjContext.ExhibitorPaymentDetails on exhibitorsClass.ExhibitorId equals paymentdetails.ExhibitorId
                             join f in _ObjContext.Fees on paymentdetails.FeeId equals f.FeeId
                             where exhibitors.IsActive == true && exhibitors.IsDeleted == false &&
                             exhibitors.BackNumber == resultExhibitorRequest.BackNumber && exhibitorsClass.ClassId == resultExhibitorRequest.ClassId
                             && exhibitorsClass.IsActive == true && exhibitorsClass.IsDeleted == false
                             select new ResultExhibitorDetails
                             {
                                 ExhibitorId=exhibitors.ExhibitorId,
                                 ExhibitorName = exhibitors.FirstName + " " + exhibitors.LastName,
                                 BirthYear = exhibitors.BirthYear,
                                 HorseName = _ObjContext.Horses.Where(x => x.HorseId == exhibitorsClass.HorseId).Select(x => x.Name).FirstOrDefault(),
                                 Address= city.Name+", "+state.Code,
                                 AmountPaid= paymentdetails.Amount,
                                 AmountDue= ((int)(Convert.ToDecimal(f.FeeAmount) - paymentdetails.Amount))

                             }).FirstOrDefault();

            return exhibitor;
        }
    }
}
