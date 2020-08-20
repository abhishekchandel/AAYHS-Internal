using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Service.IService
{
   public interface IAdvertisementService
    {
        public MainResponse GetAllAdvertisements(BaseRecordFilterRequest request);
    }
}
