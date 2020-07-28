using AAYHS.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class ExhibitorSponsor:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ExhibitorSponsorId { get; set; }
        public int ExhibitorId { get; set; }
        public int SponsorId { get; set; }
        public int SponsorTypeId { get; set; }
        public int TypeId { get; set; }
    }
}
