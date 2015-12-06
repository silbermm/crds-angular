namespace CrossroadsStripeOnboarding.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Models.StripeOnboardingContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Models.StripeOnboardingContext context)
        {
        }
    }
}
