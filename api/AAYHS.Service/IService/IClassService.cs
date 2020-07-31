using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AAYHS.Service.IService
{
    public interface IClassService
    {
        MainResponse GetAllClasses(ClassRequest classRequest);
        MainResponse CreateClass(AddClassRequest addClassRequest);
        MainResponse AddExhibitorToClass(AddClassExhibitor addClassExhibitor);
        MainResponse GetClassExhibitors(ClassRequest classRequest);
        MainResponse GetClassExhibitorsAndHorses(ClassExhibitorHorsesRequest request);
        MainResponse RemoveClass(RemoveClass removeClass);
        MainResponse SplitClass(SplitRequest splitRequest);
    }
}
