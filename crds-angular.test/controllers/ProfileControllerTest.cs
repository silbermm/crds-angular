using System;
using NUnit.Framework;
using crds_angular.Controllers;
using MvcContrib.TestHelper;
using crds_angular.Controllers.API;
using System.Web.Http;

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
            profileController.Request = new System.Net.Http.HttpRequestMessage();
            profileController.Configuration = new HttpConfiguration();
        }

        [Test]
        public void GetWithTwoParamsShouldFail()
        {
            //IHttpActionResult result = profileController.Get(0, 0);
            //var msg = profileController.Content.ReadAsAsync<string>();

            //Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            

        }
    }
}
