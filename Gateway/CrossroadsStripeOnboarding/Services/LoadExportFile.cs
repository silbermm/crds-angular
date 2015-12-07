using System.IO;
using CrossroadsStripeOnboarding.Models;

namespace CrossroadsStripeOnboarding.Services
{
    public class LoadExportFile
    {
        public static Messages ReadFile(string file)
        {
            var msg = ValidateIsFile(file);
            return msg == Messages.FileFound ? LoadFileToJson(file) : msg;
        }

        private static Messages ValidateIsFile(string file)
        {
            if (file == null || file.Trim().Length == 0)
            {
                return Messages.FileNameRequired;
            }

            if (file.Equals("X"))
            {
                return Messages.Close;
            }

            return File.Exists(file) ? Messages.FileFound : Messages.FileNotFound;
        }

        private static Messages LoadFileToJson(string file)
        {
            return Messages.LoadFileSuccess;
        }

        public static void ImportFile()
        {
            
        }
    }
}
