using AAYHS.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class YearlyMaintainence:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int YearlyMaintainenceId { get; set; }
        [Column(TypeName = "Date")]
        public DateTime Year { get; set; }
        public DateTime ShowStartDate { get; set; }
        public DateTime ShowEndDate { get; set; }
        public DateTime PreEntryCutOffDate { get; set; }      
        public DateTime SponcerCutOffDate { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Location { get; set; }
        public DateTime Date { get; set; }

    }
}
