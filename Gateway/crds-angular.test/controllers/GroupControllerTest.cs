using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Models;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NSubstitute;
using NUnit.Framework;

namespace crds_angular.test.controllers
{
    [TestFixture]
    public class GroupControllerTest
    {
        private GroupController fixture;
        private Mock<IGroupService> groupServiceMock;
        private Mock<IEventService> eventServiceMock;
        private Mock<IAuthenticationService> authenticationServiceMock;
        private Mock<IContactRelationshipService> contactRelationshipServiceMock; 
        private string authType;
        private string authToken;
        private const int GroupRoleId = 16;

        [SetUp]
        public void SetUp()
        {
            groupServiceMock = new Mock<IGroupService>();
            eventServiceMock = new Mock<IEventService>();
            authenticationServiceMock = new Mock<IAuthenticationService>();
            contactRelationshipServiceMock = new Mock<IContactRelationshipService>();
            fixture = new GroupController(groupServiceMock.Object, eventServiceMock.Object,
                authenticationServiceMock.Object, contactRelationshipServiceMock.Object);

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

            List<int> particpantIdToAdd = new List<int> {90210, 41001};

            List<Event> events = new List<Event>();
            Event e1 = new Event();
            e1.EventId = 101;
            Event e2 = new Event();
            e2.EventId = 202;
            events.Add(e1);
            events.Add(e2);
            groupServiceMock.Setup(mocked => mocked.getGroupDetails(groupId)).Returns(new Group());
            groupServiceMock.Setup(mocked => mocked.getAllEventsForGroup(groupId)).Returns(events);
            groupServiceMock.Setup(mocked => mocked.addParticipantToGroup(particpantIdToAdd[0], groupId, It.IsAny<int>(), It.IsAny<DateTime>(), null, It.IsAny<Boolean>())).Returns(123);
            groupServiceMock.Setup(mocked => mocked.addParticipantToGroup(particpantIdToAdd[1], groupId, It.IsAny<int>(), It.IsAny<DateTime>(), null, It.IsAny<Boolean>())).Returns(456);

            IHttpActionResult result = fixture.Post(groupId, new PartID { partId = particpantIdToAdd });

            authenticationServiceMock.VerifyAll();
            groupServiceMock.VerifyAll();
            eventServiceMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<List<Dictionary<string, object>>>), result);
            var okResult = (OkNegotiatedContentResult<List<Dictionary<string, object>>>)result;
            Assert.IsNotNull(okResult.Content);
            Assert.AreEqual(2, okResult.Content.Count);
            
            //Assert.AreEqual(groupParticipantId, okResult.Content["groupParticipantId"]);
            //Assert.AreEqual(p, okResult.Content["participantId"]);
            
        }

        [Test]
        [Ignore("how do I make this test fail - group needs to be full")]
        public void testPostParticipantToGroupFails()
        {
            Exception ex = new Exception();
            int groupId = 456;
            
            List<int> particpantIdToAdd = new List<int> { 90201 };
            IHttpActionResult result = fixture.Post(groupId, new PartID());
            authenticationServiceMock.VerifyAll();
            groupServiceMock.VerifyAll();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof (BadRequestResult), result);
            
        }

        [Test]
        public void testGetGroupDetails()
        {
            int groupId = 333;
            Group g = new Group();
            g.GroupId = 333;
            g.GroupType = 8;
            g.GroupRole = "Member";
            g.Name = "Test Me";
            g.GroupId = 123456;
            g.TargetSize = 5;
            g.WaitList = true;
            g.WaitListGroupId = 888;

            Participant participant = new Participant();
            participant.ParticipantId = 90210;
            authenticationServiceMock.Setup(
                mocked => mocked.GetParticipantRecord(fixture.Request.Headers.Authorization.ToString()))
                .Returns(participant);

            ContactRelationship signupRelations = new ContactRelationship();
            signupRelations.Birth_date = new DateTime(1968,01, 01);
            signupRelations.Contact_Id = 111;
            signupRelations.Email_Address = " ";
            signupRelations.Last_Name = "Tester";
            signupRelations.Participant_Id = participant.ParticipantId;
            signupRelations.Preferred_Name = "Mister";
            signupRelations.Relationship_Id = 1;

            var relationRecord = new GroupSignupRelationships
            {
                RelationshipId = 1,
                RelationshipMinAge = "00",
                RelationshipMaxAge = "100"
            };


            groupServiceMock.Setup(mocked => mocked.GetGroupSignupRelations(g.GroupType)).Returns(new List<GroupSignupRelationships>() { relationRecord });

            //var currRelationships = contactRelationshipService.GetMyCurrentRelationships(contactId, token);



            groupServiceMock.Setup(mocked => mocked.getGroupDetails(groupId)).Returns(g);
            groupServiceMock.Setup(mocked => mocked.checkIfUserInGroup(It.IsAny<int>(), It.IsAny<List<int>>()));
            IHttpActionResult result = fixture.Get(groupId);
            Assert.IsNotNull(result);
            //Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<Dictionary<string, object>>), result);
            //OkNegotiatedContentResult<Dictionary<string, object>> okResult = (OkNegotiatedContentResult<Dictionary<string, object>>)result;
            groupServiceMock.VerifyAll();

            Assert.NotNull(g);
            Assert.NotNull(result);
        }

        [Test]
        public void testCallGroupServiceFailsUnauthorized()
        {
            fixture.Request.Headers.Authorization = null;
            IHttpActionResult result = fixture.Post(3 , new PartID());
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof (UnauthorizedResult), result);
            groupServiceMock.VerifyAll();
        }
    }
}