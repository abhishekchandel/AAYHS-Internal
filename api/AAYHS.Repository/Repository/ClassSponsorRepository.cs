using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Data.DBContext;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;


namespace AAYHS.Repository.Repository
{
   public class ClassSponsorRepository : GenericRepository<ClassSponsors>, IClassSponsorRepository
    {
        #region readonly
        private readonly IMapper _Mapper;
        #endregion

        #region Private
        private MainResponse _mainResponse;
        #endregion

        #region public
        public AAYHSDBContext _context;
        #endregion

        public ClassSponsorRepository(AAYHSDBContext ObjContext, IMapper Mapper) : base(ObjContext)
        {
            _mainResponse = new MainResponse();
            _context = ObjContext;
            _Mapper = Mapper;
        }

        public MainResponse GetAllClassSponsor()
        {
            IEnumerable<ClassSponsorResponse> classSponsorResponses;
            ClassSponsorListResponse classSponsorListResponse = new ClassSponsorListResponse();

            classSponsorResponses = (from classSponsor in _context.ClassSponsors
                                     where classSponsor.IsActive == true && classSponsor.IsDeleted == false
                                select new ClassSponsorResponse
                                {
                                    ClassSponsorId= classSponsor.ClassSponsorId,
                                    SponsorId = classSponsor.SponsorId,
                                    ClassId= classSponsor.ClassId,

                                }).ToList();

            classSponsorListResponse.classSponsorResponses = classSponsorResponses.ToList();
            _mainResponse.ClassSponsorListResponse = classSponsorListResponse;
            return _mainResponse;
        }

        public MainResponse GetAllClassSponsorWithFilters(BaseRecordFilterRequest request)
        {
            IEnumerable<ClassSponsorResponse> classSponsorResponses;
            ClassSponsorListResponse classSponsorListResponse = new ClassSponsorListResponse();

            classSponsorResponses = (from classSponsor in _context.ClassSponsors
                                     where classSponsor.IsActive == true && classSponsor.IsDeleted == false
                                     select new ClassSponsorResponse
                                     {
                                         ClassSponsorId = classSponsor.ClassSponsorId,
                                         SponsorId = classSponsor.SponsorId,
                                         ClassId = classSponsor.ClassId,

                                     }).ToList();

            if (classSponsorResponses.Count() > 0)
            {
                var propertyInfo = typeof(ClassSponsorResponse).GetProperty(request.OrderBy);
                if (request.OrderByDescending == true)
                {
                    classSponsorResponses = classSponsorResponses.OrderByDescending(s => s.GetType().GetProperty(request.OrderBy).GetValue(s)).ToList();
                }
                else
                {
                    classSponsorResponses = classSponsorResponses.AsEnumerable().OrderBy(s => propertyInfo.GetValue(s, null)).ToList();
                }

                if (request.AllRecords == true)
                {
                    classSponsorResponses = classSponsorResponses.ToList();
                }
                else
                {
                    classSponsorResponses = classSponsorResponses.Skip((request.Page - 1) * request.Limit).Take(request.Limit).ToList();
                }
            }


            classSponsorListResponse.classSponsorResponses = classSponsorResponses.ToList();
            _mainResponse.ClassSponsorListResponse = classSponsorListResponse;
            return _mainResponse;
        }

        public MainResponse GetClassSponsorbyId(int ClassSponsorId)
        {
            ClassSponsorResponse classSponsorResponse=new ClassSponsorResponse();
            classSponsorResponse = (from classSponsor in _context.ClassSponsors
                                     where classSponsor.ClassSponsorId==ClassSponsorId
                                     select new ClassSponsorResponse
                                     {
                                         ClassSponsorId = classSponsor.ClassSponsorId,
                                         SponsorId = classSponsor.SponsorId,
                                         ClassId = classSponsor.ClassId,
                                     }).FirstOrDefault();

            _mainResponse.ClassSponsorResponse = classSponsorResponse;
            return _mainResponse;
        }

