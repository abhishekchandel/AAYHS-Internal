using AAYHS.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class GlobalCodes:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int GlobalCodeId { get; set; }
        public int CategoryId { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string CodeName { get; set; }
        [Column(TypeName = "varchar(1000)")]
        public string Description { get; set; }
    }
}
