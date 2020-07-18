using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class Groups:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int GroupId { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string GroupName { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string ContactName { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string ContactPhone { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string ContactEmail { get; set; }
    }
}
