using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Request
{
   public class BaseRequest
    {
        public string UserName { get; set; }
    }
    public class ActionBaseRequest
    {
        /// <summary>
        /// User logged id,leave it as blank
        /// </summary>
        public string ActionBy { get; set; }
    }
    public class BaseRecordFilterRequest
    {
        /// <summary>
        /// Send According to the number of page required
        /// </summary>
        public int Page { get; set; } = 1;
        /// <summary>
        /// Number of record required on a page
        /// </summary>
        public int Limit { get; set; } = 10;
        /// <summary>
        /// Need to send like UserId,CreatedOn etc
        /// </summary>
        public string OrderBy { get; set; } = "CreatedOn";
        /// <summary>
        /// Need to send true if record required  in descending order
        /// </summary>
        public bool OrderByDescending { get; set; } = true;
        /// <summary>
        /// Need to send false when page and limit are sending and for all records send true
        /// </summary>
        public bool AllRecords { get; set; } = false;
    }
}
