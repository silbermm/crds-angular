using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Translation.Services;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class ContactServiceTest
    {
        [Test]
        public void GetMyProfile()
        {
            var username = "tmaddox@aol.com";
            var password = "crds1234";
            var token = AuthenticationService.authenticate(username, password); 
            Assert.IsNotNull(token, "Token should be valid");

            var contactSvc = new ContactService();
            var tmp = contactSvc.GetMyProfile(token);
            Assert.IsNotNull(tmp);
        }
    }
}
