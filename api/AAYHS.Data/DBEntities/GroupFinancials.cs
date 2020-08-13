using AAYHS.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class GroupFinancials : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int GroupFinancialId { get; set; }
        public int GroupId { get; set; }
        public DateTime Date { get; set; }
        public int FeeTypeId { get; set; }
        public int TimeFrameId { get; set; }
        public double Amount { get; set; }
    }
}
