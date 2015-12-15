
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrossroadsStripeOnboarding.Models
{
    public class StripeAccount
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public AccountType Type { get; set; }

        public string OldCardId { get; set; }
        public string NewCardId { get; set; }
        public string Fingerprint { get; set; }
        public string Last4 { get; set; }
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
        public string Institution { get; set; }
        public int StripeCustomerId { get; set; }

        [ForeignKey("StripeCustomerId")]   
        public virtual StripeCustomer StripeCustomer { get; set; } 
    }
}
