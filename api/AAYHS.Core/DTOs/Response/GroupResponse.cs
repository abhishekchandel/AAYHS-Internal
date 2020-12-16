using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response
{
   public class GroupResponse
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public float AmountReceived { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public int? StateId { get; set; }
       public List<GroupStallAssignmentResponse> groupStallAssignmentResponses { get; set; }
    }

    public class GroupStallAssignmentResponse
    {
        public int StallAssignmentId { get; set; }
        public int StallId { get; set; }
        public int StallAssignmentTypeId { get; set; }
        public int GroupId { get; set; }
        public int ExhibitorId { get; set; }
        public string BookedByType { get; set; }
        public string BookedByName { get; set; }
        public DateTime StallAssignmentDate { get; set; }
    }

    public class GroupListResponse
    {
        public int TotalRecords { get; set; }
        public List<GroupResponse> groupResponses { get; set; }
    }
    public class GetGroupExhibitors
    {
        public int GroupExhibitorId { get; set; }
        public int ExhibitorId { get; set; }
        public string ExhibitorName { get; set; }       
        public int? BirthYear { get; set; }
        public List<GroupExhibitorHorses> getGroupExhibitorHorses { get; set; }
    }
    public class GetAllGroupExhibitors
    {
        public List<GetGroupExhibitors> getGroupExhibitors { get; set; }
        public int TotalRecords { get; set; }
    }
    public class GroupExhibitorHorses
    {  
        public string HorseName { get; set; }
    }

    public class GetAllGroupsFinacialsModule
    {
        public List<GetGroupFinacialsTotals> getGroupFinacialsTotalsList { get; set; }
    }

  



    public class GetAllGroupFinacials
    {
        public List<GetGroupFinacials> getGroupFinacials {get;set;}
        public GetGroupFinacialsTotals getGroupFinacialsTotals {get;set; }
        public string GroupName { get; set; }
        public int GroupId { get; set; }

        public int TotalRecords { get; set; }
    }
  
    public class GetGroupFinacials
    {

        public int GroupFinancialId { get; set; }
        public DateTime Date { get; set; }
        public int FeeTypeId { get; set; }
        public string FeeTypeName { get; set; }
        public string FeeTimeFrame { get; set; }
        public int TimeFrameId { get; set; }
        public string TimeFrameName { get; set; }
        public double Amount { get; set; }

    }
    public class GetGroupFinacialsTotals
    {
        public string GroupName { get; set; }
        public int GroupId { get; set; }
        public double PreStallSum { get; set; }
        public double PreTackStallSum { get; set; }
        public double PreTotal { get; set; }
        public double PostStallSum { get; set; }
        public double PostTackStallSum { get; set; }
        public double PostTotal { get; set; }
        public double PrePostStallSum { get; set; }
        public double PrePostTackStallSum { get; set; }
        public double PrePostTotal { get; set; }

    }
    public class GetGroupInfo
    {
        public string GroupName { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string CityStateZip { get; set; }
        public string PhoneNumebr { get; set; }
        public string Email { get; set; }
    }
    public class GroupStatement
    {
        public double TotalHorseStall { get; set; }
        public double TotalTackStall { get; set; }
        public int StallQuantity { get; set; }
        public int TackStallQuantity { get; set; }
        public decimal Refund { get; set; }
        public decimal AmountDue { get; set; }
        public decimal ReceviedAmount { get; set; }
        public decimal OverPayment { get; set; }
    }
    public class GetStatementExhibitor
    {
        public int? BackNumber { get; set; }
        public string ExhibitorName { get; set; }
        public string HorseName { get; set; }
    }
    public class GetAllStatementExhibitor
    {
        public List<GetStatementExhibitor> getStatementExhibitors { get; set; }
        public int TotalRecords { get; set; }
    }
    public class GetGroupStatement
    {
        public AAYHSInfo aAYHSInfo { get; set; }
        public GetGroupInfo getGroupInfo { get; set; }
        public GroupStatement groupStatement { get; set; }
        public List<GetStatementExhibitor> getStatementExhibitors { get; set; }

    }
    public class AAYHSInfo
    {
        public string Email { get; set; }
        public string Address { get; set; }
        public string CityStateZip { get; set; }
        public string PhoneNumber { get; set; }
    }
}
