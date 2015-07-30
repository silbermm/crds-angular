using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Trip;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Extensions;
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
            }
            
            return participants.Values.OrderBy(o=>o.Lastname).ThenBy(o=>o.Nickname).ToList();
            
        }

        public MyTripsDTO GetMyTrips(int contactId)
        {
            var trips = _donationService.GetMyTripDistributions(contactId);

            var myTrips = new MyTripsDTO();
            var events = trips.Select(e => new Trip {EventId = e.EventId, EventTitle = e.EventTitle, EventStartDate = e.EventStartDate.ToShortDateString(), EventEndDate = e.EventEndDate.ToShortDateString(), FundraisingGoal = e.TotalPledge, FundraisingDaysLeft = (e.CampaignEndDate - DateTime.Today).Days}).Distinct().ToList();
            foreach (var e in events)
            {
                var donations = trips.Where(d => d.EventId == e.EventId).ToList();
                foreach (var donation in donations)
                {
                    var gift = new TripGift
                    {
                        DonorNickname = donation.DonorNickname,
                        DonorFirstName = donation.DonorFirstName,
                        DonorLastName = donation.DonorLastName,
                        DonorEmail = donation.DonorEmail,
                        DonationDate = donation.DonationDate.ToShortDateString(),
                        DonationAmount = donation.DonationAmount
                    };
                    e.TripGifts.Add(gift);
                    e.TotalRaised += donation.DonationAmount;
                }
                myTrips.MyTrips.Add(e);
            }
            return myTrips;
        }
    }
}