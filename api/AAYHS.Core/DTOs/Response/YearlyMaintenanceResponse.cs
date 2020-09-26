﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response
{
    public class GetAllYearlyMaintenance
    {
        public List<GetYearlyMaintenance> getYearlyMaintenances { get; set; }
        public int TotalRecords { get; set; }
    }
    public class GetYearlyMaintenance
    {
        public int YearlyMaintenanceId { get; set; }
        public int Year { get; set; }
        public DateTime PreEntryCutOffDate { get; set; }
        public decimal PreEntryFee { get; set; }
        public decimal PostEntryFee { get; set; }
        public DateTime DateCreated { get; set; }
    }
    public class GetYearlyMaintenanceById
    {
        public int YearlyMaintenanceId { get; set; }
        public int Year { get; set; }
        public DateTime ShowStartDate { get; set; }
        public DateTime ShowEndDate { get; set; }
        public DateTime PreEntryCutOffDate { get; set; }       
        public string Location { get; set; }
        public DateTime Date { get; set; }
    }

}
