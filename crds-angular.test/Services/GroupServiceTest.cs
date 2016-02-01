using System;
using System.Collections.Generic;
using AutoMapper;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Events;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using crds_angular.test.Models.Crossroads.Events;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Exceptions;
using Moq;
using NUnit.Framework;
using Event = MinistryPlatform.Models.Event;
using MPServices = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.test.Services
{
    public class GroupServiceTest
    {
        private GroupService fixture;
        private Mock<MPServices.IAuthenticationService> authenticationService;
        private Mock<MPServices.IGroupService> groupService;
        private Mock<MPServices.IEventService> eventService;
        private Mock<MPServices.IContactRelationshipService> contactRelationshipService;
        private Mock<IServeService> serveService;
        private Mock<IConfigurationWrapper> config;

        private readonly List<ParticipantSignup> mockParticipantSignup = new List<ParticipantSignup>
        {
            new ParticipantSignup()
            {
                particpantId = 999,
                childCareNeeded = false
            },
            new ParticipantSignup()
            {
                particpantId = 888,
                childCareNeeded = false
            }
        };

        private const int GROUP_ROLE_DEFAULT_ID = 123;

        [SetUp]
        public void SetUp()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<EventProfile>());

            authenticationService = new Mock<MPServices.IAuthenticationService>();
            groupService = new Mock<MPServices.IGroupService>(MockBehavior.Strict);
            eventService = new Mock<MPServices.IEventService>(MockBehavior.Strict);
            contactRelationshipService = new Mock<MPServices.IContactRelationshipService>();
            serveService = new Mock<IServeService>();
            config = new Mock<IConfigurationWrapper>();

            config.Setup(mocked => mocked.GetConfigIntValue("Group_Role_Default_ID")).Returns(GROUP_ROLE_DEFAULT_ID);

            fixture = new GroupService(groupService.Object, config.Object, eventService.Object, contactRelationshipService.Object, serveService.Object);
        }

        [Test]
        public void shouldThrowExceptionWhenAddingToGroupIfGetGroupDetailsFails()
        {
            Exception exception = new Exception("Oh no, Mr. Bill!");
            groupService.Setup(mocked => mocked.getGroupDetails(456)).Throws(exception);

            try
            {
                fixture.addParticipantsToGroup(456, mockParticipantSignup);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOf(typeof (ApplicationException), e);
                Assert.AreSame(exception, e.InnerException);
            }

            groupService.VerifyAll();
        }

        [Test]
        public void shouldThrowGroupIsFullExceptionWhenGroupFullIndicatorIsSet()
        {
            var g = new Group
            {
                TargetSize = 3,
                Full = true,
                Participants = new List<GroupParticipant>
                {
                    new GroupParticipant()
                }
            };
            groupService.Setup(mocked => mocked.getGroupDetails(456)).Returns(g);

            try
            {
                fixture.addParticipantsToGroup(456, mockParticipantSignup);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOf(typeof (GroupFullException), e);
            }

            groupService.VerifyAll();
        }

        [Test]
        public void shouldThrowGroupIsFullExceptionWhenNotEnoughSpaceRemaining()
        {
            var g = new Group
            {
                TargetSize = 2,
                Full = false,
                Participants = new List<GroupParticipant>
                {
                    new GroupParticipant()
                }
            };
            groupService.Setup(mocked => mocked.getGroupDetails(456)).Returns(g);

            try
            {
                fixture.addParticipantsToGroup(456, mockParticipantSignup);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOf(typeof (GroupFullException), e);
            }

            groupService.VerifyAll();
        }

        [Test]
        public void shouldAddParticipantsToGroupAndEvents()
        {
            var g = new Group
            {
                TargetSize = 0,
                Full = false,
                Participants = new List<GroupParticipant>()
            };
            groupService.Setup(mocked => mocked.getGroupDetails(456)).Returns(g);

            groupService.Setup(mocked => mocked.addParticipantToGroup(999, 456, GROUP_ROLE_DEFAULT_ID, false, It.IsAny<DateTime>(), null, false)).Returns(999456);
            groupService.Setup(mocked => mocked.addParticipantToGroup(888, 456, GROUP_ROLE_DEFAULT_ID, false, It.IsAny<DateTime>(), null, false)).Returns(888456);
            groupService.Setup(mocked => mocked.SendCommunityGroupConfirmationEmail(It.IsAny<int>(), 456, true, false));

            var events = new List<Event>
            {
                new Event {EventId = 777},
                new Event {EventId = 555},
                new Event {EventId = 444}
            };
            groupService.Setup(mocked => mocked.getAllEventsForGroup(456)).Returns(events);

            eventService.Setup(mocked => mocked.RegisterParticipantForEvent(999, 777, 456, 999456)).Returns(999777);
            eventService.Setup(mocked => mocked.RegisterParticipantForEvent(999, 555, 456, 999456)).Returns(999555);
            eventService.Setup(mocked => mocked.RegisterParticipantForEvent(999, 444, 456, 999456)).Returns(999444);

            eventService.Setup(mocked => mocked.RegisterParticipantForEvent(888, 777, 456, 888456)).Returns(888777);
            eventService.Setup(mocked => mocked.RegisterParticipantForEvent(888, 555, 456, 888456)).Returns(888555);
            eventService.Setup(mocked => mocked.RegisterParticipantForEvent(888, 444, 456, 888456)).Returns(888444);

            fixture.addParticipantsToGroup(456, mockParticipantSignup);

            groupService.VerifyAll();
            eventService.VerifyAll();
        }

        [Test]
        public void testGetGroupDetails()
        {
            var g = new Group
            {
                TargetSize = 0,
                Full = true,
                Participants = new List<GroupParticipant>(),
                GroupType = 90210,
                WaitList = true,
                WaitListGroupId = 10101,
                GroupId = 98765
            };

            var eventList = new List<Event>()
            {
                EventHelpers.TranslationEvent()
            };

            groupService.Setup(mocked => mocked.getGroupDetails(456)).Returns(g);

            groupService.Setup(mocked => mocked.getAllEventsForGroup(456)).Returns(eventList);

            var relations = new List<GroupSignupRelationships>
            {
                new GroupSignupRelationships {RelationshipId = 111}
            };
            groupService.Setup(mocked => mocked.GetGroupSignupRelations(90210)).Returns(relations);

            var contactRelations = new List<ContactRelationship>
            {
                new ContactRelationship
                {
                    Contact_Id = 333,
                    Relationship_Id = 111,
                    Participant_Id = 222
                }
            };
            contactRelationshipService.Setup(mocked => mocked.GetMyCurrentRelationships(777, "auth token")).Returns(contactRelations);

            var participant = new Participant
            {
                ParticipantId = 555,
            };
            groupService.Setup(mocked => mocked.checkIfUserInGroup(555, It.IsAny<List<GroupParticipant>>())).Returns(false);
            groupService.Setup(mocked => mocked.checkIfUserInGroup(222, It.IsAny<List<GroupParticipant>>())).Returns(false);

            var response = fixture.getGroupDetails(456, 777, participant, "auth token");

            groupService.VerifyAll();
            contactRelationshipService.VerifyAll();

            Assert.IsNotNull(response);
            Assert.IsTrue(response.GroupFullInd);
            Assert.AreEqual(g.GroupId, response.GroupId);
            Assert.AreEqual(2, response.SignUpFamilyMembers.Count);
            Assert.AreEqual(g.WaitListGroupId, response.WaitListGroupId);
            Assert.AreEqual(g.WaitList, response.WaitListInd);
        }
    }
}