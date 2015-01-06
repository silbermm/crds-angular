using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinistryPlatform.Translation.Test
{
    [TestFixture]
    public class LookupTest
    {

        private const string USERNAME = "testme";
        private const string PASSWORD = "changeme";
        private const string EMAIL = "testme@test.com";


        [Test]
        public void ShouldReturnAValidObjectWithUserIdAndEmailAddress()
        {
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(token);
            var contactId = AuthenticationService.GetContactId(token);
            Assert.IsNotNull(contactId);
            var emails = MinistryPlatform.Translation.Services.LookupService.EmailSearch(EMAIL, token);
            Assert.IsNotEmpty(emails);
        }

        [Test]
        public void ShouldReturnValidObjectForUpperCaseEmailAddress()
        {
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(token);
            var contactId = AuthenticationService.GetContactId(token);
            Assert.IsNotNull(contactId);
            var emails = MinistryPlatform.Translation.Services.LookupService.EmailSearch(EMAIL.ToUpper(), token);
            Assert.IsEmpty(emails);
        }

        public void ShouldBeEmpty()
        {
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(token);
            var contactId = AuthenticationService.GetContactId(token);
            Assert.IsNotNull(contactId);
            var emails = MinistryPlatform.Translation.Services.LookupService.EmailSearch("CRAP@CRAP.com", token);
            Assert.IsEmpty(emails);
        }
    }
}
