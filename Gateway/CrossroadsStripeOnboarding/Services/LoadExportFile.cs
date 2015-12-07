using System.Collections.Generic;
using System.Data.Entity.Migrations.Sql;
using System.IO;
using CrossroadsStripeOnboarding.Models;
using CrossroadsStripeOnboarding.Models.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CrossroadsStripeOnboarding.Services
{
    public class LoadExportFile
    {
        public static KeyValuePair<Messages, StripeJsonExport> ReadFile(string file)
        {
            var msg = ValidateIsFile(file);
            return msg == Messages.FileFound ? LoadFileToJson(file) : new KeyValuePair<Messages, StripeJsonExport>(msg, null);
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

        private static KeyValuePair<Messages, StripeJsonExport> LoadFileToJson(string file)
        {
            // read JSON directly from a file
            using (var reader = File.OpenText(@file))
            {
                var serializer = new JsonSerializer();
                var jsonExport = (StripeJsonExport) serializer.Deserialize(reader, typeof(StripeJsonExport));

                return new KeyValuePair<Messages, StripeJsonExport>(Messages.ReadFileSuccess, jsonExport); 
            }
        }
    }
}
