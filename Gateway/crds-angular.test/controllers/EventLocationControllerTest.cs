using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads;
using NUnit.Framework;

namespace crds_angular.test.controllers
{
    [TestFixture]
    class EventLocationControllerTest
    {
        private EventLocationController controller;

        [SetUp]
        public void SetUp()
        {
            controller = new EventLocationController();
            
        }

        [Test]
        public void ShouldReturnTodaysEvents()
        {

            // Make the call...
            IHttpActionResult result = controller.Get("Oakley");
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<List<Event>>), result);
        }

    }
}
