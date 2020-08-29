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
        MainResponse GetExhibitorHorses(int exhibitorId);
        MainResponse DeleteExhibitorHorse(int exhibitorHorseId, string actionBy);
        MainResponse GetAllHorses(int exhibitorId);
        MainResponse GetHorseDetail(int horseId);
        MainResponse AddExhibitorHorse(AddExhibitorHorseRequest addExhibitorHorseRequest, string actionBy);
        MainResponse GetAllClassesOfExhibitor(int exhibitorId);
    }
}
