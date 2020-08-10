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
        private MainResponse _MainResponse;
        #endregion

        #region public
        public AAYHSDBContext _context;
        #endregion

        public SponsorExhibitorRepository(AAYHSDBContext ObjContext, IMapper Mapper) : base(ObjContext)
        {
            _MainResponse = new MainResponse();
            _context = ObjContext;
            _Mapper = Mapper;
        }

        public List<SponsorExhibitorResponse> GetSponsorExhibitorBySponsorId(GetSponsorExhibitorRequest request)
        {

            var data = (from sponsorexhibitor in _context.SponsorExhibitor
                        join exhibitor in _context.Exhibitors
                        on sponsorexhibitor.ExhibitorId equals exhibitor.ExhibitorId
                        into data1
                        from data2 in data1.DefaultIfEmpty()
                        where sponsorexhibitor.SponsorId == request.SponsorId
                        && sponsorexhibitor.IsActive == true && sponsorexhibitor.IsDeleted == false
                        select new SponsorExhibitorResponse
                        {
                            SponsorExhibitorId = sponsorexhibitor.SponsorExhibitorId,
                            SponsorId = sponsorexhibitor.SponsorId,
                            ExhibitorId = data2!=null? data2.ExhibitorId:0,
                            FirstName = data2 != null ? data2.FirstName:"",
                            LastName = data2 != null ? data2.LastName:"",
                            BirthYear = data2 != null ? data2.BirthYear:0,
                            SponsorTypeId = sponsorexhibitor.SponsorTypeId,
                            TypeId = sponsorexhibitor.TypeId,
                            IdNumber = sponsorexhibitor.SponsorTypeId == (int)SponsorTypes.Class ? Convert.ToString(_context.Classes.Where(x => x.ClassId == sponsorexhibitor.TypeId).Select(x => x.ClassNumber).FirstOrDefault())
                                       : (sponsorexhibitor.SponsorTypeId == (int)SponsorTypes.Add ? Convert.ToString(_context.Advertisements.Where(x => x.AdvertisementId == sponsorexhibitor.TypeId).Select(x => x.AdvertisementId).FirstOrDefault())
                                       : Convert.ToString(0)),
                        }).ToList();


            return data;
        }

    }
}
