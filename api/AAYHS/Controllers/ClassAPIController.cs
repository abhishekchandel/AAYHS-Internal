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
    [Route("api/[controller]")]
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
        [Authorize]
        public IActionResult GetAllClasses(ClassRequest classRequest)
        {
            _mainResponse = _classService.GetAllClasses(classRequest);
            _jsonString = Mapper.Convert<GetAllClasses>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used for adding the class
        /// </summary>
        /// <param name="addClassRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateClass(AddClassRequest addClassRequest)
        {
            _mainResponse = await _classService.CreateClass(addClassRequest);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }

    }
}
