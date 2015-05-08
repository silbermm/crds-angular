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
        private Mock<IGroupService> _groupService;
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
            _groupService = new Mock<IGroupService>();
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

            _fixture = new ServeService(_groupService.Object, _contactRelationshipService.Object, _personService.Object,
                _authenticationService.Object, _opportunityService.Object, _eventService.Object,
                _participantService.Object, _groupParticipantService.Object);
             

            //force AutoMapper to register
            AutoMapperConfig.RegisterMappings();
        }

        [Test]
        public void GetMyFamilyTest()
        {
            const string token = "some-string";
            const int contactId = 123456;

            _contactRelationshipService.Setup(
                mocked => mocked.GetMyImmediatieFamilyRelationships(contactId, It.IsAny<string>()))
                .Returns(MockGetMyFamilyResponse());

            _participantService.Setup(m => m.GetParticipant(It.IsAny<int>()))
                .Returns(new Participant {ParticipantId = 1});

            var familyMembers = _fixture.GetMyImmediateFamily(contactId, token);

            _contactRelationshipService.VerifyAll();
            _participantService.VerifyAll();

            Assert.IsNotNull(familyMembers);
            Assert.AreEqual(3, familyMembers.Count);

            var familyMember = familyMembers[0];
            Assert.AreEqual(1, familyMember.ContactId);
            Assert.AreEqual("person-one@test.com", familyMember.Email);
            Assert.AreEqual("person-one", familyMember.LastName);
            Assert.AreEqual("preferred-name-one", familyMember.PreferredName);

            familyMember = familyMembers[1];
            Assert.AreEqual(2, familyMember.ContactId);
            Assert.AreEqual("person-two@test.com", familyMember.Email);
            Assert.AreEqual("person-two", familyMember.LastName);
            Assert.AreEqual("preferred-name-two", familyMember.PreferredName);

            familyMember = familyMembers[2];
            Assert.AreEqual(123456, familyMember.ContactId);
            Assert.AreEqual("contact@email.com", familyMember.Email);
            Assert.AreEqual("last-name", familyMember.LastName);
            Assert.AreEqual("nickname", familyMember.PreferredName);
        }

        [Test]
        public void GetMyFamilyNoNicknameTest()
        {
            const string token = "some-string";
            const int contactId = 123456;

            //return 0 family members, only testing logic for main contact
            _contactRelationshipService.Setup(
                mocked => mocked.GetMyImmediatieFamilyRelationships(contactId, It.IsAny<string>()))
                .Returns(new List<ContactRelationship>());

            _participantService.Setup(m => m.GetParticipant(It.IsAny<int>()))
                .Returns(new Participant {ParticipantId = 1});

            var familyMembers = _fixture.GetMyImmediateFamily(contactId, token);

            _contactRelationshipService.VerifyAll();
            _participantService.VerifyAll();

            Assert.IsNotNull(familyMembers);
            Assert.AreEqual(1, familyMembers.Count);

            var familyMember = familyMembers[0];
            Assert.AreEqual(123456, familyMember.ContactId);
            Assert.AreEqual("contact@email.com", familyMember.Email);
            Assert.AreEqual("last-name", familyMember.LastName);
            Assert.AreEqual("nickname", familyMember.PreferredName);
        }

        [Test]
        public void GetMyFamiliesServingTeamsTest()
        {
            const string token = "some-string";
            const int contactId = 123456;

            _contactRelationshipService.Setup(
                mocked => mocked.GetMyImmediatieFamilyRelationships(contactId, It.IsAny<string>()))
                .Returns(MockGetMyFamilyResponse());

            var familyMember1Groups = new List<Group>
            {
                new Group {GroupId = 1, Name = "group-1", GroupRole = "group-role-member"},
                new Group {GroupId = 2, Name = "group-2", GroupRole = "group-role-member"},
                new Group {GroupId = 2, Name = "group-2", GroupRole = "group-role-leader"},
                new Group {GroupId = 3, Name = "group-3", GroupRole = "group-role-member"},
                new Group {GroupId = 4, Name = "group-4", GroupRole = "group-role-member"}
            };

            var familyMember2Groups = new List<Group>
            {
                new Group {GroupId = 1, Name = "group-1", GroupRole = "group-role-member"},
                new Group {GroupId = 2, Name = "group-2", GroupRole = "group-role-member"}
            };

            var myGroups = new List<Group>
            {
                new Group {GroupId = 1, Name = "group-1", GroupRole = "group-role-member"},
                new Group {GroupId = 2, Name = "group-2", GroupRole = "group-role-member"},
                new Group {GroupId = 3, Name = "group-3", GroupRole = "group-role-leader"}
            };

            const int mockFamilyContactId1 = 1;
            const int mockFamilyContactId2 = 2;
            _groupService.Setup(mocked => mocked.GetServingTeams(mockFamilyContactId1, token))
                .Returns(familyMember1Groups);
            _groupService.Setup(mocked => mocked.GetServingTeams(mockFamilyContactId2, token))
                .Returns(familyMember2Groups);
            _groupService.Setup(mocked => mocked.GetServingTeams(contactId, token)).Returns(myGroups);

            _participantService.Setup(m => m.GetParticipant(It.IsAny<int>()))
                .Returns(new Participant {ParticipantId = 1});

            var teams = _fixture.GetServingTeams(token);

            _contactRelationshipService.VerifyAll();
            _groupService.VerifyAll();
            _participantService.VerifyAll();

            Assert.IsNotNull(teams);
            Assert.AreEqual(4, teams.Count);

            var team = teams[0];
            Assert.AreEqual(1, team.GroupId);
            Assert.AreEqual("group-1", team.Name);
            Assert.AreEqual(3, team.Members.Count);
            var member = team.Members[0];
            Assert.AreEqual(1, member.ContactId);
            Assert.AreEqual("preferred-name-one", member.Name);
            Assert.AreEqual(1, member.Roles.Count);

            team = teams[1];
            Assert.AreEqual(2, team.GroupId);
            Assert.AreEqual("group-2", team.Name);
            Assert.AreEqual(3, team.Members.Count);
            member = team.Members[0];
            Assert.AreEqual(1, member.ContactId);
            Assert.AreEqual("preferred-name-one", member.Name);
            Assert.AreEqual(2, member.Roles.Count);

            team = teams[2];
            Assert.AreEqual(3, team.GroupId);
            Assert.AreEqual("group-3", team.Name);
            Assert.AreEqual(2, team.Members.Count);

            team = teams[3];
            Assert.AreEqual(4, team.GroupId);
            Assert.AreEqual("group-4", team.Name);
            Assert.AreEqual(1, team.Members.Count);
        }

        [Test]
        public void GetMyFamiliesServingEventsTest()
        {
            var today = DateTime.Now;
            var startTimeEightThirty = new DateTime(today.Year, today.Month, today.Day, 8, 30, 0);
            var startTimeTen = new DateTime(today.Year, today.Month, today.Day, 10, 00, 0);

            var eventsList1 = new List<Event>
            {
                new Event
                {
                    EventId = 1,
                    EventStartDate = startTimeEightThirty,
                    EventTitle = "event-1-title",
                    EventType = "event-type-1"
                },
                new Event
                {
                    EventId = 2,
                    EventStartDate = startTimeTen,
                    EventTitle = "event-2-title",
                    EventType = "event-type-2"
                }
            };

            var eventsList2 = new List<Event>
            {
                new Event
                {
                    EventId = 3,
                    EventStartDate = startTimeEightThirty,
                    EventTitle = "event-3-title",
                    EventType = "event-type-3"
                },
                new Event
                {
                    EventId = 4,
                    EventStartDate = startTimeTen,
                    EventTitle = "event-4-title",
                    EventType = "event-type-4"
                }
            };

            var opportunities = new List<Opportunity>
            {
                new Opportunity
                {
                    EventType = "event-type-1",
                    Events = eventsList1,
                    OpportunityId = 1,
                    OpportunityName = "opportunity-name-1",
                    RoleTitle = "opportunity-1-role-title",
                    Responses = new List<Response> {new Response {Event_ID = 1}, new Response {Event_ID = 2}}
                },
                new Opportunity
                {
                    EventType = "event-type-2",
                    Events = eventsList2,
                    OpportunityId = 2,
                    OpportunityName = "opportunity-name-2",
                    RoleTitle = "opportunity-2-role-title",
                    Responses = new List<Response> {new Response {Event_ID = 3}, new Response {Event_ID = 4}}
                }
            };
            _opportunityService.Setup(mocked => mocked.GetOpportunitiesForGroup(It.IsAny<Int32>(), It.IsAny<string>()))
                .Returns(opportunities);

            var teams = new List<ServingTeam> {new ServingTeam {GroupId = 1}, new ServingTeam {GroupId = 2}};
            //_serveService.Setup(mocked => mocked.GetServingTeams(It.IsAny<string>())).Returns(teams);

            _groupService.Setup(mocked => mocked.GetServingTeams(123456, It.IsAny<string>())).Returns(new List<Group>
            {
                new Group {GroupId = 1, Name = "group-1", GroupRole = "group-role-member"},
                new Group {GroupId = 2, Name = "group-2", GroupRole = "group-role-member"},
                new Group {GroupId = 2, Name = "group-2", GroupRole = "group-role-leader"},
                new Group {GroupId = 3, Name = "group-3", GroupRole = "group-role-member"},
                new Group {GroupId = 4, Name = "group-4", GroupRole = "group-role-member"}
            });

            _participantService.Setup(m => m.GetParticipant(It.IsAny<int>()))
                .Returns(new Participant {ParticipantId = 1});

            var servingDays = _fixture.GetServingDays(It.IsAny<string>());

            _opportunityService.VerifyAll();
            _serveService.VerifyAll();
            _groupService.VerifyAll();
            _participantService.VerifyAll();

            Assert.IsNotNull(servingDays);
            Assert.AreEqual(1, servingDays.Count);
            var servingDay = servingDays[0];
            Assert.AreEqual(2, servingDay.ServeTimes.Count);

            var servingTime = servingDay.ServeTimes[0];
            Assert.AreEqual(4, servingTime.ServingTeams.Count);
            Assert.AreEqual("08:30:00", servingTime.Time);

            servingTime = servingDay.ServeTimes[1];
            Assert.AreEqual(4, servingTime.ServingTeams.Count);
            Assert.AreEqual("10:00:00", servingTime.Time);
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

            var capacity = _fixture.OpportunityCapacity(opportunity, eventId, It.IsAny<string>());

            //_opportunityService.VerifyAll();

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

            var capacity = _fixture.OpportunityCapacity(opportunity, eventId, It.IsAny<string>());

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

            SetUpRSVPMocks(contactId, eventTypeId, opportunityId, signUp);

            _fixture.SaveServeRsvp(It.IsAny<string>(), contactId, opportunityId, eventTypeId, It.IsAny<DateTime>(),
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


            SetUpRSVPMocks(contactId, eventTypeId, opportunityId, signUp);

            _fixture.SaveServeRsvp(It.IsAny<string>(), contactId, opportunityId, eventTypeId, It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), signUp, alternateWeeks);

            _participantService.VerifyAll();
            _eventService.Verify(
                m =>
                    m.GetEventsByTypeForRange(eventTypeId, It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                        It.IsAny<string>()), Times.Exactly(1));
            _opportunityService.Verify(
                (m => m.RespondToOpportunity(47, opportunityId, It.IsAny<string>(), It.IsAny<int>(), signUp)),
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
            var expectedEventIds = new List<int> {1, 3, 5};

            SetUpRSVPMocks(contactId, eventTypeId, opportunityId, signUp);

            _fixture.SaveServeRsvp(It.IsAny<string>(), contactId, opportunityId, eventTypeId, It.IsAny<DateTime>(),
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
            var expectedEventIds = new List<int> {1, 3, 5};

            SetUpRSVPMocks(contactId, eventTypeId, opportunityId, signUp);

            _fixture.SaveServeRsvp(It.IsAny<string>(), contactId, opportunityId, eventTypeId, It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), signUp, alternateWeeks);

            _participantService.VerifyAll();
            _eventService.Verify(
                m =>
                    m.GetEventsByTypeForRange(eventTypeId, It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                        It.IsAny<string>()), Times.Exactly(1));
            _eventService.Verify(m => m.registerParticipantForEvent(47, It.IsAny<int>()), Times.Never());
            _opportunityService.Verify(
                (m =>
                    m.RespondToOpportunity(47, opportunityId, It.IsAny<string>(), It.IsIn<int>(expectedEventIds), signUp)),
                Times.Exactly(3));
        }

        [Test]
        public void GetServingDaysFaster()
        {
            var servingParticipants = new List<GroupServingParticipant>
            {
                new GroupServingParticipant
                {
                    ContactId = 2,
                    DomainId = 1,
                    EventId = 3,
                    EventStartDateTime = DateTime.Now,
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

            _groupParticipantService.Setup(m => m.GetServingParticipants()).Returns(servingParticipants);

            var servingDays = _fixture.GetServingDaysFaster(It.IsAny<string>());

            Assert.NotNull(servingDays);
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