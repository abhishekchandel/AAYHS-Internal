using AAYHS.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cleng.DataAccess.HorseShowEntities
{
    public class AdvertisementNumbers : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AdvertisementNumberId { get; set; }
        public string Number { get; set; }
        public string Comments { get; set; }
    }
  
}
