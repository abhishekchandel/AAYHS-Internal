using AAYHS.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class SponsorDistributions : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int SponsorDistributionId { get; set; }
        public int SponsorId { get; set; }
        public int SponsorTypeId { get; set; }
        public string TypeId { get; set; }
        public int AdTypeId { get; set; }
        public decimal TotalDistribute { get; set; }
    }
}
