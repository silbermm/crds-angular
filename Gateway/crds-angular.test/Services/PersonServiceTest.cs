using System;
using System.Collections.Generic;
using crds_angular.App_Start;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    internal class PersonServiceTest
    {
        private Mock<IContactRelationshipService> _contactRelationshipService;
        private Mock<IGroupService> _groupService;
        private Mock<IContactService> _contactService;
        private Mock<IOpportunityService> _opportunityService;

        private PersonService _fixture;

        [SetUp]
        public void SetUp()
        {
            _contactRelationshipService = new Mock<IContactRelationshipService>();
            _groupService = new Mock<IGroupService>();
            _contactService = new Mock<IContactService>();
            _opportunityService = new Mock<IOpportunityService>();

            _fixture = new PersonService(_groupService.Object, _contactRelationshipService.Object,
                _contactService.Object, _opportunityService.Object);

            //force AutoMapper to register
            AutoMapperConfig.RegisterMappings();
        }

        [Test]
        public void GetLoggedInUserProfileTest()
        {
            const string token = "some-string";

            var myContact = new MyContact
            {
                ContactId = 123456,
                EmailAddress = "contact@email.com",
                LastName = "last-name",
                NickName = "nickname",
                FirstName = "first-name",
                MiddleName = "middle-name",
                MaidenName = "maiden-name",
                MobilePhone = "mobile-phone",
                MobileCarrierId = 999,
                DateOfBirth = "date-of-birth",
                MaritalStatusId = 5,
                GenderId = 2,
                EmployerName = "employer-name",
                AddressLine1 = "address-line-1",
                AddressLine2 = "address-line-2",
                City = "city",
                State = "state",
                PostalCode = "postal-code",
                AnniversaryDate = "anniversary-date",
                ForeignCountry = "foreign-country",
                HomePhone = "home-phone",
                CongregationId = 8,
                HouseholdId = 7,
                AddressId = 6
            };
            _contactService.Setup(mocked => mocked.GetMyProfile(token)).Returns(myContact);
            
            var person = _fixture.GetLoggedInUserProfile(token);

            Assert.IsNotNull(person);

            Assert.AreEqual(123456,person.ContactId);
            Assert.AreEqual("contact@email.com", person.EmailAddress);
            Assert.AreEqual("nickname", person.NickName);
            Assert.AreEqual("first-name", person.FirstName);
            Assert.AreEqual("middle-name", person.MiddleName);
            Assert.AreEqual("last-name", person.LastName);
            Assert.AreEqual("maiden-name", person.MaidenName);
            Assert.AreEqual("mobile-phone", person.MobilePhone);
            Assert.AreEqual(999, person.MobileCarrierId);
            Assert.AreEqual("date-of-birth", person.DateOfBirth);
            Assert.AreEqual(5, person.MaritalStatusId);
            Assert.AreEqual(2, person.GenderId);
            Assert.AreEqual("employer-name", person.EmployerName);
            Assert.AreEqual("address-line-1", person.AddressLine1);
            Assert.AreEqual("address-line-2", person.AddressLine2);
            Assert.AreEqual("city", person.City);
            Assert.AreEqual("state", person.State);
            Assert.AreEqual("postal-code", person.PostalCode);
            Assert.AreEqual("anniversary-date", person.AnniversaryDate);
            Assert.AreEqual("foreign-country", person.ForeignCountry);
            Assert.AreEqual("home-phone", person.HomePhone);
            Assert.AreEqual(8, person.CongregationId);
            Assert.AreEqual(7, person.HouseholdId);
            Assert.AreEqual(6, person.AddressId);
        }

        [Test]
        public void GetMyFamilyTest()
        {
            const string token = "some-string";
            const int contactId = 123456;

            _contactRelationshipService.Setup(mocked => mocked.GetMyFamily(contactId, It.IsAny<string>()))
                .Returns(MockGetMyFamilyResponse());
            
            _contactService.Setup(mocked => mocked.GetMyProfile(token)).Returns(MockMyContact());

            var familyMembers = _fixture.GetMyFamily(contactId, token);

            _contactRelationshipService.VerifyAll();
            _contactService.VerifyAll();

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
            Assert.AreEqual("main-contact@email.com", familyMember.Email);
            Assert.AreEqual("main-contact", familyMember.LastName);
            Assert.AreEqual("main-contact-nickname", familyMember.PreferredName);
        }

        [Test]
        public void GetMyFamilyNoNicknameTest()
        {
            const string token = "some-string";
            const int contactId = 123456;

            //return 0 family members, only testing logic for main contact
            _contactRelationshipService.Setup(mocked => mocked.GetMyFamily(contactId, It.IsAny<string>()))
                .Returns(new List<Contact_Relationship>());

            var myContact = new MyContact
            {
                ContactId = 123456,
                EmailAddress = "main-contact@email.com",
                LastName = "main-contact",
                FirstName = "main-contact-firstname"
            };
            _contactService.Setup(mocked => mocked.GetMyProfile(token)).Returns(myContact);

            var familyMembers = _fixture.GetMyFamily(contactId, token);

            _contactRelationshipService.VerifyAll();
            _contactService.VerifyAll();

            Assert.IsNotNull(familyMembers);
            Assert.AreEqual(1, familyMembers.Count);

            var familyMember = familyMembers[0];
            Assert.AreEqual(123456, familyMember.ContactId);
            Assert.AreEqual("main-contact@email.com", familyMember.Email);
            Assert.AreEqual("main-contact", familyMember.LastName);
            Assert.AreEqual("main-contact-firstname", familyMember.PreferredName);
        }

        [Test]
        public void GetMyFamiliesServingTeamsTest()
        {
            const string token = "some-string";
            const int contactId = 123456;

            _contactRelationshipService.Setup(mocked => mocked.GetMyFamily(contactId, It.IsAny<string>()))
                .Returns(MockGetMyFamilyResponse());

            _contactService.Setup(mocked => mocked.GetMyProfile(token)).Returns(MockMyContact());

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
            _groupService.Setup(mocked => mocked.GetMyServingTeams(mockFamilyContactId1, token)).Returns(familyMember1Groups);
            _groupService.Setup(mocked => mocked.GetMyServingTeams(mockFamilyContactId2, token)).Returns(familyMember2Groups);
            _groupService.Setup(mocked => mocked.GetMyServingTeams(contactId, token)).Returns(myGroups);

            var teams = _fixture.GetMyFamiliesServingTeams(contactId,token);

            _contactRelationshipService.VerifyAll();
            _contactService.VerifyAll();
            _groupService.VerifyAll();

            Assert.IsNotNull(teams);
            Assert.AreEqual(4,teams.Count);

            var team = teams[0];
            Assert.AreEqual(1, team.GroupId);
            Assert.AreEqual("group-1", team.Name);
            Assert.AreEqual(3,team.Members.Count);
            var member = team.Members[0];
            Assert.AreEqual(1,member.ContactId);
            Assert.AreEqual("preferred-name-one", member.Name);
            Assert.AreEqual(1,member.Roles.Count);

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
            //const string token = "some-string";

            var today = DateTime.Now;
            var startTimeEightThirty= new DateTime(today.Year, today.Month, today.Day, 8, 30,0);
            var startTimeTen = new DateTime(today.Year, today.Month, today.Day, 10, 00, 0);

            var eventsList1 = new List<Event> {new Event {
                EventId = 1,
                EventStartDate = startTimeEightThirty,
                EventTitle = "event-1-title",
                EventType = "event-type-1"
            },new Event
            {
                EventId = 2,
                EventStartDate = startTimeTen,
                EventTitle = "event-2-title",
                EventType = "event-type-2"
            }};

            var eventsList2 = new List<Event> {new Event {
                EventId = 3,
                EventStartDate = startTimeEightThirty,
                EventTitle = "event-3-title",
                EventType = "event-type-3"
            },new Event
            {
                EventId = 4,
                EventStartDate = startTimeTen,
                EventTitle = "event-4-title",
                EventType = "event-type-4"
            }};


            var opportunities = new List<Opportunity>{new Opportunity
            {
                Capacity = 1,
                EventType = "event-type-1",
                Events = eventsList1,
                OpportunityId = 1,
                OpportunityName = "opportunity-name-1",
                RoleTitle = "opportunity-1-role-title"
            },new Opportunity
            {
                Capacity = 2,
                EventType = "event-type-2",
                Events = eventsList2,
                OpportunityId = 2,
                OpportunityName = "opportunity-name-2",
                RoleTitle = "opportunity-1-role-title"
            }};
            _opportunityService.Setup(mocked => mocked.GetOpportunitiesForGroup(1, It.IsAny<string>())).Returns(opportunities);
            _opportunityService.Setup(mocked => mocked.GetOpportunitiesForGroup(2, It.IsAny<string>())).Returns(opportunities);

            var teams = new List<ServingTeam> { new ServingTeam { GroupId = 1 }, new ServingTeam { GroupId = 2 } };
            var servingDays = _fixture.GetMyFamiliesServingDays(teams, It.IsAny<string>());

            _opportunityService.VerifyAll();

            Assert.IsNotNull(servingDays);
            Assert.AreEqual(1,servingDays.Count);
            var servingDay = servingDays[0];
            Assert.AreEqual(2,servingDay.ServeTimes.Count);
            
            var servingTime = servingDay.ServeTimes[0];
            Assert.AreEqual(2,servingTime.ServingTeams.Count);
            Assert.AreEqual("08:30:00", servingTime.Time);

            servingTime = servingDay.ServeTimes[1];
            Assert.AreEqual(2, servingTime.ServingTeams.Count);
            Assert.AreEqual("10:00:00", servingTime.Time);

        }
        
        private List<Contact_Relationship> MockGetMyFamilyResponse()
        {
            var getMyFamilyResponse = new List<Contact_Relationship>
            {
                new Contact_Relationship
                {
                    Contact_Id = 1,
                    Email_Address = "person-one@test.com",
                    Last_Name = "person-one",
                    Preferred_Name = "preferred-name-one"
                },
                new Contact_Relationship
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
                ContactId = 123456,
                EmailAddress = "main-contact@email.com",
                LastName = "main-contact",
                NickName = "main-contact-nickname",
                FirstName = "main-contact-firstname"
            };
            return myContact;
        }
    }
}