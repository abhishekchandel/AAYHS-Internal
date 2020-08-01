using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Service.IService
{
   public interface IClassSponsorService
    {

         MainResponse AddUpdateClassSponsor(ClassSponsorRequest request);
        MainResponse GetClassSponsorbyId(int ClassSponsorId);
        MainResponse GetAllClassSponsor();
        MainResponse GetAllClassSponsorWithFilter(BaseRecordFilterRequest request);
        MainResponse DeleteClassSponsor(int ClassSponsorId);
        MainResponse GetSponsorClassesbySponsorId(int SponsorId);

    }
}
