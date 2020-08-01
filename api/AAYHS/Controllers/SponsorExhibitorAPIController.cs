using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Core.Shared.Static;


namespace AAYHS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SponsorExhibitorAPIController : ControllerBase
    {
        private readonly ISponsorExhibitorService _SponsorExhibitorService;
        private MainResponse _mainResponse;
        private string _jsonString = string.Empty;
        public SponsorExhibitorAPIController(ISponsorExhibitorService SponsorExhibitorService)
        {
            _SponsorExhibitorService = SponsorExhibitorService;
            _mainResponse = new MainResponse();
        }

        /// <summary>
        /// This API is used to get all Sponsor Exhibitors .
        /// </summary>
        /// <param name="Sponsor Id parameter is required"></param>
        /// <returns> Exhibitors Sponsor list</returns>
        [HttpGet]
        public ActionResult GetSponsorExhibitorBySponsorId(int SponsorId)
        {
            _mainResponse = _SponsorExhibitorService.GetSponsorExhibitorBySponsorId(SponsorId);
            _jsonString = Mapper.Convert<SponsorExhibitorListResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }


        /// <summary>
        /// This API is used to delete Exhibitors Sponsor.
        /// </summary>
        /// <param name="Exhibitor Sponsor Id parameters is required"></param>
        /// <returns>Success  true or false with message</returns>
        [HttpDelete]
        public ActionResult DeleteSponsorExhibitor(int SponsorExhibitorId)
        {
            _mainResponse = _SponsorExhibitorService.DeleteSponsorExhibitor(SponsorExhibitorId);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }

        /// <summary>
        /// This API is used to Add/update  Exhibitor Sponsor.
        /// </summary>
        /// <param name="Exhibitor Sponsor detail parameter is required"></param>
        /// <returns> Success  true or false with message</returns>
        [HttpPost]
        public ActionResult AddUpdateSponsorExhibitor([FromBody] SponsorExhibitorRequest request)
        {
            _mainResponse = _SponsorExhibitorService.AddUpdateSponsorExhibitor(request);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
       
    }
}
