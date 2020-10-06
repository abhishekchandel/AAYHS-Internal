﻿using System;
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
    public class AddContactInfoRequest
    {
        public int AAYHSContactId { get; set; }
        public int YearlyMaintainenceId { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string ExhibitorSponsorConfirmationAddress { get; set; }
        public string ExhibitorSponsorConfirmationCity { get; set; }
        public int ExhibitorSponsorConfirmationStateId { get; set; }
        public string ExhibitorSponsorConfirmationZipCode { get; set; }
        public string ExhibitorSponsorRefundStatementAddress { get; set; }
        public string ExhibitorSponsorRefundStatementCity { get; set; }
        public int ExhibitorSponsorRefundStatementStateId { get; set; }
        public string ExhibitorSponsorRefundStatementZipCode { get; set; }
        public string ExhibitorConfirmationEntriesAddress { get; set; }
        public string ExhibitorConfirmationEntriesCity { get; set; }
        public int ExhibitorConfirmationEntriesStateId { get; set; }
        public string ExhibitorConfirmationEntriesZipCode { get; set; }

    }

    public class RemoveGeneralFee
    {
        public int YearlyMaintenanceFeeId { get; set; }
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
}
