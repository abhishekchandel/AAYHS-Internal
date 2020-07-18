using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class TackStall:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int TackStallId { get; set; }
        public int TackStallNumber { get; set; }
        public int MaxNumberOfHorseAssignment { get; set; }
        [Column(TypeName = "varchar(5000)")]
        public int Comments { get; set; }
    }
}
