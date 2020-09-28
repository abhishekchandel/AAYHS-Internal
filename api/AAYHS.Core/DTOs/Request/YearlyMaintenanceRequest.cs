﻿using System;
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
    public class AddYearlyRequest
    {
        public DateTime Year { get; set; }
        public DateTime ShowStartDate { get; set; }
        public DateTime ShowEndDate { get; set; }
        public DateTime PreCutOffDate { get; set; } 
        public DateTime SponcerCutOffDate { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
    }
}
