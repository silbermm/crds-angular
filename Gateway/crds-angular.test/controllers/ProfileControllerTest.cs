using System;
using NUnit.Framework;
using crds_angular.Controllers;
using MvcContrib.TestHelper;
using crds_angular.Controllers.API;
using crds_angular.Models;
using System.Web.Http.Results;
using System.Web.Http;
using System.Net.Http;

namespace crds_angular.test.controllers
{
    [TestFixture]
    public class ProfileControllerTest
    {

        private ProfileController profileController;

        [SetUp]
        public void SetUp()
        {
            profileController = new ProfileController();
            profileController.Request = new HttpRequestMessage();
            profileController.Configuration = new HttpConfiguration();
        }

        [Test]
        public void GetWithOneParamShouldBeUnAuthorized()
        {
            
            IHttpActionResult result = profileController.GetProfile();
            Assert.IsInstanceOf(typeof(UnauthorizedResult), result);
        }
    }
}
