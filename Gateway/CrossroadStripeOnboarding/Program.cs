using System;
using System.Linq;

namespace CrossroadStripeOnboarding
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new StripeOnboardingContext())
            {
                // Create and save a new Blog 
                Console.Write("Enter the File Path for the stripe onboarding export file: ");
                var name = Console.ReadLine();

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            } 
        }
    }
}
