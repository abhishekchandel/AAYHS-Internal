using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
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
                                     && classes.IsDeleted ==false
                                     select new SponsorClassResponse
                                     {
                                         ClassSponsorId = sponsorClass.ClassSponsorId,
                                         SponsorId = sponsorClass.SponsorId,
                                         ClassId = sponsorClass.ClassId,
                                         ClassNumber = classes.ClassNumber,
                                         Name = classes.Name,
                                         AgeGroup = classes.AgeGroup,
                                         ExhibitorId =Convert.ToInt32((from classexhibitor in _context.ExhibitorClass
                                                                       where classexhibitor.ClassId == sponsorClass.ClassId
                                                                       && classexhibitor.IsActive==true && classexhibitor.IsDeleted==false
                                                                       select classexhibitor.ExhibitorId).FirstOrDefault()),

                                         HorseId = Convert.ToInt32((from classexhibitor in _context.ExhibitorClass
                                                                    where classexhibitor.ClassId == sponsorClass.ClassId
                                                                     && classexhibitor.IsActive == true && classexhibitor.IsDeleted == false
                                                                    select classexhibitor.HorseId).FirstOrDefault()),

                                         ExhibitorName = (from classexhibitor in _context.ExhibitorClass join
                                                          exhibitor in _context.Exhibitors 
                                                          on classexhibitor.ExhibitorId equals exhibitor.ExhibitorId
                                                          where classexhibitor.ClassId == sponsorClass.ClassId
                                                           && classexhibitor.IsActive == true && classexhibitor.IsDeleted == false
                                                          select exhibitor.FirstName+ ' '+ exhibitor.LastName).FirstOrDefault(),

                                         HorseName = (from classexhibitor in _context.ExhibitorClass join
                                                     horse in _context.Horses on classexhibitor.HorseId equals horse.HorseId
                                                        where classexhibitor.ClassId == sponsorClass.ClassId
                                                         && classexhibitor.IsActive == true && classexhibitor.IsDeleted == false
                                                      select horse.Name).FirstOrDefault(),

                                     }).ToList();
            sponsorClassesListResponse.sponsorClassesListResponses = sponsorClassResponses.ToList();
            _mainResponse.SponsorClassesListResponse = sponsorClassesListResponse;

            return _mainResponse;
        }
    }
}
