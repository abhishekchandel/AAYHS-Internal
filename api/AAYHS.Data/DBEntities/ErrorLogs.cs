using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class ErrorLogs
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Key]
        public int ErrorLogId { get; set; }
        public string ExceptionMsg { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionSource { get; set; }
        public DateTime? LogDateTime { get; set; }
        public virtual ICollection<Apilogs> Apilogs { get; set; }
    }
}
