using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Repository.IRepository
{
    public interface IYearlyMaintenanceRepository:IGenericRepository<YearlyMaintainence>
    {
        GetAllYearlyMaintenance GetAllYearlyMaintenance(GetAllYearlyMaintenanceRequest getAllYearlyMaintenanceRequest, int feeTypeId);
        GetYearlyMaintenanceById GetYearlyMaintenanceById(int yearlyMaintenanceId);
        int GetCategoryId(string categoryType);
        GetAllAdFees GetAllAdFees(int yearlyMaintenanceId);
        GetAllGeneralFees GetAllGeneralFees(int yearlyMaintenanceId);
    }
}
