using System;
using NUnit.Framework;
using crds_angular.Controllers;
using MvcContrib.TestHelper;
using crds_angular.Controllers.API;
using System.Web.Http.Results;


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
        }

        [Test]
        public void GetWithTwoParamsShouldFail()
        {
            var result = profileController.Get(0, 0);
            System.Console.WriteLine(result); 
            Assert.IsInstanceOf(typeof(BadRequestResult), result);

        }

        [Test]
        public void GetWithOneParamShouldPass()
        {
            var result = profileController.Get(455);
            System.Console.WriteLine(result.ToString());
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<Person>), result);
            OkNegotiatedContentResult<Person> o = (OkNegotiatedContentResult<Person>) result;
            Assert.IsInstanceOf(typeof(Person), o.Content);
            

        }
    }
}
