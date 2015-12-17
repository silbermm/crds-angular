using System;
using System.Collections.Generic;
using System.Configuration;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using crds_angular.Util.Interfaces;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;
using IEventService = MinistryPlatform.Translation.Services.Interfaces.IEventService;
using Participant = MinistryPlatform.Translation.Models.People.Participant;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class ChildcareServiceTest
    {
        private Mock<IEventParticipantService> _eventParticipantService;
        private Mock<ICommunicationService> _communicationService;
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private Mock<IContactService> _contactService;
        private Mock<IEventService> _eventService;
        private Mock<IParticipantService> _participantService;
        private Mock<IServeService> _serveService;
        private Mock<IDateTime> _dateTimeWrapper;
        // Interfaces.IEventService crdsEventService, IApiUserService apiUserService
        private Mock<crds_angular.Services.Interfaces.IEventService> _crdsEventService;
        private Mock<IApiUserService> _apiUserService;

        private ChildcareService _fixture;

        [SetUp]
        public void SetUp()
        {
            _eventParticipantService = new Mock<IEventParticipantService>();
            _communicationService = new Mock<ICommunicationService>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _contactService = new Mock<IContactService>();
            _eventService = new Mock<IEventService>();
            _participantService = new Mock<IParticipantService>();
            _serveService = new Mock<IServeService>();
            _dateTimeWrapper = new Mock<IDateTime>();
            _crdsEventService = new Mock<crds_angular.Services.Interfaces.IEventService>();
            _apiUserService = new Mock<IApiUserService>();

            _fixture = new ChildcareService(_eventParticipantService.Object,
                                            _communicationService.Object,
                                            _configurationWrapper.Object,
                                            _contactService.Object,
                                            _eventService.Object,
                                            _participantService.Object,
                                            _serveService.Object,
                                            _dateTimeWrapper.Object,
                                            _apiUserService.Object, _crdsEventService.Object);
        }

        [Test]
        public void GetMyKidsForChildcare()
        {
            var mockDateTime = new DateTime(2015, 5, 29);
            _dateTimeWrapper.Setup(m => m.Today).Returns(mockDateTime);

            _serveService.Setup(m => m.GetImmediateFamilyParticipants(It.IsAny<string>())).Returns(MockFamily());

            _configurationWrapper.Setup(m => m.GetConfigIntValue("MaxAgeWithoutGrade")).Returns(8);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("MaxGradeForChildcare")).Returns(5);

            var x = _fixture.MyChildren("fake-token");

            _serveService.VerifyAll();
            Assert.AreEqual(2, x.Count);
        }

        private static List<FamilyMember> MockFamily()
        {
            return new List<FamilyMember>
            {
                new FamilyMember
                {
                    ContactId = 1111,
                    ParticipantId = 1111,
                    PreferredName = "Husband",
                    LastName = null,
                    LoggedInUser = true,
                    Email = "tmaddox33mp1@gmail.com",
                    RelationshipId = 0,
                    Age = 40
                },
                new FamilyMember
                {
                    ContactId = 2222,
                    ParticipantId = 2222,
                    PreferredName = "Child",
                    LastName = "Bunch",
                    LoggedInUser = false,
                    Email = null,
                    RelationshipId = 6,
                    Age = 12,
                    HighSchoolGraduationYear = 2021
                },
                new FamilyMember
                {
                    ContactId = 2222,
                    ParticipantId = 2222,
                    PreferredName = "Child",
                    LastName = "Bunch",
                    LoggedInUser = false,
                    Email = null,
                    RelationshipId = 6,
                    Age = 8,
                    HighSchoolGraduationYear = 2025
                },
                new FamilyMember
                {
                    ContactId = 2222,
                    ParticipantId = 2222,
                    PreferredName = "Child2",
                    LastName = "Bunch",
                    LoggedInUser = false,
                    Email = null,
                    RelationshipId = 6,
                    Age = 7
                },
                new FamilyMember
                {
                    ContactId = 2222,
                    ParticipantId = 2222,
                    PreferredName = "Child2",
                    LastName = "Bunch",
                    LoggedInUser = false,
                    Email = null,
                    RelationshipId = 6,
                    Age = 9
                },
                new FamilyMember
                {
                    ContactId = 333,
                    ParticipantId = 333,
                    PreferredName = "wife",
                    LastName = "spouse",
                    LoggedInUser = false,
                    Email = "wife@spouse.net",
                    RelationshipId = 1,
                    Age = 23
                }
            };
        }

        [Test, TestCaseSource("TestCases")]
        public void CalcGrade(int expectedGrade, int graduationYear, DateTime mockDateTime)
        {
            _dateTimeWrapper.Setup(m => m.Today).Returns(mockDateTime);

            var grade = _fixture.SchoolGrade(graduationYear);

            Assert.AreEqual(expectedGrade, grade);
        }

        private static readonly object[] TestCases =
        {
            new object[] {0, 2014, new DateTime(2015, 5, 29)},
            new object[] {2, 2025, new DateTime(2015, 5, 29)},
            new object[] {3, 2025, new DateTime(2015, 11, 10)},
            new object[] {0, 2035, new DateTime(2015, 5, 29)}
        };

        [Test]
        public void SendTwoRsvpEmails()
        {
            const int daysBefore = 999;
            const int emailTemplateId = 77;
            const int unassignedContact = 7386594;
            var participants = new List<EventParticipant>
            {
                new EventParticipant
                {
                    ParticipantId = 1,
                    EventId = 123,
                    ContactId = 987654
                },
                new EventParticipant
                {
                    ParticipantId = 2,
                    EventId = 456,
                    ContactId = 456123
                }
            };

            var mockPrimaryContact = new Contact
            {
                ContactId = 98765,
                EmailAddress = "wonder-woman@ip.com"
            };

            var defaultContact = new MyContact
            {
                Contact_ID = 123456,
                Email_Address = "gmail@gmail.com"
            };

            var mockEvent1 = new Event {EventType = "Childcare", PrimaryContact = mockPrimaryContact};
            var mockEvent2 = new Event {EventType = "DoggieDaycare", PrimaryContact = mockPrimaryContact};
            var mockEvents = new List<Event> {mockEvent1, mockEvent2};

            _configurationWrapper.Setup(m => m.GetConfigIntValue("NumberOfDaysBeforeEventToSend")).Returns(daysBefore);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("ChildcareRequestTemplate")).Returns(emailTemplateId);
            _communicationService.Setup(m => m.GetTemplate(emailTemplateId)).Returns(new MessageTemplate());            
            _eventParticipantService.Setup(m => m.GetChildCareParticipants(daysBefore)).Returns(participants);
            _communicationService.Setup(m => m.SendMessage(It.IsAny<Communication>())).Verifiable();

            var kids = new List<Participant> { new Participant { ContactId = 456321987 } };
            _crdsEventService.Setup(m => m.EventParticpants(987654321, It.IsAny<string>())).Returns(kids);
            var mockChildcareEvent = new Event {EventId = 987654321};
            var mockContact = new Contact
            {
                ContactId = 8888888,
                EmailAddress = "sometest@test.com"
            };
            mockChildcareEvent.PrimaryContact = mockContact;
            _crdsEventService.Setup(m => m.GetChildcareEvent(participants[0].EventId)).Returns(mockChildcareEvent);
            _crdsEventService.Setup(m => m.GetChildcareEvent(participants[1].EventId)).Returns(mockChildcareEvent);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("DefaultContactEmailId")).Returns(1234);
            _contactService.Setup(mocked => mocked.GetContactById(1234)).Returns(defaultContact); 
            var myKids = new List<Participant>();
            _crdsEventService.Setup(m => m.MyChildrenParticipants(987654, kids, It.IsAny<string>())).Returns(myKids);

            _fixture.SendRequestForRsvp();

            _configurationWrapper.VerifyAll();
            _communicationService.VerifyAll();
            _contactService.VerifyAll();
            _eventParticipantService.VerifyAll();
            _communicationService.VerifyAll();
            _communicationService.Verify(m => m.SendMessage(It.IsAny<Communication>()), Times.Exactly(2));
            _eventService.VerifyAll();
        }
    }
}