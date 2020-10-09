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
        [HttpDelete]
        public ActionResult DeleteUser(int userId)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _yearlyMaintenanceService.DeleteUser(userId, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to add and update yearly
        /// </summary>
        /// <param name="addYearly"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddUpdateYearly(AddYearlyRequest addYearly)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _yearlyMaintenanceService.AddUpdateYearly(addYearly, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to delete the yearly
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult DeleteYearly(int yearlyMaintainenceId)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _yearlyMaintenanceService.DeleteYearly(yearlyMaintainenceId, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to add ad fee
        /// </summary>
        /// <param name="addAdFee"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddADFee(AddAdFee addAdFee)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _yearlyMaintenanceService.AddADFee(addAdFee, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get all ad fees
        /// </summary>
        /// <param name="yearlyMaintenanceId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAllAdFees(int yearlyMaintenanceId)
        {

            _mainResponse = _yearlyMaintenanceService.GetAllAdFees(yearlyMaintenanceId);
            _jsonString = Mapper.Convert<GetAllAdFees>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to delete the ad fee
        /// </summary>
        /// <param name="yearlyMaintenanceFeeId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteAdFee(DeleteAdFee deleteAd)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _yearlyMaintenanceService.DeleteAdFee(deleteAd, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get all the users approved
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAllUsersApproved()
        {

            _mainResponse = _yearlyMaintenanceService.GetAllUsersApproved();
            _jsonString = Mapper.Convert<GetAllUsers>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to remove the approved user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult RemoveApprovedUser(int userId)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _yearlyMaintenanceService.RemoveApprovedUser(userId, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get all roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAllRoles()
        {
            _mainResponse = _yearlyMaintenanceService.GetAllRoles();
            _jsonString = Mapper.Convert<GetAllRoles>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get all class categories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAllClassCategory()
        {
            _mainResponse = _yearlyMaintenanceService.GetAllClassCategory();
            _jsonString = Mapper.Convert<GetAllClassCategory>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api  used to add class category
        /// </summary>
        /// <param name="addClassCategoryRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddClassCategory(AddClassCategoryRequest addClassCategoryRequest)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _yearlyMaintenanceService.AddClassCategory(addClassCategoryRequest, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to remove class category  
        /// </summary>
        /// <param name="globalCodeId"></param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult RemoveClassCategory(int globalCodeId)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _yearlyMaintenanceService.RemoveClassCategory(globalCodeId, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get all general fees
        /// </summary>
        /// <param name="yearlyMaintenanceId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAllGeneralFees(int yearlyMaintenanceId)
        {
            _mainResponse = _yearlyMaintenanceService.GetAllGeneralFees(yearlyMaintenanceId);
            _jsonString = Mapper.Convert<GetAllGeneralFees>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to add general fee
        /// </summary>
        /// <param name="addGeneralFeeRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddGeneralFees(AddGeneralFeeRequest addGeneralFeeRequest)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _yearlyMaintenanceService.AddGeneralFees(addGeneralFeeRequest, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to remove general fee
        /// </summary>
        /// <param name="yearlyMaintenanceFeeId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveGeneralFee(RemoveGeneralFee removeGeneralFee)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _yearlyMaintenanceService.RemoveGeneralFee(removeGeneralFee, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get refund detail
        /// </summary>
        /// <param name="yearlyMaintenanceId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetRefund(int yearlyMaintenanceId)
        {
            _mainResponse = _yearlyMaintenanceService.GetRefund(yearlyMaintenanceId);
            _jsonString = Mapper.Convert<GetAllRefund>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to add refund 
        /// </summary>
        /// <param name="addRefundRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddRefund(AddRefundRequest addRefundRequest)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _yearlyMaintenanceService.AddRefund(addRefundRequest, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to remove the refund
        /// </summary>
        /// <param name="refundId"></param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult RemoveRefund(int refundId)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _yearlyMaintenanceService.RemoveRefund(refundId, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get contact info
        /// </summary>
        /// <param name="yearlyMaintenanceId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetContactInfo(int yearlyMaintenanceId)
        {
            _mainResponse = _yearlyMaintenanceService.GetContactInfo(yearlyMaintenanceId);
            _jsonString = Mapper.Convert<GetContactInfo>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to add the contact info
        /// </summary>
        /// <param name="addContactInfoRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddUpdateContactInfo(AddContactInfoRequest addContactInfoRequest)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _yearlyMaintenanceService.AddUpdateContactInfo(addContactInfoRequest, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to add update location
        /// </summary>
        /// <param name="addLocationRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddUpdateLocation(AddLocationRequest addLocationRequest)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _yearlyMaintenanceService.AddUpdateLocation(addLocationRequest, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get location
        /// </summary>
        /// <param name="yearlyMaintenanceId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetLocation(int yearlyMaintenanceId)
        {
            _mainResponse = _yearlyMaintenanceService.GetLocation(yearlyMaintenanceId);
            _jsonString = Mapper.Convert<GetLocation>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
    }
}
