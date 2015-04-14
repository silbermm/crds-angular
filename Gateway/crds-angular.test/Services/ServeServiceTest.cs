using System;
using System.Collections.Generic;
using crds_angular.App_Start;
using crds_angular.Extenstions;
using crds_angular.Models;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

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
            _eventService=new Mock<IEventService>();
            _serveService = new Mock<IServeService>();
            _participantService = new Mock<IParticipantService>();

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


           // _fixture = new PersonService(_contactService.Object);
            _fixture = new ServeService( _groupService.Object ,_contactRelationshipService.Object,_personService.Object,_authenticationService.Object,_opportunityService.Object,_eventService.Object, _participantService.Object);

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

            var familyMembers = _fixture.GetMyImmediateFamily(contactId, token);

            _contactRelationshipService.VerifyAll();

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
            _contactRelationshipService.Setup(mocked => mocked.GetMyImmediatieFamilyRelationships(contactId, It.IsAny<string>()))
                .Returns(new List<ContactRelationship>());

            var familyMembers = _fixture.GetMyImmediateFamily(contactId, token);

            _contactRelationshipService.VerifyAll();

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

            var teams = _fixture.GetServingTeams(token);

            _contactRelationshipService.VerifyAll();
            _groupService.VerifyAll();

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
                    RoleTitle = "opportunity-1-role-title"
                },
                new Opportunity
                {
                    EventType = "event-type-2",
                    Events = eventsList2,
                    OpportunityId = 2,
                    OpportunityName = "opportunity-name-2",
                    RoleTitle = "opportunity-1-role-title"
                }
            };
            _opportunityService.Setup(mocked => mocked.GetOpportunitiesForGroup(It.IsAny<Int32>(), It.IsAny<string>()))
                .Returns(opportunities);

            var teams = new List<ServingTeam> { new ServingTeam { GroupId = 1 }, new ServingTeam { GroupId = 2 } };
            _serveService.Setup(mocked => mocked.GetServingTeams(It.IsAny<string>())).Returns(teams);

            _groupService.Setup(mocked => mocked.GetServingTeams(123456, It.IsAny<string>())).Returns(new List<Group>
            {
                new Group {GroupId = 1, Name = "group-1", GroupRole = "group-role-member"},
                new Group {GroupId = 2, Name = "group-2", GroupRole = "group-role-member"},
                new Group {GroupId = 2, Name = "group-2", GroupRole = "group-role-leader"},
                new Group {GroupId = 3, Name = "group-3", GroupRole = "group-role-member"},
                new Group {GroupId = 4, Name = "group-4", GroupRole = "group-role-member"}
            });

            var servingDays = _fixture.GetServingDays(It.IsAny<string>());

            _opportunityService.VerifyAll();

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
        public void OpportunityCapacityHasMinHasMax(int? min, int? max, int mockSignUpCount, Capacity expectedCapacity)
        {
            const int opportunityId = 9999;
            const int eventId = 1000;

            //mock
            _opportunityService.Setup(m => m.GetOpportunitySignupCount(opportunityId, eventId, It.IsAny<string>()))
                .Returns(mockSignUpCount);

           var capacity= _fixture.OpportunityCapacity(max, min, opportunityId, eventId, It.IsAny<string>());

            _opportunityService.VerifyAll();

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
                10, 20, 0,
                new Capacity
                {
                    Available = 10,
                    BadgeType = "label-warning",
                    Display = true,
                    Maximum = 20,
                    Message = "10 Needed",
                    Minimum = 10,
                    Taken = 0
                }
            },
            new object[]
            {
                10, null, 0,
                new Capacity
                {
                    Available = 10,
                    BadgeType = "label-warning",
                    Display = true,
                    Maximum = 10,
                    Message = "10 Needed",
                    Minimum = 10,
                    Taken = 0
                }
            },
            new object[]
            {
                null, 20, 0,
                new Capacity
                {
                    Available = 20,
                    BadgeType = "label-warning",
                    Display = true,
                    Maximum = 20,
                    Message = "20 Needed",
                    Minimum = 20,
                    Taken = 0
                }
            },
            new object[]
            {
                10, 20, 15,
                new Capacity
                {
                    Available = -5,
                    BadgeType = "label-default",
                    Display = true,
                    Maximum = 20,
                    Message = "Available",
                    Minimum = 10,
                    Taken = 15
                }
            },
            new object[]
            {
                10, 20, 20,
                new Capacity
                {
                    Available = -10,
                    BadgeType = "label-success",
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

            var capacity = _fixture.OpportunityCapacity(null, null, opportunityId, eventId, It.IsAny<string>());

            Assert.IsNotNull(capacity);
            Assert.AreEqual(capacity.Display, false);
        }

        [Test]
        public void SaveServeResponseTest()
        {
            const string token = "some-string";
            const int contactId = 123456;
            const int opportunityId = 1;
            const int eventTypeId = 2;
            var  startDate = new DateTime(2015,4,1);
            var endDate = new DateTime(2015, 4, 30);
            const int mockParticipantId = 41018;

            _participantService.Setup(m => m.GetParticipant(contactId)).Returns(new Participant { ParticipantId = mockParticipantId });

            var mockEvents = MockEvents();
            _eventService.Setup(m => m.GetEventsByTypeForRange(eventTypeId, startDate, endDate, It.IsAny<string>()))
                .Returns(mockEvents);
           
            foreach (var mockEvent in mockEvents)
            {
                var e = mockEvent;
                _eventService.Setup(m => m.registerParticipantForEvent(mockParticipantId, e.EventId)).Returns(e.EventId);

                // Mock _opportunityService.RespondToOpportunity
                _opportunityService.Setup(m => m.RespondToOpportunity(mockParticipantId, opportunityId, "", e.EventId)).Returns(8888);
            }
            
            var saveResponse = _fixture.SaveServeResponse(token, contactId, opportunityId, eventTypeId, startDate, endDate);

            _participantService.VerifyAll();

            _eventService.Verify(m => m.registerParticipantForEvent(mockParticipantId, 2), Times.Exactly(1));
            _eventService.Verify(m => m.registerParticipantForEvent(mockParticipantId, 3), Times.Exactly(1));
            _eventService.Verify(m => m.registerParticipantForEvent(mockParticipantId, 4), Times.Exactly(1));

            _opportunityService.Verify(m => m.RespondToOpportunity(mockParticipantId, opportunityId, "",2), Times.Exactly(1));
            _opportunityService.Verify(m => m.RespondToOpportunity(mockParticipantId, opportunityId, "",3), Times.Exactly(1));
            _opportunityService.Verify(m => m.RespondToOpportunity(mockParticipantId, opportunityId, "",4), Times.Exactly(1));
            _opportunityService.VerifyAll();

            //Assertions
            Assert.IsNotNull(saveResponse);
            Assert.IsTrue(saveResponse);
        }

        private List<Event> MockEvents()
        {
            
            var event1 = new Event
            {
                EventEndDate = new DateTime(2015, 3, 15),
                EventStartDate = new DateTime(2015, 3, 15),
                EventType = "event-type-2",
                EventId = 1
            };

            var event2 = new Event
            {
                EventEndDate = new DateTime(2015, 4, 1),
                EventStartDate = new DateTime(2015, 4, 1),
                EventType = "event-type-2",
                EventId = 2
            };

            var event3 = new Event
            {
                EventEndDate = new DateTime(2015, 4, 15),
                EventStartDate = new DateTime(2015, 4, 15),
                EventType = "event-type-2",
                EventId = 3
            };

            var event4 = new Event
            {
                EventEndDate = new DateTime(2015, 4, 30),
                EventStartDate = new DateTime(2015, 4, 30),
                EventType = "event-type-2",
                EventId = 4
            };

            var event5 = new Event
            {
                EventEndDate = new DateTime(2015, 5, 1),
                EventStartDate = new DateTime(2015, 5, 1),
                EventType = "event-type-2",
                EventId = 5
            };

            var eventList = new List<Event> { event2, event3, event4};
            return eventList;
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

        private MyContact MockMyContact()
        {
            var myContact = new MyContact
            {
                Contact_ID = 123456,
                Email_Address = "main-contact@email.com",
                Last_Name = "main-contact",
                Nickname = "main-contact-nickname",
                First_Name = "main-contact-firstname"
            };
            return myContact;
        }
    }
}
