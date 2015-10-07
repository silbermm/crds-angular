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
using NUnit.Framework;
using Event = MinistryPlatform.Models.Event;

namespace crds_angular.test.controllers
{
    [TestFixture]
    public class GroupControllerTest
    {
        private GroupController fixture;
        private Mock<crds_angular.Services.Interfaces.IGroupService> groupServiceMock;
        private Mock<IAuthenticationService> authenticationServiceMock;
        private Mock<IParticipantService> participantServiceMock;
        private string authType;
        private string authToken;
        private const int GroupRoleId = 16;

        [SetUp]
        public void SetUp()
        {
            groupServiceMock = new Mock<crds_angular.Services.Interfaces.IGroupService>();
            authenticationServiceMock = new Mock<IAuthenticationService>();
            participantServiceMock = new Mock<IParticipantService>();

            fixture = new GroupController(groupServiceMock.Object, authenticationServiceMock.Object,participantServiceMock.Object);

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

            List<int> particpantIdToAdd = new List<int> { 90210, 41001 };

            List<Event> events = new List<Event>();
            Event e1 = new Event();
            e1.EventId = 101;
            Event e2 = new Event();
            e2.EventId = 202;
            events.Add(e1);
            events.Add(e2);

            var participantsAdded = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object> {
                    {"123", "456"}
                },
                new Dictionary<string, object> {
                    {"abc", "def"}
                },
            };
            groupServiceMock.Setup(mocked => mocked.addParticipantsToGroup(groupId, particpantIdToAdd));

            IHttpActionResult result = fixture.Post(groupId, new PartID { partId = particpantIdToAdd });

            authenticationServiceMock.VerifyAll();
            groupServiceMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkResult), result);
        }

        [Test]
        public void testPostParticipantToGroupFails()
        {
            Exception ex = new Exception();
            int groupId = 456;
            List<int> particpantIdToAdd = new List<int> { 90210, 41001 };

            groupServiceMock.Setup(mocked => mocked.addParticipantsToGroup(groupId, particpantIdToAdd)).Throws(ex);

            IHttpActionResult result = fixture.Post(groupId, new PartID { partId = particpantIdToAdd });
            authenticationServiceMock.VerifyAll();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(BadRequestResult), result);
        }

        [Test]
        public void testGetGroupDetails()
        {
            int groupId = 333;
            int contactId = 777;

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
            participantServiceMock.Setup(
                mocked => mocked.GetParticipantRecord(fixture.Request.Headers.Authorization.ToString()))
                .Returns(participant);

            authenticationServiceMock.Setup(mocked => mocked.GetContactId(fixture.Request.Headers.Authorization.ToString())).Returns(contactId);

            var relationRecord = new GroupSignupRelationships
              {
                  RelationshipId = 1,
                  RelationshipMinAge = 00,
                  RelationshipMaxAge = 100
              };

            var groupDto = new GroupDTO
            {
            };

            groupServiceMock.Setup(mocked => mocked.getGroupDetails(groupId, contactId, participant, fixture.Request.Headers.Authorization.ToString())).Returns(groupDto);


            IHttpActionResult result = fixture.Get(groupId);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<GroupDTO>), result);
            groupServiceMock.VerifyAll();

            var groupDtoResponse = ((OkNegotiatedContentResult<GroupDTO>)result).Content;

            Assert.NotNull(result);
            Assert.AreSame(groupDto, groupDtoResponse);
        }

        [Test]
        public void testCallGroupServiceFailsUnauthorized()
        {
            fixture.Request.Headers.Authorization = null;
            IHttpActionResult result = fixture.Post(3, new PartID());
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(UnauthorizedResult), result);
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

            List<int> particpantIdToAdd = new List<int> { 90210, 41001 };
            var groupFull = new GroupFullException(g);
            groupServiceMock.Setup(mocked => mocked.addParticipantsToGroup(groupId, particpantIdToAdd)).Throws(groupFull);

            try
            {
                fixture.Post(333, new PartID { partId = particpantIdToAdd });
                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(HttpResponseException), e.GetType());
                var ex = (HttpResponseException)e;
                Assert.IsNotNull(ex.Response);
                Assert.AreEqual((HttpStatusCode)422, ex.Response.StatusCode);
            }
        }
      }
   }
