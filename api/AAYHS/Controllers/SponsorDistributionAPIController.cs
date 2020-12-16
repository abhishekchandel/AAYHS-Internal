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
    public class SponsorDistributionAPIController : ControllerBase
    {
        private readonly ISponsorDistributionService _SponsorDistributionService;
        private MainResponse _mainResponse;
        private string _jsonString = string.Empty;
        public SponsorDistributionAPIController(ISponsorDistributionService SponsorDistributionService)
        {
            _SponsorDistributionService = SponsorDistributionService;
            _mainResponse = new MainResponse();
        }

        /// <summary>
        /// This API is used to get all Sponsor distributions .
        /// </summary>
        /// <param name="Sponsor Id paAddUpdateSponsorDistributionrameter is required"></param>
        /// <returns> distributions  list</returns>
        [HttpGet]
        public ActionResult GetSponsorDistributionBySponsorId(int SponsorId)
        {
            _mainResponse = _SponsorDistributionService.GetSponsorDistributionBySponsorId(SponsorId);
            _jsonString = Mapper.Convert<SponsorDistributionListResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }


        /// <summary>
        /// This API is used to delete distributions Sponsor.
        /// </summary>
        /// <param name="Exhibitor Sponsor Id parameters is required"></param>
        /// <returns>Success  true or false with message</returns>
        [HttpDelete]
        public ActionResult DeleteSponsorDistribution(int SponsorDistributionId)
        {
            _mainResponse = _SponsorDistributionService.DeleteSponsorDistribution(SponsorDistributionId);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }

        /// <summary>
        /// This API is used to Add/update  distributions Sponsor.
        /// </summary>
        /// <param name="Exhibitor Sponsor detail parameter is required"></param>
        /// <returns> Success  true or false with message</returns>
        [HttpPost]
        public ActionResult AddUpdateSponsorDistribution([FromBody] SponsorDistributionRequest request)
        {
            _mainResponse = _SponsorDistributionService.AddUpdateSponsorDistribution(request);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
       
    }
}
