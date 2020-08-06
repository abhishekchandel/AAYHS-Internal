using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Request
{
    public class HorseRequest : BaseRecordFilterRequest
    {
        public int HorseId { get; set; }
    }
    public class HorseAddRequest
    {
        public int HorseId { get; set; }
        public int HorseTypeId { get; set; }
        public int BackNumber { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string JumpHeight { get; set; }
        public bool NSBAIndicator { get; set; }
        public int StallId { get; set; }
        public int TackStallId { get; set; }
    }
        
}
