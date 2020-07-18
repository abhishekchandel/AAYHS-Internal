using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Request
{
    public class BaseRecordFilterRequest
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public string OrderBy { get; set; }
        public bool OrderByDescending { get; set; }
        public bool AllRecords { get; set; }
    }
}
