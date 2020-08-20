using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Data.DBContext;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace AAYHS.Repository.Repository
{
   public class AdvertisementRepository : GenericRepository<Advertisements>, IAdvertisementRepository
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

        public AdvertisementRepository(AAYHSDBContext ObjContext, IMapper Mapper) : base(ObjContext)
        {
            _MainResponse = new MainResponse();
            _context = ObjContext;
            _Mapper = Mapper;
        }
        public MainResponse GetAllAdvertisements(BaseRecordFilterRequest request)
        {

            IEnumerable<AdvertisementResponse> advertisementResponses;
            AdvertisementListResponse advertisementListResponse = new AdvertisementListResponse();
            advertisementResponses = (from ad in _context.Advertisements
                                where ad.IsActive == true
                                && ad.IsDeleted == false
                                select new AdvertisementResponse
                                {
                                    AdvertisementId = ad.AdvertisementId,
                                    AdvertisementTypeId = ad.AdvertisementTypeId,
                                    AdvertisementSizeId = ad.AdvertisementSizeId,
                                    AdvertisementNumberId = ad.AdvertisementNumberId,
                                    Name = ad.Name,
                                    Comments = ad.Comments,
                                }).ToList();

            if (advertisementResponses.Count() > 0)
            {
                var propertyInfo = typeof(SponsorResponse).GetProperty(request.OrderBy);
                if (request.OrderByDescending == true)
                {
                    advertisementResponses = advertisementResponses.OrderByDescending(s => s.GetType().GetProperty(request.OrderBy).GetValue(s)).ToList();
                }
                else
                {
                    advertisementResponses = advertisementResponses.AsEnumerable().OrderBy(s => propertyInfo.GetValue(s, null)).ToList();
                }
                advertisementListResponse.TotalRecords = advertisementResponses.Count();
                if (request.AllRecords == true)
                {
                    advertisementResponses = advertisementResponses.ToList();
                }
                else
                {
                    advertisementResponses = advertisementResponses.Skip((request.Page - 1) * request.Limit).Take(request.Limit).ToList();
                }
            }
            advertisementListResponse.advertisementResponses = advertisementResponses.ToList();
            _MainResponse.AdvertisementListResponse = advertisementListResponse;
            return _MainResponse;
        }

    }
}
