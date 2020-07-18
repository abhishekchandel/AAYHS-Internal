﻿using AAYHS.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class StallAssignment:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int StallAssignmentId { get; set; }
        public int StallId { get; set; }
        public int GroupId { get; set; }
        public int HorseId { get; set; }
        public int ExhibitorId { get; set; }
        [Column(TypeName = "varchar(5000)")]
        public int Comments { get; set; }
    }
}
