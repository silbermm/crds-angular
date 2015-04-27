using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using MvcContrib.TestHelper;
using NSubstitute;
using NUnit.Framework;
using Rhino.Mocks;
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
        private Mock<IContactRelationshipService> contactRelationshipServiceMock;
        private Mock<IMinistryPlatformService> ministryPlatformService;
        private readonly int GroupsParticipantsPageId = 298;
        private readonly int GroupsPageId = 322;
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
           
        }

        [Test]
        public void testPostParticipantToGroupFails()
        {
            Exception ex = new Exception();
            int groupId = 456;
            List<int> particpantIdToAdd = new List<int> { 90210, 41001 };
           
            groupServiceMock.Setup(
                mocked =>
                    mocked.addParticipantToGroup(particpantIdToAdd[0], groupId, GroupRoleId, It.IsAny<DateTime>(),
                        null, false)).Throws(ex);
            IHttpActionResult result = fixture.Post(groupId, new PartID { partId = particpantIdToAdd });
            authenticationServiceMock.VerifyAll();
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

          var relationRecord = new GroupSignupRelationships
            {
                RelationshipId = 1,
                RelationshipMinAge = 00,
                RelationshipMaxAge = 100
            };


            groupServiceMock.Setup(mocked => mocked.GetGroupSignupRelations(g.GroupType)).Returns(new List<GroupSignupRelationships>() { relationRecord });
            groupServiceMock.Setup(mocked => mocked.getGroupDetails(groupId)).Returns(g);
            groupServiceMock.Setup(mocked => mocked.checkIfUserInGroup(It.IsAny<int>(), It.IsAny<List<GroupParticipant>>()));
            IHttpActionResult result = fixture.Get(groupId);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<GroupDTO>), result);
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

        [Test]
        public void testAddParticipantToGroupWhenGroupFull()
        {
            int groupId = 333;
            Group g = new Group();
            g.GroupId = 333;
            g.GroupType = 8;
            g.GroupRole = "Member";
            g.Name = "Test Me";
            g.TargetSize = 5;
            g.WaitList = false;
            g.Full = true;
            
            groupServiceMock.Setup(mocked => mocked.getGroupDetails(groupId)).Returns(g);
            List<int> particpantIdToAdd = new List<int> { 90210, 41001 };
            
            IHttpActionResult result = fixture.Post(333, new PartID { partId = particpantIdToAdd });
           
            Assert.IsNotNull(result, "Should have returned a HttpResponseMessage");
          
           
            }
        }

   }
