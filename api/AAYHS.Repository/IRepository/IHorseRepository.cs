using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Repository.IRepository
{
    public interface IHorseRepository: IGenericRepository<Horses>
    {
        GetAllHorses GetAllHorses(HorseRequest horseRequest);       
        GetAllHorses SearchHorse(SearchRequest searchRequest);
        GetAllLinkedExhibitors LinkedExhibitors(HorseExhibitorRequest horseExhibitorRequest);
    }
}
