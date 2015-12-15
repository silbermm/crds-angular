using System;

namespace CrossroadsStripeOnboarding.Models
{
    using System.Data.Entity;
    using System.Configuration;

    public class MinistryPlatformContext : DbContext
    {
        public MinistryPlatformContext() : base(GetConnectionString()) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DonorAccount>()
                .HasMany(e => e.RecurringGifts)
                .WithRequired(e => e.DonorAccount)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Donor>()
                .HasMany(e => e.DonorAccounts)
                .WithRequired(e => e.Donor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Donor>()
                .HasMany(e => e.RecurringGifts)
                .WithRequired(e => e.Donor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RecurringGift>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<RecurringGift>()
                .Property(e => e.Subscription_ID)
                .IsUnicode(false);
        }

        public DbSet<Donor> Donors { get; set; }
        public DbSet<DonorAccount> DonorAccounts { get; set; }
        public DbSet<RecurringGift> RecurringGifts { get; set; }

        public static string GetConnectionString()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MinistryPlatformContext"].ConnectionString;
            connectionString = connectionString.Replace("%MP_API_DB_USER%", Environment.GetEnvironmentVariable("MP_API_DB_USER"));

            return connectionString.Replace("%MP_API_DB_PASSWORD%", Environment.GetEnvironmentVariable("MP_API_DB_PASSWORD"));
        }
    }
}
