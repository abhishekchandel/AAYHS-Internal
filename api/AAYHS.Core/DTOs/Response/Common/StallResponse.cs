﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response.Common
{
    public class StallResponse
    {
        public int StallId { get; set; }
        public int StallNumber { get; set; }
        public bool IsPortable { get; set; }
        public int ProtableStallTypeId { get; set; }
        public bool IsBooked { get; set; }
        public int BookedById { get; set; }
        public string BookedByName { get; set; }
    }
    public class GetAllStall
    {
        public List<StallResponse> stallResponses { get; set; }
    }
}
