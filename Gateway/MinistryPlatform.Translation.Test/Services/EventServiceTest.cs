using System;
using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class EventServiceTest
    {
        [SetUp]
        public void SetUp()
        {
            ministryPlatformService = new Mock<IMinistryPlatformService>();
            _authService = new Mock<IAuthenticationService>();
            _configWrapper = new Mock<IConfigurationWrapper>();
            _groupService = new Mock<IGroupService>();

            _configWrapper.Setup(m => m.GetEnvironmentVarAsString("API_USER")).Returns("uid");
            _configWrapper.Setup(m => m.GetEnvironmentVarAsString("API_PASSWORD")).Returns("pwd");
            _authService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new Dictionary<string, object> {{"token", "ABC"}, {"exp", "123"}});

            fixture = new EventService(ministryPlatformService.Object, _authService.Object, _configWrapper.Object, _groupService.Object);
        }

        private EventService fixture;
        private Mock<IMinistryPlatformService> ministryPlatformService;
        private Mock<IAuthenticationService> _authService;
        private Mock<IConfigurationWrapper> _configWrapper;
        private Mock<IGroupService> _groupService;
        private const int EventParticipantId = 12345;
        private readonly int EventParticipantPageId = 281;
        private readonly int EventParticipantStatusDefaultID = 2;
        private readonly int EventsPageId = 308;
        private readonly string EventsWithEventTypeId = "EventsWithEventTypeId";

        private List<Dictionary<string, object>> MockEventsDictionaryByEventTypeId()
        {
            return new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 100},
                    {"Event Title", "event-title-100"},
                    {"Event Type", "event-type-100"},
                    {"Event Start Date", new DateTime(2015, 3, 28, 8, 30, 0)},
                    {"Event End Date", new DateTime(2015, 3, 28, 8, 30, 0)}
                },
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 200},
                    {"Event Title", "event-title-200"},
                    {"Event Type", "event-type-200"},
                    {"Event Start Date", new DateTime(2015, 4, 1, 8, 30, 0)},
                    {"Event End Date", new DateTime(2015, 4, 1, 8, 30, 0)}
                },
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 300},
                    {"Event Title", "event-title-300"},
                    {"Event Type", "event-type-300"},
                    {"Event Start Date", new DateTime(2015, 4, 2, 8, 30, 0)},
                    {"Event End Date", new DateTime(2015, 4, 2, 8, 30, 0)}
                }
                ,
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 400},
                    {"Event Title", "event-title-400"},
                    {"Event Type", "event-type-400"},
                    {"Event Start Date", new DateTime(2015, 4, 30, 8, 30, 0)},
                    {"Event End Date", new DateTime(2015, 4, 30, 8, 30, 0)}
                }
                ,
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 500},
                    {"Event Title", "event-title-500"},
                    {"Event Type", "event-type-500"},
                    {"Event Start Date", new DateTime(2015, 5, 1, 8, 30, 0)},
                    {"Event End Date", new DateTime(2015, 5, 1, 8, 30, 0)}
                }
            };
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

        private List<Dictionary<string, object>> MockEventParticipantsByEventIdAndParticipantId()
        {
            return new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Event_Participant_ID", 8634},
                    {"Event_ID", 93},
                    {"Participant_ID", 134}
                }
            };
        }

        [Test]
        public void GetEvent()
        {
            //Arrange
            const int eventId = 999;
            const string pageKey = "EventsWithDetail";
            var mockEventDictionary = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Event_ID", 999},
                    {"Event_Title", "event-title-100"},
                    {"Event_Type", "event-type-100"},
                    {"Event_Start_Date", new DateTime(2015, 3, 28, 8, 30, 0)},
                    {"Event_End_Date", new DateTime(2015, 3, 28, 8, 30, 0)},
                    {"Contact_ID", 12345},
                    {"Email_Address", "thecinnamonbagel@react.js"},
                    {"Parent_Event_ID", 6543219},
                    {"Congregation_ID", It.IsAny<int>()}
                }
            };
            var searchString = eventId + ",";
            ministryPlatformService.Setup(m => m.GetPageViewRecords(pageKey, It.IsAny<string>(), searchString, string.Empty, 0)).Returns(mockEventDictionary);

            //Act
            var theEvent = fixture.GetEvent(eventId);

            //Assert
            ministryPlatformService.VerifyAll();

            Assert.AreEqual(eventId, theEvent.EventId);
            Assert.AreEqual("event-title-100", theEvent.EventTitle);
            Assert.AreEqual(12345, theEvent.PrimaryContact.ContactId);
            Assert.AreEqual("thecinnamonbagel@react.js", theEvent.PrimaryContact.EmailAddress);
        }

        [Test]
        public void GetEventByParentId()
        {
            const int expectedEventId = 999;
            const int parentEventId = 888;
            var searchString = string.Format(",,,{0}", parentEventId);
            const string pageKey = "EventsByParentEventID";

            var mockEventDictionary = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Event_ID", 999},
                    {"Event_Title", "event-title-100"},
                    {"Event_Type", "event-type-100"},
                    {"Event_Start_Date", new DateTime(2015, 3, 28, 8, 30, 0)},
                    {"Event_End_Date", new DateTime(2015, 3, 28, 8, 30, 0)},
                    {"Contact_ID", 12345},
                    {"Email_Address", "thecinnamonbagel@react.js"}
                }
            };

            ministryPlatformService.Setup(m => m.GetPageViewRecords(pageKey, It.IsAny<string>(), searchString, string.Empty, 0)).Returns(mockEventDictionary);

            var events = fixture.GetEventsByParentEventId(parentEventId);

            ministryPlatformService.VerifyAll();

            Assert.AreEqual(1, events.Count);
            var theEvent = events[0];
            Assert.AreEqual(expectedEventId, theEvent.EventId);
            Assert.AreEqual("event-title-100", theEvent.EventTitle);
            Assert.AreEqual(12345, theEvent.PrimaryContact.ContactId);
            Assert.AreEqual("thecinnamonbagel@react.js", theEvent.PrimaryContact.EmailAddress);
        }

        [Test]
        public void GetEventParticipant()
        {
            const int eventId = 1234;
            const int participantId = 5678;
            const string pageKey = "EventParticipantByEventIdAndParticipantId";
            var mockEventParticipants = MockEventParticipantsByEventIdAndParticipantId();

            ministryPlatformService.Setup(m => m.GetPageViewRecords(pageKey, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(mockEventParticipants);

            var participant = fixture.GetEventParticipantRecordId(eventId, participantId);

            ministryPlatformService.VerifyAll();
            Assert.IsNotNull(participant);
            Assert.AreEqual(8634, participant);
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

        [Test]
        public void GetEventsByTypeAndRange()
        {
            var eventTypeId = 1;
            var search = ",," + eventTypeId;
            ministryPlatformService.Setup(mock => mock.GetPageViewRecords(EventsWithEventTypeId, It.IsAny<string>(), search, "", 0))
                .Returns(MockEventsDictionaryByEventTypeId());

            var startDate = new DateTime(2015, 4, 1);
            var endDate = new DateTime(2015, 4, 30);
            var events = fixture.GetEventsByTypeForRange(eventTypeId, startDate, endDate, It.IsAny<string>());
            Assert.IsNotNull(events);
            Assert.AreEqual(3, events.Count);
            Assert.AreEqual("event-title-200", events[0].EventTitle);
        }

        [Test]
        public void TestRegisterParticipantForEvent()
        {
            ministryPlatformService.Setup(mocked => mocked.CreateSubRecord(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Dictionary<string, object>>(),
                It.IsAny<string>(),
                It.IsAny<bool>())).Returns(987);

            var expectedValues = new Dictionary<string, object>
            {
                {"Participant_ID", 123},
                {"Event_ID", 456},
                {"Participation_Status_ID", EventParticipantStatusDefaultID}
            };

            var eventParticipantId = fixture.RegisterParticipantForEvent(123, 456);

            ministryPlatformService.Verify(mocked => mocked.CreateSubRecord(
                EventParticipantPageId,
                456,
                expectedValues,
                It.IsAny<string>(),
                true));

            Assert.AreEqual(987, eventParticipantId);
        }
    }
}