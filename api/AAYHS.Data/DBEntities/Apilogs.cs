using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public partial class Apilogs
    {
        public int ApilogId { get; set; }
        public string Apiurl { get; set; }
        public string Apiparams { get; set; }
        public string Headers { get; set; }
        public string Method { get; set; }
        public bool Success { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public int? ErrorLogId { get; set; }

        public virtual ErrorLogs ErrorLog { get; set; }
    }
}
