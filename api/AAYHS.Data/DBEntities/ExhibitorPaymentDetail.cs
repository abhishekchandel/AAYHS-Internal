using AAYHS.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class ExhibitorPaymentDetail:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ExhibitorPaymentId { get; set; }
        public int FeeId { get; set; }
        public int ExhibitorId { get; set; }
        public decimal Amount { get; set; }
        public int TimeFrameTypeId { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string CheckNumber { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string Description { get; set; }
        public DateTime PayDate { get; set; }
    }
}
