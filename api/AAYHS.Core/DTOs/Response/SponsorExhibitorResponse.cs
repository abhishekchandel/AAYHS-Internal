using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response
{
    public class SponsorExhibitorResponse
    {
        public int SponsorExhibitorId { get; set; }
        public int SponsorId { get; set; }
        public int ExhibitorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthYear { get; set; }
        public int SponsorTypeId { get; set; }
        public string IdNumber { get; set; }

    }
    public class UnassignedSponsorExhibitor
    {
        public int ExhibitorId { get; set; }
        public string Name { get; set; }
    }
    public class SponsorExhibitorListResponse
    {
        public int TotalRecords { get; set; }
        public List<SponsorExhibitorResponse> SponsorExhibitorResponses { get; set; }
        public List<UnassignedSponsorExhibitor> UnassignedSponsorExhibitor { get; set; }
    }
}
