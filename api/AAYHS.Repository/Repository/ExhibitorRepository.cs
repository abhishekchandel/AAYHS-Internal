using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Data.DBContext;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AAYHS.Repository.Repository
{
    public class ExhibitorRepository : GenericRepository<Exhibitors>, IExhibitorRepository
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

        public ExhibitorRepository(AAYHSDBContext ObjContext, IMapper Mapper) : base(ObjContext)
        {
            _mainResponse = new MainResponse();
            _context = ObjContext;
            _Mapper = Mapper;
        }

        public ExhibitorListResponse GetAllExhibitors(BaseRecordFilterRequest filterRequest)
        {
            IEnumerable<ExhibitorResponse> exhibitorResponses = null;
            ExhibitorListResponse exhibitorListResponses = new ExhibitorListResponse();
            exhibitorResponses = (from exhibitor in _context.Exhibitors
                                  where exhibitor.IsActive == true && exhibitor.IsDeleted == false
                                  select new ExhibitorResponse
                                  {
                                      ExhibitorId = exhibitor.ExhibitorId,
                                      GroupId = exhibitor.GroupId,
                                      AddressId = exhibitor.AddressId,
                                      FirstName = exhibitor.FirstName,
                                      LastName = exhibitor.LastName,
                                      BackNumber = exhibitor.BackNumber,
                                      BirthYear = exhibitor.BirthYear,
                                      IsNSBAMember = exhibitor.IsNSBAMember,
                                      IsDoctorNote = exhibitor.IsDoctorNote,
                                      QTYProgram = exhibitor.QTYProgram,
                                      PrimaryEmail = exhibitor.PrimaryEmail,
                                      SecondaryEmail = exhibitor.SecondaryEmail,
                                      Phone = exhibitor.Phone,
                                  }).ToList();
            if (exhibitorResponses.Count() > 0)
            {
                if (filterRequest.SearchTerm!=null && filterRequest.SearchTerm!="")
                {
                    exhibitorResponses = exhibitorResponses.Where(x => Convert.ToString(x.ExhibitorId).Contains(filterRequest.SearchTerm) ||
                                         x.FirstName.ToLower().Contains(filterRequest.SearchTerm.ToLower()) || x.LastName.ToLower().Contains(filterRequest.SearchTerm.ToLower()) ||
                                         Convert.ToString(x.BirthYear).Contains(filterRequest.SearchTerm));
                }
                var propertyInfo = typeof(ExhibitorResponse).GetProperty(filterRequest.OrderBy);
                if (filterRequest.OrderByDescending == true)
                {
                    exhibitorResponses = exhibitorResponses.OrderByDescending(s => s.GetType().GetProperty(filterRequest.OrderBy).GetValue(s)).ToList();
                }
                else
                {
                    exhibitorResponses = exhibitorResponses.AsEnumerable().OrderBy(s => propertyInfo.GetValue(s, null)).ToList();
                }
                exhibitorListResponses.TotalRecords = exhibitorResponses.Count();
                if (filterRequest.AllRecords == true)
                {
                    exhibitorListResponses.exhibitorResponses = exhibitorResponses.ToList();
                }
                else
                {
                    exhibitorListResponses.exhibitorResponses = exhibitorResponses.Skip((filterRequest.Page - 1) * filterRequest.Limit).Take(filterRequest.Limit).ToList();
                }
            }
            
            return exhibitorListResponses;
        }

        public ExhibitorListResponse GetExhibitorById(int exhibitorId)
        {
            IEnumerable<ExhibitorResponse> exhibitorResponses = null;
            ExhibitorListResponse exhibitorListResponses = new ExhibitorListResponse();
            exhibitorResponses = (from exhibitor in _context.Exhibitors                                  
                                  join address in _context.Addresses on exhibitor.AddressId equals address.AddressId into address1
                                  from address2 in address1.DefaultIfEmpty()
                                  where exhibitor.IsActive == true && exhibitor.IsDeleted == false                                 
                                  && address2.IsActive==true && address2.IsDeleted==false
                                  && exhibitor.ExhibitorId== exhibitorId
                                  select new ExhibitorResponse 
                                  { 
                                    ExhibitorId=exhibitor.ExhibitorId,
                                    GroupId= exhibitor != null ? _context.Groups.Where(x => x.GroupId == exhibitor.GroupId && x.IsActive == true && x.IsDeleted == false).Select(y => y.GroupId).FirstOrDefault() : 0,
                                    GroupName=exhibitor!=null?_context.Groups.Where(x=>x.GroupId==exhibitor.GroupId && x.IsActive==true && x.IsDeleted==false).Select(y=>y.GroupName).FirstOrDefault():"",
                                    BackNumber=exhibitor.BackNumber,
                                    FirstName=exhibitor.FirstName,
                                    LastName=exhibitor.LastName,
                                    BirthYear=exhibitor.BirthYear,
                                    IsDoctorNote=exhibitor.IsDoctorNote,
                                    IsNSBAMember=exhibitor.IsNSBAMember,
                                    PrimaryEmail=exhibitor.PrimaryEmail,
                                    SecondaryEmail=exhibitor.SecondaryEmail,
                                    Phone=exhibitor.Phone,
                                    QTYProgram=exhibitor.QTYProgram,
                                    AddressId= address2 != null ? address2.AddressId:0,
                                    Address= address2 != null ? address2.Address:"",
                                    ZipCode= address2 != null ? address2.ZipCode:"",
                                    CityId= address2 != null ?address2.CityId:0,
                                    StateId = address2 != null ? _context.Cities.Where(x => x.CityId == address2.CityId).Select(y => y.StateId).FirstOrDefault() : 0,
                                  });
            if (exhibitorResponses.Count()!=0)
            {
                exhibitorListResponses.exhibitorResponses = exhibitorResponses.ToList();
                exhibitorListResponses.TotalRecords = exhibitorResponses.Count();
            }
            return exhibitorListResponses;
        }

        public ExhibitorListResponse SearchExhibitor(SearchRequest searchRequest)
        {
            IEnumerable<ExhibitorResponse> exhibitorResponses = null;
            ExhibitorListResponse exhibitorListResponses = new ExhibitorListResponse();

            exhibitorResponses=(from exhibitor in _context.Exhibitors
                                where exhibitor.IsActive == true && exhibitor.IsDeleted == false
                                 && ((searchRequest.SearchTerm != string.Empty ? Convert.ToString(exhibitor.ExhibitorId).Contains(searchRequest.SearchTerm) : (1 == 1))
                                   || (searchRequest.SearchTerm != string.Empty ? exhibitor.FirstName.Contains(searchRequest.SearchTerm) : (1 == 1))
                                   || (searchRequest.SearchTerm != string.Empty ? exhibitor.LastName.Contains(searchRequest.SearchTerm) : (1 == 1)))
                                select new ExhibitorResponse
                                {
                                    ExhibitorId = exhibitor.ExhibitorId,
                                    GroupId = exhibitor.GroupId,
                                    AddressId = exhibitor.AddressId,
                                    FirstName = exhibitor.FirstName,
                                    LastName = exhibitor.LastName,
                                    BackNumber = exhibitor.BackNumber,
                                    BirthYear = exhibitor.BirthYear,
                                    IsNSBAMember = exhibitor.IsNSBAMember,
                                    IsDoctorNote = exhibitor.IsDoctorNote,
                                    QTYProgram = exhibitor.QTYProgram,
                                    PrimaryEmail = exhibitor.PrimaryEmail,
                                    SecondaryEmail = exhibitor.SecondaryEmail,
                                    Phone = exhibitor.Phone,
                                }).ToList();

            if (exhibitorResponses.Count() > 0)
            {
                var propertyInfo = typeof(SponsorResponse).GetProperty(searchRequest.OrderBy);
                if (searchRequest.OrderByDescending == true)
                {
                    exhibitorResponses = exhibitorResponses.OrderByDescending(s => s.GetType().GetProperty(searchRequest.OrderBy).GetValue(s)).ToList();
                }
                else
                {
                    exhibitorResponses = exhibitorResponses.AsEnumerable().OrderBy(s => propertyInfo.GetValue(s, null)).ToList();
                }
                exhibitorListResponses.TotalRecords = exhibitorResponses.Count();
                if (searchRequest.AllRecords == true)
                {
                    exhibitorResponses = exhibitorResponses.ToList();
                }
                else
                {
                    exhibitorResponses = exhibitorResponses.Skip((searchRequest.Page - 1) * searchRequest.Limit).Take(searchRequest.Limit).ToList();
                }
            }

            exhibitorListResponses.exhibitorResponses = exhibitorResponses.ToList();
            return exhibitorListResponses;
        }

        public ExhibitorHorsesResponse GetExhibitorHorses(int exhibitorId)
        {
            IEnumerable<ExhibitorHorses> exhibitorHorses = null;
            ExhibitorHorsesResponse exhibitorHorsesResponse = new ExhibitorHorsesResponse();

            exhibitorHorses = (from exhibitorHorse in _context.ExhibitorHorse
                               join horse in _context.Horses on exhibitorHorse.HorseId equals horse.HorseId                              
                               where exhibitorHorse.IsActive == true && exhibitorHorse.IsDeleted == false
                               && horse.IsActive == true && horse.IsDeleted == false                               
                               && exhibitorHorse.ExhibitorId == exhibitorId
                               select new ExhibitorHorses 
                               { 
                                 ExhibitorHorseId=exhibitorHorse.ExhibitorHorseId,
                                 HorseName=horse.Name,
                                 HorseType=_context.GlobalCodes.Where(x=>x.GlobalCodeId==horse.HorseTypeId).Select(y=>y.CodeName).First(),
                                 BackNumber= exhibitorHorse.BackNumber
                               });

            if (exhibitorHorses.Count()!=0)
            {
                exhibitorHorsesResponse.exhibitorHorses = exhibitorHorses.ToList();
                exhibitorHorsesResponse.TotalRecords = exhibitorHorses.Count();
            }
            return exhibitorHorsesResponse;
        }
        
        
    }
}
