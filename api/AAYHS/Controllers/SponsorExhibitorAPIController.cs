using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AAYHS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SponsorExhibitorAPIController : ControllerBase
    {
        private readonly ISponsorExhibitorService _SponsorExhibitorService;
        private MainResponse _response;
        public SponsorExhibitorAPIController(ISponsorExhibitorService SponsorExhibitorService)
        {
            _SponsorExhibitorService = SponsorExhibitorService;
            _response = new MainResponse();
        }

        /// <summary>
        /// This API is used to get all Sponsor Exhibitors .
        /// </summary>
        /// <param name="Sponsor Id parameter is required"></param>
        /// <returns> Exhibitors Sponsor list</returns>
        [HttpPost]
        public ActionResult GetSponsorExhibitorBySponsorId(GetSponsorExhibitorRequest request)
        {
            _response = _SponsorExhibitorService.GetSponsorExhibitorBySponsorId(request);
            return new OkObjectResult(_response);
        }


        /// <summary>
        /// This API is used to delete Exhibitors Sponsor.
        /// </summary>
        /// <param name="Exhibitor Sponsor Id parameters is required"></param>
        /// <returns>Success  true or false with message</returns>
        [HttpDelete]
        public ActionResult DeleteSponsorExhibitor([FromBody] DeleteSponsorExhibitorRequest request)
        {
            _response = _SponsorExhibitorService.DeleteSponsorExhibitor(request);
            return new OkObjectResult(_response);
        }

        /// <summary>
        /// This API is used to Add  Exhibitor Sponsor.
        /// </summary>
        /// <param name="Exhibitor Sponsor detail parameter is required"></param>
        /// <returns> Success  true or false with message</returns>
        [HttpPost]
        public ActionResult AddSponsorExhibitor([FromBody] SponsorExhibitorRequest request)
        {
            _response = _SponsorExhibitorService.AddSponsorExhibitor(request);
            return new OkObjectResult(_response);
        }
        [HttpPost]
        public ActionResult UpdateSponsorExhibitor([FromBody] SponsorExhibitorRequest request)
        {
            _response = _SponsorExhibitorService.UpdateSponsorExhibitor(request);
            return new OkObjectResult(_response);
        }
    }
}
