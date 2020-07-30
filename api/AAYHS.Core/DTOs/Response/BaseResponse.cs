using AAYHS.Core.DTOs.Response.Common;
﻿using AAYHS.Core.Shared.Static;
using AAYHS.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response
{
    /// <summary>
    /// This class will contain common property that is used by all API response in the application.
    /// </summary>
    public class BaseResponse
    {
        public string Message { get; set; }
        public int TotalRecords { get; set; }
        public bool Success { get; set; } = true;
        
    }

    public class MainResponse : BaseResponse
    {
    

        public MainResponse()
        {
            Data = new Data();
        }
        public Data Data { get; set; }
    }
    public class Data
    {
        public BaseResponse BaseResponse { get; set; }
        public ClassResponse ClassResponse { get; set; }
        public GetAllClasses GetAllClasses { get; set; }
        public GetClass GetClass { get; set; }
        public GlobalCodeMainResponse GlobalCodeMainResponse { get; set; }
        public GetAllClassExhibitor GetAllClassExhibitor { get; set; }
        public SponsorResponse SponsorResponse { get; set; }
        public List<SponsorResponse> SponsorListResponse { get; set; }

        public ClassSponsorResponse ClassSponsorResponse { get; set; }
        public List<ClassSponsorResponse> ClassSponsorListResponse { get; set; }

        public ExhibitorResponse ExhibitorResponse { get; set; }
        public List<ExhibitorResponse> ExhibitorListResponse { get; set; }
        public List<SponsorExhibitorResponse> SponsorExhibitorListResponses { get; set; }
        public SponsorExhibitorResponse SponsorExhibitorResponse { get; set; }

    }
    public class Response<T> : BaseResponse
    {
        public T Data { get; set; }
    }
}
