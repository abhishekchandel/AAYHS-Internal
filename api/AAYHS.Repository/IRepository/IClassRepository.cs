using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AAYHS.Repository.IRepository
{
    public interface IClassRepository: IGenericRepository<Classes>
    {
        MainResponse GetAllClasses(ClassRequest classRequest);
        MainResponse GetClass(ClassRequest classRequest);
        MainResponse CreateClass(AddClassRequest addClassRequest);
        MainResponse AddExhibitorToClass(AddClassExhibitor addClassExhibitor);
        MainResponse GetClassExhibitors(ClassRequest classRequest);
        MainResponse GetClassExhibitorsAndHorses(ClassExhibitorHorsesRequest classRequest);
        MainResponse SplitClass(SplitRequest splitRequest);
    }
}
