namespace CrossroadsStripeOnboarding.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Donor_Accounts")]
    public class DonorAccount
    {
        public DonorAccount()
        {
            RecurringGifts = new HashSet<RecurringGift>();
        }

        [Key]
        public int Donor_Account_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Institution_Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Account_Number { get; set; }

        [StringLength(50)]
        public string Routing_Number { get; set; }

        public int Donor_ID { get; set; }

        [Column("Non-Assignable")]
        public bool Non_Assignable { get; set; }

        public int Domain_ID { get; set; }

        public int Account_Type_ID { get; set; }

        public bool Closed { get; set; }

        public int? Bank_ID { get; set; }

        [StringLength(255)]
        public string Encrypted_Account { get; set; }

        [StringLength(50)]
        public string Processor_Account_ID { get; set; }

        [StringLength(255)]
        public string Processor_ID { get; set; }

        public virtual Donor Donor { get; set; }

        public virtual ICollection<RecurringGift> RecurringGifts { get; set; }
    }
}
