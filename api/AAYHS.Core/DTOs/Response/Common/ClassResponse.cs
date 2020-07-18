using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response.Common
{
    public class ClassResponse
    {
        public int ClassId { get; set; }
        public int SponsorId { get; set; }
        public int ClassNumber { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int FromAge { get; set; }
        public int ToAge { get; set; }       
        public int Entries { get; set; }
    }
    public class GetAllClasses
    {
        public List<ClassResponse> classResponses { get; set; }
    }
}
