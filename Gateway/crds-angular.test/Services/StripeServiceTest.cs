using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using StringAssert = NUnit.Framework.StringAssert;

namespace crds_angular.test.Services
{
    class StripeServiceTest
    {
        StripeService stripeService; 

        [SetUp]
        public void Setup()
        {
            stripeService = new StripeService();
        }

        [Test]
        [NUnit.Framework.Ignore("Need to mock out stripe api")]
        public void shouldCallCreateCustomer()
        {
            string customerId = stripeService.createCustomer("tok_15xfdqEldv5NE53suo6XXxZY");
            Assert.NotNull(customerId); 
            StringAssert.StartsWith("cus_", customerId);
        }

        [Test]
        public void shouldThrowExceptionWhenTokenIsInvalid()
        {
            Assert.Throws<StripeException>(() => stripeService.createCustomer("tok_is_bad"));
        }

    }
}
