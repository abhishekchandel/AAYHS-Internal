using AAYHS.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
    public class AAYHSContactAddresses : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AAYHSContactAddressId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string ZipCode { get; set; }
        public string? Phone { get; set; }
    }
}
