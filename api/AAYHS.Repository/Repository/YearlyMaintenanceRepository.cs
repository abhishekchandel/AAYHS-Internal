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
using AAYHS.Core.DTOs.Response.Common;

namespace AAYHS.Repository.Repository
{
    public class YearlyMaintenanceRepository : GenericRepository<YearlyMaintainence>, IYearlyMaintenanceRepository
    {
        #region readonly
        private readonly AAYHSDBContext _ObjContext;
        private IMapper _Mapper;
        #endregion

        #region private 
        private MainResponse _MainResponse;
        #endregion

        public YearlyMaintenanceRepository(AAYHSDBContext ObjContext, IMapper Mapper) : base(ObjContext)
        {
            _ObjContext = ObjContext;
            _Mapper = Mapper;
            _MainResponse = new MainResponse();
        }

        public GetAllYearlyMaintenance GetAllYearlyMaintenance(GetAllYearlyMaintenanceRequest getAllYearlyMaintenanceRequest, GlobalCodeMainResponse feeType)
        {
            IEnumerable<GetYearlyMaintenance> data;
            GetAllYearlyMaintenance getAllYearlyMaintenance = new GetAllYearlyMaintenance();         
            List<GetYearlyMaintenance> getYearlyMaintenances = new List<GetYearlyMaintenance>();
            var allYears = _ObjContext.YearlyMaintainence.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();

            if (allYears.Count()!=0)
            {
                for (int i = 0; i <= allYears.Count()-1; i++)
                {
                    GetYearlyMaintenance getYearlyMaintenance = new GetYearlyMaintenance();
                 
                    var yearlyFee = _ObjContext.YearlyMaintainenceFee.Where(x => x.YearlyMaintainenceId == allYears[i].YearlyMaintainenceId &&
                                    x.FeeType== "GeneralFee" &&
                                    x.FeeName == "Administration" && x.IsActive == true && x.IsDeleted == false).ToList();

                    decimal yearlyPreFee = yearlyFee.Where(x => x.TimeFrame == "Pre").Select(x => x.Amount).FirstOrDefault();
                    decimal yearlyPostFee = yearlyFee.Where(x => x.TimeFrame == "Post").Select(x => x.Amount).FirstOrDefault();

                    getYearlyMaintenance.YearlyMaintenanceId = allYears[i].YearlyMaintainenceId;
                    getYearlyMaintenance.Year = allYears[i].Years;
                    getYearlyMaintenance.PreEntryCutOffDate = allYears[i].PreEntryCutOffDate;
                    getYearlyMaintenance.PreEntryFee = yearlyPreFee;
                    getYearlyMaintenance.PostEntryFee= yearlyPostFee;
                    getYearlyMaintenance.DateCreated = allYears[i].Date;

                    getYearlyMaintenances.Add(getYearlyMaintenance);
                }

            }
            data = getYearlyMaintenances.ToList();
            if (data.Count() != 0)
            {
                if (getAllYearlyMaintenanceRequest.SearchTerm != null && getAllYearlyMaintenanceRequest.SearchTerm != "")
                {
                    data = data.Where(x => Convert.ToString(x.Year).Contains(getAllYearlyMaintenanceRequest.SearchTerm) || Convert.ToString(x.PreEntryCutOffDate.Date).Contains(getAllYearlyMaintenanceRequest.SearchTerm) ||
                                     Convert.ToString(x.PreEntryFee).Contains(getAllYearlyMaintenanceRequest.SearchTerm) || Convert.ToString(x.PostEntryFee).Contains(getAllYearlyMaintenanceRequest.SearchTerm) ||
                                     Convert.ToString(x.DateCreated.Date).Contains(getAllYearlyMaintenanceRequest.SearchTerm));
                }
                if (getAllYearlyMaintenanceRequest.OrderByDescending == true)
                {
                    data = data.OrderByDescending(x => x.GetType().GetProperty(getAllYearlyMaintenanceRequest.OrderBy).GetValue(x));
                }
                else
                {
                    data = data.OrderBy(x => x.GetType().GetProperty(getAllYearlyMaintenanceRequest.OrderBy).GetValue(x));
                }
                getAllYearlyMaintenance.TotalRecords = data.Count();
                if (getAllYearlyMaintenanceRequest.AllRecords)
                {
                    getAllYearlyMaintenance.getYearlyMaintenances = data.ToList();
                }
                else
                {
                    getAllYearlyMaintenance.getYearlyMaintenances = data.Skip((getAllYearlyMaintenanceRequest.Page - 1) * getAllYearlyMaintenanceRequest.Limit).Take(getAllYearlyMaintenanceRequest.Limit).ToList();

                }

            }
            return getAllYearlyMaintenance;
        }

