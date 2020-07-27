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
    public class ClassSponsorAPIController : ControllerBase
    {
        private readonly IClassSponsorService _ClassSponsorService;
        private MainResponse _response;
        public ClassSponsorAPIController(IClassSponsorService ClassClassSponsorService)
        {
            _ClassSponsorService = ClassClassSponsorService;
            _response = new MainResponse();
        }


        /// <summary>
        /// This API is used to get all Class Sponsors.
        /// </summary>
        /// <param name="No parameter is required"></param>
        /// <returns>All Class Sponsors list</returns>
        [HttpGet]
        public ActionResult GetAllClassSponsors()
        {

            _response = _ClassSponsorService.GetAllClassSponsor();
            return new OkObjectResult(_response);
        }


        /// <summary>
        /// This API is used to get all Class Sponsors with filters.
        /// </summary>
        /// <param name="filter parameters is required"></param>
        /// <returns>All Class Sponsors list with filter</returns>
        [HttpGet]
        public ActionResult GetAllClassSponsorsWithFilter([FromBody] BaseRecordFilterRequest request)
        {

            _response = _ClassSponsorService.GetAllClassSponsorWithFilter(request);
            return new OkObjectResult(_response);
        }

        /// <summary>
        /// This API is used to get Class Sponsor by Class Sponsor id.
        /// </summary>
        /// <param name="Class Sponsor id parameter is required"></param>
        /// <returns> Single Class Sponsor record</returns>
        [HttpGet]
        public ActionResult GetClassSponsorById([FromBody] GetClassSponsorRequest request)
        {
            _response = _ClassSponsorService.GetClassSponsorbyId(request);
            return new OkObjectResult(_response);
        }

        /// <summary>
        /// This API is used to add new Class Sponsor.
        /// </summary>
        /// <param name="Class Sponsor detail is required"></param>
        /// <returns> success true or false with message</returns>
        [HttpPost]
        public ActionResult AddClassSponsor([FromBody] ClassSponsorRequest request)
        {
            _response = _ClassSponsorService.AddClassSponsor(request);
            return new OkObjectResult(_response);
        }

        /// <summary>
        /// This API is used to update existing Class Sponsor.
        /// </summary>
        /// <param name="Class Sponsor detail with Class Sponsor id is required"></param>
        /// <returns> success true or false with message</returns>
        [HttpPut]
        public ActionResult UpdateClassSponsor([FromBody] ClassSponsorRequest request)
        {
            _response = _ClassSponsorService.UpdateClassSponsor(request);
            return new OkObjectResult(_response);
        }

        /// <summary>
        /// This API is used to delete existing Class Sponsor.
        /// </summary>
        /// <param name="Class Sponsor detail with Class Sponsor id is required"></param>
        /// <returns> success true or false with message</returns>
        [HttpDelete]
        public ActionResult DeleteClassSponsor([FromBody] GetClassSponsorRequest request)
        {
            _response = _ClassSponsorService.DeleteClassSponsor(request);
            return new OkObjectResult(_response);
        }

    }
}
