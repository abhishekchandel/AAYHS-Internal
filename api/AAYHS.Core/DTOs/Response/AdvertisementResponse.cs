using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response
{
   public class AdvertisementResponse
    {
        public int AdvertisementId { get; set; }
        public int AdvertisementTypeId { get; set; }
        public int AdvertisementSizeId { get; set; }
        public int AdvertisementNumber { get; set; }
        public string Name { get; set; }
        public string Comments { get; set; }
    }

    public class AdvertisementListResponse
    {
        public int TotalRecords { get; set; }
        public List<AdvertisementResponse> advertisementResponses { get; set; }
    }
}
