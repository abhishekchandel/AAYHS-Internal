using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Request
{
   public class SponsorDistributionRequest
    {
        public int SponsorDistributionId { get; set; }
        public int SponsorId { get; set; }
        public int SponsorTypeId { get; set; }
        public int? AdTypeId { get; set; }
        public string TypeId { get; set; }
        public decimal TotalDistribute { get; set; }

    }
  
}
