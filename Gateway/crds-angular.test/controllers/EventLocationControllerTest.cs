using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.controllers
{
    [TestFixture]
    internal class EventLocationControllerTest
    {
        private EventController controller;

        private Mock<IMinistryPlatformService> _ministryPlatfromServiceMock;
        private Mock<IApiUserService> _apiUserService;


        [SetUp]
        public void SetUp()
        {
            _ministryPlatfromServiceMock = new Mock<IMinistryPlatformService>();

            _apiUserService = new Mock<IApiUserService>();
            _apiUserService.Setup(m => m.GetToken()).Returns("something");

            controller = new EventController(_ministryPlatfromServiceMock.Object, _apiUserService.Object);
        }


        [Test]
        public void ShouldReturnTodaysEvents()
        {
            const string site = "Oakley";

            var todaysEvents = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Event_Start_Date", new DateTime(2015, 3, 30, 9, 30, 0)},
                    {"Event_Title", "Mock Event"},
                    {"Room_Name", "Mock Room"},
                    {"Room_Number", 999}
                }
            };

            _ministryPlatfromServiceMock.Setup(
                m => m.GetRecordsDict("TodaysEventLocationRecords", It.IsAny<string>(), site, "5 asc"))
                .Returns(todaysEvents);

            // Make the call...
            IHttpActionResult result = controller.Get(site);

            _apiUserService.VerifyAll();
            _ministryPlatfromServiceMock.VerifyAll();

            Assert.IsInstanceOf(typeof (OkNegotiatedContentResult<List<Event>>), result);
        }
    }
}