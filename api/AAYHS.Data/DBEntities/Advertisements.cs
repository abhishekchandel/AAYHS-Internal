using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class Advertisements : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AdvertisementId { get; set; }
        public int AdvertisementTypeId { get; set; }
        public int AdvertisementSizeId { get; set; }
        public int AdvertisementNumberId { get; set; }
        public string Comments { get; set; }
       
    }
}
