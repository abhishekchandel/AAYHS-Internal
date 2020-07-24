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
        Task<MainResponse> CreateClass(AddClassRequest addClassRequest);
        Task<MainResponse> AddExhibitorToClass(AddClassExhibitor addClassExhibitor);
        MainResponse GetClassExhibitors(ClassRequest classRequest);
        Task<MainResponse> RemoveClass(RemoveClass removeClass);
        Task<MainResponse> SplitClass(SplitRequest splitRequest);
    }
}
