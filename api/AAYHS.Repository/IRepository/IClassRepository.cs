﻿using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
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
        MainResponse GetClassExhibitors(ClassRequest classRequest);
        List<GetBackNumber> GetBackNumberForAllExhibitor(BackNumberRequest backNumberRequest);
        ResultExhibitorDetails GetResultExhibitorDetails(ResultExhibitorRequest resultExhibitorRequest);
    }
}
