using System;
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
        public string Description { get; set; }
        public bool IsBooked { get; set; } = false;
        public int BookedById { get; set; }
        public int StallAssignmentId { get; set; }
        public int StallAssignmentTypeId { get; set; }
        public string BookedByName { get; set; }
        public string BookedByType { get; set; }
    }
    public class GetAllStall
    {
        public List<StallResponse> stallResponses { get; set; }
        public int TotalRecords { get; set; }
    }
}
