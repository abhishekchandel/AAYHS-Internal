using AAYHS.Core.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response
{
    public class GetExhibitorRegistrationReport
    {
     
        public ExhibitorInfo getExhibitorInfo { get; set; }
        public GetAAYHSContactInfo getAAYHSContactInfo { get; set; }
        public StallAndTackStallNumber stallAndTackStallNumber { get; set; }
        public List<HorseClassDetail> horseClassDetails { get; set; }
        public FinancialsDetail financialsDetail { get; set; }

    }
    public class GetExhibitorGroupInformationReport
    {
        public GroupInfo getGroupInfo { get; set; }
        public GetAAYHSContactInfo getAAYHSContactInfo { get; set; }
        public StallAndTackStallNumber stallAndTackStallNumber { get; set; }
        public List<GetGroupExhibitors> exhibitorDetails { get; set; }
        public FinancialsDetail financialsDetail { get; set; }

    }
    public class GetExhibitorGroupInformationReportForAllGroups
    {
        public List<GetExhibitorGroupInformationReport> getExhibitorGroupInformationReports { get; set; }
        public GetAAYHSContactInfo getAAYHSContactInfo { get; set; }
    }
    public class GetExhibitorSponsorConfirmationReport
    {
        public GetAAYHSContactInfo getAAYHSContactInfo { get; set; }
        public ExhibitorInfo exhibitorinfo { get; set; }
        public List<HorseInfo> horseinfo { get; set; }
        public string ReportText { get; set; }
    }
    public class GetExhibitorSponsorConfirmationReportForAllExhibitors
    {
        public List<GetExhibitorSponsorConfirmationReport> getExhibitorSponsorConfirmationReports { get; set; }
        public GetAAYHSContactInfo getAAYHSContactInfo { get; set; }
        public string ReportText { get; set; }
    }

    public class HorseInfo
    {
        public int ExhibitorHorseId { get; set; }
        public string HorseName { get; set; }
        public int HorseId { get; set; }
        public int HorseTypeId { get; set; }
        public string HorseTypeName { get; set; }
        public List<Sponsordetail> sponsordetail { get; set; }
        public decimal SponsorAmountTotal { get; set; }


    }

    public class Sponsordetail
    {
        public string SponsorName { get; set; }
        public int SponsorId { get; set; }
        public decimal Amount { get; set; }
    }
    public class GroupInfo
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string StateZipcode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

    }
    public class GetAAYHSContactInfo
    {
        public string Email1 { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string StateZipcode { get; set; }
        public string Phone1 { get; set; }
        public string ShowDates { get; set; }
    }
    public class StallAndTackStallNumber
    {
        public List<HorseStall> horseStalls { get; set; }
        public List<TackStall> tackStalls { get; set; }
        public int ExhibitorId { get; set; }
        public int? ExhibitorBirthYear { get; set; }
    }
    public class HorseStall
    {
        public int? HorseStallNumber { get; set; }
    }
    public class TackStall
    {
        public int? TackStallNumber { get; set; }
    }
    public class HorseClassDetail
    {
        public string HorseName { get; set; }
        public int? BackNumber { get; set; }
        public string ClassNumber { get; set; }
        public string ClassName { get; set; }
    }
    public class FinancialsDetail
    {
        public int ClassQty { get; set; }
        public decimal ClassAmount { get; set; }
        public int NSBAClassQty { get; set; }
        public decimal NSBAClassAmount { get; set; }
        public int AdministrationQty { get; set; }
        public decimal AdministrationAmount { get; set; }
        public int AdditionalProgramQty { get; set; }
        public decimal AdditionalProgramAmount { get; set; }

        public int HorseStallQty { get; set; }       
        public decimal HorseStallAmount { get; set; }
        public int TackStallQty { get; set; }
        public decimal TackStallAmount { get; set; }
        public decimal Refund { get; set; }
        public decimal AmountDue { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal Overpayment { get; set; }
        public decimal BalanceDue { get; set; }

    }
    public class GetProgramReportOfAllClasses
    {
        public List<GetProgramReport> getProgramReport { get; set; }
    }
    public class GetProgramReport
    {
        public string ClassNumber { get; set; }
        public string ClassName { get; set; }
        public string Age { get; set; }
        public List< SponsorInfo> sponsorInfo { get; set; }
        public List< ClassInfo> classInfo { get; set; }
    }
    public class SponsorInfo
    {
        public string SponsorName { get; set; }
        public string City { get; set; }
        public string StateZipcode { get; set; }
    }
    public class ClassInfo
    {
        public int? BackNumber { get; set; }
        public string NSBA { get; set; }
        public string HorseName { get; set; }
        public string ExhibitorName { get; set; }        
        public string CityState { get; set; }
    }

    public class GetPaddockReport
    {
        public string ClassNumber { get; set; }
        public string ClassName { get; set; }
        public string Age { get; set; }
        public List<ClassDetail> classDetails { get; set; }
    }
    public class ClassDetail
    {
        public int? BackNumber { get; set; }
        public string Scratch { get; set; }
        public string NSBA { get; set; }
        public string HorseName { get; set; }
        public string ExhibitorName { get; set; }
        public string CityState { get; set; }
        public int Split { get; set; }
    }
    public class GetPaddockReportOfAllClasses
    {
        public List<GetPaddockReport> getPaddockReport { get; set; }
    }
    public class GetAllClassesEntries
    {
        public List<GetClassEntriesCount> getClassEntriesCount { get; set; }
    }
    public class GetClassEntriesCount
    {
        public string ClassNumber { get; set; }
        public string ClassName { get; set; }
        public string ClassAgeGroup { get; set; }
        public int EntryTotal { get; set; }
    }
    public class GetClassResultReport
    {
        public List<GetClassesResult> getClassesResult { get; set; }
    }
    public class GetClassesResult
    {
        public string ClassHeader { get; set; }
        public List<GetClassesInfoAndResult> getClassesInfoAndResult { get; set; }
    }           
    public class GetClassesInfoAndResult
    {        
        public string ClassNumber { get; set; }
        public string ClassName { get; set; }
        public string AgeGroup { get; set; }
        public List<GetClassesSponsors> getClassesSponsors { get; set; }
        public List<GetClassResult> getClassResults { get; set; }
    }
    public class GetClassesSponsors
    {
        public string SponsorName { get; set; }
    }
    public class GetClassResult
    {
        public int Place { get; set; }
        public int? BackNumber { get; set; }
        public string ExhibitorName { get; set; }
        public string HorseName { get; set; }
        public string GroupName { get; set; }
    }
}
public class ExhibitorInfo
{
    public int ExhibitorId { get; set; }
    public string ExhibitorName { get; set; }
    public string Address { get; set; }
    public string CityName { get; set; }
    public string StateZipcode { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public int QtyProgram { get; set; }

}
public class GetClassResultReport
{
    public List<GetClassesResult> getClassesResult { get; set; }
    public string Message { get; set; }    
    public bool Success { get; set; }
}
public class GetClassesResult
{
    public string ClassHeader { get; set; }
    public List<GetClassesInfoAndResult> getClassesInfoAndResult { get; set; }
}
public class GetClassesInfoAndResult
{
    public string ClassHeader { get; set; }
    public string ClassNumber { get; set; }
    public string ClassName { get; set; }
    public string AgeGroup { get; set; }
    public List<GetClassesSponsors> getClassesSponsors { get; set; }
    public List<GetClassResult> getClassResults { get; set; }
}
public class GetClassesSponsors
{
    public string SponsorName { get; set; }
}
public class GetClassResult
{
    public int Place { get; set; }
    public int? BackNumber { get; set; }
    public string ExhibitorName { get; set; }
    public string HorseName { get; set; }
    public string GroupName { get; set; }
}
public class GetSingleClassResult
{
    public string ClassHeader { get; set; }
    public string ClassNumber { get; set; }
    public string ClassName { get; set; }
    public string AgeGroup { get; set; }
    public List<GetClassesSponsors> getClassSponsors { get; set; }
    public List<GetClassResult> getClassResults { get; set; }
}
public class GetExhibitorSponsorRefundStatement
{
    public GetAAYHSContactInfo getAAYHSContactInfo { get; set; }
    public ExhibitorInfo exhibitorInfo { get; set; }
    public List<HorsesSponsor> horsesSponsors { get; set; }    
}

public class HorsesSponsor
{
    public int ExhibitorId { get; set; }
    public int HorseId { get; set; }
    public string HorseName { get; set; }
    public List<HorseSponsorInfo> horseSponsorInfos { get; set; }
    public decimal TotalAmount { get; set; }
    public RefundableCosts refundableCosts { get; set; }
    public ShowCosts showCosts { get; set; }
    public decimal TotalShowCost { get; set; }
}

public class RefundableCosts
{
    public int? Incentive { get; set; }
    public string IncentiveText { get; set; }
}
public class HorseSponsorInfo
{
    public int SponsorId { get; set; }
    public string SponsorName { get; set; }
    public decimal Amount { get; set; }
}

public class ShowCosts
{
    public int ExhibitorId { get; set; }
    public decimal ClassFee { get; set; }
    public decimal HorseStallFee { get; set; }
    public decimal TackStallFee { get; set; }
    public decimal ProgramFee { get; set; }
}
public class AllSponsorsYealry
{
    public List<SponsorsYealry>  sponsors { get; set; }
}
public class SponsorsYealry
{
    public int SponsorId { get; set; }
    public string SponsorName { get; set; }
}
public class ShowSummaryReport
{
    public int TotalNumberOfExhibiitors { get; set; }
    public NumberOfClasses numberOfClasses { get; set; }
    public NumberOfNSBAClasses numberOfNSBAClasses { get; set; }
    public GeneralFees generalFees { get; set; }
    public NumberOfStall numberOfStall { get; set; }
}
public class NumberOfClasses
{
    public int TotalClasses { get; set; }
    public int TotalPreEntries { get; set; }
    public int TotalPostEntries { get; set; }
    public int TotalEntries { get; set; }
}
public class NumberOfNSBAClasses
{
    public int TotalNSBAClasses { get; set; }
    public int TotalPreEntries { get; set; }
    public int TotalPostEntries { get; set; }
    public int TotalEntries { get; set; }
}
public class GeneralFees
{
    public decimal AdminstrationsFee { get; set; }
    public decimal BoxStallFee { get; set; }
    public decimal TackStallFee { get; set; }
    public decimal ClassEntryFee { get; set; }
    public decimal ProgramFee { get; set; }
    public decimal NsbaEntryFee { get; set; }
    public decimal TotalFees { get; set; }
}
public class NumberOfStall
{
    public int TotalPortableStalls { get; set; }
    public int TotalPermanentStalls { get; set; }
    public int TotalStalls { get; set; }
}
public class GetExhibitorsSponsorRefundReport
{
    public GetAAYHSContactInfo getAAYHSContactInfo { get; set; }
    public List<ExhibitorsHorseAndSponsors> exhibitorsHorseAndSponsors { get; set; }
}
public class ExhibitorsHorseAndSponsors
{
    public ExhibitorInfo exhibitorInfo { get; set; }
    public List<HorsesSponsor> horsesSponsors { get; set; }
}
public class GetAdministrativeReport
{
    public GetFeeCategories getFeeCategories { get; set; }
    public GetStatement getStatement { get; set; }
    public GetAAYHSInfo getAAYHSInfo { get; set; }
}
public class GetFeeCategories
{
    public List<GetAdFee> getAdFees { get; set; }
    public List<GetClassCategories> getClassCategories { get; set; }
    public List<GetGeneralFee> getGeneralFees { get; set; }
    public List<GetScratchRefund> getScratchRefunds { get; set; }
    public List<GetIncentiveRefund> getIncentiveRefunds { get; set; }
}
public class GetAdFee
{
    public int YearlyMaintainenceFeeId { get; set; }
    public string FeeName { get; set; }
    public decimal Amount { get; set; }
    public bool Active { get; set; }
}
public class GetClassCategories
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
}
public class GetGeneralFee
{
    public int YearlyMaintenanceFeeId { get; set; }
    public string TimeFrame { get; set; }
    public string FeeName { get; set; }
    public decimal Amount { get; set; }
    public bool Active { get; set; }
}
public class GetScratchRefund
{
    public int RefundId { get; set; }
    public DateTime DateAfter { get; set; }
    public DateTime DateBefore { get; set; }
    public string RefundType { get; set; }
    public decimal Refund { get; set; }
    public bool Active { get; set; }
}
public class GetIncentiveRefund
{
    public int SponsorIncentiveId { get; set; }
    public decimal SponsorAmount { get; set; }
    public int Award { get; set; }
}
public class GetStatement
{
    public List<SponsorRefundStatement> sponsorRefundStatements { get; set; }
}
public class SponsorRefundStatement
{
    public int YearlyStatementTextId { get; set; }
    public string StatementName { get; set; }
    public string StatementNumber { get; set; }
    public string StatementText { get; set; }
    public int? Incentive { get; set; }
}
public class GetAAYHSInfo
{
    public int AAYHSContactId { get; set; }
    public string ShowLocation { get; set; }
    public string Email1 { get; set; }
    public string Email2 { get; set; }
    public string Phone1 { get; set; }
    public string Phone2 { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public ExhibitorSponsorConfirmation exhibitorSponsorConfirmation { get; set; }
    public ExhibitorRefundStatement exhibitorRefundStatement { get; set; }
    public ConfirmationEntriesAndStalls confirmationEntriesAndStalls { get; set; }

}
public class ExhibitorSponsorConfirmation
{
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
}
public class ExhibitorRefundStatement
{
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
}
public class ConfirmationEntriesAndStalls
{
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
}
public class GetNSBAExhibitorFee
{
    public List<NSBAExhibitorFee> nSBAExhibitorFee { get; set; }
}
public class NSBAExhibitorFee
{
    public int? BackNumber { get; set; }
    public int ExhibitorId { get; set; }
    public string Exhibitor { get; set; }
    public int HorseId { get; set; }
    public string Horse { get; set; }
    public decimal PreEntryTotal { get; set; }
    public decimal PostEntryTotal { get; set; }
}
public class GetNonExhibitorSponsor
{
    public int SponsorId { get; set; }
    public string SponsorName { get; set; }
    public decimal Total { get; set; }
    public List<NonExhibitorSponsorType> nonExhibitorSponsorTypes { get; set; }
    public decimal TotalDistribution { get; set; }
    public decimal Remaining { get; set; }
}
public class NonExhibitorSponsorType
{
    public string SponsorType { get; set; }
    public decimal Contribution { get; set; }
    public string IDNumber { get; set; }
    public string AdSize { get; set; }    
}
public class GetAllNonExhibitorSponsors
{
    public List<GetNonExhibitorSponsor> getNonExhibitorSponsors { get; set; }
}

public class ExhibiorAdsSponsorReportResponse
{
    public int ExhibitorId { get; set; }
    public string ExhibitorName { get; set; }
    public string ExhibitorEmail { get; set; }
    public int HorseId { get; set; }
    public string HorseName { get; set; }
    public string BackNumber { get; set; }
    public string AdId { get; set; }
    public int SponsorId { get; set; }
    public string SponsorName { get; set; }
    public string SponsorEmail { get; set; }
    public decimal Amount { get; set; }
}
public class ExhibiorAdsSponsorReportListResponse
{
    public int TotalRecords { get; set; } = 0;
    public decimal TotalAmount { get; set; } = 0;
    public List<ExhibiorAdsSponsorReportResponse> exhibiorAdsSponsorReportResponses { get; set; }
}
public class NonExhibiorSummarySponsorDistributionsResponse
{
   
    public int SponsorId { get; set; }
    public string SponsorName { get; set; }
    public decimal AmountReceived { get; set; }
    public decimal NonExhibitorContribution { get; set; }
    public decimal ExhibitorContribution { get; set; }
    public decimal Remaining { get; set; }
}
public class NonExhibiorSummarySponsorDistributionsListResponse
{
    public int TotalRecords { get; set; } = 0;
    public decimal TotalReceived { get; set; } = 0;
    public decimal TotalNonExhibitorContribution { get; set; } = 0;
    public decimal TotalExhibitorContribution { get; set; } = 0;
    public decimal TotalRemaining { get; set; } = 0;
    public List<NonExhibiorSummarySponsorDistributionsResponse> nonExhibiorSummarySponsorDistributionsResponses { get; set; }
}
public class GetExhibitorSponsoredAd
{
    public int ExhibitorId { get; set; }
    public string ExhibitorName { get; set; }
    public string AdNumber { get; set; }
    public string SponsorName { get; set; }
    public decimal Amount { get; set; }
}
public class GetAllExhibitorSponsoredAd
{
    public List<GetExhibitorSponsoredAd> getExhibitorSponsoredAds { get; set; }
    public decimal TotalAmount { get; set; }
}
public class GetAllNonExhibitorSponsorAd
{
    public List<GetNonExhibitorSponsorAd> getNonExhibitorSponsorAds { get; set; }
    public decimal TotalAmount { get; set; }
}
public class GetNonExhibitorSponsorAd
{
    public int SponsorId { get; set; }
    public string SponsorName { get; set; }
    public string AdId { get; set; }
    public decimal Amount { get; set; }
}


