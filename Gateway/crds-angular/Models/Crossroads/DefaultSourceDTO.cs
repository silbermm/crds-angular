

namespace crds_angular.Models.Crossroads
{
    public class DefaultSourceDTO
    {
        public CreditCardDTO credit_card { get; set; }
        public BankAccountDTO bank_account { get; set; }
    }

    public class BankAccountDTO
    {
        public string routing { get; set; }
        public string last4 { get; set; }
    }

    public class CreditCardDTO
    {
        public string last4 { get; set; }
        public string brand { get; set; }
        public string address_zip { get; set; }
        public string exp_date { get; set; }
    }

}