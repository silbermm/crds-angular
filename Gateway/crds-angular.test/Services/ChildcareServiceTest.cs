using System.Collections.Generic;
using crds_angular.Services;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

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

            _fixture = new ChildcareService(_eventParticipantService.Object,
                                            _communicationService.Object,
                                            _configurationWrapper.Object,
                                            _contactService.Object,
                                            _eventService.Object,
                                            _participantService.Object);
        }

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
                    EventId = 123
                },
                new EventParticipant
                {
                    ParticipantId = 2,
                    EventId = 456
                }
            };

            var mockPrimaryContact = new Contact
            {
                ContactId = 98765,
                EmailAddress = "wonder-woman@ip.com"
            };

            var mockEvent1 = new Event {EventType = "Childcare", PrimaryContact = mockPrimaryContact};
            var mockEvent2 = new Event {EventType = "DoggieDaycare", PrimaryContact = mockPrimaryContact};
            var mockEvents = new List<Event> {mockEvent1, mockEvent2};

            _configurationWrapper.Setup(m => m.GetConfigIntValue("NumberOfDaysBeforeEventToSend")).Returns(daysBefore);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("ChildcareRequestTemplate")).Returns(emailTemplateId);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("EmailAuthorId")).Returns(1);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("UnassignedContact")).Returns(unassignedContact);
            _communicationService.Setup(m => m.GetTemplate(emailTemplateId)).Returns(new MessageTemplate());
            _contactService.Setup(m => m.GetContactById(unassignedContact)).Returns(new MyContact());
            _eventParticipantService.Setup(m => m.GetChildCareParticipants(daysBefore)).Returns(participants);
            _communicationService.Setup(m => m.SendMessage(It.IsAny<Communication>())).Verifiable();
            _eventService.Setup(m => m.GetEventsByParentEventId(123)).Returns(mockEvents);
            _eventService.Setup(m => m.GetEventsByParentEventId(456)).Returns(mockEvents);

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