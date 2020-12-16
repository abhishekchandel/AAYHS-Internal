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
    public class SponsorDistributionRepository : GenericRepository<SponsorDistributions>, ISponsorDistributionRepository
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

        public SponsorDistributionRepository(AAYHSDBContext ObjContext, IMapper Mapper) : base(ObjContext)
        {
            _mainResponse = new MainResponse();
            _context = ObjContext;
            _Mapper = Mapper;
        }

        public MainResponse GetSponsorDistributionBySponsorId(int SponsorId)
        {
            IEnumerable<SponsorDistributionResponse> sponsorDistributionResponses = null;
            SponsorDistributionListResponse sponsorDistributionListResponses = new SponsorDistributionListResponse();


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


            sponsorDistributionResponses = (from sponsordistribution in _context.SponsorDistributions

                                            where sponsordistribution.SponsorId == SponsorId
                                            && sponsordistribution.IsActive == true && sponsordistribution.IsDeleted == false

                                            select new SponsorDistributionResponse
                                            {
                                                SponsorDistributionId = sponsordistribution.SponsorDistributionId,
                                                SponsorId = sponsordistribution.SponsorId,
                                                TotalDistribute = sponsordistribution.TotalDistribute,
                                                SponsorTypeId = sponsordistribution.SponsorTypeId,
                                                AdTypeId = sponsordistribution.AdTypeId,
                                                SponsorTypeName = (from code in _context.GlobalCodes where code.GlobalCodeId == sponsordistribution.SponsorTypeId select code.CodeName).FirstOrDefault(),
                                                AdTypeName = (from fee in _context.YearlyMaintainenceFee where fee.YearlyMaintainenceFeeId == sponsordistribution.AdTypeId select fee.FeeName).FirstOrDefault(),
                                                IdNumber = sponsordistribution.SponsorTypeId == Convert.ToInt32(classSponsorTypeId) ? Convert.ToString(_context.Classes.Where(x => x.ClassId == Convert.ToInt32(sponsordistribution.TypeId)).Select(x => x.ClassNumber).FirstOrDefault())
                                          : (sponsordistribution.SponsorTypeId == Convert.ToInt32(adSponsorTypeId) ? sponsordistribution.TypeId
                                          : Convert.ToString(0)),
                                            }).ToList();
            sponsorDistributionListResponses.SponsorDistributionResponses = sponsorDistributionResponses.ToList();

            decimal totalSponsorAmount = 0;
            var sponsor =(from sponser in  _context.Sponsors where sponser.SponsorId == SponsorId select sponser).FirstOrDefault();
            if(sponsor!=null && sponsor.SponsorId>0)
            {
                totalSponsorAmount =Convert.ToDecimal(sponsor.AmountReceived);
            }
         
            decimal sponsorexhibitorPaid = _context.SponsorExhibitor.Where(x => x.SponsorId == SponsorId && x.IsActive == true).Select(x => x.SponsorAmount).Sum();
            decimal sponsordistributionPaid = _context.SponsorDistributions.Where(x => x.SponsorId == SponsorId && x.IsActive == true).Select(x => x.TotalDistribute).Sum();
            decimal remainedSponsorAmount =totalSponsorAmount - (sponsorexhibitorPaid +  sponsordistributionPaid);


            sponsorDistributionListResponses.TotalSponsorAmount = Convert.ToDecimal(totalSponsorAmount);
            sponsorDistributionListResponses.TotalSponsorExhibitorPaid = sponsorexhibitorPaid;
            sponsorDistributionListResponses.TotalSponsorDistributionPaid = sponsordistributionPaid;
            sponsorDistributionListResponses.RemainedSponsorAmount = remainedSponsorAmount;

            _mainResponse.SponsorDistributionListResponse = sponsorDistributionListResponses;
            return _mainResponse;
        }

       
    }
}
