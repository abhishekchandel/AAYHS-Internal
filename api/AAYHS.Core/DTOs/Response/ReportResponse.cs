using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response
{
    public class GetExhibitorRegistrationReport
    {
        public string ExhibitorName { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string StateZipcode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }       
        public GetAAYHSContactInfo getAAYHSContactInfo { get; set; }
        public StallAndTackStallNumber stallAndTackStallNumber { get; set; }
        public List<HorseClassDetail> horseClassDetails { get; set; }
        public FinancialsDetail financialsDetail { get; set; }

    }

    public class GetAAYHSContactInfo
    {
        public string Email1 { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string StateZipcode { get; set; }
        public string Phone1 { get; set; }
    }
    public class StallAndTackStallNumber
    {
        public List<HorseStall> horseStalls { get; set; }
        public List<TackStall> tackStalls { get; set; }
        public int ExhibitorId { get; set; }
        public int? ExhibitorBirthYear { get; set; }
    }
    public class HorseStall
    {
        public int? HorseStallNumber { get; set; }
    }
    public class TackStall
    {
        public int? TackStallNumber { get; set; }
    }
    public class HorseClassDetail
    {
        public string HorseName { get; set; }
        public int? BackNumber { get; set; }
        public string ClassNumber { get; set; }
        public string ClassName { get; set; }
    }
    public class FinancialsDetail
    {
        public int ClassQty { get; set; }
        public decimal ClassAmount { get; set; }
        public int HorseStallQty { get; set; }       
        public decimal HorseStallAmount { get; set; }
        public int TackStallQty { get; set; }
        public decimal TackStallAmount { get; set; }
        public decimal Refund { get; set; }
        public decimal AmountDue { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal Overpayment { get; set; }
        public decimal BalanceDue { get; set; }

    }
    public class GetProgramReport
    {
        public string ClassNumber { get; set; }
        public string ClassName { get; set; }
        public string Age { get; set; }
        public List< SponsorInfo> sponsorInfo { get; set; }
        public List< ClassInfo> classInfo { get; set; }
    }
    public class SponsorInfo
    {
        public string SponsorName { get; set; }
        public string City { get; set; }
        public string StateZipcode { get; set; }
    }
    public class ClassInfo
    {
        public int? BackNumber { get; set; }
        public string NSBA { get; set; }
        public string HorseName { get; set; }
        public string ExhibitorName { get; set; }        
        public string CityStateZipcode { get; set; }
    }

    public class GetPaddockReport
    {
        public string ClassNumber { get; set; }
        public string ClassName { get; set; }
        public string Age { get; set; }
        public List<ClassDetail> classDetails { get; set; }
    }
    public class ClassDetail
    {
        public int? BackNumber { get; set; }
        public string Scratch { get; set; }
        public string NSBA { get; set; }
        public string HorseName { get; set; }
        public string ExhibitorName { get; set; }
        public string CityStateZipcode { get; set; }
        public int Split { get; set; }
    }
    public class GetAllClassesEntries
    {
        public List< GetClassEntriesCount> getClassEntriesCount { get; set; }
    }
    public class GetClassEntriesCount
    {
        public string ClassNumber { get; set; }
        public string ClassName { get; set; }
        public string  ClassAgeGroup { get; set; }
        public int EntryTotal { get; set; }
    }
}
