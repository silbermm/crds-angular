using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Trip;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Services
{
    public class TripService : MinistryPlatformBaseService, ITripService
    {
        private readonly IEventParticipantService _eventParticipantService;

        public TripService(IEventParticipantService eventParticipant)
        {
            _eventParticipantService = eventParticipant;
        }

        public List<TripParticipantDto> Search(string search, string token)
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
                    Nickname = x.Key.Nickname
                }).ToDictionary(y => y.ParticipantId);

            foreach (var result in results)
            {
                var tp = new TripDto();
                tp.EventParticipantId = result.EventParticipantId;
                tp.EventEndDate = result.EventEndDate.ToUnixTime();
                tp.EventId = result.EventId;
                tp.EventStartDate = result.EventStartDate.ToUnixTime();
                tp.EventTitle = result.EventTitle;
                tp.EventType = result.EventType;

                var participant = participants[result.ParticipantId];
                participant.Trips.Add(tp);                
            }

            return participants.Values.ToList();
            
        }

        
       
    }
}