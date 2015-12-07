using System;
using CrossroadsStripeOnboarding.Models;
using CrossroadsStripeOnboarding.Services;

namespace CrossroadsStripeOnboarding
{
    class Program
    {
        static void Main()
        {
            var msg = Messages.NotRun;
            while (msg != Messages.Success)
            {
                Console.Write("Enter the exports file path/location or X to close the program: ");
                msg = LoadExportFile.ReadFile(Console.ReadLine());
                
                if (msg == Messages.LoadFileSuccess)
                {
                    //TODO::add success path
                }
                else if (msg == Messages.FileNameRequired || msg == Messages.FileNotFound)
                {
                    Console.Write("Please enter a valid file.");
                }
                else if (msg == Messages.FileContainsInvalidData)
                {
                    Console.Write("The file contains invalid data please investigate.");
                }
                else if (msg == Messages.Close)
                {
                    msg = Messages.Success;
                    Console.ReadKey();
                }
            }

            if (msg == Messages.Success)
            {
                Console.Write("The file was processed successfully.  Prease any key to exit.");
                Console.ReadKey();
            }
        }
    }
}