using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Request
{
    public class ClassRequest: BaseRecordFilterRequest
    {
        public int ClassId { get; set; }
    }

    public class AddClassRequest
    {
        public int ClassId { get; set; }
        public int SponsorId { get; set; }
        public int ClassNumber { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int FromAge { get; set; }
        public int ToAge { get; set; }
        public DateTime ScheduleDate { get; set; }
        public TimeSpan ScheduleTime { get; set; }
        public string CreatedBy { get; set; }
    }
}
