using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.controllers
{
    public class UserControllerTest
    {
        private UserController _fixture;

        private Mock<IAccountService> _accountService;

        [SetUp]
        public void SetUp()
        {
            _accountService = new Mock<IAccountService>();

            _fixture = new UserController(_accountService.Object);
        }

        [Test]
        public void ShouldRegisterNewUser()
        {
            var user = new User();
            var newUser = new Dictionary<string, string>();
            _accountService.Setup(mocked => mocked.RegisterPerson(user)).Returns(newUser);

            var response = _fixture.Post(user);
            _accountService.VerifyAll();

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<OkNegotiatedContentResult<Dictionary<string, string>>>(response);
            var responseData = ((OkNegotiatedContentResult<Dictionary<string, string>>) response).Content;
            Assert.IsNotNull(responseData);
            Assert.AreSame(newUser, responseData);
        }

        [Test]
        public void ShouldReturnBadResponseForDuplicateUser()
        {
            var user = new User();
            _accountService.Setup(mocked => mocked.RegisterPerson(user)).Throws(new DuplicateUserException("me@here.com"));

            try
            {
                _fixture.Post(user);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (HttpResponseException e)
            {
                // Expected
            }
            _accountService.VerifyAll();
        }
    }
}
