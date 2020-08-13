using AAYHS.Core.DTOs.Response.Common;
﻿using AAYHS.Core.Shared.Static;
using AAYHS.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response
{
    /// <summary
    /// This class will contain common property that is used by all API response in the application.
    /// </summary
    public class BaseResponse
    {
        public string Message { get; set; }     
        public int NewId { get; set; }
        public bool Success { get; set; } = true;

    }
  
    public class MainResponse : BaseResponse
    {
        public BaseResponse BaseResponse { get; set; }
        public GetAllClasses GetAllClasses { get; set; }
        public GetClass GetClass { get; set; }
        public GlobalCodeMainResponse GlobalCodeMainResponse { get; set; }
        public GetClassAllExhibitors GetClassAllExhibitors { get; set; }
        public ClassExhibitorHorses ClassExhibitorHorses { get; set; }
        public GetAllClassEntries GetAllClassEntries { get; set; }
        public SponsorResponse SponsorResponse { get; set; }
        public SponsorListResponse SponsorListResponse { get; set; }
        public SponsorClassesListResponse SponsorClassesListResponse { get; set; }
        public ClassSponsorResponse ClassSponsorResponse { get; set; }
        public ClassSponsorListResponse ClassSponsorListResponse { get; set; }

        public ExhibitorResponse ExhibitorResponse { get; set; }
        public ExhibitorListResponse ExhibitorListResponse { get; set; }
        public SponsorExhibitorListResponse SponsorExhibitorListResponse { get; set; }
        public SponsorExhibitorResponse SponsorExhibitorResponse { get; set; }
        public ResultExhibitorDetails ResultExhibitorDetails { get; set; }
        public GetAllBackNumber GetAllBackNumber { get; set; }
        public CityResponse CityResponse { get; set; }
        public StateResponse StateResponse { get; set; }
        public GetExhibitorAllHorses GetExhibitorAllHorses { get; set; }
        public GetResult GetResult { get; set; }
        public GetAllHorses GetAllHorses { get; set; }
        public UserResponse UserResponse { get; set; }

        public GroupResponse GroupResponse  { get; set; }
        public GroupListResponse GroupListResponse  { get; set; }
        public GetAllLinkedExhibitors GetAllLinkedExhibitors { get; set; }
        public GetAllGroups GetAllGroups { get; set; }
        public GetHorseById GetHorseById { get; set; }
        public GetAllGroupExhibitors GetAllGroupExhibitors { get; set; }
        public GetAllGroupFinacials GetAllGroupFinacials { get; set; }

    }
   
    public class Response<T> : BaseResponse
    {
        public T Data { get; set; }
    }
}
