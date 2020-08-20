using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Service.IService;
using Microsoft.AspNetCore.Mvc;
using AAYHS.Core.Shared.Static;

namespace AAYHS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdvertisementAPIController : ControllerBase
    {
        private readonly IAdvertisementService _AdvertisementService;
        private MainResponse _mainResponse;
        private string _jsonString = string.Empty;
        public AdvertisementAPIController(IAdvertisementService AdvertisementService)
        {
            _AdvertisementService = AdvertisementService;
            _mainResponse = new MainResponse();
        }


        /// <summary>
        /// This API is used to get all advertisements with filters.
        /// </summary>
        /// <param name="filter parameters is required"></param>
        /// <returns>All advertisements list with filter</returns>
        [HttpPost]
        public ActionResult GetAllAdvertisements(BaseRecordFilterRequest request)
        {
            _mainResponse = _AdvertisementService.GetAllAdvertisements(request);
            _jsonString = Mapper.Convert<AdvertisementListResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }




    }
}
