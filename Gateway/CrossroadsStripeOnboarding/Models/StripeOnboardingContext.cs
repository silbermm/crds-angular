namespace CrossroadsStripeOnboarding.Models
{
    using System.Data.Entity;

    public class StripeOnboardingContext : DbContext
    {
        public StripeOnboardingContext(): base("StripeOnboardingContext")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //one-to-many StripeCustomer has many StripeAccounts
            modelBuilder.Entity<StripeAccount>().HasRequired<StripeCustomer>(s => s.StripeCustomer).WithMany(s => s.StripeAccounts).HasForeignKey(s => s.StripeCustomerId);
        }

        public DbSet<StripeCustomer> StripeCustomers { get; set; }
        public DbSet<StripeAccount> StripeAccounts { get; set; } 
    }
}
