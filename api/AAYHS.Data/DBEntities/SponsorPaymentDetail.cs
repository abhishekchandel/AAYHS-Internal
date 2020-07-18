using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class SponsorPaymentDetail:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int SponsorPaymentId { get; set; }
        public int FeeId { get; set; }
        public int SponsorId { get; set; }
        public decimal Amount { get; set; }
        public int TimeFrameTypeId { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string CheckNumber { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string Description { get; set; }
    }
}
