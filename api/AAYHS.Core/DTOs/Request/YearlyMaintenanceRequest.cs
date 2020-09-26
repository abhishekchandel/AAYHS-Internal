using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Request
{
    public class GetAllYearlyMaintenanceRequest: BaseRecordFilterRequest
    {
        public int YearlyMaintenanceId { get; set; }
    }
    public class UserApprovedRequest
    {
        public int UserId { get; set; }
        public bool IsApproved { get; set; }
    }
}
