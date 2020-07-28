﻿using AAYHS.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class Result:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ResultId { get; set; }
        public int ClassId { get; set; }
        public string AgeGroup { get; set; }
        public int ExhibitorId { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Placement { get; set; }       
    }
}
