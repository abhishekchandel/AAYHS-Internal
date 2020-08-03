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
    }
    public class ClassSponsorListResponse
    {
        public int TotalRecords { get; set; }
        public List<ClassSponsorResponse> classSponsorResponses { get; set; }
    }

    public class SponsorClassResponse
    {
        public int ClassSponsorId { get; set; }
        public int SponsorId { get; set; }
        public int ClassId { get; set; }
        public string ClassNumber { get; set; }
        public string Name { get; set; }
        public string AgeGroup { get; set; }
        public int ExhibitorId { get; set; }
        public string ExhibitorName { get; set; }
        public int HorseId { get; set; }
        public string HorseName { get; set; }
        public List<string> ClassExhibitorsAndHorses { get; set; }

    }
    public class SponsorClassesListResponse
    {
        public int TotalRecords { get; set; }
        public List<SponsorClassResponse> sponsorClassesListResponses { get; set; }
    }
}
