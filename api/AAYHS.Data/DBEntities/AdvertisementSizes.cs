using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace AAYHS.Data.DBEntities
{
    public class AdvertisementSizes : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AdvertisementSizeId { get; set; }
        public string Size { get; set; }
        public string Comments { get; set; }
    }
}