        public GetYearlyMaintenanceById GetYearlyMaintenanceById(int yearlyMaintenanceId)
        {
            IEnumerable<GetYearlyMaintenanceById> data;
            GetYearlyMaintenanceById getYearlyMaintenanceById = new GetYearlyMaintenanceById();

            data = (from yearlyMaintenance in _ObjContext.YearlyMaintainence
                    where yearlyMaintenance.IsActive == true && yearlyMaintenance.IsDeleted == false
                    && yearlyMaintenance.YearlyMaintainenceId==yearlyMaintenanceId
                    select new GetYearlyMaintenanceById 
                    { 
                       YearlyMaintenanceId=yearlyMaintenance.YearlyMaintainenceId,
                       Year=yearlyMaintenance.Years,
                       PreEntryCutOffDate=yearlyMaintenance.PreEntryCutOffDate,
                       ShowStartDate=yearlyMaintenance.ShowStartDate,
                       ShowEndDate=yearlyMaintenance.ShowEndDate,
                       SponcerCutOffDate=yearlyMaintenance.SponcerCutOffDate,
                       Date = yearlyMaintenance.Date

                    });

            if (data.Count()!=0)
            {
                getYearlyMaintenanceById = data.FirstOrDefault();
            }
            return getYearlyMaintenanceById;
        }      

        public int GetCategoryId(string categoryType)
        {
            int categoryId = _ObjContext.GlobalCodeCategories.Where(x => x.CategoryName == categoryType).Select(x=>x.GlobalCodeCategoryId).FirstOrDefault();

            return categoryId;
        }

        public GetAllAdFees GetAllAdFees(int yearlyMaintenanceId)
        {          
            IEnumerable<GetAdFees> data;
            GetAllAdFees getAllAdFees = new GetAllAdFees();

            data = (from fees in _ObjContext.YearlyMaintainenceFee                   
                    where fees.IsDeleted == false                   
                    && fees.YearlyMaintainenceId==yearlyMaintenanceId
                    && fees.FeeType=="AdFee"
                    select new GetAdFees
                    { 
                      YearlyMaintenanceFeeId=fees.YearlyMaintainenceFeeId,                    
                      AdSize= fees.FeeName,
                      Amount=fees.Amount,
                      Active=fees.IsActive
                    });

            if (data.Count()!=0)
            {
                getAllAdFees.getAdFees = data.ToList();
            }
            return getAllAdFees;
        }

        public GetAllClassCategory GetAllClassCategory()
        {
            IEnumerable<GetClassCategory> data;
            GetAllClassCategory getAllClassCategory = new GetAllClassCategory();

            data = (from globalCategory in _ObjContext.GlobalCodeCategories
                    join globalCode in _ObjContext.GlobalCodes on globalCategory.GlobalCodeCategoryId equals globalCode.CategoryId
                    where globalCode.IsDeleted == false
                    && globalCategory.CategoryName=="ClassHeaderType"
                    select new GetClassCategory
                    {
                        GlobalCodeId=globalCode.GlobalCodeId,
                        CodeName= globalCode.CodeName,
                        IsActive=globalCode.IsActive
                    });

            if (data.Count()!=0)
            {
                getAllClassCategory.getClassCategories = data.ToList();
            }

            return getAllClassCategory;
        }

        public GetAllGeneralFees GetAllGeneralFees(int yearlyMaintenanceId)
        {
            IEnumerable<GetGeneralFeesResponse> data;          
            GetAllGeneralFees getAllGeneralFees = new GetAllGeneralFees();
                    
            data = (from yearlyFee in _ObjContext.YearlyMaintainenceFee                   
                    where yearlyFee.IsDeleted == false                   
                    && yearlyFee.YearlyMaintainenceId==yearlyMaintenanceId 
                    && yearlyFee.FeeType=="GeneralFee"
                    select new GetGeneralFeesResponse
                    {
                        YearlyMaintenanceFeeId=yearlyFee.YearlyMaintainenceFeeId,                        
                        FeeType= yearlyFee.FeeName,
                        TimeFrame=yearlyFee.TimeFrame,
                        Amount=yearlyFee.Amount,
                        Active=yearlyFee.IsActive
                    });
            if (data.Count()!=0)
            {
                getAllGeneralFees.getGeneralFeesResponses = data.ToList();
            }
            return getAllGeneralFees;
        }

        public GetAllRefund GetAllRefund(int yearlyMaintenanceId)
        {
            IEnumerable<GetRefund> data;
            GetAllRefund getAllRefund = new GetAllRefund();

            data = (from refund in _ObjContext.RefundDetail
                    join feeType in _ObjContext.GlobalCodes on refund.FeeTypeId equals feeType.GlobalCodeId
                    where  refund.IsDeleted == false
                    && refund.YearlyMaintenanceId==yearlyMaintenanceId
                    select new GetRefund
                    {
                        RefundId = refund.RefundDetailId,
                        DateAfter = refund.DateAfter,
                        DateBefore = refund.DateBefore,
                        RefundType=feeType.CodeName,
                        Refund = refund.RefundPercentage,
                        Active = refund.IsActive
                    });

            if (data.Count()!=0)
            {
                getAllRefund.getRefunds = data.ToList();
            }

            return getAllRefund;
        }

