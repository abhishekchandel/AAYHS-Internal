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
        /// This api used to get all scan document of exhibitors
        /// </summary>
        /// <param name="getScanRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAllScan(GetScanRequest getScanRequest)
        {            
            _mainResponse = _yearlyMaintenanceService.GetAllScan(getScanRequest);
            _jsonString = Mapper.Convert<GetAllScan>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to add and update yealry statement text
        /// </summary>
        /// <param name="addStatementTextRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddUpdateYearlyStatementText(AddStatementTextRequest addStatementTextRequest)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _yearlyMaintenanceService.AddUpdateYearlyStatementText(addStatementTextRequest, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get all yearly statement text
        /// </summary>
        /// <param name="yearlyMaintenanceId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetYearlyStatementText(int yearlyMaintenanceId)
        {
            _mainResponse = _yearlyMaintenanceService.GetYearlyStatementText(yearlyMaintenanceId);
            _jsonString = Mapper.Convert<GetAllStatementText>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to add and update sponsor incentive
        /// </summary>
        /// <param name="addSponsorIncentiveRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddAndUpdateSponsorIncentive(AddSponsorIncentiveRequest addSponsorIncentiveRequest)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _yearlyMaintenanceService.AddAndUpdateSponsorIncentive(addSponsorIncentiveRequest, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get sponsor incentive
        /// </summary>
        /// <param name="yearlyMaintenanceId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSponsorIncentive(int yearlyMaintenanceId)
        {
            _mainResponse = _yearlyMaintenanceService.GetSponsorIncentive(yearlyMaintenanceId);
            _jsonString = Mapper.Convert<GetSponsorAllIncentives>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to delete sponsor incentive
        /// </summary>
        /// <param name="sponsorIncentiveId"></param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult DeleteSponsorIncentive(int sponsorIncentiveId)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _yearlyMaintenanceService.DeleteSponsorIncentive(sponsorIncentiveId, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to active or inactive the user
        /// </summary>
        /// <param name="activeInActiveRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ActiveInActiveUser(ActiveInActiveRequest activeInActiveRequest)
        {
            _mainResponse = _yearlyMaintenanceService.ActiveInActiveUser(activeInActiveRequest);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to active or inactive the general fee
        /// </summary>
        /// <param name="activeInActiveGeneralFeeRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ActiveInActiveGeneralFee(ActiveInActiveGeneralFeeRequest activeInActiveGeneralFeeRequest)
        {
            _mainResponse = _yearlyMaintenanceService.ActiveInActiveGeneralFee(activeInActiveGeneralFeeRequest);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to active and inactive the ad fee
        /// </summary>
        /// <param name="activeInActiveAdFeeRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ActiveInActiveAdFee(ActiveInActiveAdFeeRequest activeInActiveAdFeeRequest)
        {
            _mainResponse = _yearlyMaintenanceService.ActiveInActiveAdFee(activeInActiveAdFeeRequest);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to active and inactive the class category
        /// </summary>
        /// <param name="activeInActiveClassCategory"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ActiveInActiveClassCategory(ActiveInActiveClassCategory activeInActiveClassCategory)
        {
            _mainResponse = _yearlyMaintenanceService.ActiveInActiveClassCategory(activeInActiveClassCategory);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to active and inactive the scratch refund fee
        /// </summary>
        /// <param name="activeInActiveScratchRefund"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ActiveInActiveScratchRefund(ActiveInActiveScratchRefund activeInActiveScratchRefund)
        {
            _mainResponse = _yearlyMaintenanceService.ActiveInActiveScratchRefund(activeInActiveScratchRefund);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to add yearly maintenance fee and category to diffrent year
        /// </summary>
        /// <param name="yearlyMaintenanceId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult YearlyDataForNextYear([FromBody]int yearlyMaintenanceId)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _yearlyMaintenanceService.YearlyDataForNextYear(yearlyMaintenanceId,actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
    }
}
