using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Core.Enums;
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
    public class SponsorExhibitorRepository : GenericRepository<SponsorExhibitor>, ISponsorExhibitorRepository
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

        public SponsorExhibitorRepository(AAYHSDBContext ObjContext, IMapper Mapper) : base(ObjContext)
        {
            _mainResponse = new MainResponse();
            _context = ObjContext;
            _Mapper = Mapper;
        }

        public MainResponse GetSponsorExhibitorBySponsorId(int SponsorId)
        {
            IEnumerable<SponsorExhibitorResponse> sponsorExhibitorResponses = null;
            SponsorExhibitorListResponse sponsorExhibitorListResponses = new SponsorExhibitorListResponse();


            var sponsorTypes = (from gcc in _context.GlobalCodeCategories
                                join gc in _context.GlobalCodes on gcc.GlobalCodeCategoryId equals gc.CategoryId
                                where gcc.CategoryName == "SponsorTypes" && gc.IsDeleted == false && gc.IsActive == true
                                select new GlobalCodeResponse
                                {
                                    GlobalCodeId = gc.GlobalCodeId,
                                    CodeName = (gc.CodeName == null ? "" : gc.CodeName),
                                    Description = (gc.Description == null ? String.Empty : gc.Description),
                                    GlobalCodeCategory = gcc.CategoryName,
                                    CategoryId = gc.CategoryId,
                                }).ToList();
            var adSponsorTypeId = 0;
            var classSponsorTypeId = 0;
            if (sponsorTypes != null && sponsorTypes.Count > 0)
            {
                adSponsorTypeId = sponsorTypes.Where(x => x.CodeName == "Ad").Select(x => x.GlobalCodeId).FirstOrDefault();
                classSponsorTypeId = sponsorTypes.Where(x => x.CodeName == "Class").Select(x => x.GlobalCodeId).FirstOrDefault();
            }




            sponsorExhibitorResponses = (from sponsorexhibitor in _context.SponsorExhibitor
                        join exhibitor in _context.Exhibitors
                        on sponsorexhibitor.ExhibitorId equals exhibitor.ExhibitorId
                        where sponsorexhibitor.SponsorId == SponsorId
                        && sponsorexhibitor.IsActive == true && sponsorexhibitor.IsDeleted == false
                        && exhibitor.IsActive == true && exhibitor.IsDeleted == false
                                         select new SponsorExhibitorResponse
                                         {
                                             SponsorExhibitorId = sponsorexhibitor.SponsorExhibitorId,
                                             SponsorId = sponsorexhibitor.SponsorId,
                                             ExhibitorId = exhibitor.ExhibitorId,
                                             FirstName = exhibitor.FirstName,
                                             LastName = exhibitor.LastName,
                                             BirthYear = exhibitor.BirthYear,
                                             SponsorTypeId = sponsorexhibitor.SponsorTypeId,
                                             AdTypeId = sponsorexhibitor.AdTypeId,
                                             SponsorTypeName = (from code in _context.GlobalCodes where code.GlobalCodeId == sponsorexhibitor.SponsorTypeId select code.CodeName).FirstOrDefault(),
                                             AdTypeName = (from fee in _context.YearlyMaintainenceFee where fee.YearlyMaintainenceFeeId == sponsorexhibitor.AdTypeId select fee.FeeName).FirstOrDefault(),
                                             IdNumber = sponsorexhibitor.SponsorTypeId == Convert.ToInt32(classSponsorTypeId) ? Convert.ToString(_context.Classes.Where(x => x.ClassId == Convert.ToInt32(sponsorexhibitor.TypeId)).Select(x => x.ClassNumber).FirstOrDefault())
                                       : (sponsorexhibitor.SponsorTypeId == Convert.ToInt32(adSponsorTypeId) ? sponsorexhibitor.TypeId
                                       : Convert.ToString(0)),
                                         }).ToList();
            sponsorExhibitorListResponses.SponsorExhibitorResponses = sponsorExhibitorResponses.ToList();
            _mainResponse.SponsorExhibitorListResponse = sponsorExhibitorListResponses;
            _mainResponse.SponsorExhibitorListResponse.UnassignedSponsorExhibitor = GetUnassignedSponsorExhibitorBySponsorId(sponsorExhibitorResponses.ToList());
            return _mainResponse;
        }

        public List<UnassignedSponsorExhibitor> GetUnassignedSponsorExhibitorBySponsorId(List<SponsorExhibitorResponse> sponsorExhibitor)
        {
           
            List<UnassignedSponsorExhibitor> list = new List<UnassignedSponsorExhibitor>();

            var exhibitorlist = (from exhibitor in _context.Exhibitors
                                 where exhibitor.IsActive==true && exhibitor.IsDeleted==false
                                 select new UnassignedSponsorExhibitor
                                 {
                                     ExhibitorId = exhibitor.ExhibitorId,
                                     Name = exhibitor.FirstName + ' ' + exhibitor.LastName
                                 }).OrderBy(x=>x.Name).ToList();

            if (sponsorExhibitor != null && sponsorExhibitor.Count() > 0)
            {
                foreach (var exb in exhibitorlist)
                {
                    var count = sponsorExhibitor.Where(x => x.ExhibitorId == exb.ExhibitorId).Count();
                    if (count <= 0 && !list.Contains(exb))
                    {
                        list.Add(exb);
                    }
                }
            }
            else
            {
                list = exhibitorlist;
            }

            return list;
        }

    }
}
