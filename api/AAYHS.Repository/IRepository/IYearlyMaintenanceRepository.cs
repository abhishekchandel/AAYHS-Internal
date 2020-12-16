using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Repository.IRepository
{
    public interface IYearlyMaintenanceRepository:IGenericRepository<YearlyMaintainence>
    {
        GetAllYearlyMaintenance GetAllYearlyMaintenance(GetAllYearlyMaintenanceRequest getAllYearlyMaintenanceRequest, GlobalCodeMainResponse feeType);
        GetYearlyMaintenanceById GetYearlyMaintenanceById(int yearlyMaintenanceId);
        int GetCategoryId(string categoryType);
        GetAllAdFees GetAllAdFees(int yearlyMaintenanceId);
        GetAllClassCategory GetAllClassCategory();
        GetAllGeneralFees GetAllGeneralFees(int yearlyMaintenanceId);
        GetAllRefund GetAllRefund(int yearlyMaintenanceId);
        GetContactInfo GetContactInfo(int yearlyMaintenanceId);
        void DeleteYearlyFee(int yearlyMaintenanceId);
    }
}
