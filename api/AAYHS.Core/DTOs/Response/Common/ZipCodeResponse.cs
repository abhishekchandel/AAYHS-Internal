using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response.Common
{
    public class ZipCodeResponse
    {
       public List<GetZipCodes> ZipCode { get; set; }
    }
    public class GetZipCodes
    {
        public int ZipCodeId { get; set; }
        public int ZipCode { get; set; }       
    }

}
