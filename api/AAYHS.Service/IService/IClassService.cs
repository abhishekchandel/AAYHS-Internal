﻿using AAYHS.Core.DTOs.Request;
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
        MainResponse GetClass(ClassRequest classRequest);
        Task<MainResponse> CreateUpdateClass(AddClassRequest addClassRequest, string actionBy);
        Task<MainResponse> AddExhibitorToClass(AddClassExhibitor addClassExhibitor, string actionBy);
        MainResponse GetClassExhibitors(ClassRequest classRequest);
        Task<MainResponse> RemoveClass(RemoveClass removeClass, string actionBy);
        Task<MainResponse> AddUpdateSplitClass(List<SplitRequest> splitRequest, string actionBy);
        MainResponse GetBackNumberForAllExhibitor(BackNumberRequest backNumberRequest);
        MainResponse GetResultExhibitorDetails(ResultExhibitorRequest resultExhibitorRequest);
    }
}
