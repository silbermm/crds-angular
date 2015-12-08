using System;
using System.Collections.Generic;
using CrossroadsStripeOnboarding.Models;
using CrossroadsStripeOnboarding.Models.Json;
using CrossroadsStripeOnboarding.Services;

namespace CrossroadsStripeOnboarding
{
    class Program
    {
        static void Main()
        {
            LoadAndImportFile();
        }

        private static void LoadAndImportFile()
        {
            var result = new KeyValuePair<Messages, StripeJsonExport>(Messages.NotRun, null);

            while (result.Key != Messages.ImportFileSuccess && result.Key != Messages.SkipImportProcess )
            {
                Console.WriteLine("Enter the exports file path/location to import.  Otherwise press S to skip this step or X to close the program: ");
                result = LoadExportFile.ReadFile(Console.ReadLine());

                switch (result.Key)
                {
                    case Messages.ReadFileSuccess:
                        result = LoadExportFile.ImportFile(result.Value);

                        if (result.Key == Messages.ImportFileSuccess)
                        {
                            Console.WriteLine("The file was processed successfully.");
                        }
                        break;
                    case Messages.FileNameRequired:
                    case Messages.FileNotFound:
                        Console.WriteLine("Please enter a valid file.");
                        break;
                    case Messages.FileContainsInvalidData:
                        Console.WriteLine("The file contains invalid data please investigate.");
                        break;
                    case Messages.Close:
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}