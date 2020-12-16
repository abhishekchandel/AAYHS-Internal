using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Request
{
    public class HorseExhibitorsRequest
    {
        public int ExhibitorId { get; set; }
        public int HorseId { get; set; }
    }
    public class EmailDocumentRequest
    {
     
        public string emailid { get; set; }
        public IFormFile reportfile { get; set; }
    }
    public class EmailDocumentRequest1
    {

        public string emailid { get; set; }
        public string reportfile { get; set; }
    }
}
