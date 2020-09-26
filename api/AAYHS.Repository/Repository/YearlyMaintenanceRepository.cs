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

            data = (from yearlyMaintenance in _ObjContext.YearlyMaintainence
                    join yearlyMaintenanceFee in _ObjContext.YearlyMaintainenceFee on yearlyMaintenance.YearlyMaintainenceId equals
                    yearlyMaintenanceFee.YearlyMaintainenceId into yearlyMaintenanceFee1
                    from yearlyMaintenanceFee2 in yearlyMaintenanceFee1.DefaultIfEmpty()
                    where yearlyMaintenance.IsActive == true && yearlyMaintenance.IsDeleted == false
                    && yearlyMaintenanceFee2.IsActive == true && yearlyMaintenanceFee2.IsDeleted == false
                    && yearlyMaintenanceFee2.FeeTypeId == feeTypeId
                    select new GetYearlyMaintenance
                    {
                        YearlyMaintenanceId = yearlyMaintenance.YearlyMaintainenceId,
                        Year = yearlyMaintenance.Year.Year,
                        PreEntryCutOffDate = yearlyMaintenance.PreEntryCutOffDate,
                        PreEntryFee = yearlyMaintenanceFee2.PreEntryFee,
                        PostEntryFee = yearlyMaintenanceFee2.PostEntryFee,
                        //DateCreated = yearlyMaintenance.Date
                    });

            if (data.Count() != 0)
            {
                if (getAllYearlyMaintenanceRequest.SearchTerm != null && getAllYearlyMaintenanceRequest.SearchTerm != "")
                {
                    data = data.Where(x => Convert.ToString(x.Year).Contains(getAllYearlyMaintenanceRequest.SearchTerm) || Convert.ToString(x.PreEntryCutOffDate).Contains(getAllYearlyMaintenanceRequest.SearchTerm) ||
                                     Convert.ToString(x.PreEntryFee).Contains(getAllYearlyMaintenanceRequest.SearchTerm) || Convert.ToString(x.PostEntryFee).Contains(getAllYearlyMaintenanceRequest.SearchTerm) ||
                                     Convert.ToString(x.DateCreated).Contains(getAllYearlyMaintenanceRequest.SearchTerm));
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
                       Year=yearlyMaintenance.Year.Year,
                       PreEntryCutOffDate=yearlyMaintenance.PreEntryCutOffDate,
                       ShowStartDate=yearlyMaintenance.ShowStartDate,
                       ShowEndDate=yearlyMaintenance.ShowEndDate,
                       Location=yearlyMaintenance.Location,
                       //Date=yearlyMaintenance.Date                    
                    });

            if (data.Count()!=0)
            {
                getYearlyMaintenanceById = data.FirstOrDefault();
            }
            return getYearlyMaintenanceById;
        }      
    }
}
