using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Service.IService
{
    public interface IYearlyMaintenanceService
    {
        MainResponse GetAllYearlyMaintenance(GetAllYearlyMaintenanceRequest getAllYearlyMaintenanceRequest);
        MainResponse GetYearlyMaintenanceById(int yearlyMaintenanceId);
        MainResponse GetAllUsers();
        MainResponse ApprovedUser(UserApprovedRequest userApprovedRequest, string actionBy);
        MainResponse DeleteUser(int userId, string actionBy);
        MainResponse AddUpdateYearly(AddYearlyRequest addYearly, string actionBy);
        MainResponse DeleteYearly(int yearlyMaintainenceId, string actionBy);
        MainResponse AddADFee(AddAdFee addAdFee, string actionBy);
        MainResponse GetAllAdFees(int yearlyMaintenanceId);
        MainResponse DeleteAdFee(int yearlyMaintenanceFeeId, string actionBy);
        MainResponse GetAllUsersApproved();
        MainResponse RemoveApprovedUser(int userId, string actionBy);
    }
}
