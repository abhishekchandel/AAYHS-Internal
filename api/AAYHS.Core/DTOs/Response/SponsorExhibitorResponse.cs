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
        public DateTime BirthYear { get; set; }
        public int SponsorTypeId { get; set; }
        public int TypeId { get; set; }
        public string IdNumber { get; set; }

    }
}
