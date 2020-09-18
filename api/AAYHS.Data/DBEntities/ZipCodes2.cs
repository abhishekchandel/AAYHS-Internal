using AAYHS.Data.Base;
using AAYHS.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
   public class ZipCodes2
    {
        public int ZipCodeId { get; set; }
        public string StateAbbrv { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
    }
}
