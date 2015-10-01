using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    public class UserServiceTest
    {
        private UserService _fixture;

        private Mock<IAuthenticationService> _authenticationService;
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private Mock<IMinistryPlatformService> _ministryPlatformService;

        [SetUp]
        public void SetUp()
        {
            _authenticationService = new Mock<IAuthenticationService>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _ministryPlatformService = new Mock<IMinistryPlatformService>(MockBehavior.Strict);

            _configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("UsersApiLookupPageView")).Returns(102030);
            _authenticationService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new Dictionary<string, object> { { "token", "ABC" }, { "exp", "123" } });

            _fixture = new UserService(_authenticationService.Object, _configurationWrapper.Object, _ministryPlatformService.Object);
        }

        [Test]
        public void TestGetUserById()
        {
            var mpResult = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Can_Impersonate", true},
                    {"User_GUID", Guid.NewGuid()},
                    {"User_Name", "me@here.com"}
                        
                }
            };
            _ministryPlatformService.Setup(mocked => mocked.GetPageViewRecords(102030, "ABC", "\"me@here.com\",", string.Empty, 0)).Returns(mpResult);

            var user = _fixture.GetByUserId("me@here.com");
            _authenticationService.VerifyAll();
            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(user);
            Assert.AreEqual("me@here.com", user.UserId);
            Assert.AreEqual(mpResult[0]["User_GUID"].ToString(), user.Guid);
            Assert.IsTrue(user.CanImpersonate);
        }

        [Test]
        public void TestGetUserByAuthenticationToken()
        {
            var mpResult = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Can_Impersonate", true},
                    {"User_GUID", Guid.NewGuid()},
                    {"User_Name", "me@here.com"}
                        
                }
            };

            _authenticationService.Setup(mocked => mocked.GetContactId("logged in")).Returns(123);
            _ministryPlatformService.Setup(mocked => mocked.GetPageViewRecords(102030, "ABC", ",\"123\"", string.Empty, 0)).Returns(mpResult);

            var user = _fixture.GetByAuthenticationToken("logged in");
            _authenticationService.VerifyAll();
            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(user);
            Assert.AreEqual("me@here.com", user.UserId);
            Assert.AreEqual(mpResult[0]["User_GUID"].ToString(), user.Guid);
            Assert.IsTrue(user.CanImpersonate);
        }

    }
}
