﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Request
{
    public class ClassRequest: BaseRecordFilterRequest
    {
        public int ClassId { get; set; }
    }

    public class AddClassRequest
    {
        public int ClassId { get; set; }      
        public int ClassNumber { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string AgeGroup { get; set; }    
        public DateTime ScheduleDate { get; set; }
        public TimeSpan ScheduleTime { get; set; }
    }
    public class AddClassExhibitor
    {
        public int ExhibitorId { get; set; }
        public int ClassId { get; set; }     
        public int HorseId { get; set; }
        public bool Scratch { get; set; }
    }
    public class RemoveClass
    {
        public int ClassId { get; set; }
    }
    public class SplitRequest
    {
        public int ClassId { get; set; }
        public int SplitNumber { get; set; }
        public int Entries { get; set; }
    }
    public class ResultExhibitorRequest
    {
        public int BackNumber { get; set; }
        public int ClassId { get; set; }
    }
    public class BackNumberRequest
    {
        public int ClassId { get; set; }
    }
}
