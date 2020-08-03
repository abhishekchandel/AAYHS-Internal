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
    public class SponsorRepository : GenericRepository<Sponsors>, ISponsorRepository
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

        public SponsorRepository(AAYHSDBContext ObjContext, IMapper Mapper) : base(ObjContext)
        {
            _MainResponse = new MainResponse();
            _context = ObjContext;
            _Mapper = Mapper;
        }
        public SponsorResponse GetSponsorById(GetSponsorRequest request)
        {
            var sponsorResponse = (from sponsor in _context.Sponsors
                                   join address in _context.Addresses
                                        on sponsor.AddressId equals address.AddressId
                                        into data1
                                   from data in data1.DefaultIfEmpty()
                                   where sponsor.SponsorId == request.SponsorId 
                                   select new SponsorResponse
                                   {
                                       SponsorId = sponsor.SponsorId,
                                       SponsorName = sponsor.SponsorName,
                                       ContactName = sponsor.ContactName,
                                       Phone = sponsor.Phone,
                                       Email = sponsor.Email,
                                       AmountReceived = sponsor.AmountReceived,
                                       Address = data != null ? data.Address : "",
                                       ZipCode = data != null ? data.ZipCode : "",
                                       CityId = data != null ? data.CityId : 0,
                                       StateId = data != null ? _context.Cities.Where(x => x.CityId == data.CityId).Select(y => y.StateId).FirstOrDefault() : 0,
                                   }).FirstOrDefault();
            return sponsorResponse;
        }

        public List<SponsorResponse> GetAllSponsor()
        {
            var sponsorResponse = (from sponsor in _context.Sponsors
                                   join address in _context.Addresses
                                        on sponsor.AddressId equals address.AddressId
                                        into data1
                                   from data in data1.DefaultIfEmpty()
                                   where sponsor.IsActive == true && sponsor.IsDeleted == false
                                   select new SponsorResponse
                                   {
                                       SponsorId = sponsor.SponsorId,
                                       SponsorName = sponsor.SponsorName,
                                       ContactName = sponsor.ContactName,
                                       Phone = sponsor.Phone,
                                       Email = sponsor.Email,
                                       AmountReceived = sponsor.AmountReceived,
                                       Address = data != null ? data.Address : "",
                                       ZipCode = data != null ? data.ZipCode : "",
                                       CityId = data != null ? data.CityId : 0,
                                       StateId = data != null ? _context.Cities.Where(x => x.CityId == data.CityId).Select(y => y.StateId).FirstOrDefault() : 0,
                                   }).ToList();
            return sponsorResponse;
        }

        public List<SponsorResponse> GetAllSponsorsWithFilter(BaseRecordFilterRequest request)
        {
            List<SponsorResponse> sponsorResponse = (from sponsor in _context.Sponsors
                                   join address in _context.Addresses
                                        on sponsor.AddressId equals address.AddressId
                                        into data1
                                   from data in data1.DefaultIfEmpty()
                                   where sponsor.IsActive == true && sponsor.IsDeleted == false
                                   select new SponsorResponse
                                   {
                                       SponsorId = sponsor.SponsorId,
                                       SponsorName = sponsor.SponsorName,
                                       ContactName = sponsor.ContactName,
                                       Phone = sponsor.Phone,
                                       Email = sponsor.Email,
                                       AmountReceived = sponsor.AmountReceived,
                                       Address = data != null ? data.Address : "",
                                       ZipCode = data != null ? data.ZipCode : "",
                                       CityId = data != null ? data.CityId : 0,
                                       StateId = data != null ? _context.Cities.Where(x => x.CityId == data.CityId).Select(y => y.StateId).FirstOrDefault() : 0,
                                   }).ToList();
           
            if (sponsorResponse.Count > 0)
            {
                var propertyInfo = typeof(SponsorResponse).GetProperty(request.OrderBy);
                if (request.OrderByDescending == true)
                {
                    sponsorResponse = sponsorResponse.OrderByDescending(s => s.GetType().GetProperty(request.OrderBy).GetValue(s)).ToList();
                }
                else
                {
                    sponsorResponse = sponsorResponse.AsEnumerable().OrderBy(s => propertyInfo.GetValue(s, null)).ToList();
                }

                if (request.AllRecords == true)
                {
                    sponsorResponse = sponsorResponse.ToList();
                }
                else
                {
                    sponsorResponse = sponsorResponse.Skip((request.Page - 1) * request.Limit).Take(request.Limit).ToList();
                }
            }

            return sponsorResponse;
        }

        public List<SponsorResponse> SearchSponsor(SearchRequest searchRequest)
        {
            var sponsorResponse = (from sponsor in _context.Sponsors
                                   join address in _context.Addresses
                                        on sponsor.AddressId equals address.AddressId
                                        into data1
                                   from data in data1.DefaultIfEmpty()
                                   where sponsor.IsActive == true && sponsor.IsDeleted == false
                                   && ((searchRequest.SearchTerm != string.Empty ?Convert.ToString(sponsor.SponsorId).Contains(searchRequest.SearchTerm) : (1 == 1))
                                   || (searchRequest.SearchTerm != string.Empty ? sponsor.SponsorName.Contains(searchRequest.SearchTerm) : (1 == 1)))
                                   select new SponsorResponse
                                   {
                                       SponsorId = sponsor.SponsorId,
                                       SponsorName = sponsor.SponsorName,
                                       ContactName = sponsor.ContactName,
                                       Phone = sponsor.Phone,
                                       Email = sponsor.Email,
                                       AmountReceived = sponsor.AmountReceived,
                                       Address = data != null ? data.Address : "",
                                       ZipCode = data != null ? data.ZipCode : "",
                                       CityId = data != null ? data.CityId : 0,
                                       StateId = data != null ? _context.Cities.Where(x => x.CityId == data.CityId).Select(y => y.StateId).FirstOrDefault() : 0,
                                   }).ToList();
            return sponsorResponse;
        }

    }
}
