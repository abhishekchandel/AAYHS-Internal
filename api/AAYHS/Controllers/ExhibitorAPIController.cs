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
        #region private       
        private MainResponse _mainResponse;
        private string _jsonString = string.Empty;
        #endregion

        #region readonly
        private readonly IExhibitorService _ExhibitorService;
        #endregion

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
        public ActionResult GetAllExhibitors(BaseRecordFilterRequest filterRequest)
        {

            _mainResponse = _ExhibitorService.GetAllExhibitors(filterRequest); 
              _jsonString = Mapper.Convert<ExhibitorListResponse> (_mainResponse);
            return new OkObjectResult(_jsonString);
        }
      
        /// <summary>
        /// This API is used to get  Exhibitor by Exhibitor id.
        /// </summary>
        /// <param name="Exhibitor id parameter is required"></param>
        /// <returns> Single Exhibitor record</returns>
        [HttpGet]
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
            string actionBy = User.Identity.Name;
            _mainResponse = _ExhibitorService.AddUpdateExhibitor(request, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }


        /// <summary>
        /// This API is used to delete existing Exhibitor.
        /// </summary>
        /// <param name="Exhibitor detail with Exhibitor id is required"></param>
        /// <returns> success true or false with message</returns>
        [HttpDelete]
        public ActionResult DeleteExhibitor(int ExhibitorId)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _ExhibitorService.DeleteExhibitor(ExhibitorId, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }

    }
}
