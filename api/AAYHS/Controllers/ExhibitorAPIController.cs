﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Core.Shared.Static;

namespace AAYHS.API.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExhibitorAPIController : ControllerBase
    {
        #region private       
        private MainResponse _mainResponse;
        private string _jsonString = string.Empty;
        #endregion

        #region readonly
        private readonly IExhibitorService _exhibitorService;
        #endregion

        public ExhibitorAPIController(IExhibitorService exhibitorService)
        {
            _exhibitorService = exhibitorService;
            _mainResponse = new MainResponse();
        }

        /// <summary>
        /// This API is used to get all Exhibitors.
        /// </summary>
        /// <param name="No parameter is required"></param>
        /// <returns>All Exhibitors list</returns>
        [HttpPost]
        public ActionResult GetAllExhibitors(BaseRecordFilterRequest filterRequest)
        {

            _mainResponse = _exhibitorService.GetAllExhibitors(filterRequest); 
              _jsonString = Mapper.Convert<ExhibitorListResponse> (_mainResponse);
            return new OkObjectResult(_jsonString);
        }
      
        /// <summary>
        /// This API is used to get  Exhibitor by Exhibitor id.
        /// </summary>
        /// <param name="Exhibitor id parameter is required"></param>
        /// <returns> Single Exhibitor record</returns>
        [HttpGet]
        public ActionResult GetExhibitorById(int exhibitorId)
        {
            _mainResponse = _exhibitorService.GetExhibitorById(exhibitorId);
            _jsonString = Mapper.Convert<ExhibitorListResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }

        /// <summary>
        /// This API is used to add/update new  Exhibitor.
        /// </summary>
        /// <param name="Exhibitor detail is required"></param>
        /// <returns> success true or false with message</returns>
        [HttpPost]
        public ActionResult AddUpdateExhibitor([FromBody] ExhibitorRequest request)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _exhibitorService.AddUpdateExhibitor(request, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }


        /// <summary>
        /// This API is used to delete existing Exhibitor.
        /// </summary>
        /// <param name="Exhibitor detail with Exhibitor id is required"></param>
        /// <returns> success true or false with message</returns>
        [HttpDelete]
        public ActionResult DeleteExhibitor(int exhibitorId)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _exhibitorService.DeleteExhibitor(exhibitorId, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This API used to search exhibitor by id and name
        /// </summary>
        /// <param name="filterRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchExhibitor(SearchRequest searchRequest)
        {

            _mainResponse = _exhibitorService.SearchExhibitor(searchRequest);
            _jsonString = Mapper.Convert<ExhibitorListResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This API used to get exhibitor horses
        /// </summary>
        /// <param name="exhibitorId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetExhibitorHorses(int exhibitorId)
        {
            _mainResponse = _exhibitorService.GetExhibitorHorses(exhibitorId);
            _jsonString = Mapper.Convert<ExhibitorHorsesResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This API used to delete exhibitor horse
        /// </summary>
        /// <param name="exhibitorId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteExhibitorHorse(int exhibitorId)
        {
            string actionBy = User.Identity.Name;
            _mainResponse = _exhibitorService.DeleteExhibitorHorse(exhibitorId, actionBy);
            _jsonString = Mapper.Convert<BaseResponse>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
        /// <summary>
        /// This API used to get all horses 
        /// </summary>
        /// <param name="exhibitorId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAllHorses()
        {
            _mainResponse = _exhibitorService.GetAllHorses();
            _jsonString = Mapper.Convert<GetExhibitorHorsesList>(_mainResponse);
            return new OkObjectResult(_jsonString);
        }
    }
}
