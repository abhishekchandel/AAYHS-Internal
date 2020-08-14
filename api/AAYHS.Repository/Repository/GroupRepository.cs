﻿using AAYHS.Core.DTOs.Request;
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
using System.Security.Cryptography.X509Certificates;

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

        public GetAllGroupExhibitors GetGroupExhibitors(int GroupId)
        {
            IEnumerable<GetGroupExhibitors> data;
            GetAllGroupExhibitors getAllGroupExhibitors = new GetAllGroupExhibitors();

            data = (from groupExhibitors in _context.GroupExhibitors
                    join exhibitors in _context.Exhibitors on groupExhibitors.ExhibitorId equals exhibitors.ExhibitorId into exhibitors1
                    from exhibitors2 in exhibitors1.DefaultIfEmpty()                    
                    where groupExhibitors.GroupId == GroupId && groupExhibitors.IsActive == true &&
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
                    });
           
            getAllGroupExhibitors.getGroupExhibitors = data.ToList();
            getAllGroupExhibitors.TotalRecords = data.Count();
            return getAllGroupExhibitors;
        }

        public GetAllGroupFinacials GetAllGroupFinancials(int GroupId)
        {
            GetAllGroupFinacials getAllGroupFinacials = new GetAllGroupFinacials();
            List<GetGroupFinacials> list = new List<GetGroupFinacials>();
            list = (from financial in _context.GroupFinancials
                    join feetype in _context.GlobalCodes on financial.FeeTypeId equals feetype.GlobalCodeId
                    join timeframe in _context.GlobalCodes on financial.TimeFrameId equals timeframe.GlobalCodeId
                    where financial.GroupId == GroupId && financial.IsActive == true && financial.IsDeleted == false
                    && feetype.IsActive == true && feetype.IsDeleted == false
                      && timeframe.IsActive == true && timeframe.IsDeleted == false
                    select new GetGroupFinacials
                    {
                        GroupFinancialId = financial.GroupFinancialId,
                        Date = financial.Date,
                        FeeTypeId = financial.FeeTypeId,
                        FeeTypeName = feetype.CodeName,
                        TimeFrameId = financial.TimeFrameId,
                        TimeFrameName = timeframe.CodeName,
                        Amount = financial.Amount
                    }).ToList();
          


            getAllGroupFinacials.getGroupFinacials = list;
            getAllGroupFinacials.getGroupFinacialsTotals = getTotals(list);
            return getAllGroupFinacials;
        }
        public GetGroupFinacialsTotals getTotals(List<GetGroupFinacials> list)
        {
            GetGroupFinacialsTotals getGroupFinacialsTotals = new GetGroupFinacialsTotals();

            var codes = (from gcc in _context.GlobalCodeCategories
                         join gc in _context.GlobalCodes on gcc.GlobalCodeCategoryId equals gc.CategoryId
                         where gcc.CategoryName == "TimeFrameType" && gc.IsDeleted == false && gc.IsActive == true
                         select new
                         {
                             gc.GlobalCodeId,
                             gc.CodeName

                         }).ToList();
            var Preid = (from code in codes where code.CodeName == "Pre" select code.GlobalCodeId).FirstOrDefault();
            var Postid = (from code in codes where code.CodeName == "Post" select code.GlobalCodeId).FirstOrDefault();
            var fees = (from gcc in _context.GlobalCodeCategories
                         join gc in _context.GlobalCodes on gcc.GlobalCodeCategoryId equals gc.CategoryId
                         where gcc.CategoryName == "FeeType" && gc.IsDeleted == false && gc.IsActive == true
                         select new
                         {
                             gc.GlobalCodeId,
                             gc.CodeName

                         }).ToList();
            var horsestallid = (from code in fees where code.CodeName == "Horse Stall" select code.GlobalCodeId).FirstOrDefault();
            var tackstallid = (from code in fees where code.CodeName == "Tack Stall" select code.GlobalCodeId).FirstOrDefault();


            getGroupFinacialsTotals.PreStallSum = list.Where(x=>x.TimeFrameId==Preid && x.FeeTypeId== horsestallid).Select(x=>x.Amount).Sum();
            getGroupFinacialsTotals.PreTackStallSum = list.Where(x => x.TimeFrameId == Preid && x.FeeTypeId == tackstallid).Select(x => x.Amount).Sum();
            getGroupFinacialsTotals.PreTotal = list.Where(x => x.TimeFrameId == Preid).Select(x => x.Amount).Sum(); ;

            getGroupFinacialsTotals.PostStallSum = list.Where(x => x.TimeFrameId == Postid && x.FeeTypeId == horsestallid).Select(x => x.Amount).Sum(); 
            getGroupFinacialsTotals.PostTackStallSum = list.Where(x => x.TimeFrameId == Postid && x.FeeTypeId == tackstallid).Select(x => x.Amount).Sum();
            getGroupFinacialsTotals.PostTotal = list.Where(x => x.TimeFrameId == Postid).Select(x => x.Amount).Sum();

            getGroupFinacialsTotals.PrePostStallSum = list.Where(x => x.FeeTypeId == horsestallid).Select(x => x.Amount).Sum();
            getGroupFinacialsTotals.PrePostTackStallSum = list.Where(x => x.FeeTypeId == tackstallid).Select(x => x.Amount).Sum();
            getGroupFinacialsTotals.PrePostTotal = list.Select(x => x.Amount).Sum();

            return getGroupFinacialsTotals;
        }
    }
}
