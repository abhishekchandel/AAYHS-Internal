using AAYHS.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class Scans : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ScansId { get; set; }
        public int ExhibitorId { get; set; }
        public int DocumentType { get; set; }
        [Column(TypeName = "varchar(5000)")]
        public string DocumentPath { get; set; }
    }
}
