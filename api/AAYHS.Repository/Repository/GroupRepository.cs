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
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;

namespace AAYHS.Repository.Repository
{
   public class GroupRepository : GenericRepository<Groups>, IGroupRepository
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

        public GroupRepository(AAYHSDBContext ObjContext, IMapper Mapper) : base(ObjContext)
        {
            _MainResponse = new MainResponse();
            _context = ObjContext;
            _Mapper = Mapper;
        }

        public MainResponse GetGroupById(int GroupId)
        {
            GroupResponse GroupResponse = new GroupResponse();
            GroupResponse = (from groups in _context.Groups
                             join address in _context.Addresses
                                  on groups.AddressId equals address.AddressId
                                  into data1
                             from data in data1.DefaultIfEmpty()
                             where groups.GroupId == GroupId
                              && groups.IsActive == true
                                   && groups.IsDeleted == false
                                   && data.IsActive == true &&
                                   data.IsDeleted == false
                             select new GroupResponse
                             {
                                 GroupId = groups.GroupId,
                                 GroupName = groups.GroupName,
                                 ContactName = groups.ContactName,
                                 Phone = groups.Phone,
                                 Email = groups.Email,
                                 AmountReceived = groups.AmountReceived,
                                 Address = data != null ? data.Address : "",
                                 ZipCode = data != null ? data.ZipCode : "",
                                 CityId = data != null ? data.CityId : 0,
                                 StateId = data != null ? _context.Cities.Where(x => x.CityId == data.CityId).Select(y => y.StateId).FirstOrDefault() : 0,
                             }).FirstOrDefault();
            _MainResponse.GroupResponse = GroupResponse;
            return _MainResponse;
        }

        public MainResponse GetAllGroups(BaseRecordFilterRequest request)
        {
            IEnumerable<GroupResponse> GroupResponses;
            GroupListResponse GroupListResponse = new GroupListResponse();
            GroupResponses = (from Group in _context.Groups
                                join address in _context.Addresses
                                     on Group.AddressId equals address.AddressId
                                     into data1
                                from data in data1.DefaultIfEmpty()
                                where Group.IsActive == true
                                && Group.IsDeleted == false
                                && data.IsActive == true
                                && data.IsDeleted == false
                              select new GroupResponse
                                {
                                    GroupId = Group.GroupId,
                                    GroupName = Group.GroupName,
                                    ContactName = Group.ContactName,
                                    Phone = Group.Phone,
                                    Email = Group.Email,
                                    AmountReceived = Group.AmountReceived,
                                    Address = data != null ? data.Address : "",
                                    ZipCode = data != null ? data.ZipCode : "",
                                    CityId = data != null ? data.CityId : 0,
                                    StateId = data != null ? _context.Cities.Where(x => x.CityId == data.CityId).Select(y => y.StateId).FirstOrDefault() : 0,
                                }).ToList();

            if (GroupResponses.Count() > 0)
            {
                var propertyInfo = typeof(GroupResponse).GetProperty(request.OrderBy);
                if (request.OrderByDescending == true)
                {
                    GroupResponses = GroupResponses.OrderByDescending(s => s.GetType().GetProperty(request.OrderBy).GetValue(s)).ToList();
                }
                else
                {
                    GroupResponses = GroupResponses.AsEnumerable().OrderBy(s => propertyInfo.GetValue(s, null)).ToList();
                }
                GroupListResponse.TotalRecords = GroupResponses.Count();

                if (request.AllRecords == true)
                {
                    GroupResponses = GroupResponses.ToList();
                }
                else
                {
                    GroupResponses = GroupResponses.Skip((request.Page - 1) * request.Limit).Take(request.Limit).ToList();
                }
            }
            GroupListResponse.groupResponses = GroupResponses.ToList();
            _MainResponse.GroupListResponse = GroupListResponse;
            return _MainResponse;
        }

