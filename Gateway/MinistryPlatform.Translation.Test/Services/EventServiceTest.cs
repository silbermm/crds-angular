using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Translation.Services;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class EventServiceTest
    {
        private EventServiceTester fixture;
        private const int EventParticipantId = 12345;
        private readonly int EventParticipantStatusDefaultID = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Event_Participant_Status_Default_ID"));

        [SetUp]
        public void SetUp()
        {
            fixture = new EventServiceTester();
            ((EventServiceTester)fixture).eventParticipantId = EventParticipantId;
        }

        [Test]
        public void testRegisterParticipantForEvent()
        {
            fixture.registerParticipantForEvent(123, 456);

            var values = ((EventServiceTester)fixture).receivedValues;
            Assert.IsNotNull(values);
            Assert.AreEqual(3, values.Count);
            Assert.AreEqual(123, values["Participant_ID"]);
            Assert.AreEqual(456, values["Event_ID"]);
            Assert.AreEqual(EventParticipantStatusDefaultID, values["Participation_Status_ID"]);
        }
    }

    /**
     * This class exists solely for the purpose of "intercepting" the call to createEventParticipant() in order to unit test.
     */
    class EventServiceTester : EventService
    {
        public int receivedEventId { get; set; }
        public Dictionary<string, object> receivedValues { get; set; }
        public int eventParticipantId { get; set; }

        protected override int createEventParticipant(int eventId, Dictionary<string, object> values)
        {
            receivedEventId = eventId;
            receivedValues = values;
            return (eventParticipantId);
        }
    }
}
