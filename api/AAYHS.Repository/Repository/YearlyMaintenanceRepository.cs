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

        public GetAllYearlyMaintenance GetAllYearlyMaintenance(GetAllYearlyMaintenanceRequest getAllYearlyMaintenanceRequest, int feeTypeId)
        {
            IEnumerable<GetYearlyMaintenance> data;
            GetAllYearlyMaintenance getAllYearlyMaintenance = new GetAllYearlyMaintenance();
            
            List<GetYearlyMaintenance> getYearlyMaintenances = new List<GetYearlyMaintenance>();
            var allYear = _ObjContext.YearlyMaintainence.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();

            if (allYear.Count()!=0)
            {
                for (int i = 0; i <= allYear.Count()-1; i++)
                {
                    GetYearlyMaintenance getYearlyMaintenance = new GetYearlyMaintenance();
                    var yearlyFee = _ObjContext.YearlyMaintainenceFee.Where(x => x.YearlyMaintainenceId == allYear[i].YearlyMaintainenceId &&
                    x.FeeTypeId == feeTypeId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();

                    getYearlyMaintenance.YearlyMaintenanceId = allYear[i].YearlyMaintainenceId;
                    getYearlyMaintenance.Year = allYear[i].Years;
                    getYearlyMaintenance.PreEntryCutOffDate = allYear[i].PreEntryCutOffDate;
                    getYearlyMaintenance.PreEntryFee = yearlyFee != null ? yearlyFee.PreEntryFee : 0;
                    getYearlyMaintenance.PostEntryFee= yearlyFee != null ? yearlyFee.PostEntryFee : 0;
                    getYearlyMaintenance.DateCreated = allYear[i].Date;

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
                       Location=yearlyMaintenance.Location,
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
            int adCategoryId = GetCategoryId("AdTypes");
            IEnumerable<GetAdFees> data;
            GetAllAdFees getAllAdFees = new GetAllAdFees();

            data = (from fees in _ObjContext.YearlyMaintainenceFee
                    join adSize in _ObjContext.GlobalCodes on fees.FeeTypeId equals adSize.GlobalCodeId
                    where fees.IsActive == true && fees.IsDeleted == false
                    && adSize.IsActive == true && adSize.IsDeleted == false
                    && fees.YearlyMaintainenceId==yearlyMaintenanceId
                    && adSize.CategoryId== adCategoryId
                    select new GetAdFees
                    { 
                      YearlyMaintenanceFeeId=fees.YearlyMaintainenceFeeId,
                      AdSize=adSize.CodeName,
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
                    where globalCode.IsActive == true && globalCode.IsDeleted == false
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
            IEnumerable<GetGeneralFees> data;
            List<GetGeneralFees> getGeneralFees = new List<GetGeneralFees>();
            GetAllGeneralFees getAllGeneralFees = new GetAllGeneralFees();

            data = (from yearlyFee in _ObjContext.YearlyMaintainenceFee
                    join feeType in _ObjContext.GlobalCodes on yearlyFee.FeeTypeId equals feeType.GlobalCodeId
                    where yearlyFee.IsActive == true && yearlyFee.IsDeleted == false
                    && yearlyFee.YearlyMaintainenceId==yearlyMaintenanceId
                    select new GetGeneralFees
                    {
                        YearlyMaintenanceFeeId=yearlyFee.YearlyMaintainenceFeeId,
                        FeeType=feeType.CodeName,
                        PreEntryFee=yearlyFee.PreEntryFee,
                        PostEntryFee=yearlyFee.PostEntryFee,
                        Amount=yearlyFee.Amount,
                        Active=yearlyFee.IsActive
                    });
            if (data.Count()!=0)
            {
                getGeneralFees = data.ToList();
                List<GetGeneralFeesResponse> getAllGeneral = new List<GetGeneralFeesResponse>();
                for (int i = 0; i <= getGeneralFees.Count()-1; i++)
                {
                    GetGeneralFeesResponse getGeneralFeesResponse;

                    if (getGeneralFees[i].PreEntryFee!=0)
                    {
                        getGeneralFeesResponse = new GetGeneralFeesResponse();
                        getGeneralFeesResponse.YearlyMaintenanceFeeId = getGeneralFees[i].YearlyMaintenanceFeeId;
                        getGeneralFeesResponse.TimeFrame = "Pre";
                        getGeneralFeesResponse.FeeType= getGeneralFees[i].FeeType;
                        getGeneralFeesResponse.Amount = getGeneralFees[i].PreEntryFee;
                        getGeneralFeesResponse.Active= getGeneralFees[i].Active;
                        getAllGeneral.Add(getGeneralFeesResponse);

                    }
                    if (getGeneralFees[i].PostEntryFee != 0)
                    {
                        getGeneralFeesResponse = new GetGeneralFeesResponse();
                        getGeneralFeesResponse.YearlyMaintenanceFeeId = getGeneralFees[i].YearlyMaintenanceFeeId;
                        getGeneralFeesResponse.TimeFrame = "Post";
                        getGeneralFeesResponse.FeeType = getGeneralFees[i].FeeType;
                        getGeneralFeesResponse.Amount = getGeneralFees[i].PostEntryFee;
                        getGeneralFeesResponse.Active = getGeneralFees[i].Active;
                        getAllGeneral.Add(getGeneralFeesResponse);
                    }

                    if (getGeneralFees[i].PreEntryFee == 0 && getGeneralFees[i].PostEntryFee == 0)
                    {
                        getGeneralFeesResponse = new GetGeneralFeesResponse();
                        getGeneralFeesResponse.YearlyMaintenanceFeeId = getGeneralFees[i].YearlyMaintenanceFeeId;
                        getGeneralFeesResponse.TimeFrame = "";
                        getGeneralFeesResponse.FeeType = getGeneralFees[i].FeeType;
                        getGeneralFeesResponse.Amount = getGeneralFees[i].Amount;
                        getGeneralFeesResponse.Active = getGeneralFees[i].Active;
                        getAllGeneral.Add(getGeneralFeesResponse);
                    }
                }
                getAllGeneralFees.getGeneralFeesResponses = getAllGeneral;
            }
            return getAllGeneralFees;
        }

        public GetContactInfo GetContactInfo(int yearlyMaintenanceId)
        {
            GetContactInfo getContactInfo = new GetContactInfo();
            IEnumerable<GetContactInfo> data;

            data = (from contactInfo in _ObjContext.AAYHSContact
                    join yearlyMaintenance in _ObjContext.YearlyMaintainence on contactInfo.YearlyMaintainenceId equals
                    yearlyMaintenance.YearlyMaintainenceId into yearlyMaintenance1
                    from yearlyMaintenance2 in yearlyMaintenance1.DefaultIfEmpty()
                    where contactInfo.YearlyMaintainenceId == yearlyMaintenanceId
                    select new GetContactInfo 
                    { 
                      AAYHSContactId=contactInfo.AAYHSContactId,
                      ShowStart= yearlyMaintenance2.ShowStartDate,
                      ShowEnd=yearlyMaintenance2.ShowEndDate,
                      ShowLocation=yearlyMaintenance2.Location,
                      Email1= contactInfo.Email1,
                      Email2= contactInfo.Email2,
                      Phone1= contactInfo.Phone1,
                      Phone2=contactInfo.Phone2
                    });

            if (data.Count()!=0)
            {
                getContactInfo = data.FirstOrDefault();
            }
            return getContactInfo;
        }
    }
}
