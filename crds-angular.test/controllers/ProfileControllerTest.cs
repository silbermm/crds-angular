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
        public void GetWithTwoParamsShouldFail()
        {
            var result = profileController.Get(0, 0);
            System.Console.WriteLine(result); 
            Assert.IsInstanceOf(typeof(BadRequestResult), result);

        }

        [Test]
        public void GetWithOneParamShouldBeUnAuthorized()
        {
            
            IHttpActionResult result = profileController.Get(455);
            Assert.IsInstanceOf(typeof(UnauthorizedResult), result);
            //Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<Person>), result);
            //OkNegotiatedContentResult<Person> o = (OkNegotiatedContentResult<Person>) result;
            //Assert.IsInstanceOf(typeof(Person), o.Content);
        }
    }
}
