using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
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
             CreateMap<APILogRequest, Apilogs>();
            CreateMap<SponsorRequest, Sponsors>();
            CreateMap<ExhibitorRequest, Sponsors>();
            CreateMap<SponsorExhibitorRequest, SponsorExhibitor>();
            CreateMap<APILogRequest, Apilogs>();






            // Response Mapping
            CreateMap<Sponsors, SponsorResponse>();
            CreateMap<Exhibitors, ExhibitorResponse>();
            CreateMap<ClassSponsors, ClassSponsorResponse>();
            CreateMap<Classes, GetAllClasses>();
            

        }
    }
}
