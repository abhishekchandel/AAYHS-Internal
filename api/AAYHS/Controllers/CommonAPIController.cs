using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Core.Shared.Static;
using AAYHS.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AAYHS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommonAPIController : ControllerBase
    {
        #region readonly
        private readonly IGlobalCodeService _globalCodeService;
        private MainResponse _mainResponse;
        #endregion

        #region Private
        private string _jsonString = string.Empty;
        #endregion

        public CommonAPIController(IGlobalCodeService GlobalCodeService)
        {
            _globalCodeService = GlobalCodeService;
            _mainResponse = new MainResponse();

        }
        /// <summary>
        /// This api used to get horse type
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public  IActionResult GetGlobalCode(string categoryName)
        {
            _mainResponse = _globalCodeService.GetGlobalCode(categoryName);
            _jsonString = Mapper.Convert<GlobalCodeMainResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }       
        /// <summary>
        /// This API is used for fetching all states.
        /// </summary>
        /// <param name></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetStates()
        {
            _mainResponse = _globalCodeService.GetAllStates();
            _jsonString = Mapper.Convert<StateResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }

        /// <summary>
        /// This API is used for fetching all cities.
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCities(int stateId)
        {

            _mainResponse = _globalCodeService.GetAllCities(stateId);
            _jsonString = Mapper.Convert<CityResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }

        /// <summary>
        /// This API is used for fetching all zipcodes.
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetZipCodes(ZipCodeRequest request)
        {

            _mainResponse = _globalCodeService.GetAllZipCodes(request);
            _jsonString = Mapper.Convert<ZipCodeResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
    }
}
