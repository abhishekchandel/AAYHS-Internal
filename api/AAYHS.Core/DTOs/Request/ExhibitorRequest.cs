﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Request
{
   public class ExhibitorRequest
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
        public int ZipCodeId { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string Address { get; set; }
   }
   public class AddExhibitorHorseRequest
   {
        public int ExhibitorId { get; set; }
        public int HorseId { get; set; }
        public int BackNumber { get; set; }
   }
    public class AddExhibitorToClass
    {
        public int ExhibitorId { get; set; }
        public int ClassId { get; set; }
    }
    public class UpdateScratch
    {
        public int exhibitorClassId { get; set; }
        public bool IsScratch { get; set; }
    }
}
