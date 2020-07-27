using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response
{
   public class ClassSponsorResponse
    {
        public int ClassSponsorId { get; set; }
        public int SponsorId { get; set; }
        public int ClassId { get; set; }
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
    }

}
