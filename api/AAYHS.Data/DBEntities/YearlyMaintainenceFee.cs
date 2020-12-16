using AAYHS.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class YearlyMaintainenceFee:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int YearlyMaintainenceFeeId { get; set; }
        public int YearlyMaintainenceId { get; set; }       
        public string FeeType { get; set; }
        public string FeeName{ get; set; }
        public string TimeFrame { get; set; }
        public decimal Amount { get; set; }
        public decimal RefundPercentage { get; set; }
    }
}
