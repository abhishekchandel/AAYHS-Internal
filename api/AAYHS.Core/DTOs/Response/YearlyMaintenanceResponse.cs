using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response
{
    public class GetAllYearlyMaintenance
    {
        public List<GetYearlyMaintenance> getYearlyMaintenances { get; set; }
        public int TotalRecords { get; set; }
    }
    public class GetYearlyMaintenance
    {
        public int YearlyMaintenanceId { get; set; }
        public int Year { get; set; }
        public DateTime PreEntryCutOffDate { get; set; }
        public decimal PreEntryFee { get; set; }
        public decimal PostEntryFee { get; set; }
        public DateTime DateCreated { get; set; }
    }
    public class GetYearlyMaintenanceById
    {
        public int YearlyMaintenanceId { get; set; }
        public int Year { get; set; }
        public DateTime ShowStartDate { get; set; }
        public DateTime ShowEndDate { get; set; }
        public DateTime PreEntryCutOffDate { get; set; }
        public DateTime SponcerCutOffDate { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
    }
    public class GetAllUsers
    {
        public List<GetUser> getUsers { get; set; }
    }
    public class GetUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
    }

    public class GetAllAdFees
    {
        public List<GetAdFees>getAdFees { get; set; }
    }
    public class GetAdFees
    {
        public int YearlyMaintenanceFeeId { get; set; }
        public string AdSize  { get; set; }
        public decimal Amount { get; set; }
        public bool Active { get; set; }
 
    }
    public class GetAllClassCategory
    {
        public List<GetClassCategory>getClassCategories { get; set; }
    }
    public class GetClassCategory
    {
        public int GlobalCodeId { get; set; }
        public string CodeName { get; set; }
        public bool IsActive { get; set; }
    }
    public class GetAllRoles
    {
        public List<GetRoles> getRoles { get; set; }
    }
    public class GetRoles
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
    public class GetAllGeneralFees
    {
        public List<GetGeneralFeesResponse> getGeneralFeesResponses { get; set; }
    }
    public class GetGeneralFeesResponse
    {
        public int YearlyMaintenanceFeeId { get; set; }
        public string TimeFrame { get; set; }
        public int FeeTypeId { get; set; }
        public string FeeType { get; set; }
        public decimal Amount { get; set; }
        public bool Active { get; set; }
    }
    

    public class GetAllRefund
    {
        public List<GetRefund> getRefunds { get; set; }
    }
    public class GetRefund
    {
        public int RefundId { get; set; }
        public DateTime DateAfter { get; set; }
        public DateTime DateBefore { get; set; }
        public string RefundType { get; set; }
        public decimal Refund { get; set; }
        public bool Active { get; set; }
    }   
    public class GetContactInfo
    {
        public ContactInfo contactInfo { get; set; }
        public ExhibitorSponsorConfirmationResponse exhibitorSponsorConfirmationResponse { get; set; }
        public ExhibitorSponsorRefundStatementResponse exhibitorSponsorRefundStatementResponse { get; set; }
        public ExhibitorConfirmationEntriesResponse exhibitorConfirmationEntriesResponse { get; set; }
    }
    public class ContactInfo
    {
        public int AAYHSContactId { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
    }
    public class ExhibitorSponsorConfirmationResponse
    {
        public int AAYHSContactAddressId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
    public class ExhibitorSponsorRefundStatementResponse
    {
        public int AAYHSContactAddressId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
    public class ExhibitorConfirmationEntriesResponse
    {
        public int AAYHSContactAddressId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    public class GetLocation
    {
        public int AAYHSContactAddressId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
    }

    public class GetAllScan
    {
        public List< GetScan> getScans { get; set; }
    }
    public class GetScan
    {
        public int ScanId { get; set; }
        public int ExhibitorId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
    }
    public class GetAllStatementText
    {
        public List<GetStatementText> getStatementTexts { get; set; }
    }
    public class GetStatementText
    {
        public int YearlyStatementTextId { get; set; }
        public int YearlyMaintenanceId { get; set; }
        public string StatementName { get; set; }
        public string StatementNumber { get; set; }
        public string StatementText { get; set; }
        public int? Incentive { get; set; }
    }
    public class GetSponsorAllIncentives
    {
        public List<GetSponsorIncentives> getSponsorIncentives { get; set; }
    }
    public class GetSponsorIncentives
    {
        public int SponsorIncentiveId { get; set; }
        public decimal SponsorAmount { get; set; }
        public int Award { get; set; }
    }
}
