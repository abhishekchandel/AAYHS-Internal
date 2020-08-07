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
    public class HorseAPIController : ControllerBase
    {
        #region readonly
        private readonly IHorseService _horseService;
        private MainResponse _mainResponse;
        #endregion

        #region private
        private string _jsonString = string.Empty;
        #endregion

        public HorseAPIController(IHorseService horseService)
        {
            _horseService = horseService;
            _mainResponse = new MainResponse();
        }

        /// <summary>
        /// This api used to get all horses
        /// </summary>
        /// <param name="horseRequest"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize]
        public IActionResult GetAllHorses(HorseRequest horseRequest)
        {
            _mainResponse = _horseService.GetAllHorses(horseRequest);
            _jsonString = Mapper.Convert<GetAllHorses>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to remove the horse
        /// </summary>
        /// <param name="horseId"></param>
        /// <returns></returns>
        [HttpDelete]
        //[Authorize]
        public IActionResult RemoveHorse(int horseId)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _horseService.RemoveHorse(horseId, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to add horse
        /// </summary>
        /// <param name="horseId"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize]
        public IActionResult AddHorse(HorseAddRequest horseAddRequest)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _horseService.AddHorse(horseAddRequest, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
    }  
}
