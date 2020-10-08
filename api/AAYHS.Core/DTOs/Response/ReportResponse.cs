using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response
{
    public class GetExhibitorRegistrationReport
    {
        public string Email1 { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string StateZipcode { get; set; }
        public string Phone1 { get; set; }
        public ExhibitorInfo exhibitorInfo { get; set; }
        public StallAndTackStallNumber stallAndTackStallNumber { get; set; }
        public HorseDetail horseDetail { get; set; }
        public FinancialsDetail financialsDetail { get; set; }

    }

    public class ExhibitorInfo
    {
        public string ExhibitorName { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string StateZipcode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
    public class StallAndTackStallNumber
    {
        public int HorseStallNumber { get; set; }
        public int TackStallNumber { get; set; }
        public int ExhibitorBirthYear { get; set; }
    }
    public class HorseDetail
    {
        public string HorseName { get; set; }
        public int? BackNumber { get; set; }
        public int ExhibitorId { get; set; }
        public bool NSBAIndicator { get; set; }
    }
    public class FinancialsDetail
    {
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
        public bool NSBA { get; set; }
        public string HorseName { get; set; }
        public string ExhibitorName { get; set; }
        public string City { get; set; }
        public string StateZipcode { get; set; }
    }
}
