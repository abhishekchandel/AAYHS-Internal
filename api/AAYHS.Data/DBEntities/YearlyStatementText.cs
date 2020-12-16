using AAYHS.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class YearlyStatementText:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int YearlyStatementTextId { get; set; }
        public int YearlyMaintenanceId { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string StatementName { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string StatementNumber { get; set; }
        [Column(TypeName = "varchar(5000)")]
        public string StatementText { get; set; }
        public int? Incentive { get; set; }

    }
}
