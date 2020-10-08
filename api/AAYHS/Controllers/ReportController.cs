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
        [HttpPost]
        public ActionResult GetExhibitorRegistrationReport(RegistrationReportRequest registrationReportRequest)
        {
            _mainResponse = _reportService.GetExhibitorRegistrationReport(registrationReportRequest);
            _jsonString = Mapper.Convert<GetExhibitorRegistrationReport>(_mainResponse);
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
    }
}
