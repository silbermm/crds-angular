namespace CrossroadStripeOnboarding
{
    using System.Data.Entity;

    public class StripeOnboardingContext : DbContext
    {
        public StripeOnboardingContext()
            : base("StripeOnboardingContext")
        {
        }

        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
