using AAYHS.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cleng.DataAccess.HorseShowEntities
{
   public class Addresses : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AddressId { get; set; }
        public string Address { get; set; }
        public string AddressExtension { get; set; }
        public string ZipCode { get; set; }
        public int CityId { get; set; }
    }


}
