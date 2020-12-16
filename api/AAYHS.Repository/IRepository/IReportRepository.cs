using System;
using System.Collections.Generic;
using System.Text;
using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;

namespace AAYHS.Repository.IRepository
{
    public interface IReportRepository
    {
        GetExhibitorRegistrationReport GetExhibitorRegistrationReport(int exhibitorId);
        GetExhibitorGroupInformationReport GetExhibitorGroupInformationReport(int groupId);
        GetExhibitorGroupInformationReportForAllGroups GetExhibitorGroupInformationReportForAllGroups();
        GetProgramReport GetProgramsReport(int classId);
        GetProgramReportOfAllClasses GetProgramReportOfAllClasses();
        GetPaddockReport GetPaddockReport(int classId);
        GetPaddockReportOfAllClasses GetPaddockReportOfAllClasses();
        GetAllClassesEntries GetAllClassesEntries(int classId);
        GetClassResultReport GetClassResultReport();
        GetSingleClassResult GetSingleClassResult(int classId);
        GetExhibitorSponsorConfirmationReport GetExhibitorSponsorConfirmation(HorseExhibitorsRequest request);
        GetExhibitorSponsorConfirmationReportForAllExhibitors GetExhibitorSponsorConfirmationReportForAllExhibitors();
        GetExhibitorSponsorRefundStatement GetExhibitorSponsorRefundReport(int exhibitorId);
        AllSponsorsYealry AllPatronSponsorsYearly();
        ShowSummaryReport ShowSummaryReport();
        GetExhibitorsSponsorRefundReport GetExhibitorsSponsorRefundReport();
        GetAdministrativeReport GetAdministrativeReport();
        GetNSBAExhibitorFee GetNSBAExhibitorsFeeReport();
        GetNSBAExhibitorFee GetNSBAandClassesExhibitorsFeeReport();      
        GetClassResultReport GetNSBAClassesResultReport();
        GetNonExhibitorSponsor GetNonExhibitorSponsor(int sponsorId);
        GetAllNonExhibitorSponsors GetAllNonExhibitorSponsors();
        ExhibiorAdsSponsorReportListResponse GetExhibiorAdsSponsorReport();
        NonExhibiorSummarySponsorDistributionsListResponse GetNonExhibiorSummarySponsorDistributionsReport();
        GetAllExhibitorSponsoredAd GetAllExhibitorSponsoredAd();
        GetAllNonExhibitorSponsorAd GetAllNonExhibitorSponsorAd();
    }
}

