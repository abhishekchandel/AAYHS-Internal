using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Repository.IRepository
{
   public interface ISponsorRepository : IGenericRepository<Sponsors>
   
    {
        MainResponse GetSponsorById(int SponsorId);
        MainResponse GetAllSponsors(BaseRecordFilterRequest request);
        SponsorListResponse SearchSponsor(SearchRequest searchRequest);
    }
}
