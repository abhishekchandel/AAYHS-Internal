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
        public string ShowStartDate { get; set; }
        public string ShowEndDate { get; set; }
        public string PreCutOffDate { get; set; } 
        public string SponcerCutOffDate { get; set; }
        public DateTime Date { get; set; }
    }
    public class AddAdFee
    {
        public int YearlyMaintainenceId { get; set; }
        public string AdSize { get; set; }
        public decimal Amount { get; set; }
    }
    public class DeleteAdFee
    {
        public int YearlyMaintenanceFeeId { get; set; }
        public int AdSizeId { get; set; }
    }
    public class AddClassCategoryRequest
    {
        public string CategoryName { get; set; }
    }
    public class AddGeneralFeeRequest
    {
        public int YearlyMaintainenceFeeId { get; set; }
        public int YearlyMaintainenceId { get; set; }
        public string TimeFrame { get; set; }
        public string FeeType { get; set; }
        public decimal Amount { get; set; }
    }
    public class AddContactInfoRequest
    {
        public int AAYHSContactId { get; set; }
        public int YearlyMaintenanceId { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string ExhibitorSponsorAddress { get; set; }
        public string ExhibitorSponsorCity { get; set; }
        public int ExhibitorSponsorState{ get; set; }
        public string ExhibitorSponsorZip { get; set; }
        public string ExhibitorSponsorPhone { get; set; }
        public string ExhibitorSponsorEmail { get; set; }
        public string ExhibitorRefundAddress { get; set; }
        public string ExhibitorRefundCity { get; set; }
        public int ExhibitorRefundState { get; set; }
        public string ExhibitorRefundZip { get; set; }
        public string ExhibitorRefundPhone { get; set; }
        public string ExhibitorRefundEmail { get; set; }
        public string ReturnAddress { get; set; }
        public string ReturnCity { get; set; }
        public int ReturnState { get; set; }
        public string ReturnZip { get; set; }
        public string ReturnPhone { get; set; }
        public string ReturnEmail { get; set; }

    }

    public class RemoveGeneralFee
    {
        public int YearlyMaintenanceFeeId { get; set; }
        public int FeeTypeId { get; set; }
        public string TimeFrame { get; set; }
    }
    public class AddRefundRequest
    {
        public int  YearlyMaintenanceId { get; set; }
        public DateTime DateAfter { get; set; }
        public DateTime DateBefore { get; set; }
        public int FeeTypeId { get; set; }
        public decimal Refund { get; set; }
    }
    public class AddLocationRequest
    {
        public int YearlyMaintenanceId { get; set; }        
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
    }
    public class GetScanRequest
    {
        public int YearlyMaintenanceId { get; set; }
        public int DocumentTypeId { get; set; }
    }
    public class AddStatementTextRequest
    {
        public int YearlyStatementTextId { get; set; }
        public int YearlyMaintenanceId { get; set; }
        public string StatementName { get; set; }
        public string StatementNumber { get; set; }
        public string StatementText { get; set; }
        public int? Incentive { get; set; }
    }
    public class AddSponsorIncentiveRequest
    {
        public int SponsorIncentiveId { get; set; }
        public int YearlyMaintenanceId { get; set; }
        public decimal Amount { get; set; }
        public int Award { get; set; }
    }
    public class ActiveInActiveRequest
    {
        public int UserId { get; set; }
        public bool IsActive { get; set; }
    }
    public class ActiveInActiveGeneralFeeRequest
    {
        public int YearlyMaintenanceFeeId { get; set; }
        public bool IsActive { get; set; }
    }
    public class ActiveInActiveAdFeeRequest
    {
        public int YearlyMaintenanceFeeId { get; set; }
        public bool IsActive { get; set; }
    }
    public class ActiveInActiveClassCategory
    {
        public int GlobalCodeId { get; set; }
        public bool IsActive { get; set; }
    }
    public class ActiveInActiveScratchRefund
    {
        public int RefundDetailId { get; set; }
        public bool IsActive { get; set; }
    }
}
