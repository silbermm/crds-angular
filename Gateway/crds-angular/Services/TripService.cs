using System.Collections.Generic;
using crds_angular.Models.Crossroads.Trip;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Services
{
    public class TripService : MinistryPlatformBaseService,ITripService
    {
        private readonly IEventParticipantService _eventParticipantService;

        public TripService(IEventParticipantService eventParticipant)
        {
            _eventParticipantService = eventParticipant;
        }

        public List<Particpant> Search(string search, string token)
        {
            var result = _eventParticipantService.TripParticipants(search);

            var participants = new List<Particpant> ();

            foreach (var r in result)
            {
                //need to group list results by participant id
                //ling way?

                var p = new Particpant();
                p.Email = r.EmailAddress;
                p.Lastname = r.Lastname;
                p.Nickname = r.Nickname;
                //p.ParticipantId = r.ParticipantId;
                //p.ParticipantName = "abc";

                var tp = new TripParticipant();
                tp.EventParticipantId = r.EventParticipantId;
                var trip = new Trip();
                trip.EventEndDate = r.EventEndDate.ToUnixTime();
                trip.EventId = r.EventId;
                trip.EventStartDate = r.EventStartDate.ToUnixTime();
                trip.EventTitle = r.EventTitle;
                trip.EventType = r.EventType;

                tp.Trip = trip;

                p.TripParticipant = tp;


            }
            return participants;

        }
    }
}