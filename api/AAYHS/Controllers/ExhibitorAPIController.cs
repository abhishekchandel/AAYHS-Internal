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
    public class ExhibitorAPIController : ControllerBase
    {
        private readonly IExhibitorService _ExhibitorService;
        private MainResponse _mainResponse;
        private string _jsonString = string.Empty;
        public ExhibitorAPIController(IExhibitorService ExhibitorService)
        {
            _ExhibitorService = ExhibitorService;
            _mainResponse = new MainResponse();
        }

        /// <summary>
        /// This API is used to get all Exhibitors.
        /// </summary>
        /// <param name="No parameter is required"></param>
        /// <returns>All Exhibitors list</returns>
        [HttpPost]
        public ActionResult GetAllExhibitors()
        {

            _mainResponse = _ExhibitorService.GetAllExhibitors(); 
              _jsonString = Mapper.Convert<ExhibitorListResponse> (_mainResponse);
            return new OkObjectResult(_jsonString);
        }


        /// <summary>
        /// This API is used to get all Exhibitors with filters.
        /// </summary>
        /// <param name="filter parameters is required"></param>
        /// <returns>All Exhibitors list with filter</returns>
        [HttpPost]
        public ActionResult GetAllExhibitorsWithFilter([FromBody] BaseRecordFilterRequest request)
        {

            _mainResponse = _ExhibitorService.GetAllExhibitorsWithFilter(request);
            _jsonString = Mapper.Convert<ExhibitorListResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }

        /// <summary>
        /// This API is used to get  Exhibitor by Exhibitor id.
        /// </summary>
        /// <param name="Exhibitor id parameter is required"></param>
        /// <returns> Single Exhibitor record</returns>
        [HttpPost]
        public ActionResult GetExhibitorById(int ExhibitorId)
        {
            _mainResponse = _ExhibitorService.GetExhibitorById(ExhibitorId);
            _jsonString = Mapper.Convert<ExhibitorResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }

        /// <summary>
        /// This API is used to add/update new  Exhibitor.
        /// </summary>
        /// <param name="Exhibitor detail is required"></param>
        /// <returns> success true or false with message</returns>
        [HttpPost]
        public ActionResult AddUpdateExhibitor([FromBody] ExhibitorRequest request)
        {
            _mainResponse = _ExhibitorService.AddUpdateExhibitor(request);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }


        /// <summary>
        /// This API is used to delete existing Exhibitor.
        /// </summary>
        /// <param name="Exhibitor detail with Exhibitor id is required"></param>
        /// <returns> success true or false with message</returns>
        [HttpPost]
        public ActionResult DeleteExhibitor(int ExhibitorId)
        {
            _mainResponse = _ExhibitorService.DeleteExhibitor(ExhibitorId);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }

    }
}
