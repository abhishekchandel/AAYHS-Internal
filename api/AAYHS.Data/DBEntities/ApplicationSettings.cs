using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class ApplicationSettings
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ApplicationSettingsId { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string CompanyEmail { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string CompanyPassword { get; set; }
    }
}
