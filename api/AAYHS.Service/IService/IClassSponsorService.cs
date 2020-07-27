﻿using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Service.IService
{
   public interface IClassSponsorService
    {

         MainResponse AddClassSponsor(ClassSponsorRequest request);
        MainResponse UpdateClassSponsor(ClassSponsorRequest request);
        MainResponse GetClassSponsorbyId(GetClassSponsorRequest request);
        MainResponse GetAllClassSponsor();
        MainResponse GetAllClassSponsorWithFilter(BaseRecordFilterRequest request);
        MainResponse DeleteClassSponsor(GetClassSponsorRequest request);



    }
}
