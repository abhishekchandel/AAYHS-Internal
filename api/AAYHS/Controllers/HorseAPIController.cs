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
        /// This api used to get horse 
        /// </summary>
        /// <param name="horseRequest"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize]
        public IActionResult GetHorse(HorseRequest horseRequest)
        {
            _mainResponse = _horseService.GetHorse(horseRequest);
            _jsonString = Mapper.Convert<GetHorse>(_mainResponse);
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
        public IActionResult AddUpdateHorse(HorseAddRequest horseAddRequest)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _horseService.AddUpdateHorse(horseAddRequest, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to search the horse
        /// </summary>
        /// <param name="horseAddRequest"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize]
        public IActionResult SearchHorse(SearchRequest searchRequest)
        {           
            _mainResponse = _horseService.SearchHorse(searchRequest);
            _jsonString = Mapper.Convert<GetAllHorses>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get exhibitor linked with horse
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize]
        public IActionResult LinkedExhibitors(HorseExhibitorRequest horseExhibitorRequest)
        {
            _mainResponse = _horseService.LinkedExhibitors(horseExhibitorRequest);
            _jsonString = Mapper.Convert<GetAllLinkedExhibitors>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This api used to get group id and name
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize]
        public IActionResult GetGroup()
        {
            _mainResponse = _horseService.GetGroup();
            _jsonString = Mapper.Convert<GetGroup>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
    }  
}
