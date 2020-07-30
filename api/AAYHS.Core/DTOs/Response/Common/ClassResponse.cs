using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace AAYHS.Core.DTOs.Response.Common
{
    public class ClassResponse
    {
        public int ClassId { get; set; }
        public int SponsorId { get; set; }
        public int ClassNumber { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string AgeGroup { get; set; }
        public int Entries { get; set; }
    }
    public class GetAllClasses
    {
        public List<ClassResponse> classResponses { get; set; }
    }
    public class GetClass
    {
        public int ClassId { get; set; }
        public string SponsorName { get; set; }
        public int ClassNumber { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string AgeGroup { get; set; }
        public DateTime ScheduleDate { get; set; }
        public TimeSpan SchedulTime { get; set; }
    }
    public class GetAllClassExhibitor
    {
        public List<GetClassExhibitor> getClassExhibitors { get; set; }
    }
    public class ClassExhibitorHorses
    {
        public List<string> ClassExhibitorHorse { get; set; }
    }
    public class GetClassExhibitor
    {
        public int ExhibitorClassId { get; set; }
        public int ExhibitorNumber { get; set; }
        public string ExhibitorName { get; set; }
        public string Horse { get; set; }
        public DateTime BirthYear { get; set; }
        public decimal AmountPaid { get; set; }
        public int AmountDue { get; set; }
        public bool Scratch { get; set; }
    }
}
