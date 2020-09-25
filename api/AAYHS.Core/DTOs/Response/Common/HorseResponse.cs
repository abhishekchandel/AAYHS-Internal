using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response.Common
{
    public class HorseResponse
    {
        public int HorseId { get; set; }
        public string HorseType { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; }     
        public int Number { get; set; }    
        public string JumpHeight { get; set; }
        public bool NSBAIndicator { get; set; }      

    }
    public class GetAllHorses
    {
        public List<HorseResponse> horsesResponse { get; set; }
        public int TotalRecords { get; set; }
    }
    public class GetHorseById
    {
        public int HorseId { get; set; }
        public string Name { get; set; }
        public int HorseTypeId { get; set; }
        public int Number { get; set; }
        public int GroupId { get; set; }
        public int JumpHeightId { get; set; }
        public bool NSBAIndicator { get; set; }        
    }   
    public class GetLinkedExhibitors
    {
        public int ExhibitorId { get; set; }
        public string ExhibitorName { get; set; }
        public int? BirthYear { get; set; }
    }
    public class GetAllLinkedExhibitors
    {
        public List<GetLinkedExhibitors> getLinkedExhibitors { get; set; }
        public int TotalRecords { get; set; }
    }
    public class GetAllGroups
    {
       public List<GetGroup> getGroups { get; set; }      
    }
    public class GetGroup
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
    }
}
