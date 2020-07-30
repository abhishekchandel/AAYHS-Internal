using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Service.IService
{
   public interface ISponsorExhibitorService
    {
        MainResponse AddSponsorExhibitor(SponsorExhibitorRequest request);
        MainResponse GetSponsorExhibitorBySponsorId(GetSponsorExhibitorRequest request);
        MainResponse DeleteSponsorExhibitor(DeleteSponsorExhibitorRequest request);
        MainResponse UpdateSponsorExhibitor(SponsorExhibitorRequest request);
    }
}
