using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Repository.IRepository
{
   public interface IExhibitorRepository : IGenericRepository<Exhibitors>
    {
        MainResponse GetAllExhibitors();
        MainResponse GetAllExhibitorsWithFilters(BaseRecordFilterRequest request);
    }
}