        public GroupListResponse SearchGroup(SearchRequest searchRequest)
        {
            IEnumerable<GroupResponse> GroupResponses;
            GroupListResponse GroupListResponse = new GroupListResponse();

            GroupResponses = (from Group in _context.Groups
                                join address in _context.Addresses
                                     on Group.AddressId equals address.AddressId
                                     into data1
                                from data in data1.DefaultIfEmpty()
                                where Group.IsActive == true
                                && Group.IsDeleted == false
                                && data.IsActive == true
                                && data.IsDeleted == false
                                && ((searchRequest.SearchTerm != string.Empty ? Convert.ToString(Group.GroupId).Contains(searchRequest.SearchTerm) : (1 == 1))
                                || (searchRequest.SearchTerm != string.Empty ? Group.GroupName.Contains(searchRequest.SearchTerm) : (1 == 1)))
                                select new GroupResponse
                                {
                                    GroupId = Group.GroupId,
                                    GroupName = Group.GroupName,
                                    ContactName = Group.ContactName,
                                    Phone = Group.Phone,
                                    Email = Group.Email,
                                    AmountReceived = Group.AmountReceived,
                                    Address = data != null ? data.Address : "",
                                    ZipCode = data != null ? data.ZipCode : "",
                                    CityId = data != null ? data.CityId : 0,
                                    StateId = data != null ? _context.Cities.Where(x => x.CityId == data.CityId).Select(y => y.StateId).FirstOrDefault() : 0,
                                }).ToList();

            if (GroupResponses.Count() > 0)
            {
                var propertyInfo = typeof(GroupResponse).GetProperty(searchRequest.OrderBy);
                if (searchRequest.OrderByDescending == true)
                {
                    GroupResponses = GroupResponses.OrderByDescending(s => s.GetType().GetProperty(searchRequest.OrderBy).GetValue(s)).ToList();
                }
                else
                {
                    GroupResponses = GroupResponses.AsEnumerable().OrderBy(s => propertyInfo.GetValue(s, null)).ToList();
                }
                GroupListResponse.TotalRecords = GroupResponses.Count();
                if (searchRequest.AllRecords == true)
                {
                    GroupResponses = GroupResponses.ToList();
                }
                else
                {
                    GroupResponses = GroupResponses.Skip((searchRequest.Page - 1) * searchRequest.Limit).Take(searchRequest.Limit).ToList();
                }
            }

            GroupListResponse.groupResponses = GroupResponses.ToList();
            return GroupListResponse;
        }

        public GetAllGroupExhibitors GetGroupExhibitors(GroupExhibitorsRequest groupExhibitorsRequest)
        {
            IEnumerable<GetGroupExhibitors> data;
            GetAllGroupExhibitors getAllGroupExhibitors = new GetAllGroupExhibitors();

            data = (from groupExhibitors in _context.GroupExhibitors
                    join exhibitors in _context.Exhibitors on groupExhibitors.ExhibitorId equals exhibitors.ExhibitorId into exhibitors1
                    from exhibitors2 in exhibitors1.DefaultIfEmpty()                    
                    where groupExhibitors.GroupId == groupExhibitorsRequest.GroupId && groupExhibitors.IsActive == true &&
                    groupExhibitors.IsDeleted == false && exhibitors2.IsActive==true && exhibitors2.IsDeleted==false
                    select new GetGroupExhibitors 
                    { 
                       GroupExhibitorId=groupExhibitors.GroupExhibitorId,
                       ExhibitorId=groupExhibitors.ExhibitorId,
                       ExhibitorName=exhibitors2.FirstName+" "+exhibitors2.LastName,                      
                       BirthYear=exhibitors2.BirthYear,
                       getGroupExhibitorHorses=(from exhibitorHorse in _context.ExhibitorHorse
                                  join horse in _context.Horses on exhibitorHorse.HorseId equals horse.HorseId into horse1
                                  from horse2 in horse1.DefaultIfEmpty()
                                  where groupExhibitors.ExhibitorId==exhibitorHorse.ExhibitorId && exhibitorHorse.IsActive==true 
                                  && exhibitorHorse.IsDeleted==false && horse2.IsActive==true && horse2.IsDeleted==false
                                  select new GroupExhibitorHorses 
                                  { 
                                     HorseName=horse2.Name

                                  }).ToList()
                    });;

            if (data.Count() != 0)
            {
                if (groupExhibitorsRequest.OrderByDescending == true)
                {
                    data = data.OrderByDescending(x => x.GetType().GetProperty(groupExhibitorsRequest.OrderBy).GetValue(x));
                }
                else
                {
                    data = data.OrderBy(x => x.GetType().GetProperty(groupExhibitorsRequest.OrderBy).GetValue(x));
                }
                getAllGroupExhibitors.TotalRecords = data.Count();
                if (groupExhibitorsRequest.AllRecords)
                {
                    getAllGroupExhibitors.getGroupExhibitors = data.ToList();
                }
                else
                {
                    getAllGroupExhibitors.getGroupExhibitors = data.Skip((groupExhibitorsRequest.Page - 1) * groupExhibitorsRequest.Limit).Take(groupExhibitorsRequest.Limit).ToList();

                }

            }
            return getAllGroupExhibitors;
        }
    }
}
