using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Data.DBEntities
{
   public class ErrorLogs
    {
        public int ErrorLogId { get; set; }
        public string ExceptionMsg { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionSource { get; set; }
        public DateTime? LogDateTime { get; set; }
        public virtual ICollection<Apilogs> Apilogs { get; set; }
    }
}
