using System;
using System.Collections.Generic;
using crds_angular.App_Start;
using crds_angular.Models;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class ServeServiceTest
    {
        private Mock<IContactRelationshipService> _contactRelationshipService;
        private Mock<IContactService> _contactService;
        private Mock<IOpportunityService> _opportunityService;
        private Mock<IAuthenticationService> _authenticationService;
        private Mock<IPersonService> _personService;
        private Mock<IServeService> _serveService;
        private Mock<IEventService> _eventService;
        private Mock<IParticipantService> _participantService;
        private Mock<IGroupParticipantService> _groupParticipantService;

        private ServeService _fixture;

        [SetUp]
        public void SetUp()
        {
            _contactRelationshipService = new Mock<IContactRelationshipService>();
            _contactService = new Mock<IContactService>();
            _opportunityService = new Mock<IOpportunityService>();
            _authenticationService = new Mock<IAuthenticationService>();
            _personService = new Mock<IPersonService>();
            _eventService = new Mock<IEventService>();
            _serveService = new Mock<IServeService>();
            _participantService = new Mock<IParticipantService>();
            _groupParticipantService = new Mock<IGroupParticipantService>();

            _authenticationService.Setup(mocked => mocked.GetContactId(It.IsAny<string>())).Returns(123456);
            var myContact = new MyContact
            {
                Contact_ID = 123456,
                Email_Address = "contact@email.com",
                Last_Name = "last-name",
                Nickname = "nickname",
                First_Name = "first-name",
                Middle_Name = "middle-name",
                Maiden_Name = "maiden-name",
                Mobile_Phone = "mobile-phone",
                Mobile_Carrier = 999,
                Date_Of_Birth = "date-of-birth",
                Marital_Status_ID = 5,
                Gender_ID = 2,
                Employer_Name = "employer-name",
                Address_Line_1 = "address-line-1",
                Address_Line_2 = "address-line-2",
                City = "city",
                State = "state",
                Postal_Code = "postal-code",
                Anniversary_Date = "anniversary-date",
                Foreign_Country = "foreign-country",
                Home_Phone = "home-phone",
                Congregation_ID = 8,
                Household_ID = 7,
                Address_ID = 6
            };
            _contactService.Setup(mocked => mocked.GetMyProfile(It.IsAny<string>())).Returns(myContact);

            var person = new Person();
            person.ContactId = myContact.Contact_ID;
            person.EmailAddress = myContact.Email_Address;
            person.LastName = myContact.Last_Name;
            person.NickName = myContact.Nickname;

            _personService.Setup(m => m.GetLoggedInUserProfile(It.IsAny<string>())).Returns(person);

            _fixture = new ServeService(_contactRelationshipService.Object,
                _opportunityService.Object, _eventService.Object,
                _participantService.Object, _groupParticipantService.Object);

            //force AutoMapper to register
            AutoMapperConfig.RegisterMappings();
        }

        [Test]
        public void GetMyFamiliesServingEventsTest()
        {
            var contactId = 123456;

            _contactRelationshipService.Setup(m => m.GetMyImmediatieFamilyRelationships(contactId, It.IsAny<string>())).Returns(MockContactRelationships());

            _participantService.Setup(m => m.GetParticipant(It.IsAny<int>()))
                .Returns(new Participant { ParticipantId = 1 });

            _groupParticipantService.Setup(g => g.GetServingParticipants(It.IsAny<List<int>>(),It.IsAny<long>(), It.IsAny<long>())).Returns(MockGroupServingParticipants());

            var servingDays = _fixture.GetServingDays(It.IsAny<string>(), contactId, It.IsAny<long>(), It.IsAny<long>());
            _contactRelationshipService.VerifyAll();
            _groupParticipantService.Verify();
            _serveService.VerifyAll();
            _participantService.VerifyAll();

            Assert.IsNotNull(servingDays);
            Assert.AreEqual(2, servingDays.Count);
            var servingDay = servingDays[0];
            Assert.AreEqual(2, servingDay.ServeTimes.Count);

            var servingTime = servingDay.ServeTimes[0];
            Assert.AreEqual(1, servingTime.ServingTeams.Count);

            servingTime = servingDay.ServeTimes[1];
            Assert.AreEqual(1, servingTime.ServingTeams.Count);
        }

        private static List<GroupServingParticipant> MockGroupServingParticipants()
        {
            var startDate = DateTime.Today;
            var servingParticipants = new List<GroupServingParticipant>
            {
                new GroupServingParticipant
                {
                    ContactId = 2,
                    DomainId = 1,
                    EventId = 3,
                    EventStartDateTime = startDate,
                    EventTitle = "Serving Event",
                    EventType = "Event Type",
                    EventTypeId = 4,
                    GroupId = 5,
                    GroupName = "Group",
                    GroupPrimaryContactEmail = "group@leader.com",
                    GroupRoleId = 6,
                    OpportunityId = 7,
                    OpportunityMaximumNeeded = 10,
                    OpportunityMinimumNeeded = 5,
                    OpportunityRoleTitle = "Member",
                    OpportunityShiftEnd = TimeSpan.Parse("8:30"),
                    OpportunityShiftStart = TimeSpan.Parse("10:30"),
                    OpportunitySignUpDeadline = 7,
                    OpportunityTitle = "Serving",
                    ParticipantEmail = "partici@pants.com",
                    ParticipantId = 8,
                    ParticipantLastName = "McServer",
                    ParticipantNickname = "Servy",
                    Rsvp = true
                },
                new GroupServingParticipant
                {
                    ContactId = 2,
                    DomainId = 1,
                    EventId = 3,
                    EventStartDateTime = startDate.AddHours(4),
                    EventTitle = "Serving Event",
                    EventType = "Event Type",
                    EventTypeId = 4,
                    GroupId = 5,
                    GroupName = "Group",
                    GroupPrimaryContactEmail = "group@leader.com",
                    GroupRoleId = 6,
                    OpportunityId = 7,
                    OpportunityMaximumNeeded = 10,
                    OpportunityMinimumNeeded = 5,
                    OpportunityRoleTitle = "Member",
                    OpportunityShiftEnd = TimeSpan.Parse("8:30"),
                    OpportunityShiftStart = TimeSpan.Parse("10:30"),
                    OpportunitySignUpDeadline = 7,
                    OpportunityTitle = "Serving",
                    ParticipantEmail = "partici@pants.com",
                    ParticipantId = 8,
                    ParticipantLastName = "McServer",
                    ParticipantNickname = "Servy",
                    Rsvp = true
                },
                new GroupServingParticipant
                {
                    ContactId = 2,
                    DomainId = 1,
                    EventId = 3,
                    EventStartDateTime = startDate.AddDays(1),
                    EventTitle = "Serving Event",
                    EventType = "Event Type",
                    EventTypeId = 4,
                    GroupId = 5,
                    GroupName = "Group",
                    GroupPrimaryContactEmail = "group@leader.com",
                    GroupRoleId = 6,
                    OpportunityId = 7,
                    OpportunityMaximumNeeded = 10,
                    OpportunityMinimumNeeded = 5,
                    OpportunityRoleTitle = "Member",
                    OpportunityShiftEnd = TimeSpan.Parse("8:30"),
                    OpportunityShiftStart = TimeSpan.Parse("10:30"),
                    OpportunitySignUpDeadline = 7,
                    OpportunityTitle = "Serving",
                    ParticipantEmail = "partici@pants.com",
                    ParticipantId = 8,
                    ParticipantLastName = "McServer",
                    ParticipantNickname = "Servy",
                    Rsvp = true
                },
                new GroupServingParticipant
                {
                    ContactId = 2,
                    DomainId = 1,
                    EventId = 3,
                    EventStartDateTime = startDate.AddDays(1).AddHours(4),
                    EventTitle = "Serving Event",
                    EventType = "Event Type",
                    EventTypeId = 4,
                    GroupId = 5,
                    GroupName = "Group",
                    GroupPrimaryContactEmail = "group@leader.com",
                    GroupRoleId = 6,
                    OpportunityId = 7,
                    OpportunityMaximumNeeded = 10,
                    OpportunityMinimumNeeded = 5,
                    OpportunityRoleTitle = "Member",
                    OpportunityShiftEnd = TimeSpan.Parse("8:30"),
                    OpportunityShiftStart = TimeSpan.Parse("10:30"),
                    OpportunitySignUpDeadline = 7,
                    OpportunityTitle = "Serving",
                    ParticipantEmail = "partici@pants.com",
                    ParticipantId = 8,
                    ParticipantLastName = "McServer",
                    ParticipantNickname = "Servy",
                    Rsvp = true
                }
            };
            return servingParticipants;
        }

        private static List<ContactRelationship> MockContactRelationships()
        {
            var mockRelationships = new List<ContactRelationship>();
            var mockRelationship1 = new ContactRelationship();
            mockRelationship1.Contact_Id = 1111111;
            mockRelationship1.Participant_Id = 1;
            var mockRelationship2 = new ContactRelationship();
            mockRelationship2.Contact_Id = 123456;
            mockRelationship2.Participant_Id = 2;
            mockRelationships.Add(mockRelationship1);
            mockRelationships.Add(mockRelationship2);
            return mockRelationships;
        }

        [Test, TestCaseSource("OpportunityCapacityCases")]
        public void OpportunityCapacityHasMinHasMax(int? min, int? max, List<Response> mockResponses,
            Capacity expectedCapacity)
        {
            const int opportunityId = 9999;
            const int eventId = 1000;

            var opportunity = new Opportunity();
            opportunity.MaximumNeeded = max;
            opportunity.MinimumNeeded = min;
            opportunity.OpportunityId = opportunityId;
            opportunity.Responses = mockResponses;

            _opportunityService.Setup(m => m.GetOpportunityResponses(opportunityId, It.IsAny<string>()))
                .Returns(opportunity.Responses);

            var capacity = _fixture.OpportunityCapacity(opportunityId, eventId, min, max, It.IsAny<string>());

            Assert.IsNotNull(capacity);
            Assert.AreEqual(capacity.Available, expectedCapacity.Available);
            Assert.AreEqual(capacity.BadgeType, expectedCapacity.BadgeType);
            Assert.AreEqual(capacity.Display, expectedCapacity.Display);
            Assert.AreEqual(capacity.Maximum, expectedCapacity.Maximum);
            Assert.AreEqual(capacity.Message, expectedCapacity.Message);
            Assert.AreEqual(capacity.Minimum, expectedCapacity.Minimum);
            Assert.AreEqual(capacity.Taken, expectedCapacity.Taken);
        }

        private static readonly object[] OpportunityCapacityCases =
        {
            new object[]
            {
                10, 20, new List<Response>(),
                new Capacity
                {
                    Available = 10,
                    BadgeType = "label-info",
                    Display = true,
                    Maximum = 20,
                    Message = "10 Needed",
                    Minimum = 10,
                    Taken = 0
                }
            },
            new object[]
            {
                10, null, new List<Response>(),
                new Capacity
                {
                    Available = 10,
                    BadgeType = "label-info",
                    Display = true,
                    Maximum = 10,
                    Message = "10 Needed",
                    Minimum = 10,
                    Taken = 0
                }
            },
            new object[]
            {
                null, 20, new List<Response>(),
                new Capacity
                {
                    Available = 20,
                    BadgeType = "label-info",
                    Display = true,
                    Maximum = 20,
                    Message = "20 Needed",
                    Minimum = 20,
                    Taken = 0
                }
            },
            new object[]
            {
                10, 20, MockFifteenResponses(),
                new Capacity
                {
                    Display = true,
                    Maximum = 20,
                    Minimum = 10,
                }
            },
            new object[]
            {
                10, 20, MockTwentyResponses(),
                new Capacity
                {
                    Available = -10,
                    BadgeType = "label-default",
                    Display = true,
                    Maximum = 20,
                    Message = "Full",
                    Minimum = 10,
                    Taken = 20
                }
            }
        };

        [Test]
        public void OpportunityCapacityMinAndMaxNull()
        {
            const int opportunityId = 9999;
            const int eventId = 1000;

            var opportunity = new Opportunity();
            opportunity.MaximumNeeded = null;
            opportunity.MinimumNeeded = null;
            opportunity.OpportunityId = opportunityId;
            opportunity.Responses = new List<Response>();

            _opportunityService.Setup(m => m.GetOpportunityResponses(opportunityId, It.IsAny<string>()))
                .Returns(opportunity.Responses);

            var capacity = _fixture.OpportunityCapacity(opportunityId, eventId, opportunity.MinimumNeeded, opportunity.MaximumNeeded, It.IsAny<string>());

            Assert.IsNotNull(capacity);
            Assert.AreEqual(capacity.Display, false);
        }

        [Test]
        public void RespondToServeOpportunityYesEveryWeek()
        {
            const int contactId = 8;
            const int opportunityId = 12;
            const int eventTypeId = 3;
            const bool signUp = true;
            const bool alternateWeeks = false;
            var oppIds = new List<int>() { 1, 2, 3, 4, 5 };

            SetUpRSVPMocks(contactId, eventTypeId, opportunityId, signUp);

            _fixture.SaveServeRsvp(It.IsAny<string>(), contactId, opportunityId,oppIds, eventTypeId, It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), signUp, alternateWeeks);

            _participantService.VerifyAll();
            _eventService.Verify(
                m =>
                    m.GetEventsByTypeForRange(eventTypeId, It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                        It.IsAny<string>()), Times.Exactly(1));
            _eventService.Verify(m => m.registerParticipantForEvent(47, It.IsAny<int>()), Times.Exactly(5));
            _opportunityService.Verify(
                (m => m.RespondToOpportunity(47, opportunityId, It.IsAny<string>(), It.IsAny<int>(), signUp)),
                Times.Exactly(5));
        }

        [Test]
        public void RespondToServeOpportunityNoEveryWeek()
        {
            const int contactId = 8;
            const int opportunityId = 12;
            const int eventTypeId = 3;
            const bool signUp = false;
            const bool alternateWeeks = false;
            var oppIds = new List<int>() {1,2,3,4,5};


            SetUpRSVPMocks(contactId, eventTypeId, opportunityId, signUp);

            _fixture.SaveServeRsvp(It.IsAny<string>(), contactId, opportunityId, oppIds, eventTypeId, It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), signUp, alternateWeeks);

            _participantService.VerifyAll();
            _eventService.Verify(
                m =>
                    m.GetEventsByTypeForRange(eventTypeId, It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                        It.IsAny<string>()), Times.Exactly(1));
            
            _opportunityService.Verify(
                (m => m.RespondToOpportunity(47, 1, It.IsAny<string>(), It.IsAny<int>(), signUp)),
                Times.Exactly(5));
            _opportunityService.Verify(
                (m => m.RespondToOpportunity(47, 2, It.IsAny<string>(), It.IsAny<int>(), signUp)),
                Times.Exactly(5));
            _eventService.Verify(m => m.registerParticipantForEvent(47, It.IsAny<int>()), Times.Never());
        }

        [Test]
        public void RespondToServeOpportunityYesForEveryOtherWeek()
        {
            const int contactId = 8;
            const int opportunityId = 12;
            const int eventTypeId = 3;
            const bool signUp = true;
            const bool alternateWeeks = true;
            var expectedEventIds = new List<int> { 1, 3, 5 };
            var oppIds = new List<int>() { 1, 2, 3, 4, 5 };

            SetUpRSVPMocks(contactId, eventTypeId, opportunityId, signUp);

            _fixture.SaveServeRsvp(It.IsAny<string>(), contactId, opportunityId, oppIds, eventTypeId, It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), signUp, alternateWeeks);

            _participantService.VerifyAll();
            _eventService.Verify(
                m =>
                    m.GetEventsByTypeForRange(eventTypeId, It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                        It.IsAny<string>()), Times.Exactly(1));
            _eventService.Verify(m => m.registerParticipantForEvent(47, It.IsIn<int>(expectedEventIds)),
                Times.Exactly(3));
            _opportunityService.Verify(
                (m =>
                    m.RespondToOpportunity(47, opportunityId, It.IsAny<string>(), It.IsIn<int>(expectedEventIds), signUp)),
                Times.Exactly(3));
        }

        [Test]
        public void RespondToServeOpportunityNoForEveryOtherWeek()
        {
            const int contactId = 8;
            const int opportunityId = 12;
            const int eventTypeId = 3;
            const bool signUp = false;
            const bool alternateWeeks = true;
            var expectedEventIds = new List<int> { 1, 3, 5 };
            var oppIds = new List<int>() { 1, 2, 3, 4, 5 };

            SetUpRSVPMocks(contactId, eventTypeId, opportunityId, signUp);

            _fixture.SaveServeRsvp(It.IsAny<string>(), contactId, opportunityId, oppIds, eventTypeId, It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), signUp, alternateWeeks);

            _participantService.VerifyAll();
            _eventService.Verify(
                m =>
                    m.GetEventsByTypeForRange(eventTypeId, It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                        It.IsAny<string>()), Times.Exactly(1));
            _eventService.Verify(m => m.registerParticipantForEvent(47, It.IsAny<int>()), Times.Never());
            _opportunityService.Verify(
                (m =>
                    m.RespondToOpportunity(47, It.IsInRange(1,5, Range.Inclusive), It.IsAny<string>(), It.IsIn<int>(expectedEventIds), signUp)),
                Times.Exactly(15));
        }

        private void SetUpRSVPMocks(int contactId, int eventTypeId, int opportunityId, bool signUp)
        {
            var mockParticipant = new Participant
            {
                ParticipantId = 47
            };

            var mockEvents = new List<Event>
            {
                new Event
                {
                    EventId = 1
                },
                new Event
                {
                    EventId = 2
                },
                new Event
                {
                    EventId = 3
                },
                new Event
                {
                    EventId = 4
                },
                new Event
                {
                    EventId = 5
                },
            };
            //mock it up
            _participantService.Setup(m => m.GetParticipant(contactId)).Returns(mockParticipant);
            _eventService.Setup(
                m =>
                    m.GetEventsByTypeForRange(eventTypeId, It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                        It.IsAny<string>())).Returns(mockEvents);
            foreach (var mockEvent in mockEvents)
            {
                _eventService.Setup(m => m.registerParticipantForEvent(mockParticipant.ParticipantId, mockEvent.EventId));
                _opportunityService.Setup(
                    m =>
                        m.RespondToOpportunity(mockParticipant.ParticipantId, opportunityId, It.IsAny<string>(),
                            mockEvent.EventId, signUp));
            }
        }


        private List<ContactRelationship> MockGetMyFamilyResponse()
        {
            var getMyFamilyResponse = new List<ContactRelationship>
            {
                new ContactRelationship
                {
                    Contact_Id = 1,
                    Email_Address = "person-one@test.com",
                    Last_Name = "person-one",
                    Preferred_Name = "preferred-name-one"
                },
                new ContactRelationship
                {
                    Contact_Id = 2,
                    Email_Address = "person-two@test.com",
                    Last_Name = "person-two",
                    Preferred_Name = "preferred-name-two"
                }
            };
            return getMyFamilyResponse;
        }

        private static List<Response> MockTwentyResponses()
        {
            var responses = new List<Response>();
            for (var i = 0; i < 20; i++)
            {
                responses.Add(new Response { Event_ID = 1000 });
            }
            return responses;
        }

        private static List<Response> MockFifteenResponses()
        {
            var responses = new List<Response>();
            for (var i = 0; i < 15; i++)
            {
                responses.Add(new Response { Event_ID = 1000 });
            }
            return responses;
        }
    }
}
