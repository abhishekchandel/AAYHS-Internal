using AAYHS.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class RefundDetail:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int RefundDetailId { get; set; }
        public int ExhibitorId { get; set; }
        public int RefundTypeId { get; set; }
        public decimal RefundPercentage { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal RefundAmount { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string CheckNumber { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string Description { get; set; }
    }
}
