using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Request
{
    public class GetAllYearlyMaintenanceRequest: BaseRecordFilterRequest
    {
    }
    public class UserApprovedRequest
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public bool IsApproved { get; set; }
    }
    public class AddYearlyRequest
    {
        public int YearlyMaintainenceId { get; set; }
        public int Year { get; set; }
        public DateTime ShowStartDate { get; set; }
        public DateTime ShowEndDate { get; set; }
        public DateTime PreCutOffDate { get; set; } 
        public DateTime SponcerCutOffDate { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
    }
    public class AddAdFee
    {
        public int YearlyMaintainenceId { get; set; }
        public string AdSize { get; set; }
        public decimal Amount { get; set; }
    }
    public class AddClassCategoryRequest
    {
        public string CategoryName { get; set; }
    }
    public class AddGeneralFeeRequest
    {
        public int YearlyMaintainenceId { get; set; }
        public string TimeFrame { get; set; }
        public string FeeType { get; set; }
        public decimal Amount { get; set; }
    }
}
