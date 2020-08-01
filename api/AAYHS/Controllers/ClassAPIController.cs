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
    public class ClassAPIController : ControllerBase
    {

        #region readonly
        private readonly IClassService _classService;
        private MainResponse _mainResponse;
        #endregion

        #region private
        private string _jsonString = string.Empty;
        #endregion

        public ClassAPIController(IClassService classService)
        {
            _classService = classService;
            _mainResponse = new MainResponse();
        }

        /// <summary>
        /// This api used for fetching all classes
        /// </summary>
        /// <param name="classRequest"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize]
        public IActionResult GetAllClasses(ClassRequest classRequest)
        {
            _mainResponse = _classService.GetAllClasses(classRequest);
            _jsonString = Mapper.Convert<GetAllClasses>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used for fetching one classs details
        /// </summary>
        /// <param name="classRequest"></param>
        /// <returns></returns>
        [HttpGet]
        //[Authorize]
        public IActionResult GetClass(int ClassId)
        {
            _mainResponse = _classService.GetClass(ClassId);
            _jsonString = Mapper.Convert<GetClass>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get class Exhibitors
        /// </summary>
        /// <param name="ClassId"></param>
        /// <returns></returns>
        [HttpGet]
        //[Authorize]
        public IActionResult GetClassExhibitors(int ClassId)
        {
            _mainResponse = _classService.GetClassExhibitors(ClassId);
            _jsonString = Mapper.Convert<GetClassAllExhibitors>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }

        /// <summary>
        /// This api used to fetch class exhibitors and horse names
        /// </summary>
        /// <param name="classRequest"></param>
        /// <returns></returns>

        [HttpPost]
        //[Authorize]
        public IActionResult GetClassExhibitorsAndHorses(ClassExhibitorHorsesRequest classRequest)
        {
            _mainResponse = _classService.GetClassExhibitorsAndHorses(classRequest);
            _jsonString = Mapper.Convert<ClassExhibitorHorses>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }


        /// <summary>
        /// This api used to get exhibitor horses 
        /// </summary>
        /// <param name="ClassId"></param>
        /// <returns></returns>
        [HttpGet]
        //[Authorize]
        public IActionResult GetExhibitorHorses(int ExhibitorId)
        {
            _mainResponse = _classService.GetExhibitorHorses(ExhibitorId);
            _jsonString = Mapper.Convert<GetExhibitorAllHorses>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used for adding the class
        /// </summary>
        /// <param name="addClassRequest"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> CreateUpdateClass(AddClassRequest addClassRequest)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = await _classService.CreateUpdateClass(addClassRequest, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used for adding the exhibitor to a class
        /// </summary>
        /// <param name="addClassExhibitor"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> AddExhibitorToClass(AddClassExhibitor addClassExhibitor)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = await _classService.AddExhibitorToClass(addClassExhibitor, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to fetch class exhibitors
        /// </summary>
        /// <param name="classRequest"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize]
        public IActionResult GetClassEntries(ClassRequest classRequest)
        {
            _mainResponse = _classService.GetClassEntries(classRequest);
            _jsonString = Mapper.Convert<GetAllClassEntries>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to delete class exhibitor
        /// </summary>
        /// <param name="ExhibitorClassId"></param>
        /// <returns></returns>
        [HttpDelete]
        //[Authorize]
        public async Task<IActionResult> DeleteClassExhibitor(int ExhibitorClassId)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = await _classService.DeleteClassExhibitor(ExhibitorClassId, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used for removing the class
        /// </summary>
        /// <param name="removeClass"></param>
        /// <returns></returns>
        [HttpDelete]
        //[Authorize]
        public async Task<IActionResult> RemoveClass(int ClassId)
        {
             string actionBy = User.Identity.Name;
            _mainResponse =await _classService.RemoveClass(ClassId, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used for split the class
        /// </summary>
        /// <param name="splitRequest"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult>AddUpdateSplitClass(List<SplitRequest> splitRequest)
        {
             string actionBy = User.Identity.Name;
            _mainResponse = await _classService.AddUpdateSplitClass(splitRequest, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get back number of a class exhibitors
        /// </summary>
        /// <param name="backNumberRequest"></param>
        /// <returns></returns>
        [HttpGet]
        //[Authorize]
        public IActionResult GetBackNumberForAllExhibitor(int ClassId)
        {

            _mainResponse = _classService.GetBackNumberForAllExhibitor(ClassId);
            _jsonString = Mapper.Convert<GetAllBackNumber>(_mainResponse);      
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get exhibitor details for a class
        /// </summary>
        /// <param name="resultExhibitorRequest"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize]
        public IActionResult GetResultExhibitorDetails(ResultExhibitorRequest resultExhibitorRequest)
        {
           
            _mainResponse =  _classService.GetResultExhibitorDetails(resultExhibitorRequest);
            _jsonString = Mapper.Convert<ResultExhibitorDetails>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to add class result
        /// </summary>
        /// <param name="resultExhibitorRequest"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize]
        public async Task< IActionResult> AddClassResult(AddClassResultRequest addClassResultRequest)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = await _classService.AddClassResult(addClassResultRequest, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
    }
}
