using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        //private Mock<IPersonService> _personService;
        private Mock<IServeService> _serveService;

        private ServeService _fixture;

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
                    Capacity = 1,
                    EventType = "event-type-1",
                    Events = eventsList1,
                    OpportunityId = 1,
                    OpportunityName = "opportunity-name-1",
                    RoleTitle = "opportunity-1-role-title"
                },
                new Opportunity
                {
                    Capacity = 2,
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
