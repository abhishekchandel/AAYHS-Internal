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
        MainResponse GetClass(int ClassId);
        MainResponse GetClassExhibitors(int ClassId);
        MainResponse GetExhibitorHorses(int ExhibitorId);
        Task<MainResponse> CreateUpdateClass(AddClassRequest addClassRequest, string actionBy);
        Task<MainResponse> AddExhibitorToClass(AddClassExhibitor addClassExhibitor, string actionBy);      
        MainResponse GetClassEntries(ClassRequest classRequest);
        Task<MainResponse> DeleteClassExhibitor(int ExhibitorClassId, string actionBy);
        Task<MainResponse> RemoveClass(int ClassId, string actionBy);
        Task<MainResponse> AddUpdateSplitClass(SplitRequest splitRequest, string actionBy);
        MainResponse GetBackNumberForAllExhibitor(int ClassId);
        MainResponse GetResultExhibitorDetails(ResultExhibitorRequest resultExhibitorRequest);
        Task<MainResponse> AddClassResult(AddClassResultRequest addClassResultRequest, string actionBy);
        MainResponse GetResultOfClass(ClassRequest classRequest);
        MainResponse SearchClass(SearchRequest searchRequest);
        MainResponse UpdateClassExhibitorScratch(ClassExhibitorScratch classExhibitorScratch, string actionBy);
    }
}
