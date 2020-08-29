using AAYHS.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
   public class ScheduleDates : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ScheduleDateId { get; set; }
        public int ClassId { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
    }
}
