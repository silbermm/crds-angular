using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    public class EventParticipantServiceTest
    {
        private EventParticipantService _fixture;
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IAuthenticationService> _authService;
        private Mock<IConfigurationWrapper> _configWrapper;

        [SetUp]
        public void Setup()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _authService = new Mock<IAuthenticationService>();
            _configWrapper = new Mock<IConfigurationWrapper>();

            _authService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new Dictionary<string, object> {{"token", "ABC"}, {"exp", "123"}});

            _configWrapper.Setup(mocked => mocked.GetEnvironmentVarAsString("API_USER")).Returns("api-user");
            _configWrapper.Setup(mocked => mocked.GetEnvironmentVarAsString("API_PASSWORD")).Returns("api-password");
            _configWrapper.Setup(mocked => mocked.GetConfigIntValue("TripDestinationDocuments")).Returns(1234);

            _fixture = new EventParticipantService(_ministryPlatformService.Object, _authService.Object, _configWrapper.Object);
        }

        [Test]
        public void AddDocumentsToTripParticipantTest()
        {
            const int eventParticipantId = 9;
            const int eventId = 7;
            var docs = new List<TripDocuments>
            {
                new TripDocuments
                {
                    DocumentId = 1,
                    Description = "doc 1 desc",
                    Document = "doc 1"
                },
                new TripDocuments
                {
                    DocumentId = 2,
                    Description = "doc 2 desc",
                    Document = "doc 2"
                }
            };

            _ministryPlatformService.Setup(m => m.CreateSubRecord("EventParticipantDocuments", eventParticipantId, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>(), true))
                .Returns(34567);
            var returnVal = _fixture.AddDocumentsToTripParticipant(docs, eventParticipantId);

            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(returnVal);
            Assert.AreEqual(true, returnVal);
        }
    }
}