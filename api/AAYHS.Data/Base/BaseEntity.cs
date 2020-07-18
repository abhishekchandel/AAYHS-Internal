using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.Base
{
    public class BaseEntity
    {
        [DefaultValue("1")]
        public bool IsActive { get; set; } = true;
        [Column(TypeName = "varchar(50)")]
        public string CreatedBy { get; set; }

        [DefaultValue("GETDATE()")]
        [Column(TypeName = "Datetime")]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        [Column(TypeName = "varchar(50)")]
        public string ModifiedBy { get; set; }

        [Column(TypeName = "Datetime")]
        public DateTime? ModifiedDate { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string DeletedBy { get; set; }

        [Column(TypeName = "Datetime")]
        public DateTime? DeletedDate { get; set; }

        [DefaultValue("0")]
        public bool IsDeleted { get; set; }
    }
}
