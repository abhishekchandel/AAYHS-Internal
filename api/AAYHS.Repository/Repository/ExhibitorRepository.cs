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
        public MainResponse GetAllExhibitors()
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
            exhibitorListResponses.exhibitorResponses = exhibitorResponses.ToList();
            _mainResponse.ExhibitorListResponse = exhibitorListResponses;
            return _mainResponse;
        }
        public MainResponse GetAllExhibitorsWithFilters(BaseRecordFilterRequest request)
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
                var propertyInfo = typeof(ExhibitorResponse).GetProperty(request.OrderBy);
                if (request.OrderByDescending == true)
                {
                    exhibitorResponses = exhibitorResponses.OrderByDescending(s => s.GetType().GetProperty(request.OrderBy).GetValue(s)).ToList();
                }
                else
                {
                    exhibitorResponses = exhibitorResponses.AsEnumerable().OrderBy(s => propertyInfo.GetValue(s, null)).ToList();
                }

                if (request.AllRecords == true)
                {
                    exhibitorResponses = exhibitorResponses.ToList();
                }
                else
                {
                    exhibitorResponses = exhibitorResponses.Skip((request.Page - 1) * request.Limit).Take(request.Limit).ToList();
                }
            }
            exhibitorListResponses.exhibitorResponses = exhibitorResponses.ToList();
            _mainResponse.ExhibitorListResponse = exhibitorListResponses;
            return _mainResponse;
        }
    }
}
