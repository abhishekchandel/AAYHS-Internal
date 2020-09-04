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
        public int QTYProgram { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }
        public string Phone { get; set; }
        public int ZipCodeId { get; set; }
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
        public string HorseType { get; set; }
    }
    public class GetExhibitorHorsesList
    {
        public List<GetHorses> getHorses { get; set; }
    }
    public class GetClassesOfExhibitor
    {
        public int ExhibitorClassId { get; set; }
        public string ClassNumber { get; set; }
        public string Name { get; set; }
        public string AgeGroup { get; set; }
        public int Entries { get; set; }
        public bool Scratch { get; set; }
    }
    public class GetAllClassesOfExhibitor
    {
        public List<GetClassesOfExhibitor> getClassesOfExhibitors { get; set; }

        public int TotalRecords { get; set; }
    }
    public class GetClassesForExhibitor
    {
        public int ClassId { get; set; }
        public string ClassNumber { get; set; }
        public string Name { get; set; }
        public string AgeGroup { get; set; }
        public int Entries { get; set; }
        public bool IsScratch { get; set; }
    }
    public class GetAllClassesForExhibitor
    {
        public List<GetClassesForExhibitor> getClassesForExhibitor { get; set; }
    }
    public class GetSponsorsOfExhibitor
    {
        public int SponsorExhibitorId { get; set; }
        public int SponsorId { get; set; }
        public string Sponsor { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zipcode { get; set; }
        public string Email { get; set; }
        public float Amount { get; set; }
        public int AdNumber { get; set; }
        public string AdSize { get; set; }
    }
    public class GetAllSponsorsOfExhibitor
    {
        public List<GetSponsorsOfExhibitor> getSponsorsOfExhibitors { get; set; }
        public int TotalRecords { get; set; }
    }
    public class GetSponsorForExhibitor
    {
        public int SponsorId { get; set; }
        public string SponsorName { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Email { get; set; }
        public float AmountReceived { get; set; }
    }
    public class GetAllSponsorForExhibitor
    {
        public List<GetSponsorForExhibitor> getSponsorForExhibitors { get; set; }     
    }
    public class GetSponsorDetailedInfo
    {
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
    }
}
