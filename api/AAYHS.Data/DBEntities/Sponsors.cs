using AAYHS.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
   public class Sponsors : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int SponsorId { get; set; }
        public int SponsorTypeId { get; set; }
        public int AddSizeId { get; set; }
        public int AddNumberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public float AmountReceived { get; set; }
        public string Phone { get; set; }
        public string Comments { get; set; }

    }
}
