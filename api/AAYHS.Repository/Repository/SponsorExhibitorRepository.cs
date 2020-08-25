using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
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
                            IdNumber =sponsorexhibitor.SponsorTypeId== (int)SponsorTypes.Class?Convert.ToString(_context.Classes.Where(x=>x.ClassId==Convert.ToInt32(sponsorexhibitor.TypeId)).Select(x=>x.ClassNumber).FirstOrDefault())
                                       :(sponsorexhibitor.SponsorTypeId == (int)SponsorTypes.Add? sponsorexhibitor.TypeId
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
                                 }).ToList();

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
