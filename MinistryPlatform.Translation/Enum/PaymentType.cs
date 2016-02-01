using System;

namespace MinistryPlatform.Translation.Enum
{
    public sealed class PaymentType
    {
        public static readonly PaymentType Check = new PaymentType(1, "check", "Check", 12200, 13002, 13010);
        public static readonly PaymentType CreditCard = new PaymentType(4, "cc", "Credit Card", 11940, 13002, 13010);
        public static readonly PaymentType Bank = new PaymentType(5, "bank", "Bank", 11940, 13002, 13010);
        private static readonly PaymentType[] paymentTypes = {Check, CreditCard, Bank};

        public readonly String abbrv;
        public readonly int id;
        public readonly String name;
        public readonly int declineEmailTemplateId;
        public readonly int recurringGiftDeclineEmailTemplateId;
        public readonly int recurringGiftCancelEmailTemplateId;

        private PaymentType(int id, String abbrv, String name, int declineEmailTemplateId, int recurringGiftDeclineEmailTemplateId, int recurringGiftCancelEmailTemplateId)
        {
            this.name = name;
            this.abbrv = abbrv;
            this.id = id;
            this.declineEmailTemplateId = declineEmailTemplateId;
            this.recurringGiftDeclineEmailTemplateId = recurringGiftDeclineEmailTemplateId;
            this.recurringGiftCancelEmailTemplateId = recurringGiftCancelEmailTemplateId;
        }
        
        public static PaymentType GetPaymentType(int paymentTypeId)
        {
            foreach (var paymentType in paymentTypes)
            {
                if (paymentType.id == paymentTypeId)
                    return paymentType;
            }

            return CreditCard;
        }

        public static PaymentType GetPaymentType(string paymentTypeName)
        {
            foreach (var paymentType in paymentTypes)
            {
                if (paymentType.name.Equals(paymentTypeName) || paymentType.abbrv.Equals(paymentTypeName))
                    return paymentType;
            }

            return CreditCard;
        }
    }
}