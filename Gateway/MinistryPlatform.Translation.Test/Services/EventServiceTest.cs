using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

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
        private readonly int EventsPageId = 308;

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
                {"Participant_ID", 123},
                {"Event_ID", 456},
                {"Participation_Status_ID", EventParticipantStatusDefaultID},
            };

            int eventParticipantId = fixture.registerParticipantForEvent(123, 456);

            ministryPlatformService.Verify(mocked => mocked.CreateSubRecord(
                EventParticipantPageId, 456, expectedValues, It.IsAny<string>(), true));

            Assert.AreEqual(987, eventParticipantId);
        }

        [Test]
        public void GetEventsByType()
        {
            const string eventType = "Oakley: Saturday at 4:30";

            var search = ",," + eventType;
            ministryPlatformService.Setup(mock => mock.GetRecordsDict(EventsPageId, It.IsAny<string>(), search, ""))
                .Returns(MockEventsDictionary());

            var events = fixture.GetEvents(eventType, It.IsAny<string>());
            Assert.IsNotNull(events);
        }

        private List<Dictionary<string, object>> MockEventsDictionary()
        {
            return new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 100},
                    {"Event_Title", "event-title-100"},
                    {"Event_Type", "event-type-100"},
                    {"Event_Start_Date", new DateTime(2015, 3, 28, 8, 30, 0)},
                    {"Event_End_Date", new DateTime(2015, 3, 28, 8, 30, 0)}
                },
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 200},
                    {"Event_Title", "event-title-200"},
                    {"Event_Type", "event-type-200"},
                    {"Event_Start_Date", new DateTime(2015, 3, 28, 8, 30, 0)},
                    {"Event_End_Date", new DateTime(2015, 3, 28, 8, 30, 0)}
                },
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 300},
                    {"Event_Title", "event-title-300"},
                    {"Event_Type", "event-type-300"},
                    {"Event_Start_Date", new DateTime(2015, 3, 28, 8, 30, 0)},
                    {"Event_End_Date", new DateTime(2015, 3, 28, 8, 30, 0)}
                }
            };
        }
    }
}