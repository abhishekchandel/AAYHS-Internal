using AAYHS.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
   public class Horses : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int HorseId { get; set; }
        public int HorseTypeId { get; set; }
        public int BackNumber { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string JumpHeight { get; set; }
        public bool NSBAIndicator { get; set; }
    }
}
