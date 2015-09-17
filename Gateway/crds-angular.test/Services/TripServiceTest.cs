using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;
using IDonationService = MinistryPlatform.Translation.Services.Interfaces.IDonationService;
using IDonorService = MinistryPlatform.Translation.Services.Interfaces.IDonorService;
using IGroupService = MinistryPlatform.Translation.Services.Interfaces.IGroupService;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class TripServiceTest
    {
        private Mock<IEventParticipantService> _eventParticipantService;
        private Mock<IDonationService> _donationService;
        private Mock<IGroupService> _groupService;
        private Mock<IFormSubmissionService> _formSubmissionService;
        private Mock<IEventService> _eventService;
        private Mock<IDonorService> _donorService;
        private Mock<IPledgeService> _pledgeService;
        private Mock<ICampaignService> _campaignService;
        private Mock<IPrivateInviteService> _privateInviteService;
        private Mock<ICommunicationService> _communicationService;
        private Mock<IContactService> _contactService;
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private Mock<IPersonService> _personService;
        private Mock<IServeService> _serveService;
        private TripService _fixture;

        [SetUp]
        public void SetUp()
        {
            _eventParticipantService = new Mock<IEventParticipantService>();
            _donationService = new Mock<IDonationService>();
            _groupService = new Mock<IGroupService>();
            _formSubmissionService = new Mock<IFormSubmissionService>();
            _eventService = new Mock<IEventService>();
            _donorService = new Mock<IDonorService>();
            _pledgeService = new Mock<IPledgeService>();
            _campaignService = new Mock<ICampaignService>();
            _privateInviteService = new Mock<IPrivateInviteService>();
            _communicationService = new Mock<ICommunicationService>();
            _contactService = new Mock<IContactService>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _personService = new Mock<IPersonService>();
            _serveService = new Mock<IServeService>();

            _fixture = new TripService(_eventParticipantService.Object,
                                       _donationService.Object,
                                       _groupService.Object,
                                       _formSubmissionService.Object,
                                       _eventService.Object,
                                       _donorService.Object,
                                       _pledgeService.Object,
                                       _campaignService.Object,
                                       _privateInviteService.Object,
                                       _communicationService.Object,
                                       _contactService.Object,
                                       _configurationWrapper.Object,
                                       _personService.Object,
                                       _serveService.Object);
        }

        [Test]
        public void Search()
        {
            _eventParticipantService.Setup(m => m.TripParticipants(It.IsAny<string>())).Returns(MockMpSearchResponse());

            var searchResults = _fixture.Search(It.IsAny<string>());

            _eventParticipantService.VerifyAll();
            Assert.AreEqual(2, searchResults.Count);

            var p1 = searchResults.FirstOrDefault(s => s.ParticipantId == 9999);
            Assert.IsNotNull(p1);
            Assert.AreEqual(2, p1.Trips.Count);

            var p2 = searchResults.FirstOrDefault(s => s.ParticipantId == 5555);
            Assert.IsNotNull(p2);
            Assert.AreEqual(1, p2.Trips.Count);
        }

        [Test]
        public void ShouldGetMyTrips()
        {
            _donationService.Setup(m => m.GetMyTripDistributions(It.IsAny<int>())).Returns(MockTripDonationsResponse());
            _eventParticipantService.Setup(m => m.TripParticipants(It.IsAny<string>())).Returns(mockTripParticipants());
            var myTrips = _fixture.GetMyTrips(It.IsAny<int>());

            _donationService.VerifyAll();

            Assert.IsNotNull(myTrips);
            Assert.AreEqual(1, myTrips.MyTrips.Count);
            Assert.AreEqual(2, myTrips.MyTrips[0].TripGifts.Count);
        }

        [Test]
        public void FundraisingDaysLeftShouldNotBeNegative()
        {
            _donationService.Setup(m => m.GetMyTripDistributions(It.IsAny<int>())).Returns(MockFundingPastTripDonationsResponse());
            _eventParticipantService.Setup(m => m.TripParticipants(It.IsAny<string>())).Returns(mockTripParticipants());
            var myTrips = _fixture.GetMyTrips(It.IsAny<int>());

            Assert.IsNotNull(myTrips);
            Assert.AreEqual(0, myTrips.MyTrips[0].FundraisingDaysLeft);
        }

        private List<TripDistribution> MockFundingPastTripDonationsResponse()
        {
            return new List<TripDistribution>
            {
                new TripDistribution
                {
                    ContactId = 1234,
                    EventTypeId = 6,
                    EventId = 8,
                    EventTitle = "GO Someplace",
                    EventStartDate = DateTime.Today,
                    EventEndDate = DateTime.Today,
                    TotalPledge = 1000,
                    CampaignStartDate = DateTime.Today.AddDays(-15),
                    CampaignEndDate = DateTime.Today.AddDays(-10),
                    DonorNickname = "John",
                    DonorFirstName = "John",
                    DonorLastName = "Donor",
                    DonorEmail = "crdsusertest+johndonor@gmail.com",
                    DonationDate = DateTime.Today,
                    DonationAmount = 350
                }
            };
        }

        private List<TripParticipant> mockTripParticipants()
        {
            return new List<TripParticipant>
            {
                new TripParticipant()
                {
                    EmailAddress = "myEmail@Address.com",
                    EventStartDate = new DateTime(2015, 10, 08),
                    EventEndDate = new DateTime(2015, 10, 23),
                    EventId = 20,
                    EventParticipantId = 21,
                    EventTitle = "Go Someplace",
                    EventType = "MissionTrip",
                    Lastname = "Name",
                    Nickname = "Funny",
                    ParticipantId = 213,
                    ProgramId = 2,
                    ProgramName = "Go Someplace"
                }
            };
        }

        private List<TripDistribution> MockTripDonationsResponse()
        {
            return new List<TripDistribution>
            {
                new TripDistribution
                {
                    ContactId = 1234,
                    EventTypeId = 6,
                    EventId = 8,
                    EventTitle = "GO Someplace",
                    EventStartDate = DateTime.Today,
                    EventEndDate = DateTime.Today,
                    TotalPledge = 1000,
                    CampaignStartDate = DateTime.Today,
                    CampaignEndDate = DateTime.Today,
                    DonorNickname = "John",
                    DonorFirstName = "John",
                    DonorLastName = "Donor",
                    DonorEmail = "crdsusertest+johndonor@gmail.com",
                    DonationDate = DateTime.Today,
                    DonationAmount = 350
                },
                new TripDistribution
                {
                    ContactId = 1234,
                    EventTypeId = 6,
                    EventId = 8,
                    EventTitle = "Go Someplace",
                    EventStartDate = DateTime.Today,
                    EventEndDate = DateTime.Today,
                    TotalPledge = 1000,
                    CampaignStartDate = DateTime.Today,
                    CampaignEndDate = DateTime.Today,
                    DonorNickname = "John",
                    DonorFirstName = "John",
                    DonorLastName = "Donor",
                    DonorEmail = "crdsusertest+johndonor@gmail.com",
                    DonationDate = DateTime.Today,
                    DonationAmount = 200
                }
            };
        }

        private static List<TripParticipant> MockMpSearchResponse()
        {
            return new List<TripParticipant>
            {
                new TripParticipant
                {
                    EmailAddress = "test@aol.com",
                    EventEndDate = new DateTime(2015, 7, 1),
                    EventId = 1,
                    EventParticipantId = 7777,
                    EventStartDate = new DateTime(2015, 7, 1),
                    EventTitle = "Test Trip 1",
                    EventType = "Go Trip",
                    Lastname = "Subject",
                    Nickname = "Test",
                    ParticipantId = 9999
                },
                new TripParticipant
                {
                    EmailAddress = "test@aol.com",
                    EventEndDate = new DateTime(2015, 8, 1),
                    EventId = 2,
                    EventParticipantId = 888,
                    EventStartDate = new DateTime(2015, 8, 1),
                    EventTitle = "Test Trip 2",
                    EventType = "Go Trip",
                    Lastname = "Subject",
                    Nickname = "Test",
                    ParticipantId = 9999
                },
                new TripParticipant
                {
                    EmailAddress = "spec@aol.com",
                    EventEndDate = new DateTime(2015, 7, 1),
                    EventId = 1,
                    EventParticipantId = 4444,
                    EventStartDate = new DateTime(2015, 7, 1),
                    EventTitle = "Test Trip 1",
                    EventType = "Go Trip",
                    Lastname = "Dummy",
                    Nickname = "Crash",
                    ParticipantId = 5555
                }
            };
        }
    }
}