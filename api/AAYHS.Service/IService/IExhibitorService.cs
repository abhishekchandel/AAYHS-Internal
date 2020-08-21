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
        MainResponse GetExhibitorById(int exhibitorId);
        MainResponse DeleteExhibitor(int exhibitorId, string actionBy);
        MainResponse SearchExhibitor(SearchRequest searchRequest);
        MainResponse GetExhibitorHorses(int exhibitorId);
        MainResponse DeleteExhibitorHorse(int exhibitorHorseId, string actionBy);
        MainResponse GetAllHorses();
    }
}
