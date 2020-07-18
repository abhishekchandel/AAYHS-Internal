using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class GlobalCodeCategories:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int GlobalCodeCategoryId { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string CategoryName { get; set; }
        [Column(TypeName = "varchar(1000)")]
        public string Description { get; set; }

    }
}
