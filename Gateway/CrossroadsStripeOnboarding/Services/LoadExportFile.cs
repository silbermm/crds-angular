using System.Collections.Generic;
using System.IO;
using CrossroadsStripeOnboarding.Models;
using CrossroadsStripeOnboarding.Models.Json;
using Newtonsoft.Json;
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

            if (file.Equals("S"))
            {
                return Messages.SkipImportProcess;
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

        public static KeyValuePair<Messages, StripeJsonExport> ImportFile(StripeJsonExport jsonExport)
        {
            using (var db = new StripeOnboardingContext())
            {
                ImportCustomerAndAccounts(db, jsonExport.CustomersMap);
            }

            return new KeyValuePair<Messages, StripeJsonExport>(Messages.ImportFileSuccess, jsonExport);
        }

        private static void ImportCustomerAndAccounts(StripeOnboardingContext db, Dictionary<string, StripeJsonCustomer> customersMap)
        {
            foreach (var customerDetails in customersMap)
            {
                var customer = ImportCustomer(db, customerDetails.Key, customerDetails.Value);
                ImportAccounts(db, customerDetails.Value, customer);
                db.SaveChanges();
            }
        }

        private static StripeCustomer ImportCustomer(StripeOnboardingContext db, string oldCustomerId, StripeJsonCustomer customerDetails)
        {
            var customer = new StripeCustomer
            {
                CustomerId = customerDetails.NewCustomerId,
                ExternalPersonId = oldCustomerId,
                Imported = false,
            };


            db.StripeCustomers.Add(customer);
            return customer;
        }

        private static void ImportAccounts(StripeOnboardingContext db, StripeJsonCustomer customerDetails, StripeCustomer customer)
        {
            foreach (var bankDetails in customerDetails.BanksMap)
            {
                ImportAccount(db, bankDetails.Key, bankDetails.Value, customer);
            }

            foreach (var cardDetails in customerDetails.CardsMap)
            {
                ImportAccount(db, cardDetails.Key, cardDetails.Value, customer);
            }
        }

        private static void ImportAccount(StripeOnboardingContext db, string oldAccountId, StripeJsonAccount accountDetails, StripeCustomer customer)
        {
            var account = new StripeAccount
            {
                Type = accountDetails.Type,
                OldCardId = oldAccountId,
                NewCardId = accountDetails.NewAccountId,
                Fingerprint = accountDetails.Fingerprint,
                Last4 = accountDetails.Last4,
                ExpMonth = accountDetails.ExpMonth,
                ExpYear = accountDetails.ExpYear,
                Institution = accountDetails.Institution,
                StripeCustomerId = customer.Id,
            };

            db.StripeAccounts.Add(account);
        }
    }
}
