using System;
using System.Collections.Generic;
using System.Net.Mime;
using CrossroadsStripeOnboarding.Models;
using CrossroadsStripeOnboarding.Models.Json;
using CrossroadsStripeOnboarding.Services;

namespace CrossroadsStripeOnboarding
{
    class Program
    {
        static void Main()
        {
            var result = new KeyValuePair<Messages, StripeJsonExport>(Messages.NotRun, null);
            while (result.Key != Messages.Success)
            {
                Console.WriteLine("Enter the exports file path/location or X to close the program: ");
                result = LoadExportFile.ReadFile(Console.ReadLine());
                
                if (result.Key == Messages.ReadFileSuccess)
                {
                    //TODO::add success path
                    Console.WriteLine("The file was processed successfully.  Prease any key to exit.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                else if (result.Key == Messages.FileNameRequired || result.Key == Messages.FileNotFound)
                {
                    Console.WriteLine("Please enter a valid file.");
                }
                else if (result.Key == Messages.FileContainsInvalidData)
                {
                    Console.WriteLine("The file contains invalid data please investigate.");
                }
                else if (result.Key == Messages.Close)
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}