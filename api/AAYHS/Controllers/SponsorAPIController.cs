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
    public class SponsorAPIController : ControllerBase
    {
        private readonly ISponsorService _SponsorService;
        private MainResponse _response;
        public SponsorAPIController(ISponsorService SponsorService)
        {
            _SponsorService = SponsorService;
            _response = new MainResponse();
        }

        /// <summary>
        /// This API is used to get all Sponsors.
        /// </summary>
        /// <param name="No parameter is required"></param>
        /// <returns>All Sponsors list</returns>
        [HttpGet]
        public ActionResult GetAllSponsors()
        {
            
                _response = _SponsorService.GetAllSponsors();
                return new OkObjectResult(_response);
        }


        /// <summary>
        /// This API is used to get all Sponsors with filters.
        /// </summary>
        /// <param name="filter parameters is required"></param>
        /// <returns>All Sponsors list with filter</returns>
        [HttpPost]
        public ActionResult GetAllSponsorsWithFilter(BaseRecordFilterRequest request)
        {
         
                _response = _SponsorService.GetAllSponsorsWithFilter(request);
                return new OkObjectResult(_response);
        }

        /// <summary>
        /// This API is used to get  Sponsor by Sponsor id.
        /// </summary>
        /// <param name="Sponsor id parameter is required"></param>
        /// <returns> Single Sponsor record</returns>
        [HttpGet]
        public ActionResult GetSponsorById(int sponsorId)
        {
                _response = _SponsorService.GetSponsorById(sponsorId);
                return new OkObjectResult(_response);
        }

        /// <summary>
        /// This API is used to add new  Sponsor.
        /// </summary>
        /// <param name="Sponsor detail is required"></param>
        /// <returns> success true or false with message</returns>
        [HttpPost]
        public ActionResult AddSponsor([FromBody] SponsorRequest request)
        {
                _response = _SponsorService.AddSponsor(request);
                return new OkObjectResult(_response);
        }

        /// <summary>
        /// This API is used to update existing Sponsor.
        /// </summary>
        /// <param name="Sponsor detail with Sponsor id is required"></param>
        /// <returns> success true or false with message</returns>
        [HttpPut]
        public ActionResult UpdateSponsor([FromBody] SponsorRequest request)
        {
                _response = _SponsorService.UpdateSponsor(request);
                return new OkObjectResult(_response);
        }

        /// <summary>
        /// This API is used to delete existing Sponsor.
        /// </summary>
        /// <param name="Sponsor detail with Sponsor id is required"></param>
        /// <returns> success true or false with message</returns>
        [HttpDelete]
        public ActionResult DeleteSponsor(int sponsorId)
        {
            _response = _SponsorService.DeleteSponsor(sponsorId);
            return new OkObjectResult(_response);
        }

    }
}
