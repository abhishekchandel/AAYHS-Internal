using System;
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
        public int ClassHeaderId { get; set; }
        public string ClassNumber { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string AgeGroup { get; set; }    
        public DateTime? ScheduleDate { get; set; }
        public TimeSpan? ScheduleTime { get; set; }
        public int SplitNumber { get; set; }
        public bool ChampionShipIndicator { get; set; }
        public List<SplitEntries> getClassSplit { get; set; }
    }
    public class AddClassExhibitor
    {
        public int ExhibitorId { get; set; }
        public int ClassId { get; set; }     
        public int HorseId { get; set; }
        public bool Scratch { get; set; }
    }
    
    public class SplitRequest
    {
        public int ClassId { get; set; }
        public int SplitNumber { get; set; }
        public bool ChampionShipIndicator { get; set; }
        public List<SplitEntries> splitEntries { get; set; }
    }
    public class SplitEntries
    {
        public int Entries { get; set; }
    }
    public class ResultExhibitorRequest
    {
        public int BackNumber { get; set; }
        public int ClassId { get; set; }
    }  
    public class AddClassResultRequest
    {
        public int ClassId { get; set; }
        public int ExhibitorId { get; set; }
        public int Place { get; set; }
    }    
   public class SearchRequest : BaseRecordFilterRequest
   {
        public string SearchTerm { get; set; }
   }
    public class ClassExhibitorScratch
    {
        public int ExhibitorClassId { get; set; }
        public bool IsScratch { get; set; }
    }
    public class UpdateClassResult
    {
        public int ResultId { get; set; }
        public int Place { get; set; }
        public int ExhibitorId { get; set; }
    }
}
