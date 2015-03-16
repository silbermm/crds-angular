using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Translation.Services;
using Moq;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class EventServiceTest
    {
        private EventService fixture;
        private Mock<IMinistryPlatformService> ministryPlatformService;
        private const int EventParticipantId = 12345;
        private readonly int EventParticipantPageId = 281;
        private readonly int EventParticipantStatusDefaultID = 2;

        [SetUp]
        public void SetUp()
        {
            ministryPlatformService = new Mock<IMinistryPlatformService>();
            fixture = new EventService(ministryPlatformService.Object);
        }

        [Test]
        public void testRegisterParticipantForEvent()
        {
            ministryPlatformService.Setup(mocked => mocked.CreateSubRecord(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Dictionary<string, object>>(), 
                It.IsAny<string>(), It.IsAny<Boolean>())).Returns(987);

            var expectedValues = new Dictionary<string, object>
            {
                { "Participant_ID", 123 },
                { "Event_ID", 456 },
                { "Participation_Status_ID", EventParticipantStatusDefaultID },
            };

            int eventParticipantId = fixture.registerParticipantForEvent(123, 456);

            ministryPlatformService.Verify(mocked => mocked.CreateSubRecord(
                EventParticipantPageId, 456, expectedValues, It.IsAny<string>(), false));

            Assert.AreEqual(987, eventParticipantId);
        }
    }
}