       public MainResponse GetSponsorClassesbySponsorId(int SponsorId)
        {
            IEnumerable<SponsorClassResponse> sponsorClassResponses;
            SponsorClassesListResponse sponsorClassesListResponse = new SponsorClassesListResponse();

            sponsorClassResponses = (from classes in _context.Classes join sponsorClass
                                     in _context.ClassSponsors on classes.ClassId equals sponsorClass.ClassId
                                     
                                     where sponsorClass.SponsorId==SponsorId
                                     && sponsorClass.IsActive == true && sponsorClass.IsDeleted == false
                                     && classes.IsActive ==true && classes.IsDeleted == false
                                     select new SponsorClassResponse
                                     {
                                         ClassSponsorId = sponsorClass.ClassSponsorId,
                                         SponsorId = sponsorClass.SponsorId,
                                         ClassId = sponsorClass.ClassId,
                                         ClassNumber = classes.ClassNumber,
                                         Name = classes.Name,
                                         AgeGroup = classes.AgeGroup,

                                     }).ToList();
            foreach(var item in sponsorClassResponses)
            {
                item.ClassExhibitorsAndHorses = GetClassExhibitorsAndHorses(item.ClassId, SponsorId);
            }

            sponsorClassesListResponse.sponsorClassesListResponses = sponsorClassResponses.ToList();
            sponsorClassesListResponse.unassignedSponsorClasses = GetUnassignedSponsorclasses(sponsorClassResponses.ToList());
            _mainResponse.SponsorClassesListResponse = sponsorClassesListResponse;

            return _mainResponse;
        }

        public List<string> GetClassExhibitorsAndHorses(int ClassId,int SponsorId)
        {
            List<string> list = new List<string>();
            //var exhibitors = (from ce in _context.ExhibitorClass
            //                      where ce.ClassId == ClassId && ce.IsActive==true && ce.IsDeleted==false
            //                      select ce).ToList();
            var exhibitors = (from se in _context.SponsorExhibitor
                                  where se.SponsorId == SponsorId && se.IsActive == true && se.IsDeleted == false
                                  select se).ToList();

            foreach (var data in exhibitors)
            {
                var exhibitor = (from ex in _context.Exhibitors 
                                 where ex.ExhibitorId == data.ExhibitorId 
                                 && ex.IsActive==true && ex.IsDeleted==false select ex).FirstOrDefault();

                if (exhibitor != null)
                {
                    var horses = (from hr in _context.Horses 
                                  join exbhr in _context.ExhibitorHorse 
                                  on hr.HorseId equals exbhr.HorseId
                                  where exbhr.ExhibitorId== exhibitor.ExhibitorId
                                  && hr.IsActive==true && hr.IsDeleted == false  && exbhr.IsActive == true && exbhr.IsDeleted==false
                                  select hr).ToList();

                    if (horses != null && horses.Count > 0)
                    {
                        foreach (var horse in horses)
                        {
                            var name = exhibitor.FirstName + ' ' + exhibitor.LastName + '/' + horse.Name;
                            if (!list.Contains(name))
                                list.Add(name);
                        }
                    }
                    else
                    {
                        var name = exhibitor.FirstName + ' ' + exhibitor.LastName ;
                        if (!list.Contains(name))
                            list.Add(name);
                    }
                }

            }
            return list;
        }


        public List<UnassignedSponsorClassResponse> GetUnassignedSponsorclasses(List<SponsorClassResponse> sponsorClassResponses)
        {

            List<UnassignedSponsorClassResponse> list = new List<UnassignedSponsorClassResponse>();

            var classlist = (from classes in _context.Classes
                                 where classes.IsActive == true && classes.IsDeleted == false
                                 select new UnassignedSponsorClassResponse
                                 {
                                     ClassId = classes.ClassId,
                                     ClassNumber = classes.ClassNumber,
                                     Name = classes.Name,
                                     AgeGroup = classes.AgeGroup,
                                 }).OrderBy(x=>x.Name).ToList();

            if (sponsorClassResponses != null && sponsorClassResponses.Count() > 0)
            {
                foreach (var cls in classlist)
                {
                    var count = sponsorClassResponses.Where(x => x.ClassId == cls.ClassId).Count();
                    if (count <= 0 && !list.Contains(cls))
                    {
                        list.Add(cls);
                    }
                }
            }
            else
            {
                list = classlist;
            }

            return list;
        }

    }
}
