using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Repository.IRepository
{
   public interface IClassSponsorRepository : IGenericRepository<ClassSponsors>
    {
        MainResponse GetAllClassSponsor();
        MainResponse GetAllClassSponsorWithFilters(BaseRecordFilterRequest request);
        MainResponse GetClassSponsorbyId (int ClassSponsorId);
        MainResponse GetSponsorClassesbySponsorId(int SponsorId);
    }
}
