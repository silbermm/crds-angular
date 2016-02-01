using System;
using NUnit.Framework;
using Crossroads.Utilities.Extensions;

namespace Crossroads.Utilities.Test.Extensions
{
    class DateTimeExtensionTest
    {
        [Test]
        public void ShouldBeWeekend()
        {

            var Weekends = new [] {new DateTime(2016, 01, 02), new DateTime(2016, 01, 03)};
            foreach (var day in Weekends)
            {
                Assert.IsTrue(day.IsWeekend());
            }
            
        }

        [Test]
        public void ShouldNotBeWeekend()
        {
            var Weekdays = new[] { 
                new DateTime(2016, 01, 04), 
                new DateTime(2016, 01, 05), 
                new DateTime(2016, 01, 06), 
                new DateTime(2016, 01, 07),
                new DateTime(2016, 01, 08),
            };
            foreach (var day in Weekdays)
            {
                Assert.IsFalse(day.IsWeekend());
            }
        }

        [Test]
        public void ShouldFormatToMinistryPlatformFormat()
        {
            var someDate = new DateTime(2016, 1, 4);
            Assert.AreEqual("1/4/2016", someDate.ToMinistryPlatformSearchFormat());
        }
    }
}
