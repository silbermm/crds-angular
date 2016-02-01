
using System;
using System.Reflection.Emit;
using MinistryPlatform.Models;

namespace crds_angular.test.Models.Crossroads.Events
{
    public class EventHelpers
    {
        private const int _eventId = 1222;
        private const string _location = "Atrium";
        private const string _title = "Some Test title";
        private static readonly DateTime _startDate = new DateTime();
        private static readonly DateTime _endDate = new DateTime();

        public static MinistryPlatform.Models.Contact Contact()
        {
            return new Contact()
            {
                ContactId = 12,
                EmailAddress = "test@test.com"
            };
        }


        public static MinistryPlatform.Models.Event TranslationEvent()
        {
            return new MinistryPlatform.Models.Event
            {
                EventEndDate = _endDate,
                EventId = _eventId,
                Congregation = _location,
                EventStartDate = _startDate,
                EventTitle = _title,
                EventType = "",
                PrimaryContact = Contact()
            };
        }

        public static crds_angular.Models.Crossroads.Events.Event GatewayEvent()
        {
            return new crds_angular.Models.Crossroads.Events.Event()
            {
                EventId = _eventId,
                location = _location,
                meridian = _startDate.ToString("mm"),
                name = _title,
                StartDate = _startDate,
                EndDate = _endDate
            };
        }

    }
}
