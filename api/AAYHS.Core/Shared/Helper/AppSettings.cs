using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.Shared.Helper
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string Timeout { get; set; }
        public string ApplicationContext { get; set; }
        public bool EnableAPILog { get; set; }
        public string ErrorLoggingType { get; set; }

    }
}
