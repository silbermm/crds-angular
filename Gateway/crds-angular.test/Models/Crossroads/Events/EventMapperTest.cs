using System;
using crds_angular.Models.Crossroads.Events;
using MinistryPlatform.Models;
using NUnit.Framework;


namespace crds_angular.test.Models.Crossroads.Events
{
    [TestFixture]
    public class EventMapperTest
    {
      
        [SetUp]
        public void SetUp()
        {
            AutoMapper.Mapper.Initialize(cfg => cfg.AddProfile<EventProfile>());
        }

        [Test]
        public void ShouldHaveAValidConfiguration()
        {
            AutoMapper.Mapper.AssertConfigurationIsValid<EventProfile>();
        }

        [Test]
        public void ShouldMapFromTranslationToGateway()
        {
            crds_angular.Models.Crossroads.Events.Event gatewayEvent = 
                AutoMapper.Mapper.Map<MinistryPlatform.Models.Event, crds_angular.Models.Crossroads.Events.Event>(
                    EventHelpers.TranslationEvent()
                );
            Assert.AreEqual(gatewayEvent.name, EventHelpers.GatewayEvent().name);
            Assert.AreEqual(gatewayEvent.location, EventHelpers.GatewayEvent().location);
            Assert.AreEqual(gatewayEvent.StartDate, EventHelpers.GatewayEvent().StartDate);
            Assert.AreEqual(gatewayEvent.EndDate, EventHelpers.GatewayEvent().EndDate);
            Assert.AreEqual(gatewayEvent.EventId, EventHelpers.GatewayEvent().EventId);
        }
    }
}
