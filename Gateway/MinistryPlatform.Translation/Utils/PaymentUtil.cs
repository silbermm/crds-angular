using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Crossroads.Utilities.Interfaces;

namespace MinistryPlatform.Translation.Utils
{
    internal class PaymentUtil
    {
        public static String getPaymentType(IConfigurationWrapper configuration, int paymentTypeId)
        {
            var creditCardPaymentType = configuration.GetConfigValue("CreditCard");
            var bankPaymentType = configuration.GetConfigValue("Bank");
            var checkPaymentType = configuration.GetConfigValue("Check");

            if (paymentTypeId.ToString() == creditCardPaymentType.Substring(0, 1))
                return creditCardPaymentType.Substring(2, 11);
            if (paymentTypeId.ToString() == checkPaymentType.Substring(0, 1))
                return checkPaymentType.Substring(2, 5);
            return bankPaymentType.Substring(2, 4);
        }

        public static String getPaymentTypeId(IConfigurationWrapper configuration, string paymentType)
        {
            paymentType = formatePaymentType(paymentType);

            return configuration.GetConfigValue(paymentType).Substring(0, 1);
        }

        public static int getDeclineEmailByPaymentType(IConfigurationWrapper configuration, int paymentTypeId)
        {
            var checkPaymentType = configuration.GetConfigValue("Check");

            if (paymentTypeId.ToString() == checkPaymentType.Substring(0, 1))
                return configuration.GetConfigIntValue("CheckGiveDeclineEmailTemplate");
            return configuration.GetConfigIntValue("DefaultGiveDeclineEmailTemplate");
        }

        private static String formatePaymentType(string paymentType)
        {
            // if there are any pascal casing go ahead and split
            paymentType = Regex.Replace(paymentType, "([A-Z])", " $1");

            //Capitalize all the first letters and then remove white space so Credit Card would now be CreditCard
            paymentType = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(paymentType).Replace(" ", "");

            return paymentType;
        }
    }
}