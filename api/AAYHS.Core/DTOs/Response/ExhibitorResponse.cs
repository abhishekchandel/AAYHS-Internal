using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response
{
   public class ExhibitorResponse
    {
        public int ExhibitorId { get; set; }
        public int GroupId { get; set; }
        public int AddressId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BackNumber { get; set; }
        public DateTime BirthYear { get; set; }
        public bool IsNSBAMember { get; set; }
        public bool IsDoctorNote { get; set; }
        public string QTYProgram { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }
        public string Phone { get; set; }
    }
    public class ExhibitorListResponse
    {
        public List<ExhibitorResponse> exhibitorResponses { get; set; }
    }
}
