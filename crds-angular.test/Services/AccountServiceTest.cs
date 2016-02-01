using System.Collections.Generic;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    public class AccountServiceTests
    {
        private Mock<IAuthenticationService> _authenticationService;
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private Mock<ILookupService> _lookupService;
        private Mock<ICommunicationService> _comunicationService;
        private Mock<ISubscriptionsService> _subscriptionService;
        private Mock<IMinistryPlatformService> _ministryPlatformService;

        private AccountService _fixture;

        [SetUp]
        public void SetUp()
        {
            _authenticationService = new Mock<IAuthenticationService>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _lookupService = new Mock<ILookupService>();
            _comunicationService = new Mock<ICommunicationService>();
            _subscriptionService = new Mock<ISubscriptionsService>();
            _ministryPlatformService = new Mock<IMinistryPlatformService>();

            _fixture = new AccountService(_configurationWrapper.Object,
                                          _comunicationService.Object,
                                          _authenticationService.Object,
                                          _subscriptionService.Object,
                                          _ministryPlatformService.Object,
                                          _lookupService.Object);
        }

        [Test]
        [ExpectedException(typeof(DuplicateUserException))]
        public void ShouldNotRegisterDuplicatePerson()
        {
            var newUserData = new User
            {
                firstName = "Automated",
                lastName = "Test",
                email = "auto02@crossroads.net",
                password = "password"
            };

            _configurationWrapper.Setup(mocked => mocked.GetEnvironmentVarAsString("API_USER")).Returns("user");
            _configurationWrapper.Setup(mocked => mocked.GetEnvironmentVarAsString("API_PASSWORD")).Returns("password");

            _authenticationService.Setup(mocked => mocked.Authenticate("user", "password")).Returns(new Dictionary<string, object> {{"token", "auth_token"}});

            _lookupService.Setup(mocked => mocked.EmailSearch(newUserData.email, "auth_token")).Returns(new Dictionary<string, object> { {"dp_RecordID", 123}});

            _fixture.RegisterPerson(newUserData);

            _ministryPlatformService.VerifyAll();
        }

        [Test]
        public void ShouldRegisterPerson()
        {
            var newUserData = new User
            {
                firstName = "Automated",
                lastName = "Test",
                email = "auto02@crossroads.net",
                password = "password"
            };

            _configurationWrapper.Setup(mocked => mocked.GetEnvironmentVarAsString("API_USER")).Returns("user");
            _configurationWrapper.Setup(mocked => mocked.GetEnvironmentVarAsString("API_PASSWORD")).Returns("password");

            _authenticationService.Setup(mocked => mocked.Authenticate("user", "password")).Returns(new Dictionary<string, object> { { "token", "auth_token" } });

            _lookupService.Setup(mocked => mocked.EmailSearch(newUserData.email, "auth_token")).Returns(new Dictionary<string, object>());

            _configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("Households")).Returns(123);
            _ministryPlatformService.Setup(mocked => mocked.CreateRecord(123, It.IsAny<Dictionary<string, object>>(), "auth_token", false)).Returns(321);

            _configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("Contacts")).Returns(456);
            _ministryPlatformService.Setup(mocked => mocked.CreateRecord(456, It.IsAny<Dictionary<string, object>>(), "auth_token", false)).Returns(654);

            _configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("Users")).Returns(789);
            _ministryPlatformService.Setup(mocked => mocked.CreateRecord(789, It.IsAny<Dictionary<string, object>>(), "auth_token", true)).Returns(987);

            _configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("ContactHouseholds")).Returns(234);
            _ministryPlatformService.Setup(mocked => mocked.CreateRecord(234, It.IsAny<Dictionary<string, object>>(), "auth_token", false)).Returns(432);

            _configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("Users_Roles")).Returns(345);
            _ministryPlatformService.Setup(mocked => mocked.CreateSubRecord(345, 987, It.IsAny<Dictionary<string, object>>(), "auth_token", false)).Returns(543);

            _configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("Participants")).Returns(567);
            _ministryPlatformService.Setup(mocked => mocked.CreateRecord(567, It.IsAny<Dictionary<string, object>>(), "auth_token", false)).Returns(765);

            _subscriptionService.Setup(mocked => mocked.SetSubscriptions(It.IsAny<Dictionary<string, object>>(), 654, "auth_token")).Returns(999);

            _fixture.RegisterPerson(newUserData);

            _ministryPlatformService.VerifyAll();
            _subscriptionService.VerifyAll();
        }
    }
}