using System;
using System.Collections.Generic;

namespace CrossroadsStripeOnboarding.Models
{
    public class StripeCustomer
    {
        public StripeCustomer()
        {
            StripeAccounts = new HashSet<StripeAccount>();
        }

        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string ExternalPersonId { get; set; }
        public Boolean Imported { get; set; }

        public virtual ICollection<StripeAccount> StripeAccounts { get; set; } 
    }
}
