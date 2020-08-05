using AAYHS.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class Classes : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ClassId { get; set; }
        public int ClassHeaderId { get; set; }
        public string ClassNumber { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string AgeGroup { get; set; }
     
    }
}
