using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Core.Shared.Static;
using AAYHS.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace AAYHS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StallAssignmentAPIController : Controller
    {
        #region readonly
        private readonly IStallAssignmentService _stallAssignmentService;
        private MainResponse _mainResponse;
        #endregion

        #region private
        private string _jsonString = string.Empty;
        #endregion

        public StallAssignmentAPIController(IStallAssignmentService stallAssignmentService)
        {
            _stallAssignmentService = stallAssignmentService;
            _mainResponse = new MainResponse();
        }


        /// <summary>
        /// This api used to get all stall
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize]   
        public IActionResult GetAllAssignedStalls()
        {
            _mainResponse = _stallAssignmentService.GetAllAssignedStalls();
            _jsonString = Mapper.Convert<GetAllStall>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }

       
        /// <summary>
        /// This api used to delete Stall Assignment
        /// </summary>
        /// <param name="ExhibitorClassId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        public  IActionResult DeleteAssignedStall(int StallAssignmentId)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _stallAssignmentService.DeleteAssignedStall(StallAssignmentId);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }

    }
}
