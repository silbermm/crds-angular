using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Json;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services;
using Moq;
using NUnit.Framework;
using Event = MinistryPlatform.Models.Event;

namespace crds_angular.test.controllers
{
    [TestFixture]
    public class GroupControllerTest
    {
        private GroupController fixture;
        private Mock<IGroupService> groupServiceMock;
        private Mock<IEventService> eventServiceMock;
        private Mock<IAuthenticationService> authenticationServiceMock;
        private string authType;
        private string authToken;
        private const int GroupRoleId = 16;

        [SetUp]
        public void SetUp()
        {
            groupServiceMock = new Mock<IGroupService>();
            eventServiceMock = new Mock<IEventService>();
            authenticationServiceMock = new Mock<IAuthenticationService>();

            fixture = new GroupController(groupServiceMock.Object, eventServiceMock.Object, authenticationServiceMock.Object);

            authType = "auth_type";
            authToken = "auth_token";
            fixture.Request = new HttpRequestMessage();
            fixture.Request.Headers.Authorization = new AuthenticationHeaderValue(authType, authToken);
            fixture.RequestContext = new HttpRequestContext();
        }

        [Test]
        public void testPostParticipantToGroupIsSuccessful()
        {
            int groupId = 456;
            int groupParticipantId = 444;
            Participant participant = new Participant();
            participant.ParticipantId = 90210;
            authenticationServiceMock.Setup(mocked => mocked.GetParticipantRecord(fixture.Request.Headers.Authorization.ToString())).Returns(participant);
            groupServiceMock.Setup(mocked => mocked.addParticipantToGroup(participant.ParticipantId, groupId, GroupRoleId, It.IsAny<DateTime>(), null, false)).Returns(groupParticipantId);

            List<Event> events = new List<Event>();
            Event e1 = new Event();
            e1.EventId = 101;
            Event e2 = new Event();
            e2.EventId = 202;
            events.Add(e1);
            events.Add(e2);

            groupServiceMock.Setup(mocked => mocked.getAllEventsForGroup(groupId)).Returns(events);

            eventServiceMock.Setup(mocked => mocked.registerParticipantForEvent(participant.ParticipantId, e1.EventId)).Returns(1010);
            eventServiceMock.Setup(mocked => mocked.registerParticipantForEvent(participant.ParticipantId, e2.EventId)).Returns(2020);

            IHttpActionResult result = fixture.Post(groupId+"");
            authenticationServiceMock.VerifyAll();
            groupServiceMock.VerifyAll();
            eventServiceMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<Dictionary<string, object>>), result);
            OkNegotiatedContentResult<Dictionary<string, object>> okResult = (OkNegotiatedContentResult<Dictionary<string, object>>)result;
            Assert.IsNotNull(okResult.Content);
            Assert.AreEqual(4, okResult.Content.Count);
            Assert.AreEqual(groupParticipantId, okResult.Content["groupParticipantId"]);
            Assert.AreEqual(participant.ParticipantId, okResult.Content["participantId"]);
        }

        [Test]
        public void testPostParticipantToGroupFails()
        {
            Exception ex = new Exception();
            int groupId = 456;
            Participant participant = new Participant();
            participant.ParticipantId = 90210;
            authenticationServiceMock.Setup(mocked => mocked.GetParticipantRecord(fixture.Request.Headers.Authorization.ToString())).Returns(participant);
            groupServiceMock.Setup(mocked => mocked.addParticipantToGroup(participant.ParticipantId, groupId, GroupRoleId, It.IsAny<DateTime>(), null, false)).Throws(ex);

            IHttpActionResult result = fixture.Post(groupId+"");
            authenticationServiceMock.VerifyAll();
            groupServiceMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(BadRequestResult), result);
        }

        [Test]
        public void testGetGetGroupDetails()
        {
            int groupId = 333;
            Group g = new Group();
            g.GroupId = 333;
            g.GroupRole = "Member";
            g.Name = "Test Me";
            g.RecordId = 123456;
            g.TargetSize = 5;
            g.WaitList = true;
            g.WaitListGroupId = 888;

         
            groupServiceMock.Setup(mocked => mocked.getGroupDetails(groupId)).Returns(g);
            IHttpActionResult result = fixture.Get(groupId);

            Assert.NotNull(g); 
            Assert.NotNull(result);
         }

        [Test]
        public void testCallGroupServiceFailsUnauthorized()
        {
            fixture.Request.Headers.Authorization = null;
            IHttpActionResult result = fixture.Post("3");
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(UnauthorizedResult), result);
            groupServiceMock.VerifyAll();
        }
    }
}