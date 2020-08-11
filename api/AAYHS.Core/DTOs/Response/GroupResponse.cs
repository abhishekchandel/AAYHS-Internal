using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response
{
   public class GroupResponse
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public float AmountReceived { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
    }
    public class GroupListResponse
    {
        public int TotalRecords { get; set; }
        public List<GroupResponse> groupResponses { get; set; }
    }
    public class GetGroupExhibitors
    {
        public int GroupExhibitorId { get; set; }
        public int ExhibitorId { get; set; }
        public string ExhibitorName { get; set; }       
        public int BirthYear { get; set; }
        public List<GroupExhibitorHorses> getGroupExhibitorHorses { get; set; }
    }
    public class GetAllGroupExhibitors
    {
        public List<GetGroupExhibitors> getGroupExhibitors { get; set; }
        public int TotalRecords { get; set; }
    }
    public class GroupExhibitorHorses
    {  
        public string HorseName { get; set; }
    }
}
