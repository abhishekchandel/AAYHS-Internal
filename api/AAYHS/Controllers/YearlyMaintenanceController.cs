using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.Shared.Static;
using AAYHS.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AAYHS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class YearlyMaintenanceController : ControllerBase
    {

        #region private       
        private IYearlyMaintenanceService _yearlyMaintenanceService;
        private MainResponse _mainResponse;
        private string _jsonString = string.Empty;
        #endregion
      
        public YearlyMaintenanceController(IYearlyMaintenanceService yearlyMaintenanceService)
        {
            _yearlyMaintenanceService = yearlyMaintenanceService;
            _mainResponse = new MainResponse();
        }
        /// <summary>
        /// This api used to get all yealry registration fee
        /// </summary>
        /// <param name="getAllYearlyMaintenanceRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAllYearlyMaintenance(GetAllYearlyMaintenanceRequest getAllYearlyMaintenanceRequest)
        {

            _mainResponse = _yearlyMaintenanceService.GetAllYearlyMaintenance(getAllYearlyMaintenanceRequest);
            _jsonString = Mapper.Convert<GetAllYearlyMaintenance>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get the yearly maintenance By Id
        /// </summary>
        /// <param name="yearlyMaintenanceId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetYearlyMaintenanceById(int yearlyMaintenanceId)
        {

            _mainResponse = _yearlyMaintenanceService.GetYearlyMaintenanceById(yearlyMaintenanceId);
            _jsonString = Mapper.Convert<GetYearlyMaintenanceById>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get all the user which is not approved
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAllUsers()
        {

            _mainResponse = _yearlyMaintenanceService.GetAllUsers();
            _jsonString = Mapper.Convert<GetAllUsers>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to approved and unapproved the user
        /// </summary>
        /// <param name="userApprovedRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApprovedUser(UserApprovedRequest userApprovedRequest)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _yearlyMaintenanceService.ApprovedUser(userApprovedRequest, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to delete the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteUser(int userId)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _yearlyMaintenanceService.DeleteUser(userId, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        [HttpPost]
        public ActionResult AddYearly(AddYearlyRequest addYearly)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _yearlyMaintenanceService.AddYearly(addYearly, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
    }
}