        public GetContactInfo GetContactInfo(int yearlyMaintenanceId)
        {
            IEnumerable<GetContactInfo> data;
            GetContactInfo getContactInfo = new GetContactInfo();

            data = (from contactInfo in _ObjContext.AAYHSContact
                    where contactInfo.YearlyMaintainenceId == yearlyMaintenanceId
                    select new GetContactInfo
                    {
                        contactInfo=(from info in _ObjContext.AAYHSContact
                                                    where info.YearlyMaintainenceId==yearlyMaintenanceId
                                                    select new ContactInfo 
                                                    { 
                                                      AAYHSContactId=info.AAYHSContactId,
                                                      Email1=info.Email1,
                                                      Email2=info.Email2,
                                                      Phone1=info.Phone1,
                                                      Phone2=info.Phone2,
                                                      Location=info.Location,
                                                      Address=info.Address,
                                                      City=info.City,
                                                      State=info.State,
                                                      Zipcode=info.ZipCode                                                                                                      
                                                    }).FirstOrDefault(),


                        exhibitorSponsorConfirmationResponse = (from address in _ObjContext.AAYHSContactAddresses
                                                                where contactInfo.ExhibitorSponsorConfirmationAddressId == address.AAYHSContactAddressId
                                                                && address.IsDeleted == false
                                                                select new ExhibitorSponsorConfirmationResponse
                                                                {
                                                                    AAYHSContactAddressId = address.AAYHSContactAddressId,
                                                                    Address = address.Address,
                                                                    City = address.City,
                                                                    StateId = address.StateId,
                                                                    ZipCode = address.ZipCode,
                                                                    Phone=address.Phone,
                                                                    Email=address.Email
                                                                    
                                                                }).FirstOrDefault(),
                        exhibitorSponsorRefundStatementResponse = (from address in _ObjContext.AAYHSContactAddresses
                                                                   where contactInfo.ExhibitorSponsorRefundStatementAddressId == address.AAYHSContactAddressId
                                                                   && address.IsDeleted == false
                                                                   select new ExhibitorSponsorRefundStatementResponse
                                                                   {
                                                                       AAYHSContactAddressId = address.AAYHSContactAddressId,
                                                                       Address = address.Address,
                                                                       City = address.City,
                                                                       StateId = address.StateId,
                                                                       ZipCode = address.ZipCode,
                                                                       Phone=address.Phone,
                                                                       Email=address.Email
                                                                   }).FirstOrDefault(),
                        exhibitorConfirmationEntriesResponse = (from address in _ObjContext.AAYHSContactAddresses
                                                                where contactInfo.ExhibitorConfirmationEntriesAddressId == address.AAYHSContactAddressId
                                                                && address.IsDeleted == false
                                                                select new ExhibitorConfirmationEntriesResponse
                                                                {
                                                                    AAYHSContactAddressId = address.AAYHSContactAddressId,
                                                                    Address = address.Address,
                                                                    City = address.City,
                                                                    StateId = address.StateId,
                                                                    ZipCode = address.ZipCode,
                                                                    Phone=address.Phone,
                                                                    Email=address.Email
                                                                }).FirstOrDefault()
                    });


                  
           
            getContactInfo = data.FirstOrDefault();
           
            return getContactInfo;
        }

        public void DeleteYearlyFee(int yearlyMaintenanceId)
        {
            var allFee = _ObjContext.YearlyMaintainenceFee.Where(x => x.YearlyMaintainenceId == yearlyMaintenanceId);
            _ObjContext.YearlyMaintainenceFee.RemoveRange(allFee);
            _ObjContext.SaveChanges();

            var allRefund = _ObjContext.RefundDetail.Where(x => x.YearlyMaintenanceId == yearlyMaintenanceId);
            _ObjContext.RefundDetail.RemoveRange(allRefund);
            _ObjContext.SaveChanges();

            var sponsorIncentive = _ObjContext.SponsorIncentives.Where(x => x.YearlyMaintenanceId == yearlyMaintenanceId);
            _ObjContext.SponsorIncentives.RemoveRange(sponsorIncentive);
            _ObjContext.SaveChanges();

            var allText = _ObjContext.YearlyStatementText.Where(x => x.YearlyMaintenanceId == yearlyMaintenanceId);
            _ObjContext.YearlyStatementText.RemoveRange(allText);
            _ObjContext.SaveChanges();
        }

    }
}
