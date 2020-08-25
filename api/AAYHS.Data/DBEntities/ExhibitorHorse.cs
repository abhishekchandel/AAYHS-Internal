using AAYHS.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class ExhibitorHorse:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ExhibitorHorseId { get; set; }
        public int ExhibitorId { get; set; }
        public int HorseId { get; set; }
        public int BackNumber { get; set; }
    }
}
