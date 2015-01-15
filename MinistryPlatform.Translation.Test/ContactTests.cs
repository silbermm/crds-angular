using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Configuration;
using MinistryPlatform.Translation.Services;

namespace MinistryPlatform.Translation.Test
{
    [TestFixture]
    public class ContactTests : BaseTests
    {
        private string _token;

        [TestFixtureSetUp]
        public void Init()
        {
            _token = AuthenticationService.authenticate(USERNAME, PASSWORD);
        }

        [Test]
        public void GetMyContact()
        {
            var contact = ContactService.GetMyContact(_token);
            Assert.IsNotNull(contact);
            Assert.AreEqual(FIRSTNAME, contact.First_Name);
        }

        [Test]
        public void UpdateMyContact()
        {
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);

            var contact = new MinistryPlatform.Models.Contact();

        }
    }
}
