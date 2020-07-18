using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class TackStallAssignment:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int TackStallAssignmentId { get; set; }
        public int TackStallId { get; set; }
        public int GroupId { get; set; }
        public int HorseId { get; set; }
        public int ExhibitorId { get; set; }
        [Column(TypeName = "varchar(5000)")]
        public int Comments { get; set; }
    }
}
