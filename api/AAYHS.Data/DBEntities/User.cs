using AAYHS.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AAYHS.Data.DBEntities
{
        public class User : BaseEntity
        {
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            [Key]
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }
}
