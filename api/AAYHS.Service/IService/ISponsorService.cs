using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AAYHS.Service.IService
{
    public interface ISponsorService
    {
        MainResponse AddSponsor(SponsorRequest request);
        MainResponse GetAllSponsorsWithFilter(BaseRecordFilterRequest request);
        MainResponse GetAllSponsors();
        MainResponse GetSponsorById(int sponsorId);
        MainResponse UpdateSponsor(SponsorRequest request);
        MainResponse DeleteSponsor(int sponsorId);
    }
}
