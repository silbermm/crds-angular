
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using log4net;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;


namespace crds_angular.test.Services
{
    internal class LoginServiceTest
    {
        private ILoginService _loginService;

        private Mock<ILog> _logger;

        private Mock<MPInterfaces.IAuthenticationService> _authenticationService;
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private Mock<MPInterfaces.IContactService> _contactService;
        private Mock<IEmailCommunication> _emailCommunication;
        private Mock<MPInterfaces.IUserService> _userService;
        

        [SetUp]
        public void SetUp()
        {
            _logger = new Mock<ILog>();
            _authenticationService = new Mock<MPInterfaces.IAuthenticationService>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _contactService = new Mock<MPInterfaces.IContactService>();
            _emailCommunication = new Mock<IEmailCommunication>();
            _userService = new Mock<MPInterfaces.IUserService>();
            
            _loginService = new LoginService(_authenticationService.Object, _configurationWrapper.Object, _contactService.Object, _emailCommunication.Object, _userService.Object);

            _contactService.Setup(m => m.GetContactIdByEmail(It.IsAny<string>())).Returns(123456);
            _userService.Setup(m => m.GetUserIdByUsername(It.IsAny<string>())).Returns(123456);
        }

        [Test]
        public void ShouldHandleResetRequest()
        {
            string email = "someone@someone.com";

            var result = _loginService.PasswordResetRequest(email);
            Assert.AreEqual(true, result);
        }
    }
}
