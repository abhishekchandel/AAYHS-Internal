using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response.Common
{
    public class CityResponse
    {
        public List<Cities> City { get; set; }
    }

    public class Cities
    {
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string Name { get; set; }
    }

}
