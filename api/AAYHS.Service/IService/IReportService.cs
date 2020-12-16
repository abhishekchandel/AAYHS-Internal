using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Service.IService
{
    public interface IReportService
    {
        MainResponse GetExhibitorRegistrationReport(int exhibitorId);
        MainResponse GetExhibitorGroupInformationReport(int groupId);
        MainResponse GetExhibitorGroupInformationReportForAllGroups();
        MainResponse GetProgramsReport(int classId);
        MainResponse GetProgramReportOfAllClasses();
        MainResponse GetPaddockReport(int classId);
        MainResponse GetPaddockReportOfAllClasses();
        MainResponse GetAllClassesEntries(int classId);
        GetClassResultReport GetClassResultReport();
        MainResponse GetSingleClassResult(int classId);
        MainResponse GetExhibitorSponsorConfirmation(HorseExhibitorsRequest request);
        MainResponse GetExhibitorSponsorConfirmationReportForAllExhibitors();
        MainResponse SaveAndEmail(string datauristring, string emailid);
        MainResponse GetExhibitorSponsorRefundReport(int exhibitorId);
        MainResponse AllPatronSponsorsYearly();
        MainResponse ShowSummaryReport();
        MainResponse GetExhibitorsSponsorRefundReport();
        MainResponse GetAdministrativeReport();
        MainResponse GetNSBAExhibitorsFeeReport();
        MainResponse GetNSBAandClassesExhibitorsFeeReport();
        GetClassResultReport GetNSBAClassesResultReport();
        MainResponse GetNonExhibitorSponsor(int sponsorId);
        MainResponse GetAllNonExhibitorSponsors();
        MainResponse GetExhibiorAdsSponsorReport();
        MainResponse GetNonExhibiorSummarySponsorDistributionsReport();
        MainResponse GetAllExhibitorSponsoredAd();
        MainResponse GetAllNonExhibitorSponsorAd();
    }
}
