using AAYHS.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class Fees:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int FeeId { get; set; }
        public int FeeTypeId { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string FeeName { get; set; }
        public int TimeFrameTypeId { get; set; }
        public float FeeAmount { get; set; }
    }
}
