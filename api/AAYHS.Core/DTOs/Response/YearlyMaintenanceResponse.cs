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
        public List<GetGeneralFees> getGeneralFees { get; set; }
    }
    public class GetGeneralFees
    {
        public int YearlyMaintenanceFeeId { get; set; }
        public string TimeFrame { get; set; }
        public string FeeType { get; set; }
        public decimal PreEntryFee { get; set; }
        public decimal PostEntryFee { get; set; }
        public decimal Amount { get; set; }
        public bool Active { get; set; }
    }
    public class GetContactInfo
    {
        public DateTime ShowStart{ get; set; }
        public DateTime ShowEnd { get; set; }
        public string ShowLocation { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
    }
}
