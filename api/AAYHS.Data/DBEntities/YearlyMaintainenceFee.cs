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
        public int TimeFrameTypeId { get; set; }
        public int FeeTypeId { get; set; }
        public decimal Amount { get; set; }       
    }
}
