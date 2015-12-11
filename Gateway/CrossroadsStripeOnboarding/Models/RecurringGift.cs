namespace CrossroadsStripeOnboarding.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Recurring_Gifts")]
    public class RecurringGift
    {
        [Key]
        public int Recurring_Gift_ID { get; set; }

        public int Donor_ID { get; set; }

        public int Donor_Account_ID { get; set; }

        public int Frequency_ID { get; set; }

        public Frequency Frequency { get { return (Frequency) Frequency_ID; } }

        public int? Day_Of_Month { get; set; }

        public int? Day_Of_Week_ID { get; set; }

        public DayOfWeek? DayOfWeek { get { return (Day_Of_Week_ID == null ? (DayOfWeek?) null : (DayOfWeek) Day_Of_Week_ID); } }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        public DateTime Start_Date { get; set; }

        public DateTime? End_Date { get; set; }

        public int Program_ID { get; set; }

        public int Congregation_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Subscription_ID { get; set; }

        public int Domain_ID { get; set; }

        public int Consecutive_Failure_Count { get; set; }

        public virtual DonorAccount DonorAccount { get; set; }

        public virtual Donor Donor { get; set; }
    }
}
