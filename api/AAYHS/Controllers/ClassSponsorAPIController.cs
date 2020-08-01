using System;
using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.Shared.Static;
using AAYHS.Service.IService;
using Microsoft.AspNetCore.Mvc;


namespace AAYHS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClassSponsorAPIController : ControllerBase
    {
        private readonly IClassSponsorService _ClassSponsorService;
        private MainResponse _mainResponse;
        private string _jsonString = string.Empty;
        public ClassSponsorAPIController(IClassSponsorService ClassClassSponsorService)
        {
            _ClassSponsorService = ClassClassSponsorService;
            _mainResponse = new MainResponse();
        }


        /// <summary>
        /// This API is used to get all Class Sponsors.
        /// </summary>
        /// <param name="No parameter is required"></param>
        /// <returns>All Class Sponsors list</returns>
        [HttpGet]
        public ActionResult GetAllClassSponsors()
        {

            _mainResponse = _ClassSponsorService.GetAllClassSponsor();
            _jsonString = Mapper.Convert<ClassSponsorListResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }


        /// <summary>
        /// This API is used to get all Class Sponsors with filters.
        /// </summary>
        /// <param name="filter parameters is required"></param>
        /// <returns>All Class Sponsors list with filter</returns>
        [HttpPost]
        public ActionResult GetAllClassSponsorsWithFilter([FromBody] BaseRecordFilterRequest request)
        {

            _mainResponse = _ClassSponsorService.GetAllClassSponsorWithFilter(request);
            _jsonString = Mapper.Convert<ClassSponsorListResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }

        /// <summary>
        /// This API is used to get Class Sponsor by Class Sponsor id.
        /// </summary>
        /// <param name="Class Sponsor id parameter is required"></param>
        /// <returns> Single Class Sponsor record</returns>
        [HttpPost]
        public ActionResult GetClassSponsorById([FromBody] GetClassSponsorRequest request)
        {
            _mainResponse = _ClassSponsorService.GetClassSponsorbyId(request);
            _jsonString = Mapper.Convert<ClassSponsorResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }

        /// <summary>
        /// This API is used to add new Class Sponsor.
        /// </summary>
        /// <param name="Class Sponsor detail is required"></param>
        /// <returns> success true or false with message</returns>
        [HttpPost]
        public ActionResult AddUpdateClassSponsor([FromBody] ClassSponsorRequest request)
        {
            _mainResponse = _ClassSponsorService.AddUpdateClassSponsor(request);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }


        /// <summary>
        /// This API is used to delete existing Class Sponsor.
        /// </summary>
        /// <param name="Class Sponsor detail with Class Sponsor id is required"></param>
        /// <returns> success true or false with message</returns>
        [HttpDelete]
        public ActionResult DeleteClassSponsor([FromBody] GetClassSponsorRequest request)
        {
            _mainResponse = _ClassSponsorService.DeleteClassSponsor(request);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        [HttpPost]
        public ActionResult GetSponsorClassesbySponsorId([FromBody] GetBySponsorIdRequest request)
        {
            _mainResponse = _ClassSponsorService.GetSponsorClassesbySponsorId(request);
            _jsonString = Mapper.Convert<SponsorClassesListResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
    }
}
