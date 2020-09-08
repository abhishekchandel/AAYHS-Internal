using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response.Common
{
    public class StallResponse
    {
        public int StallAssignmentId { get; set; }
        public int StallId { get; set; }
        public int StallAssignmentTypeId { get; set; }
        public int GroupId { get; set; }
        public int ExhibitorId { get; set; }
        public string BookedByType { get; set; }
        public string BookedByName { get; set; }
    }
    public class GetAllStall
    {
        public List<StallResponse> stallResponses { get; set; }
        public int TotalRecords { get; set; }
    }
}
