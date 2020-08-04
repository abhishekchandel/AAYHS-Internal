using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Service.IService;
using Microsoft.AspNetCore.Mvc;
using AAYHS.Core.Shared.Static;

namespace AAYHS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SponsorAPIController : ControllerBase
    {
        private readonly ISponsorService _SponsorService;
        private MainResponse _mainResponse;
        private string _jsonString = string.Empty;
        public SponsorAPIController(ISponsorService SponsorService)
        {
            _SponsorService = SponsorService;
            _mainResponse = new MainResponse();
        }

        /// <summary>
        /// This API is used to get all Sponsors.
        /// </summary>
        /// <param name="No parameter is required"></param>
        /// <returns>All Sponsors list</returns>
        [HttpGet]
        public ActionResult GetAllSponsors()
        {
            
                _mainResponse = _SponsorService.GetAllSponsors();
            _jsonString = Mapper.Convert<SponsorListResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }


        /// <summary>
        /// This API is used to get all Sponsors with filters.
        /// </summary>
        /// <param name="filter parameters is required"></param>
        /// <returns>All Sponsors list with filter</returns>
        [HttpPost]
        public ActionResult GetAllSponsorsWithFilter(BaseRecordFilterRequest request)
        {
         
                _mainResponse = _SponsorService.GetAllSponsorsWithFilter(request);
            _jsonString = Mapper.Convert<SponsorListResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }

        /// <summary>
        /// This API is used to get  Sponsor by Sponsor id.
        /// </summary>
        /// <param name="Sponsor id parameter is required"></param>
        /// <returns> Single Sponsor record</returns>
        [HttpGet]
        public ActionResult GetSponsorById(int SponsorId)
        {
                _mainResponse = _SponsorService.GetSponsorById(SponsorId);
            _jsonString = Mapper.Convert<SponsorResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }

        /// <summary>
        /// This API is used to add/update new  Sponsor.
        /// </summary>
        /// <param name="Sponsor detail is required"></param>
        /// <returns> success true or false with message</returns>
        [HttpPost]
        public ActionResult AddUpdateSponsor([FromBody] SponsorRequest request)
        {
                _mainResponse = _SponsorService.AddUpdateSponsor(request);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }


        /// <summary>
        /// This API is used to delete existing Sponsor.
        /// </summary>
        /// <param name="Sponsor detail with Sponsor id is required"></param>
        /// <returns> success true or false with message</returns>
        [HttpDelete]
        public ActionResult DeleteSponsor(int SponsorId)
        {
            _mainResponse = _SponsorService.DeleteSponsor(SponsorId);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to search the sponsor
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult  SearchSponsor(SearchRequest searchRequest)
        {

            _mainResponse = _SponsorService.SearchSponsor(searchRequest);
            _jsonString = Mapper.Convert<SponsorListResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
    }
}
