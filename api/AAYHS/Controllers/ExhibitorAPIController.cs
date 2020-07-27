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
    public class ExhibitorAPIController : ControllerBase
    {
        private readonly IExhibitorService _ExhibitorService;
        private MainResponse _response;
        public ExhibitorAPIController(IExhibitorService ExhibitorService)
        {
            _ExhibitorService = ExhibitorService;
            _response = new MainResponse();
        }

        /// <summary>
        /// This API is used to get all Exhibitors.
        /// </summary>
        /// <param name="No parameter is required"></param>
        /// <returns>All Exhibitors list</returns>
        [HttpPost]
        public ActionResult GetAllExhibitors()
        {

            _response = _ExhibitorService.GetAllExhibitors();
            return new OkObjectResult(_response);
        }


        /// <summary>
        /// This API is used to get all Exhibitors with filters.
        /// </summary>
        /// <param name="filter parameters is required"></param>
        /// <returns>All Exhibitors list with filter</returns>
        [HttpPost]
        public ActionResult GetAllExhibitorsWithFilter([FromBody] BaseRecordFilterRequest request)
        {

            _response = _ExhibitorService.GetAllExhibitorsWithFilter(request);
            return new OkObjectResult(_response);
        }

        /// <summary>
        /// This API is used to get  Exhibitor by Exhibitor id.
        /// </summary>
        /// <param name="Exhibitor id parameter is required"></param>
        /// <returns> Single Exhibitor record</returns>
        [HttpPost]
        public ActionResult GetExhibitorById([FromBody] GetExhibitorRequest request)
        {
            _response = _ExhibitorService.GetExhibitorById(request);
            return new OkObjectResult(_response);
        }

        /// <summary>
        /// This API is used to add new  Exhibitor.
        /// </summary>
        /// <param name="Exhibitor detail is required"></param>
        /// <returns> success true or false with message</returns>
        [HttpPost]
        public ActionResult AddExhibitor([FromBody] ExhibitorRequest request)
        {
            _response = _ExhibitorService.AddExhibitor(request);
            return new OkObjectResult(_response);
        }

        /// <summary>
        /// This API is used to update existing Exhibitor.
        /// </summary>
        /// <param name="Exhibitor detail with Exhibitor id is required"></param>
        /// <returns> success true or false with message</returns>
        [HttpPost]
        public ActionResult UpdateExhibitor([FromBody] ExhibitorRequest request)
        {
            _response = _ExhibitorService.AddExhibitor(request);
            return new OkObjectResult(_response);
        }

        /// <summary>
        /// This API is used to delete existing Exhibitor.
        /// </summary>
        /// <param name="Exhibitor detail with Exhibitor id is required"></param>
        /// <returns> success true or false with message</returns>
        [HttpPost]
        public ActionResult DeleteExhibitor([FromBody] GetExhibitorRequest request)
        {
            _response = _ExhibitorService.DeleteExhibitor(request);
            return new OkObjectResult(_response);
        }

    }
}
