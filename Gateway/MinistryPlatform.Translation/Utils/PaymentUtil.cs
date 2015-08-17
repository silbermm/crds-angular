using System;
using Crossroads.Utilities.Interfaces;

namespace MinistryPlatform.Translation.Utils
{
    class PaymentUtil
    {

        public static String getPaymentType(IConfigurationWrapper configuration, int paymentTypeId)
        {
            var creditCardPaymentType = configuration.GetConfigValue("CreditCard");
            var bankPaymentType = configuration.GetConfigValue("Bank");
            var checkPaymentType = configuration.GetConfigValue("Check");

            if (paymentTypeId.ToString() == creditCardPaymentType.Substring(0, 1))
            {
                return creditCardPaymentType.Substring(2, 11);
            }
            else if (paymentTypeId.ToString() == checkPaymentType.Substring(0, 1))
            {
                return checkPaymentType.Substring(2, 5);
            }
            else
            {
                return bankPaymentType.Substring(2, 4);
            }
        }

        public static int getPaymentTypeId(string paymentType)
        {
            switch (paymentType)
            {
                case "Bank":
                    return 4;
                case "Check":
                    return 1;
                default:
                    return 5;
            }
        }

    }
}
