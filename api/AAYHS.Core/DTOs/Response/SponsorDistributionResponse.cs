using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response
{
    public class SponsorDistributionResponse
    {
        public int SponsorDistributionId { get; set; }
        public int SponsorId { get; set; }
        public int SponsorTypeId { get; set; }
        public string SponsorTypeName { get; set; }
        public int AdTypeId { get; set; }
        public string AdTypeName { get; set; }
        public string IdNumber { get; set; }
        public decimal TotalDistribute { get; set; }
    }

    public class SponsorDistributionListResponse
    {
        public int TotalRecords { get; set; }
        public decimal TotalSponsorAmount { get; set; }
        public decimal TotalSponsorExhibitorPaid { get; set; }
        public decimal TotalSponsorDistributionPaid { get; set; }
        public decimal RemainedSponsorAmount { get; set; }
        public List<SponsorDistributionResponse> SponsorDistributionResponses { get; set; }
    }
}
