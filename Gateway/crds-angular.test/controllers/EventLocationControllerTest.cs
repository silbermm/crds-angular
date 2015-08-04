using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Services;
using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.controllers
{
    [TestFixture]
    internal class EventLocationControllerTest
    {
        private EventLocationController controller;

        private Mock<IMinistryPlatformService> _ministryPlatfromServiceMock;
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private Mock<IAuthenticationService> _authenticationServiceMock;
      
        [SetUp]
        public void SetUp()
        {
            _ministryPlatfromServiceMock = new Mock<IMinistryPlatformService>();

            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _configurationWrapper.Setup(m => m.GetEnvironmentVarAsString("API_USER")).Returns("mockApiUser");
            _configurationWrapper.Setup(m => m.GetEnvironmentVarAsString("API_PASSWORD")).Returns("mockApiPassword");

            _authenticationServiceMock = new Mock<IAuthenticationService>();            

            var authData = new Dictionary<string, object>
                {
                    {"token", "123123"},
                    {"exp", "010125"}
                };

            _authenticationServiceMock.Setup(m => m.authenticate("mockApiUser", "mockApiPassword")).Returns(authData);

            controller = new EventLocationController(_ministryPlatfromServiceMock.Object, _configurationWrapper.Object, _authenticationServiceMock.Object);
        }

        [Test]
        public void ShouldReturnTodaysEvents()
        {
            const string site = "Oakley";

            var dictionaryList = new List<Dictionary<string, object>>
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
                .Returns(dictionaryList);

            // Make the call...
            IHttpActionResult result = controller.Get(site);

            _configurationWrapper.VerifyAll();
            _authenticationServiceMock.VerifyAll();
            _ministryPlatfromServiceMock.VerifyAll();


            Assert.IsInstanceOf(typeof (OkNegotiatedContentResult<List<Event>>), result);
        }
    }
}