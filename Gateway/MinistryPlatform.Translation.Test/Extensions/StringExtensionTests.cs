using System;
using MinistryPlatform.Translation.Extensions;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Extensions
{
    [TestFixture]
    public class StringExtensionTests
    {
        [Test]
        public void StringToInt_Success()
        {
            Assert.AreEqual(10, "10".ToInt());
        }

        [Test]
        public void StringToInt_NotANumber()
        {
            Assert.AreEqual(0, "abc".ToInt());
        }

        [Test]
        [ExpectedException(typeof(System.FormatException))]
        public void StringToInt_NotANumberError()
        {
                var result = "abc".ToInt(true);
                Assert.Fail();
        }
    }
}
