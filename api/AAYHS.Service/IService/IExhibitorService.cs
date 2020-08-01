using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AAYHS.Service.IService
{
   public interface IExhibitorService
    {
        MainResponse AddUpdateExhibitor(ExhibitorRequest request);
        MainResponse GetAllExhibitorsWithFilter(BaseRecordFilterRequest request);
        MainResponse GetAllExhibitors();
        MainResponse GetExhibitorById(GetExhibitorRequest request);
        MainResponse DeleteExhibitor(GetExhibitorRequest request);
    }
}
