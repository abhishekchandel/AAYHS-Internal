using AAYHS.Core.DTOs.Request;
using AAYHS.Data.DBEntities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            // Request Mapping
            CreateMap<APILogRequest, Apilogs>();

            // Response Mapping

        }
    }
}
