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
        public int GroupId { get; set; }
        public string Name { get; set; }
        public int JumpHeightId { get; set; }
        public bool NSBAIndicator { get; set; }       
    }
     public class HorseExhibitorRequest : BaseRecordFilterRequest
    {
        public int HorseId { get; set; }
        
     }
}
