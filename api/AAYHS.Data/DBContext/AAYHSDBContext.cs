using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Data.DBContext
{
    public class  AAYHSDBContext : DbContext
    {
        public AAYHSDBContext(DbContextOptions options) : base(options)
        {

        }
    }

  
}
