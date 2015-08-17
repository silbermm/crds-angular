using System;
using System.Globalization;
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

        public static int getPaymentTypeId(IConfigurationWrapper configuration, string paymentType)
        {
            //Capitalize all the first letters and then remove white space so Credit Card would now be CreditCard
            paymentType = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(paymentType).Replace(" ", "");

            return int.Parse(configuration.GetConfigValue(paymentType).Substring(0, 1));
        }

        public static int getDeclineEmailByPaymentType(IConfigurationWrapper configuration, int paymentTypeId)
        {
            var checkPaymentType = configuration.GetConfigValue("Check");

            if (paymentTypeId.ToString() == checkPaymentType.Substring(0, 1))
            {
                return configuration.GetConfigIntValue("CheckGiveDeclineEmailTemplate");
            }
            else
            {
                return configuration.GetConfigIntValue("DefaultGiveDeclineEmailTemplate");
            }
        }

    }
}
