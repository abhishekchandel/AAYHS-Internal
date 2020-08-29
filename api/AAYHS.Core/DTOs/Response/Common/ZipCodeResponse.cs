using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response.Common
{
    public class ZipCodeResponse
    {
       public List<ZipCode> ZipCode { get; set; }
    }
    public class ZipCode
    {
        public int ZipCodeId { get; set; }
        public int Number { get; set; }
        public int CityId { get; set; }
    }

}
