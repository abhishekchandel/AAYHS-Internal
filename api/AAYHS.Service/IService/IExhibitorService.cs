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
        MainResponse AddUpdateExhibitor(ExhibitorRequest request,string actionBy);
        MainResponse GetAllExhibitors(BaseRecordFilterRequest filterRequest);
        MainResponse GetExhibitorById(int ExhibitorId);
        MainResponse DeleteExhibitor(int ExhibitorId,string actionBy);
    }
}
