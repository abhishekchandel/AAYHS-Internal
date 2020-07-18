using AAYHS.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
   public class ClassSplits : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ClassSplitId { get; set; }
        public int ClassId { get; set; }
        public int SplitNumber { get; set; }
        public int Quantity { get; set; }
       
    }
}
