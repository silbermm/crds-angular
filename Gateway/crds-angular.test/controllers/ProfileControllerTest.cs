using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using crds_angular.App_Start;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads;
using crds_angular.Services;
using MinistryPlatform.Translation.Services;
using NUnit.Framework;

namespace crds_angular.test.controllers
{
    [TestFixture]
    public class ProfileControllerTest
    {
        private ProfileController _profileController;

        private const string USERNAME = "testme";
        private const string PASSWORD = "changeme";

        [SetUp]
        public void SetUp()
        {
            _profileController = new ProfileController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        [Test]
        public void GetWithOneParamShouldBeUnAuthorized()
        {
            IHttpActionResult result = _profileController.GetProfile();
            Assert.IsInstanceOf(typeof (UnauthorizedResult), result);
        }

        [Test]
        [Ignore("fails for test user????")]
        public void GetFamilyForContact()
        {
            //force AutoMapper to register
            AutoMapperConfig.RegisterMappings();

            // First we need to get a session
            var token = TranslationService.Login(USERNAME, PASSWORD);
            Assert.IsNotNull(token, "Token should be valid");

            //Need Contact Id for token's Contact
            var contactId = AuthenticationService.GetContactId(token);

            // Set the token in the header
            var h = new HttpRequestMessage();
            h.Headers.Add("Authorization", token);
            _profileController.Request = h;

            // Make the call...
            var result = _profileController.GetFamily(contactId);
            Assert.IsInstanceOf(typeof (OkNegotiatedContentResult<List<FamilyMember>>), result);
        }
    }
}