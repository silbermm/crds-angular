using System;
using CrossroadsStripeOnboarding.Models;

namespace CrossroadsStripeOnboarding
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new StripeOnboardingContext())
            {
                // Create and save a new Blog 
                Console.Write("Enter the exports file path/location: ");
                var file = Console.ReadLine();

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            } 
        }
    }
}
