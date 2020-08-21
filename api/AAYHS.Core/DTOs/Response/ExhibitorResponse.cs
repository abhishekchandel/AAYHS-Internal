using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response
{
   public class ExhibitorResponse
    {
        public int ExhibitorId { get; set; }
        public int GroupId { get; set; }
        public int AddressId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BackNumber { get; set; }
        public int BirthYear { get; set; }
        public bool IsNSBAMember { get; set; }
        public bool IsDoctorNote { get; set; }
        public string QTYProgram { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }
        public string Phone { get; set; }
        public string ZipCode { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string GroupName { get; set; }
        public string Address { get; set; }

    }
    public class ExhibitorListResponse
    {   
        public List<ExhibitorResponse> exhibitorResponses { get; set; }
        public int TotalRecords { get; set; }
    }
    public class ExhibitorHorses
    {
        public int ExhibitorHorseId { get; set; }
        public string HorseName { get; set; }
        public string HorseType { get; set; }
        public int BackNumber { get; set; }
    }
    public class ExhibitorHorsesResponse
    {
        public List<ExhibitorHorses> exhibitorHorses { get; set; }
        public int TotalRecords { get; set; }
    }
    public class GetHorses
    {
        public int HorseId { get; set; }
        public string Name { get; set; }
    }
    public class GetExhibitorHorsesList
    {
        public List<GetHorses> getHorses { get; set; }       
    }

}
