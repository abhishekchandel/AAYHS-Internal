using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class Stall:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int StallId { get; set; }
        public int StallNumber { get; set; }
        public int MaxNumberOfHorseAssignment { get; set; }
        [Column(TypeName = "varchar(5000)")]
        public string Comments { get; set; }
    }
}
