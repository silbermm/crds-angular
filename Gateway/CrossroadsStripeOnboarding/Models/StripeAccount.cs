
using System.ComponentModel.DataAnnotations.Schema;

namespace CrossroadsStripeOnboarding.Models
{
    public class StripeAccount
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Institution { get; set; }
        public string Last4 { get; set; }
        public int StripeCustomerId { get; set; }

        [ForeignKey("StripeCustomerId")]   
        public virtual StripeCustomer StripeCustomer { get; set; } 
    }
}
