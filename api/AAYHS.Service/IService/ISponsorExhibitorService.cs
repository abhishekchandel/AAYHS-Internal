using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Service.IService
{
   public interface ISponsorExhibitorService
    {
        MainResponse AddUpdateSponsorExhibitor(SponsorExhibitorRequest request);
        MainResponse GetSponsorExhibitorBySponsorId(int SponsorId);
        MainResponse DeleteSponsorExhibitor(int SponsorExhibitorId);
    }
}
