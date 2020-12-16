using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.Shared.Static;
using AAYHS.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AAYHS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        #region readonly
        private readonly IClassService _classService;
        private MainResponse _mainResponse;
        private IReportService _reportService;
        #endregion

        #region private
        private string _jsonString = string.Empty;
        #endregion

        public ReportController(IReportService reportService)
        {
            _mainResponse = new MainResponse();
            _reportService = reportService;

        }

        /// <summary>
        /// This api used to get Registration Report
        /// </summary>
        /// <param name="registrationReportRequest"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetExhibitorRegistrationReport(int exhibitorIdst)
        {
            _mainResponse = _reportService.GetExhibitorRegistrationReport(exhibitorIdst);
            _jsonString = Mapper.Convert<GetExhibitorRegistrationReport>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get group exhibitor information Report
        /// </summary>
        /// <param name="groupinfoReportRequest"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExhibitorGroupInformationReport(int groupId)
        {
            _mainResponse = _reportService.GetExhibitorGroupInformationReport(groupId);
            _jsonString = Mapper.Convert<GetExhibitorGroupInformationReport>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get group exhibitor information Report for all groups
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetExhibitorGroupInformationReportForAllGroups()
        {
            _mainResponse = _reportService.GetExhibitorGroupInformationReportForAllGroups();
            _jsonString = Mapper.Convert<GetExhibitorGroupInformationReportForAllGroups>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get single program report
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetProgramsReport(int classId)
        {
            _mainResponse = _reportService.GetProgramsReport(classId);
            _jsonString = Mapper.Convert<GetProgramReport>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get program of all the classes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetProgramReportOfAllClasses()
        {
            _mainResponse = _reportService.GetProgramReportOfAllClasses();
            _jsonString = Mapper.Convert<GetProgramReportOfAllClasses>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get paddock report 
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPaddockReport(int classId)
        {
            _mainResponse = _reportService.GetPaddockReport(classId);
            _jsonString = Mapper.Convert<GetPaddockReport>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get paddock Report Of All Classes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPaddockReportOfAllClasses()
        {
            _mainResponse = _reportService.GetPaddockReportOfAllClasses();
            _jsonString = Mapper.Convert<GetPaddockReportOfAllClasses>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get all classes entries count
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAllClassesEntries(int classId)
        {
            _mainResponse = _reportService.GetAllClassesEntries(classId);
            _jsonString = Mapper.Convert<GetAllClassesEntries>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get all classes result
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetClassResultReport()
        {
            GetClassResultReport getClassResultReports = new GetClassResultReport();
            getClassResultReports = _reportService.GetClassResultReport();
            _jsonString = JsonConvert.SerializeObject(getClassResultReports);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get single class result report
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSingleClassResult(int classId)
        {
            _mainResponse = _reportService.GetSingleClassResult(classId);
            _jsonString = Mapper.Convert<GetSingleClassResult>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get sponsor list with amount received of single exhibitor and single horse.
        /// </summary>
        /// <param name="exhibitorId,horseId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetExhibitorSponsorConfirmation(HorseExhibitorsRequest request)
        {
            _mainResponse = _reportService.GetExhibitorSponsorConfirmation(request);
            _jsonString = Mapper.Convert<GetExhibitorSponsorConfirmationReport>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get sponsor list with amount received of all exhibitor and each horse.
        /// </summary>
        /// <param name="exhibitorId,horseId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetExhibitorSponsorConfirmationReportForAllExhibitors()
        {
            _mainResponse = _reportService.GetExhibitorSponsorConfirmationReportForAllExhibitors();
            _jsonString = Mapper.Convert<GetExhibitorSponsorConfirmationReportForAllExhibitors>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get exhibitor sponsor with amount received,refund of single exhibitor.
        /// </summary>
        /// <param name="exhibitorId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetExhibitorSponsorRefundReport(int exhibitorId)
        {
            _mainResponse = _reportService.GetExhibitorSponsorRefundReport(exhibitorId);
            _jsonString = Mapper.Convert<GetExhibitorSponsorRefundStatement>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        [HttpGet]
        public ActionResult GetExhibitorsSponsorRefundReportForAllExhibitors()
        {
            _mainResponse = _reportService.GetExhibitorsSponsorRefundReport();
            _jsonString = Mapper.Convert<GetExhibitorsSponsorRefundReport>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        [HttpPost]
        public ActionResult SaveAndEmail([FromBody]  EmailDocumentRequest1 data)
        {
            _mainResponse = _reportService.SaveAndEmail(data.reportfile, data.emailid);
            return new OkObjectResult(_mainResponse);
        }
        /// <summary>
        /// This api used to get all sponsors of a year 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AllPatronSponsorsYearly()
        {
            _mainResponse = _reportService.AllPatronSponsorsYearly();
            _jsonString = Mapper.Convert<AllSponsorsYealry>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get show summary report
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ShowSummaryReport()
        {
            _mainResponse = _reportService.ShowSummaryReport();
            _jsonString = Mapper.Convert<ShowSummaryReport>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }        
        /// <summary>
        /// This api used to get Administrative Report
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAdministrativeReport()
        {
            _mainResponse = _reportService.GetAdministrativeReport();
            _jsonString = Mapper.Convert<GetAdministrativeReport>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get nsba exhibitor fee report
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetNSBAExhibitorsFeeReport()
        {
            _mainResponse = _reportService.GetNSBAExhibitorsFeeReport();
            _jsonString = Mapper.Convert<GetNSBAExhibitorFee>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get nsba and classes exhibitor fee report
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetNSBAandClassesExhibitorsFeeReport()
        {
            _mainResponse = _reportService.GetNSBAandClassesExhibitorsFeeReport();
            _jsonString = Mapper.Convert<GetNSBAExhibitorFee>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get nsba classes result report
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetNSBAClassesResultReport()
        {
            GetClassResultReport getClassResultReports = new GetClassResultReport();
            getClassResultReports = _reportService.GetNSBAClassesResultReport();
            _jsonString = JsonConvert.SerializeObject(getClassResultReports);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get non exhibitor sponsor
        /// </summary>
        /// <param name="sponsorId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetNonExhibitorSponsor(int sponsorId)
        {
            _mainResponse = _reportService.GetNonExhibitorSponsor(sponsorId);
            _jsonString = Mapper.Convert<GetNonExhibitorSponsor>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get non exhibitor sponsor for all sponsors
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAllNonExhibitorSponsors()
        {
            _mainResponse = _reportService.GetAllNonExhibitorSponsors();
            _jsonString = Mapper.Convert<GetAllNonExhibitorSponsors>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get Exhibior Ads Sponsor Report
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetExhibiorAdsSponsorReport()
        {
            _mainResponse = _reportService.GetExhibiorAdsSponsorReport();
            _jsonString = Mapper.Convert<ExhibiorAdsSponsorReportListResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get non Exhibior summary Sponsor distributions Report
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetNonExhibiorSummarySponsorDistributionsReport()
        {
            _mainResponse = _reportService.GetNonExhibiorSummarySponsorDistributionsReport();
            _jsonString = Mapper.Convert<NonExhibiorSummarySponsorDistributionsListResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get all exhibitor of sponsor type ad
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAllExhibitorSponsoredAd()
        {
            _mainResponse = _reportService.GetAllExhibitorSponsoredAd();
            _jsonString = Mapper.Convert<GetAllExhibitorSponsoredAd>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get all non exhibitor sponsor type ad
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAllNonExhibitorSponsorAd()
        {
            _mainResponse = _reportService.GetAllNonExhibitorSponsorAd();
            _jsonString = Mapper.Convert<GetAllNonExhibitorSponsorAd>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
    }
}
