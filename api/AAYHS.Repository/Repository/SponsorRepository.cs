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
        private MainResponse _mainResponse;
        #endregion

        #region public
        public AAYHSDBContext _context;
        #endregion

        public SponsorRepository(AAYHSDBContext ObjContext, IMapper Mapper) : base(ObjContext)
        {
            _mainResponse = new MainResponse();
            _context = ObjContext;
            _Mapper = Mapper;
        }
        public MainResponse GetSponsorById(int SponsorId)
        {
            SponsorResponse sponsorResponse = new SponsorResponse();
             sponsorResponse = (from sponsor in _context.Sponsors
                                   join address in _context.Addresses
                                        on sponsor.AddressId equals address.AddressId
                                        into data1
                                   from data in data1.DefaultIfEmpty()
                                   where sponsor.SponsorId == SponsorId 
                                   && sponsor.IsActive == true
                                   && sponsor.IsDeleted == false
                                   && data.IsActive==true && 
                                   data.IsDeleted==false
                                   select new SponsorResponse
                                   {
                                       SponsorId = sponsor.SponsorId,
                                       SponsorName = sponsor.SponsorName,
                                       ContactName = sponsor.ContactName,
                                       Phone = sponsor.Phone,
                                       Email = sponsor.Email,
                                       AmountReceived = sponsor.AmountReceived,
                                       Address = data != null ? data.Address : "",
                                       ZipCodeId = data != null ? data.ZipCodeId :0,
                                       CityId = data != null ? data.CityId : 0,
                                       StateId = data != null ? _context.Cities.Where(x => x.CityId == data.CityId).Select(y => y.StateId).FirstOrDefault() : 0,
                                   }).FirstOrDefault();
            _mainResponse.SponsorResponse = sponsorResponse;
            return _mainResponse;
        }


        public MainResponse GetAllSponsors(BaseRecordFilterRequest request)
        {

            IEnumerable<SponsorResponse> sponsorResponses;
            SponsorListResponse sponsorListResponse = new SponsorListResponse();
            sponsorResponses = (from sponsor in _context.Sponsors
                                   join address in _context.Addresses
                                        on sponsor.AddressId equals address.AddressId
                                        into data1
                                   from data in data1.DefaultIfEmpty()
                                   where sponsor.IsActive == true 
                                   && sponsor.IsDeleted == false
                                   && data.IsActive == true &&
                                   data.IsDeleted == false
                                select new SponsorResponse
                                   {
                                       SponsorId = sponsor.SponsorId,
                                       SponsorName = sponsor.SponsorName,
                                       ContactName = sponsor.ContactName,
                                       Phone = sponsor.Phone,
                                       Email = sponsor.Email,
                                       AmountReceived = sponsor.AmountReceived,
                                       Address = data != null ? data.Address : "",
                                       ZipCodeId = data != null ? data.ZipCodeId :0,
                                       CityId = data != null ? data.CityId : 0,
                                       StateId = data != null ? _context.Cities.Where(x => x.CityId == data.CityId).Select(y => y.StateId).FirstOrDefault() : 0,
                                   }).ToList();
           
            if (sponsorResponses.Count() > 0)
            {
                if (request.SearchTerm != null && request.SearchTerm != "")
                {
                    sponsorResponses = sponsorResponses.Where(x => Convert.ToString(x.SponsorId).ToLower().Contains(request.SearchTerm) 
                    || x.SponsorName.ToLower().Contains(request.SearchTerm.ToLower()));
                }
                var propertyInfo = typeof(SponsorResponse).GetProperty(request.OrderBy);
                if (request.OrderByDescending == true)
                {
                    sponsorResponses = sponsorResponses.OrderByDescending(s => s.GetType().GetProperty(request.OrderBy).GetValue(s)).ToList();
                }
                else
                {
                    sponsorResponses = sponsorResponses.AsEnumerable().OrderBy(s => propertyInfo.GetValue(s, null)).ToList();
                }
                sponsorListResponse.TotalRecords = sponsorResponses.Count();
                if (request.AllRecords == true)
                {
                    sponsorResponses = sponsorResponses.ToList();
                }
                else
                {
                    sponsorResponses = sponsorResponses.Skip((request.Page - 1) * request.Limit).Take(request.Limit).ToList();
                }
            }
            sponsorListResponse.sponsorResponses = sponsorResponses.ToList();
            _mainResponse.SponsorListResponse = sponsorListResponse;
            return _mainResponse;
        }

        public SponsorListResponse SearchSponsor(SearchRequest searchRequest)
        {
            IEnumerable<SponsorResponse> sponsorResponses;
            SponsorListResponse sponsorListResponse = new SponsorListResponse();

            sponsorResponses = (from sponsor in _context.Sponsors
                                   join address in _context.Addresses
                                        on sponsor.AddressId equals address.AddressId
                                        into data1
                                   from data in data1.DefaultIfEmpty()
                                   where sponsor.IsActive == true 
                                   && sponsor.IsDeleted == false
                                   && data.IsActive == true &&
                                   data.IsDeleted == false
                                   && ((searchRequest.SearchTerm != string.Empty ? Convert.ToString(sponsor.SponsorId).Contains(searchRequest.SearchTerm) : (1 == 1))
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
                                       ZipCodeId = data != null ? data.ZipCodeId : 0,
                                       CityId = data != null ? data.CityId : 0,
                                       StateId = data != null ? _context.Cities.Where(x => x.CityId == data.CityId).Select(y => y.StateId).FirstOrDefault() : 0,
                                   }).ToList();

            if (sponsorResponses.Count() > 0)
            {
                var propertyInfo = typeof(SponsorResponse).GetProperty(searchRequest.OrderBy);
                if (searchRequest.OrderByDescending == true)
                {
                    sponsorResponses = sponsorResponses.OrderByDescending(s => s.GetType().GetProperty(searchRequest.OrderBy).GetValue(s)).ToList();
                }
                else
                {
                    sponsorResponses = sponsorResponses.AsEnumerable().OrderBy(s => propertyInfo.GetValue(s, null)).ToList();
                }
                sponsorListResponse.TotalRecords = sponsorResponses.Count();
                if (searchRequest.AllRecords == true)
                {
                    sponsorResponses = sponsorResponses.ToList();
                }
                else
                {
                    sponsorResponses = sponsorResponses.Skip((searchRequest.Page - 1) * searchRequest.Limit).Take(searchRequest.Limit).ToList();
                }
            }

            sponsorListResponse.sponsorResponses = sponsorResponses.ToList();
            return sponsorListResponse;
        }

    }
}
