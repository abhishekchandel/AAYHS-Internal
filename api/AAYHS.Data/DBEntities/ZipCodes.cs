using AAYHS.Data.Base;
using AAYHS.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
   public class ZipCodes : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ZipCodeId { get; set; }
        public int Number { get; set; }
        public int CityId { get; set; }
    }
}
