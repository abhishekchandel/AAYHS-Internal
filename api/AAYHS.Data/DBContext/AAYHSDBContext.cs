using AAYHS.Data.DBEntities;
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

        public virtual DbSet<Apilogs> Apilogs { get; set; }
        public virtual DbSet<ErrorLogs> ErrorLogs { get; set; }
        public virtual DbSet<Classes> Classes { get; set; }
        public virtual DbSet<ClassSponsor> ClassSponsors { get; set; }
        public virtual DbSet<ScheduleDates> ScheduleDates { get; set; }
        public virtual DbSet<ExhibitorClass> ExhibitorClass { get; set; }
        public virtual DbSet<GlobalCodeCategories> GlobalCodeCategories { get; set; }
        public virtual DbSet<GlobalCodes> GlobalCodes { get; set; }
        public virtual DbSet<Exhibitors> Exhibitors { get; set; }
        public virtual DbSet<Horses> Horses { get; set; }
        public virtual DbSet<ExhibitorPaymentDetail> ExhibitorPaymentDetails { get; set; }
        public virtual DbSet<Fees> Fees { get; set; }
        public virtual DbSet<Sponsors> Sponsors { get; set; }
        public virtual DbSet<ClassSplits> ClassSplits { get; set; }
    }

  
}
