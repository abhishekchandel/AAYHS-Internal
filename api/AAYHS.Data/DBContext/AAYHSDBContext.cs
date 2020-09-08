using AAYHS.Data.DBEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        public virtual DbSet<ClassSponsors> ClassSponsors { get; set; }
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
        public virtual DbSet<AAYHSContact> AAYHSContact { get; set; }
        public virtual DbSet<Addresses> Addresses { get; set; }
        public virtual DbSet<Advertisements> Advertisements { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<ClassSponsors> ClassSponsor { get; set; }
        public virtual DbSet<ExhibitorHorse> ExhibitorHorse { get; set; }
        public virtual DbSet<ExhibitorPaymentDetail> ExhibitorPaymentDetail { get; set; }
        public virtual DbSet<SponsorExhibitor> SponsorExhibitor { get; set; }
        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<RefundDetail> RefundDetail { get; set; }
        public virtual DbSet<Result> Result { get; set; }
        public virtual DbSet<SponsorPaymentDetail> SponsorPaymentDetail { get; set; }
      //  public virtual DbSet<Stall> Stall { get; set; }
        public virtual DbSet<StallAssignment> StallAssignment { get; set; }
        public virtual DbSet<States> States { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<YearlyMaintainence> YearlyMaintainence { get; set; }
        public virtual DbSet<YearlyMaintainenceFee> YearlyMaintainenceFee { get; set; }
        public virtual DbSet<ApplicationSettings> ApplicationSettings { get; set; }
        public virtual DbSet<GroupExhibitors> GroupExhibitors { get; set; }
        public virtual DbSet<GroupFinancials> GroupFinancials { get; set; }
        public virtual DbSet<ZipCodes> ZipCodes { get; set; }
        public virtual DbSet<Scans> Scans { get; set; }

    }

  
}
