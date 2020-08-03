using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace AAYHS.Core.DTOs.Response.Common
{
    public class ClassResponse
    {
        public int ClassId { get; set; }
        public string ClassHeader { get; set; }
        public string ClassNumber { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string AgeGroup { get; set; }
        public int Entries { get; set; }
        public DateTime ScheduleDate { get; set; }
        public TimeSpan SchedulTime { get; set; }
        public int SplitNumber { get; set; }
        public bool ChampionShipIndicator { get; set; }
        public List<GetSplitClass> getSplitClass { get; set; }
    }
    public class GetSplitClass
    {       
        public int Entries { get; set; }
       
    }
    public class GetAllClasses
    {
        public List<ClassResponse> classesResponse { get; set; }
        public int TotalRecords { get; set; }
    }
    public class GetClass
    {
      public List<ClassResponse> classResponse { get; set; }
      public int TotalRecords { get; set; }
    }
    public class GetClassAllExhibitors
    {
        public List<GetClassExhibitors> getClassExhibitors { get; set; }

    }
    public class GetClassExhibitors
    {
        public int ExhibitorId { get; set; }
        public string Exhibitor { get; set; }
    }
    public class GetAllClassEntries
    {
        public List<GetClassEntries> getClassEntries { get; set; }
        public int TotalRecords { get; set; }
    }
    public class GetClassEntries
    {
        public int ExhibitorClassId { get; set; }
        public string Exhibitor { get; set; }
        public string Horse { get; set; }
        public DateTime BirthYear { get; set; }
        public decimal AmountPaid { get; set; }
        public int AmountDue { get; set; }
        public bool Scratch { get; set; }
    }
    public class GetAllBackNumber
    {
        public List<GetBackNumber> getBackNumbers { get; set; }
    }
    public class GetBackNumber
    {
        public int BackNumber { get; set; }
    }
    public class ResultExhibitorDetails
    {
        public int ExhibitorId { get; set; }
        public string ExhibitorName { get; set; }
        public DateTime BirthYear { get; set; }
        public string HorseName { get; set; }
        public string Address { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal AmountDue { get; set; }
    }
    public class GetExhibitorAllHorses
    {
        public List<GetExhibitorHorses> getExhibitorHorses { get; set; }
        public int TotalRecords { get; set; }
    }
    public class GetExhibitorHorses
    {
        public int HorseId { get; set; }
        public string Horse { get; set; }
    }
    public class GetResultOfClass
    {
        public string Place { get; set; }
        public int BackNumber { get; set; }
        public int ExhibitorId { get; set; }
        public string ExhibitorName { get; set; }
        public DateTime BirthYear { get; set; }
        public string HorseName { get; set; }
        public string Address { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal AmountDue { get; set; }
    }
    public class GetResult
    {
        public List<GetResultOfClass> getResultOfClass { get; set; }
        public int TotalRecords { get; set; }
    }
}
