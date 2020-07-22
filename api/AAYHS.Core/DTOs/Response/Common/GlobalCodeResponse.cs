using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response.Common
{
    public class GlobalCodeMainResponse
    {
        public List<GlobalCodeResponse> globalCodeResponse { get; set; }
        public int totalRecords { get; set; }
    }
    public class GlobalCodeResponse
    {
        public int CategoryId { get; set; }
        public int GlobalCodeId { get; set; }
        public string CodeName { get; set; }
        public string Description { get; set; }
        public string GlobalCodeCategory { get; set; }
    }
}
