using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using crds_angular.Models.Crossroads.Trip;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Extensions;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using IDonationService = MinistryPlatform.Translation.Services.Interfaces.IDonationService;

namespace crds_angular.Services
{
    public class TripService : MinistryPlatformBaseService, ITripService
    {
        private readonly IEventParticipantService _eventParticipantService;
        private readonly IDonationService _donationService;

        public TripService(IEventParticipantService eventParticipant, IDonationService donationService)
        {
            _eventParticipantService = eventParticipant;
            _donationService = donationService;
        }

        public List<TripParticipantDto> Search(string search)
        {
            var results = _eventParticipantService.TripParticipants(search);

            var participants = results.GroupBy(r =>
                                                   new
                                                   {
                                                       r.ParticipantId,
                                                       r.EmailAddress,
                                                       r.Lastname,
                                                       r.Nickname
                                                   }).Select(x => new TripParticipantDto()
                                                   {
                                                       ParticipantId = x.Key.ParticipantId,
                                                       Email = x.Key.EmailAddress,
                                                       Lastname = x.Key.Lastname,
                                                       Nickname = x.Key.Nickname,
                                                       ShowGiveButton = true,
                                                       ShowShareButtons = false
                                                   }).ToDictionary(y => y.ParticipantId);

            foreach (var result in results)
            {
                var tp = new TripDto();
                tp.EventParticipantId = result.EventParticipantId;
                tp.EventEnd = result.EventEndDate.ToString("MMM dd, yyyy");
                tp.EventId = result.EventId;
                tp.EventStartDate = result.EventStartDate.ToUnixTime();
                tp.EventStart = result.EventStartDate.ToString("MMM dd, yyyy");
                tp.EventTitle = result.EventTitle;
                tp.EventType = result.EventType;
                var participant = participants[result.ParticipantId];
                participant.Trips.Add(tp);
                tp.ProgramId = result.ParticipantId;
                tp.ProgramName = result.ProgramName;
            }

            return participants.Values.OrderBy(o => o.Lastname).ThenBy(o => o.Nickname).ToList();
        }

        public MyTripsDTO GetMyTrips(int contactId, string token)
        {
            var trips = _donationService.GetMyTripDistributions(contactId, token).OrderBy(t => t.EventStartDate);
            
            
            var myTrips = new MyTripsDTO();

            var events = new List<Trip>();
            var eventIds = new List<int>();
            foreach (var trip in trips.Where(trip => !eventIds.Contains(trip.EventId)))
            {
                var eventParticipantId = 0;
                var eventParticipantIds = _eventParticipantService.TripParticipants("," + trip.EventId + ",,,,,,,,,,,," + contactId).FirstOrDefault();
                if (eventParticipantIds != null)
                {
                    eventParticipantId = eventParticipantIds.EventParticipantId;
                }
                eventIds.Add(trip.EventId);
                events.Add(new Trip
                {
                    EventId = trip.EventId,
                    EventType = trip.EventTypeId.ToString(),
                    EventTitle = trip.EventTitle,
                    EventStartDate = trip.EventStartDate.ToString("MMM dd, yyyy"),
                    EventEndDate = trip.EventEndDate.ToString("MMM dd, yyyy"),
                    FundraisingDaysLeft = Math.Max(0, (trip.CampaignEndDate - DateTime.Today).Days),
                    FundraisingGoal = trip.TotalPledge,
                    EventParticipantId = eventParticipantId
                });
            }

            foreach (var e in events)
            {
                var donations = trips.Where(d => d.EventId == e.EventId).OrderByDescending(d => d.DonationDate).ToList();
                foreach (var donation in donations)
                {
                    var gift = new TripGift();
                    if (donation.AnonymousGift)
                    {
                        gift.DonorNickname = "Anonymous";
                        gift.DonorLastName = "";
                    }
                    else
                    {
                        gift.DonorNickname = donation.DonorNickname ?? donation.DonorFirstName;
                        gift.DonorLastName = donation.DonorLastName;
                    }
                    gift.DonorEmail = donation.DonorEmail;
                    gift.DonationDate = donation.DonationDate.ToShortDateString();
                    gift.DonationAmount = donation.DonationAmount;
                    gift.RegisteredDonor = donation.RegisteredDonor;
                    e.TripGifts.Add(gift);
                    e.TotalRaised += donation.DonationAmount;
                }
                myTrips.MyTrips.Add(e);
            }
            return myTrips;
        }

    }
}