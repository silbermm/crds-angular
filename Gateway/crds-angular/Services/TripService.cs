using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Trip;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;
using IDonationService = MinistryPlatform.Translation.Services.Interfaces.IDonationService;
using IGroupService = MinistryPlatform.Translation.Services.Interfaces.IGroupService;

namespace crds_angular.Services
{
    public class TripService : MinistryPlatformBaseService, ITripService
    {
        private readonly IEventParticipantService _eventParticipantService;
        private readonly IDonationService _donationService;
        private readonly IGroupService _groupService;
        private readonly IFormSubmissionService _formSubmissionService;
        private readonly IEventService _mpEventService;


        public TripService(IEventParticipantService eventParticipant,
                           IDonationService donationService,
                           IGroupService groupService,
                           IFormSubmissionService formSubmissionService,
                           IEventService eventService)
        {
            _eventParticipantService = eventParticipant;
            _donationService = donationService;
            _groupService = groupService;
            _formSubmissionService = formSubmissionService;
            _mpEventService = eventService;
        }

        public List<TripGroupDto> GetGroupsByEventId(int eventId)
        {
            var mpGroups = _groupService.GetGroupsForEvent(eventId);

            return mpGroups.Select(record => new TripGroupDto
            {
                GroupId = record.GroupId,
                GroupName = record.Name
            }).ToList();
        }

        public TripFormResponseDto GetFormResponses(int selectionId, int selectionCount)
        {
            var tripApplicantResponses = GetTripAplicantsSelection(selectionId, selectionCount);

            //get groups for event, type = go trip
            var eventId = tripApplicantResponses[0].EventId;
            var eventGroups = _mpEventService.GetGroupsForEvent(eventId);

            var dto = new TripFormResponseDto();
            dto.Applicants = tripApplicantResponses.Select(s => new TripApplicant {ContactId = s.ContactId, ParticipantId = s.ParticipantId}).ToList();
            dto.Groups = eventGroups.Select(s => new TripGroupDto {GroupId = s.GroupId, GroupName = s.Name}).ToList();
            return dto;
        }

        private List<TripApplicantResponse> GetTripAplicantsSelection(int selectionId, int selectionCount)
        {
            var responses = _formSubmissionService.GetTripFormResponses(selectionId);

            //check for only one pledge campaign
            var campaignCount = responses.GroupBy(x => x.PledgeCampaignId)
                .Select(x => new {Date = x.Key, Values = x.Distinct().Count()}).Count();

            // is this check stupid?
            if (responses.Count != selectionCount)
                throw new ApplicationException("Error Retrieving Selection");

            if (campaignCount > 1)
                throw new ApplicationException("Invalid Trip Selection");

            return responses.Select(record => new TripApplicantResponse
            {
                ContactId = record.ContactId,
                PledgeCampaignId = record.PledgeCampaignId,
                EventId = record.EventId,
                ParticipantId = record.ParticipantId
            }).ToList();
        }

        public MyTripsDTO GetMyTrips(int contactId, string token)
        {
            var trips = _donationService.GetMyTripDistributions(contactId, token).OrderBy(t => t.EventStartDate);

            var myTrips = new MyTripsDTO();

            var events = new List<Trip>();
            var eventIds = new List<int>();
            foreach (var trip in trips.Where(trip => !eventIds.Contains(trip.EventId)))
            {
                eventIds.Add(trip.EventId);
                events.Add(new Trip
                {
                    EventId = trip.EventId,
                    EventType = trip.EventTypeId.ToString(),
                    EventTitle = trip.EventTitle,
                    EventStartDate = trip.EventStartDate.ToString("MMM dd, yyyy"),
                    EventEndDate = trip.EventEndDate.ToString("MMM dd, yyyy"),
                    FundraisingDaysLeft = Math.Max(0, (trip.CampaignEndDate - DateTime.Today).Days),
                    FundraisingGoal = trip.TotalPledge
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

            return participants.Values.OrderBy(o => o.Lastname).ThenBy(o => o.Nickname).ToList();
        }

        public void SaveParticipants(SaveTripParticipantsDto dto)
        {
            // need a list of contacts?
            // need a groupId
            // we have pledge campaign id, that would be nice too

            // make them a group participiant
            // get all events for group id
            // add event participant for each contact to each event found
            // create pledge for donor associated with contact
            // if donor doesn't exist create one?

            var groupStartDate = DateTime.Now;
            const int groupRoleId = 16; // wondering if eventually this will become user input?
            var events = _groupService.getAllEventsForGroup(dto.GroupId);

            foreach (var applicant in dto.Applicants)
            {
                //create group participant
                var x = _groupService.addParticipantToGroup(applicant.ParticipantId, dto.GroupId, groupRoleId, groupStartDate);

                // create pledge
                //what if no donor?

                // register for all events
                foreach (var e in events)
                {
                    _mpEventService.registerParticipantForEvent(applicant.ParticipantId, e.EventId);
                }
            }

            

            throw new NotImplementedException();
        }
    }
}