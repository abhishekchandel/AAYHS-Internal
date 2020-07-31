using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response.Common
{
    public class CityResponse
    {
        public List<City> City { get; set; }
    }

    public class City
    {
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string Name { get; set; }
    }

}
