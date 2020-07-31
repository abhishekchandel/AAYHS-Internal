﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Request
{
   public class SponsorExhibitorRequest
    {
        public int SponsorExhibitorId { get; set; }
        public int SponsorId { get; set; }
        public int ExhibitorId { get; set; }
        public int SponsorTypeId { get; set; }
        public int TypeId { get; set; }
    }
    public class GetSponsorExhibitorRequest
    {
        public int SponsorId { get; set; }
    }
    public class DeleteSponsorExhibitorRequest
    {
        public int SponsorExhibitorId { get; set; }
    }
}