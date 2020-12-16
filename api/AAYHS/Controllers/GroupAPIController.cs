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
    public class GroupAPIController : ControllerBase
    {
        private readonly IGroupService _GroupService;
        private MainResponse _mainResponse;
       
        private string _jsonString = string.Empty;
        public GroupAPIController(IGroupService GroupService)
        {
            _GroupService = GroupService;
            _mainResponse = new MainResponse();
        }

        /// <summary>
        /// This API is used to get all Groups according to filters.
        /// </summary>
        /// <param name="filter parameter is required"></param>
        /// <returns>Filtered Groups list</returns>
        [HttpPost]
        public ActionResult GetAllGroups(BaseRecordFilterRequest request)
        {

            _mainResponse = _GroupService.GetAllGroups(request);
            _jsonString = Mapper.Convert<GroupListResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }


        /// <summary>
        /// This API is used to get  Group by Group id.
        /// </summary>
        /// <param name="Group id parameter is required"></param>
        /// <returns> Single Group record</returns>
        [HttpGet]
        public ActionResult GetGroupById(int GroupId)
        {
            _mainResponse = _GroupService.GetGroupById(GroupId);
            _jsonString = Mapper.Convert<GroupResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }



        /// <summary>
        /// This API is used to add/update new  Group.
        /// </summary>
        /// <param name="Group detail is required"></param>
        /// <returns> success true or false with message</returns>
        [HttpPost]
        public ActionResult AddUpdateGroup([FromBody] GroupRequest request)
        {
            _mainResponse = _GroupService.AddUpdateGroup(request);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }

        /// <summary>
        /// This API is used to delete existing Group.
        /// </summary>
        /// <param name="Group detail with Group id is required"></param>
        /// <returns> success true or false with message</returns>
        [HttpDelete]
        public ActionResult DeleteGroup(int GroupId)
        {
            _mainResponse = _GroupService.DeleteGroup(GroupId);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }

        /// <summary>
        /// This api used to search the Group
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchGroup(SearchRequest searchRequest)
        {

            _mainResponse = _GroupService.SearchGroup(searchRequest);
            _jsonString = Mapper.Convert<GroupListResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get group exhibitors
        /// </summary>
        /// <param name="groupExhibitorsRequest"></param>
        /// <returns></returns>
        [HttpGet]
        //[Authorize]
        public IActionResult GetGroupExhibitors(int GroupId)
        {
            _mainResponse = _GroupService.GetGroupExhibitors(GroupId);
            _jsonString = Mapper.Convert<GetAllGroupExhibitors>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to delete the group exhibitor
        /// </summary>
        /// <param name="groupExhibitorId"></param>
        /// <returns></returns>
        [HttpDelete]
        //[Authorize]
        public IActionResult DeleteGroupExhibitor(int groupExhibitorId)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _GroupService.DeleteGroupExhibitor(groupExhibitorId, actionBy);
            _jsonString = Mapper.Convert<GetAllGroupExhibitors>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to add or update group financial
        /// </summary>
        /// <param name="addGroupFinancialRequest"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize]
        public IActionResult AddUpdateGroupFinancials(AddGroupFinancialRequest addGroupFinancialRequest)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _GroupService.AddUpdateGroupFinancials(addGroupFinancialRequest, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }


        /// <summary>
        /// This api used to update group financial amount
        /// </summary>
        /// <param name="UpdateGroupFinancialAmountRequest"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize]
        public IActionResult UpdateGroupFinancialsAmount(UpdateGroupFinancialAmountRequest request)
        {
            _mainResponse = _GroupService.UpdateGroupFinancialsAmount(request);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }


        /// <summary>
        /// This api used to delete group financial
        /// </summary>
        /// <param name="groupFinancialId"></param>
        /// <returns></returns>
        [HttpDelete]
        //[Authorize]
        public IActionResult DeleteGroupFinancials(int groupFinancialId)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _GroupService.DeleteGroupFinancials(groupFinancialId, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }


        /// <summary>
        /// This api used to get all group financials
        /// </summary>
        /// <param name="groupFinancialRequest"></param>
        /// <returns></returns>
        [HttpGet]
        //[Authorize]   
        public IActionResult GetAllGroupFinancials(int GroupId)
        {
            _mainResponse = _GroupService.GetAllGroupFinancials(GroupId);
            _jsonString = Mapper.Convert<GetAllGroupFinacials>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }

        /// <summary>
        /// This api used to get module groups financials
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize]   
        public IActionResult GetModuleGroupsFinancials()
        {
            _mainResponse = _GroupService.GetModuleGroupsFinancials();
            _jsonString = Mapper.Convert<GetAllGroupsFinacialsModule>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }

        /// <summary>
        /// This api used to get module group financials
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize]   
        public IActionResult GetModuleGroupFinancials(int groupId)
        {
            _mainResponse = _GroupService.GetModuleGroupsFinancials(groupId);
            _jsonString = Mapper.Convert<GetAllGroupsFinacialsModule>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }

        /// <summary>
        /// This api used to get group statement
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        [HttpGet]
        //[Authorize]   
        public IActionResult GetGroupStatement(int GroupId)
        {
            _mainResponse = _GroupService.GetGroupStatement(GroupId);
            _jsonString = Mapper.Convert<GetGroupStatement>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
    }
    
}
