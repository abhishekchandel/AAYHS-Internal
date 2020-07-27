using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Request
{
   public class ClassSponsorRequest
    {
        public int ClassSponsorId { get; set; }
        public int SponsorId { get; set; }
        public int ClassId { get; set; }
        public string AgeGroup { get; set; }
    }
    public class GetClassSponsorRequest
    {
        public int ClassSponsorId { get; set; }
    }
}
