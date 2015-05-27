using System;
using System.Collections.Generic;
using System.ServiceModel;
using Crossroads.Utilities.Services;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.PlatformService;
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
        private readonly int _opportunityPageId = 348;
        private readonly int _eventPageId = 308;
        private readonly int _groupsParticipants = 298;
        private readonly int _groupsParticipantsSubPageId = 88;
        private DateTime _today;

        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IEventService> _eventService;
        private Mock<IAuthenticationService> _authenticationService;

        private OpportunityServiceImpl _fixture;

        [SetUp]
        public void SetUp()
        {
            var now = DateTime.Now;
            _today = new DateTime(now.Year, now.Month, now.Day);
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _eventService = new Mock<IEventService>();
            _authenticationService = new Mock<IAuthenticationService>();

            _fixture = new OpportunityServiceImpl(_ministryPlatformService.Object, _eventService.Object,
                _authenticationService.Object);
        }

        [Test]
        public void CreateOpportunityResponse()
        {
            const int opportunityId = 113;
            const string comment = "Test Comment";

            const int mockParticipantId = 7777;
            _authenticationService.Setup(m => m.GetParticipantRecord(It.IsAny<string>()))
                .Returns(new Participant { ParticipantId = mockParticipantId });

            const string opportunityResponsePageKey = "OpportunityResponses";
            _ministryPlatformService.Setup(
                m => m.CreateRecord(opportunityResponsePageKey, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>(), true))
                .Returns(3333);
            
            Assert.DoesNotThrow(() => _fixture.RespondToOpportunity(It.IsAny<string>(), opportunityId, comment));

            _authenticationService.VerifyAll();
            _ministryPlatformService.VerifyAll();
        }

        [Test]
        public void ShouldNotCreateOpportunityResponse()
        {
            const int opportunityId = 10000000;
            const string comment = "Fail Test Comment";
            const int mockParticipantId = 7777;
            _authenticationService.Setup(m => m.GetParticipantRecord(It.IsAny<string>()))
                .Returns(new Participant { ParticipantId = mockParticipantId });

            const string opportunityResponsePageKey = "OpportunityResponses";
            var exceptionDetail = new System.ServiceModel.ExceptionDetail(new Exception("The creator of this fault did not specify a Reason."));
            var faultException = new FaultException<ExceptionDetail>(exceptionDetail);
            
            _ministryPlatformService.Setup(
                m =>
                    m.CreateRecord(opportunityResponsePageKey, It.IsAny<Dictionary<string, object>>(),
                        It.IsAny<string>(), true))
                .Throws(faultException);

            Assert.Throws<FaultException<ExceptionDetail>>(
                () => _fixture.RespondToOpportunity(It.IsAny<string>(), opportunityId, comment));

            _authenticationService.VerifyAll();
            _ministryPlatformService.VerifyAll();
        }

        [Test]
        public void GetOpportunitiesForGroupTest()
        {
            const int groupId = 1;

            _ministryPlatformService.Setup(
                m =>
                    m.GetSubpageViewRecords(_groupOpportunitiesEventsPageViewId, groupId, It.IsAny<string>(), "", "", 0))
                .Returns(OpportunityResponse());

            _eventService.Setup(m => m.GetEvents("Event Type 100", It.IsAny<string>()))
                .Returns(MockEvents("Event Type 100"));
            _eventService.Setup(m => m.GetEvents("Event Type 200", It.IsAny<string>()))
                .Returns(MockEvents("Event Type 200"));
            _eventService.Setup(m => m.GetEvents("Event Type 300", It.IsAny<string>()))
                .Returns(MockEvents("Event Type 300"));

            _ministryPlatformService.Setup(
                m =>
                    m.GetSubpageViewRecords(_signedupToServeSubPageViewId, It.IsAny<int>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(new List<Dictionary<string, object>>());
            

            var opportunities = _fixture.GetOpportunitiesForGroup(groupId, It.IsAny<string>());

            _ministryPlatformService.VerifyAll();
            _eventService.VerifyAll();

            Assert.IsNotNull(opportunities);
            Assert.AreEqual(3, opportunities.Count);

            var opportunity = opportunities[0];
            Assert.AreEqual(100,opportunity.MaximumNeeded);
            Assert.AreEqual(50,opportunity.MinimumNeeded);
            Assert.AreEqual("Event Type 100", opportunity.EventType);
            Assert.AreEqual(2, opportunity.Events.Count);
            Assert.AreEqual(100, opportunity.OpportunityId);
            Assert.AreEqual("Opportunity 100", opportunity.OpportunityName);
            Assert.AreEqual("Role Title 100", opportunity.RoleTitle);

            opportunity = opportunities[1];
            Assert.AreEqual(200, opportunity.MaximumNeeded);
            Assert.AreEqual(100, opportunity.MinimumNeeded);
            Assert.AreEqual("Event Type 200", opportunity.EventType);
            Assert.AreEqual(2, opportunity.Events.Count);
            Assert.AreEqual(200, opportunity.OpportunityId);
            Assert.AreEqual("Opportunity 200", opportunity.OpportunityName);
            Assert.AreEqual("Role Title 200", opportunity.RoleTitle);

            opportunity = opportunities[2];
            Assert.AreEqual(null, opportunity.MaximumNeeded);
            Assert.AreEqual(null, opportunity.MinimumNeeded);
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

        private new List<Event> MockEvents(string eventType)
        {
            return new List<Event>
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
                    {"Event Type ID", 100},
                    {"Role_Title", "Role Title 100"},
                    {"Maximum_Needed", 100}, {"Minimum_Needed", 50}
                },
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 200},
                    {"Opportunity Title", "Opportunity 200"},
                    {"Event Type", "Event Type 200"},
                    {"Event Type ID", 200},
                    {"Role_Title", "Role Title 200"},
                    {"Maximum_Needed", 200}, {"Minimum_Needed", 100}
                },
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 300},
                    {"Opportunity Title", "Opportunity 300"},
                    {"Event Type", "Event Type 300"},
                    {"Event Type ID", 300},
                    {"Role_Title", "Role Title 300"},
                    {"Maximum_Needed", null}, {"Minimum_Needed", null}
                }
            };
            return results;
        }

        [Test]
        public void GetOpportunityResponseSignUpYesTest()
        {
            //ARRANGE
            const int opportunityId = 2;
            const int eventId = 3;
            var participant = new Participant {ParticipantId = 5};

            // mock _ministryPlatformService.GetPageViewRecords
            const string viewKey = "ResponseByOpportunityAndEvent";
            var searchString = string.Format(",{0},{1},{2}", opportunityId, eventId, participant.ParticipantId);
            const string sortString = "";
            var mockResult = MockDictionaryGetOpportunityResponseSignUpYesTest();
            _ministryPlatformService.Setup(
                m => m.GetPageViewRecords(viewKey, It.IsAny<string>(), searchString, sortString, 0)).Returns(mockResult);

            //ACT
            var response = _fixture.GetOpportunityResponse(opportunityId, eventId, participant);

            //ASSERT
            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Opportunity_ID);
            Assert.AreEqual(5, response.Participant_ID);
            Assert.AreEqual(1, response.Response_Result_ID);

        }

        private List<Dictionary<string, object>> MockDictionaryGetOpportunityResponseSignUpYesTest()
        {
            var results = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 7},
                    {"Opportunity_ID", 2},
                    {"Participant_ID", 5},
                    {"Response_Result_ID", 1}
                }
            };
            return results;
        }

        [Test]
        public void GetOpportunityResponseSignUpNoTest()
        {
            //ARRANGE
            const int opportunityId = 77;
            const int eventId = 3;
            var participant = new Participant { ParticipantId = 777 };

            // mock _ministryPlatformService.GetPageViewRecords
            const string viewKey = "ResponseByOpportunityAndEvent";
            var searchString = string.Format(",{0},{1},{2}", opportunityId, eventId, participant.ParticipantId);
            const string sortString = "";
            var mockResult = MockDictionaryGetOpportunityResponseSignUpNoTest();
            _ministryPlatformService.Setup(
                m => m.GetPageViewRecords(viewKey, It.IsAny<string>(), searchString, sortString, 0)).Returns(mockResult);

            //ACT
            var response = _fixture.GetOpportunityResponse(opportunityId, eventId, participant);

            //ASSERT
            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(response);
            Assert.AreEqual(77, response.Opportunity_ID);
            Assert.AreEqual(777, response.Participant_ID);
            Assert.AreEqual(2, response.Response_Result_ID);

        }

        private List<Dictionary<string, object>> MockDictionaryGetOpportunityResponseSignUpNoTest()
        {
            var results = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 7},
                    {"Opportunity_ID", 77},
                    {"Participant_ID", 777},
                    {"Response_Result_ID", 2}
                }
            };
            return results;
        }

        [Test]
        public void GetOpportunityResponseNoExistingRsvpTest()
        {
            //ARRANGE
            const int opportunityId = 8;
            const int eventId = 3;
            var participant = new Participant { ParticipantId = 5 };

            // mock _ministryPlatformService.GetPageViewRecords
            const string viewKey = "ResponseByOpportunityAndEvent";
            var searchString = string.Format(",{0},{1},{2}", opportunityId, eventId, participant.ParticipantId);
            const string sortString = "";
            var mockResult = MockDictionaryGetOpportunityResponseNoExistingRsvpTest();
            _ministryPlatformService.Setup(
                m => m.GetPageViewRecords(viewKey, It.IsAny<string>(), searchString, sortString, 0)).Returns(mockResult);

            //ACT
            var response = _fixture.GetOpportunityResponse(opportunityId, eventId, participant);

            //ASSERT
            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(response);
            Assert.AreEqual(0, response.Opportunity_ID);
            Assert.AreEqual(0, response.Participant_ID);
            Assert.AreEqual(null, response.Response_Result_ID);

        }

        private List<Dictionary<string, object>> MockDictionaryGetOpportunityResponseNoExistingRsvpTest()
        {
            var results = new List<Dictionary<string, object>>();
            return results;
        }

        [Test]
        public void RespondToOpportunityAsLoggedInUserTest()
        {
            const int opportunityId = 9;
            const string comments = "test-comments";
            const int mockParticipantId = 7777;
            const string pageKey = "OpportunityResponses";

            _authenticationService.Setup(m => m.GetParticipantRecord(It.IsAny<string>()))
                .Returns(new Participant {ParticipantId = mockParticipantId});

            _ministryPlatformService.Setup(
                m => m.CreateRecord(pageKey, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>(), true))
                .Returns(3333);

            var responseId = _fixture.RespondToOpportunity(It.IsAny<string>(), opportunityId, comments);

            _authenticationService.VerifyAll();
            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(responseId);
            Assert.AreEqual(3333, responseId);
        }

        [Test]
        public void RespondToOpportunityAsParticipantId()
        {
            const string pageKey = "OpportunityResponses";
            const int participantId = 4444;
            const int opportunityId = 555;
            const string comment = "";
            const int eventId = 3333;
            const bool response = true;

            _ministryPlatformService.Setup(
                m => m.GetPageViewRecords("ResponseByOpportunityAndEvent", It.IsAny<string>(), It.IsAny<string>(), "", 0)).Returns(new List<Dictionary<string, object>>());
            _ministryPlatformService.Setup(
                m => m.CreateRecord(pageKey, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>(), true))
                .Returns(4444);

            var responseId = _fixture.RespondToOpportunity(participantId, opportunityId, comment, eventId, response);

            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(responseId);
            Assert.AreEqual(4444, responseId);
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

        [Test]
        public void ShouldGetAllEventDates()
        {
            const int opportunityId = 145;
            var today = DateTime.Today;

            var expectedEventType = new Dictionary<string, object>
            {
                {"Event_Type_ID_Text", "KC Nursery Oakley Sunday 8:30"}
            };
            var expectedEvents = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Event_Start_Date", today.AddDays(-7)}
                },
                new Dictionary<string, object>
                {
                     {"Event_Start_Date", today}
                }, 
                new Dictionary<string, object>
                {
                     {"Event_Start_Date", today.AddDays(7)}
                }
            };
            var expectedDates = new List<DateTime>
            {
                today,
                today.AddDays(7)
            };

            _ministryPlatformService.Setup(
                mock => mock.GetRecordDict(_opportunityPageId, opportunityId, It.IsAny<string>(), false))
                .Returns(expectedEventType);
            _ministryPlatformService.Setup(
                mock => mock.GetRecordsDict(_eventPageId, It.IsAny<string>(), ",,KC Nursery Oakley Sunday 8:30", It.IsAny<string>()))
                .Returns(expectedEvents);

            var dates = _fixture.GetAllOpportunityDates(opportunityId, It.IsAny<string>());
            Assert.IsNotNull(dates);
            Assert.AreEqual(2, dates.Count);
            Assert.AreEqual(expectedDates, dates);
        }

        [Test]
        public void ShouldGetLastEventDate()
        {
            const int opportunityId = 145;

            var expectedEventType = new Dictionary<string, object>
            {
                {"Event_Type_ID_Text", "KC Nursery Oakley Sunday 8:30"}
            };
            var expectedEvents = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Event_Start_Date", "10/11/15 08:30am"}
                }
            };
            var expectedLastDate = DateTime.Parse("10/11/15 08:30am");

            _ministryPlatformService.Setup(
                mock => mock.GetRecordDict(_opportunityPageId, opportunityId, It.IsAny<string>(), false))
                .Returns(expectedEventType);
            _ministryPlatformService.Setup(
                mock => mock.GetRecordsDict(_eventPageId, It.IsAny<string>(), ",,KC Nursery Oakley Sunday 8:30", It.IsAny<string>()))
                .Returns(expectedEvents);

            var lastDate = _fixture.GetLastOpportunityDate(opportunityId, It.IsAny<string>());
            Assert.IsNotNull(lastDate);
            Assert.AreEqual(expectedLastDate, lastDate);
        }

        [Test]
        public void ShouldThrowExceptionGettingLastEventDateWhenNoEvents()
        {
            const int opportunityId = 145;

            var expectedEventType = new Dictionary<string, object>
            {
                {"Event_Type_ID_Text", "KC Nursery Oakley Sunday 8:30"}
            };
            var expectedEvents = new List<Dictionary<string, object>>();

            _ministryPlatformService.Setup(
                mock => mock.GetRecordDict(_opportunityPageId, opportunityId, It.IsAny<string>(), false))
                .Returns(expectedEventType);
            _ministryPlatformService.Setup(
                mock => mock.GetRecordsDict(_eventPageId, It.IsAny<string>(), ",,KC Nursery Oakley Sunday 8:30", It.IsAny<string>()))
                .Returns(expectedEvents);

            Assert.Throws<Exception>(() => _fixture.GetLastOpportunityDate(opportunityId, It.IsAny<string>()));
        }

        [Test]
        public void ShouldReturnGroupParticipantsForOpportunity()
        {
            const int opportunityId = 145;

            var expectedOpportunity = new Dictionary<string, object>
            {
                {"dp_RecordID", 100},
                {"Opportunity Title", "Opportunity 100"},
                {"Event Type", "Event Type 100"},
                {"Event Type ID", 100},
                {"Role_Title", "Role Title 100"},
                {"Maximum_Needed", 100}, {"Minimum_Needed", 50},
                {"Add_to_Group", 255},
                {"Add_to_Group_Text", "Test Group"},
                {"Group_Role_ID", 1},
                {"Event_Type_ID", 385}
            };

            var expectedParticipants = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Contact_ID", 123},
                    {"Group_Role_ID", 1},
                    {"Role_Title", "Boss"},
                    {"Last_Name", "Garfunkel"},
                    {"Nickname", "Art"},
                    {"dp_RecordID", 12}
                },
                new Dictionary<string, object>
                {
                    {"Contact_ID", 456},
                    {"Group_Role_ID", 1},
                    {"Role_Title", "Boss"},
                    {"Last_Name", "Simon"},
                    {"Nickname", "Paul"},
                    {"dp_RecordID", 17}
                }
            };

            _ministryPlatformService.Setup(mock => mock.GetRecordDict(_opportunityPageId, opportunityId, It.IsAny<string>(), false)).Returns(expectedOpportunity);
            _ministryPlatformService.Setup(mock => mock.GetSubpageViewRecords(_groupsParticipantsSubPageId, 255, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(expectedParticipants);

            var output = _fixture.GetGroupParticipantsForOpportunity(opportunityId, It.IsAny<string>());

            _ministryPlatformService.VerifyAll();
            Assert.AreEqual(255, output.GroupId);
            Assert.AreEqual("Test Group", output.Name);
            Assert.AreEqual(2, output.Participants.Count);
        }

        [Test]
        public void ShouldChangeRSVPFromYesToNo()
        {
            const int opportunityId = 9;
            const int eventId = 9234;
            const string comments = "test-comments";
            const int mockParticipantId = 7777;
            const int pageId = 382;


            // mock _ministryPlatformService.GetPageViewRecords
            const string viewKey = "ResponseByOpportunityAndEvent";
            var searchString = string.Format(",{0},{1},{2}", opportunityId, eventId, mockParticipantId);
            const string sortString = "";
            var mockResult = MockDictionaryGetOpportunityResponseSignUpYesTest();
            _ministryPlatformService.Setup(
                m => m.GetPageViewRecords(viewKey, It.IsAny<string>(), searchString, sortString, 0)).Returns(mockResult);

            _ministryPlatformService.Setup(
                m => m.UpdateRecord(pageId, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));

            var responseId = _fixture.RespondToOpportunity(mockParticipantId, opportunityId, comments, eventId, false);

            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(responseId);
            Assert.AreEqual(7, responseId);
        }
    }
}