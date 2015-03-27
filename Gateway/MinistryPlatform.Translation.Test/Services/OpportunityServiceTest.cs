using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class OpportunityServiceTest
    {
        private readonly int _signedupToServeSubPageViewId = 79;
        private readonly int _groupOpportunitiesEventsPageViewId = 77;
        private DateTime _today;

        //private Mock<IAuthenticationService> _authenticationServiceMock;
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IOpportunityService> _opportuntityService;
        private Mock<IEventService> _eventService;

        private OpportunityServiceImpl _fixture;

        [SetUp]
        public void SetUp()
        {
            var now = DateTime.Now;
            _today = new DateTime(now.Year, now.Month, now.Day);
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _opportuntityService = new Mock<IOpportunityService>();
            _eventService = new Mock<IEventService>();

            //_authenticationServiceMock = new Mock<IAuthenticationService>();
            _fixture = new OpportunityServiceImpl(_ministryPlatformService.Object, _eventService.Object);
        }

        [Test]
        public void GetOpportunitiesForGroupTest()
        {
            const int groupId = 1;

            _ministryPlatformService.Setup(
                m =>
                    m.GetSubpageViewRecords(_groupOpportunitiesEventsPageViewId, groupId, It.IsAny<string>(), "", "", 0))
                .Returns(OpportunityResponse());

            //const string eventType = "Hello";
            _eventService.Setup(m => m.GetEvents("Event Type 100", It.IsAny<string>()))
                .Returns(MockEvents("Event Type 100"));
            _eventService.Setup(m => m.GetEvents("Event Type 200", It.IsAny<string>()))
                .Returns(MockEvents("Event Type 200"));
            _eventService.Setup(m => m.GetEvents("Event Type 300", It.IsAny<string>()))
                .Returns(MockEvents("Event Type 300"));

            var opportunities = _fixture.GetOpportunitiesForGroup(groupId, It.IsAny<string>());

            _ministryPlatformService.VerifyAll();
            _eventService.VerifyAll();

            Assert.IsNotNull(opportunities);
            Assert.AreEqual(3, opportunities.Count);

            var opportunity = opportunities[0];
            Assert.AreEqual(100, opportunity.Capacity);
            Assert.AreEqual("Event Type 100", opportunity.EventType);
            Assert.AreEqual(2, opportunity.Events.Count);
            Assert.AreEqual(100, opportunity.OpportunityId);
            Assert.AreEqual("Opportunity 100", opportunity.OpportunityName);
            Assert.AreEqual("Role Title 100", opportunity.RoleTitle);

            opportunity = opportunities[1];
            Assert.AreEqual(200, opportunity.Capacity);
            Assert.AreEqual("Event Type 200", opportunity.EventType);
            Assert.AreEqual(2, opportunity.Events.Count);
            Assert.AreEqual(200, opportunity.OpportunityId);
            Assert.AreEqual("Opportunity 200", opportunity.OpportunityName);
            Assert.AreEqual("Role Title 200", opportunity.RoleTitle);

            opportunity = opportunities[2];
            Assert.AreEqual(0, opportunity.Capacity);
            Assert.AreEqual("Event Type 300", opportunity.EventType);
            Assert.AreEqual(2, opportunity.Events.Count);
            Assert.AreEqual(300, opportunity.OpportunityId);
            Assert.AreEqual("Opportunity 300", opportunity.OpportunityName);
            Assert.AreEqual("Role Title 300", opportunity.RoleTitle);
            var events = opportunity.Events;
            Assert.AreEqual(2, events[1].EventId);
            Assert.AreEqual("event-title-2", events[1].EventTitle);
            Assert.AreEqual(_today, events[1].EventStartDate);
            Assert.AreEqual(_today, events[1].EventEndDate);
            Assert.AreEqual("Event Type 300", events[1].EventType);
        }

        private new List<Models.Event> MockEvents(string eventType)
        {
            return new List<Models.Event>
            {
                new Event
                {
                    EventTitle = "event-title-1",
                    EventType = eventType,
                    EventStartDate = _today,
                    EventEndDate = _today,
                    EventId = 1
                },
                new Event
                {
                    EventTitle = "event-title-2",
                    EventType = eventType,
                    EventStartDate = _today,
                    EventEndDate = _today,
                    EventId = 2
                }
            };
        }

        private List<Dictionary<string, object>> OpportunityResponse()
        {
            var results = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 100},
                    {"Opportunity Title", "Opportunity 100"},
                    {"Event Type", "Event Type 100"},
                    {"Role_Title", "Role Title 100"},
                    {"Maximum_Needed", 100}
                },
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 200},
                    {"Opportunity Title", "Opportunity 200"},
                    {"Event Type", "Event Type 200"},
                    {"Role_Title", "Role Title 200"},
                    {"Maximum_Needed", 200}
                },
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 300},
                    {"Opportunity Title", "Opportunity 300"},
                    {"Event Type", "Event Type 300"},
                    {"Role_Title", "Role Title 300"},
                    {"Maximum_Needed", null}
                }
            };
            return results;
        }

        [Test]
        public void ShouldGetPeopleSignedUpTest()
        {
            const int opportunityId = 139;
            const int eventId = 950107;

            var signedupToServeResults = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Response Date", 01/01/2001},
                    {"Display Name", "Test Group"},
                    {"Contact ID", 1},
                    {"Event Id", 99},
                    {"Event Title", "event-title"}
                },
                new Dictionary<string, object>
                {
                    {"Response Date", 01/01/2002},
                    {"Display Name", "Test Group2"},
                    {"Contact ID", 2},
                    {"Event Id", 102},
                    {"Event Title", "event-title2"}
                },
                new Dictionary<string, object>
                {
                    {"Response Date", 01/01/2003},
                    {"Display Name", "Test Group3"},
                    {"Contact ID", 3},
                    {"Event Id", 103},
                    {"Event Title", "event-title3"}
                }
            };

            var search = ",,," + eventId;
            _ministryPlatformService.Setup(mock =>
                mock.GetSubpageViewRecords(_signedupToServeSubPageViewId, opportunityId, It.IsAny<string>(), search, "",
                    0))
                .Returns(signedupToServeResults);

            var response = _fixture.GetOpportunitySignupCount(opportunityId, eventId, It.IsAny<string>());

            Assert.IsNotNull(response);
            Assert.AreEqual(3, response);
        }
    }
}