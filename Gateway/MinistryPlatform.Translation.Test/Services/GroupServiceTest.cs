using System;
using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class GroupServiceTest
    {
        private GroupService fixture;
        private Mock<IMinistryPlatformService> ministryPlatformService;
        private Mock<IConfigurationWrapper> configWrapper;
        private Mock<IAuthenticationService> authService;
        private readonly int GroupsParticipantsPageId = 298;
        private readonly int GroupsParticipantsSubPage = 88;
        private readonly int GroupsPageId = 322;
        private readonly int GroupsEventsPageId = 302;
        private readonly int EventsGroupsPageId = 408;
        private readonly int GroupsSubGroupsPageId = 299;

        [SetUp]
        public void SetUp()
        {
            ministryPlatformService = new Mock<IMinistryPlatformService>();
            configWrapper = new Mock<IConfigurationWrapper>();
            authService = new Mock<IAuthenticationService>();
            fixture = new GroupService(ministryPlatformService.Object, configWrapper.Object, authService.Object);


            configWrapper.Setup(m => m.GetEnvironmentVarAsString("API_USER")).Returns("uid");
            configWrapper.Setup(m => m.GetEnvironmentVarAsString("API_PASSWORD")).Returns("pwd");
            authService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(AuthenticateResponse());
        }
        private Dictionary<string, object> AuthenticateResponse()
        {
            return new Dictionary<string, object>
            {
                {"token", "ABC"},
                {"exp", "123"}
            };
        }

        [Test]
        public void testAddParticipantToGroup()
        {
            var getGroupPageResponse = new Dictionary<string, object>
            {
                {"Group_ID", 456},
                {"Group_Name", "Test Group"},
                {"Target_Size", (short) 1},
                {"Group_Is_Full", false},
            };

            ministryPlatformService.Setup(mocked => mocked.GetRecordDict(GroupsPageId, 456, It.IsAny<string>(), false))
                .Returns(getGroupPageResponse);

            ministryPlatformService.Setup(
                mocked => mocked.GetSubPageRecords(GroupsParticipantsPageId, 456, It.IsAny<string>()))
                .Returns((List<Dictionary<string, object>>) null);

            ministryPlatformService.Setup(mocked => mocked.CreateSubRecord(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Dictionary<string, object>>(),
                It.IsAny<string>(),
                true)).Returns(987);

            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddDays(1);

            var expectedValues = new Dictionary<string, object>
            {
                {"Participant_ID", 123},
                {"Group_Role_ID", 789},
                {"Start_Date", startDate},
                {"End_Date", endDate},
                {"Employee_Role", true}
            };

            int groupParticipantId = fixture.addParticipantToGroup(123, 456, 789, startDate, endDate, true);

            ministryPlatformService.Verify(mocked => mocked.CreateSubRecord(
                GroupsParticipantsPageId,
                456,
                expectedValues,
                It.IsAny<string>(),
                true));

            Assert.AreEqual(987, groupParticipantId);
        }


        [Test]
        public void testGetAllEventsForGroupNoGroupFound()
        {
            ministryPlatformService.Setup(
                mocked => mocked.GetSubPageRecords(GroupsEventsPageId, 456, It.IsAny<string>()))
                .Returns((List<Dictionary<string, object>>) null);
            Assert.IsNull(fixture.getAllEventsForGroup(456));

            ministryPlatformService.VerifyAll();
        }

        [Test]
        public void testGetAllEventsForGroup()
        {
            List<Dictionary<string, object>> mpResult = new List<Dictionary<string, object>>();
            mpResult.Add(new Dictionary<string, object>()
            {
                {"dp_RecordID", 987},
            });
            mpResult.Add(new Dictionary<string, object>()
            {
                {"dp_RecordID", 654},
            });
            ministryPlatformService.Setup(
                mocked => mocked.GetSubPageRecords(GroupsEventsPageId, 456, It.IsAny<string>())).Returns(mpResult);

            ministryPlatformService.Setup(
                mocked => mocked.GetRecordDict(EventsGroupsPageId, 987, It.IsAny<string>(), false))
                .Returns(new Dictionary<string, object>()
                {
                    {"Event_ID", 789}
                });
            ministryPlatformService.Setup(
                mocked => mocked.GetRecordDict(EventsGroupsPageId, 654, It.IsAny<string>(), false))
                .Returns(new Dictionary<string, object>()
                {
                    {"Event_ID", 456}
                });

            var events = fixture.getAllEventsForGroup(456);
            ministryPlatformService.VerifyAll();

            Assert.IsNotNull(events);
            Assert.AreEqual(2, events.Count);
            Assert.AreEqual(789, events[0].EventId);
            Assert.AreEqual(456, events[1].EventId);
        }

        [Test]
        public void testGetGroupDetails()
        {
            var getGroupPageResponse = new Dictionary<string, object>
            {
                {"Group_ID", 456},
                {"Group_Name", "Test Group"},
                {"Target_Size", (short) 5},
                {"Group_Is_Full", true},
                {"Enable_Waiting_List", true},
                {"dp_RecordID", 522}
            };

            ministryPlatformService.Setup(mocked => mocked.GetRecordDict(GroupsPageId, 456, It.IsAny<string>(), false))
                .Returns(getGroupPageResponse);

            var groupParticipantsPageResponse = new List<Dictionary<string, object>>();
            for (int i = 42; i <= 46; i++)
            {
                groupParticipantsPageResponse.Add(new Dictionary<string, object>()
                {
                    {"Participant_ID", i},
                    {"Contact_ID", i + 10},
                    {"Group_Role_ID", 42},
                    {"Role_Title", "Boss"},
                    {"Last_Name", "Anderson"},
                    {"Nickname", "Neo"}
                });
            }
            ministryPlatformService.Setup(
                mocked => mocked.GetSubpageViewRecords(GroupsParticipantsSubPage, 456, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(groupParticipantsPageResponse);

            var GroupsSubGroupsPageResponse = new List<Dictionary<string, object>>();
            GroupsSubGroupsPageResponse.Add(new Dictionary<string, object>()
            {
                {"Group_Name", "Test Wait List"},
                {"Group_Type", "Wait List"},
                {"Group_Type_ID", "20"},
                {"dp_RecordID", 320}
            });

            ministryPlatformService.Setup(
                mocked => mocked.GetSubPageRecords(GroupsSubGroupsPageId, 456, It.IsAny<string>()))
                .Returns(GroupsSubGroupsPageResponse);

            var g = fixture.getGroupDetails(456);

            ministryPlatformService.VerifyAll();

            Assert.NotNull(g);
            Assert.AreEqual(456, g.GroupId);
            Assert.AreEqual(5, g.TargetSize);
            Assert.AreEqual(true, g.Full);
            Assert.AreEqual(true, g.WaitList);
            Assert.AreEqual("Test Group", g.Name);
            Assert.NotNull(g.Participants);
            Assert.AreEqual(5, g.Participants.Count);
            //Assert.AreEqual(42, g.Participants[0]);
            //Assert.AreEqual(43, g.Participants[1]);
            //Assert.AreEqual(44, g.Participants[2]);
            //Assert.AreEqual(45, g.Participants[3]);
            //Assert.AreEqual(46, g.Participants[4]);
            Assert.AreEqual(true, g.WaitList);
            Assert.AreEqual(320, g.WaitListGroupId);
        }

        [Test]
        public void testIsUserInGroup()
        {
            int participantId = 123;
            List<GroupParticipant> groupParticipants = new List<GroupParticipant>
            {
                new GroupParticipant
                {
                    ParticipantId = 1111
                },
                new GroupParticipant
                {
                    ParticipantId = 2222
                },
                new GroupParticipant
                {
                    ParticipantId = 123
                }
            };
            var result = fixture.checkIfUserInGroup(participantId, groupParticipants);
            Assert.AreEqual(result, true);
        }

        [Test]
        public void ParticipantIsGroupMember()
        {
            const int groupId = 1;
            const int participantId = 1000;

            var mockResponse = new List<Dictionary<string, object>> {new Dictionary<string, object>() {{"field1", 7}}};
            ministryPlatformService.Setup(
                m => m.GetPageViewRecords(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), 0)).Returns(mockResponse);

            var result = fixture.ParticipantQualifiedServerGroupMember(groupId, participantId);
            Assert.AreEqual(result, true);

            ministryPlatformService.VerifyAll();
        }

        [Test]
        public void ParticipantIsNotGroupMember()
        {
            const int groupId = 2;
            const int participantId = 2000;

            var mockResponse = new List<Dictionary<string, object>>();
            ministryPlatformService.Setup(
                m => m.GetPageViewRecords(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), 0)).Returns(mockResponse);

            var result = fixture.ParticipantQualifiedServerGroupMember(groupId, participantId);
            Assert.AreEqual(result, false);

            ministryPlatformService.VerifyAll();
        }

        [Test]
        public void GroupsByEventIdNoRecords()
        {
            //Arrange
            const int eventId = 123456;
            const int pageViewId = 999;
            var searchString = eventId + ",";

            var mpResponse = new List<Dictionary<string, object>>();

            configWrapper.Setup(m => m.GetConfigIntValue(It.IsAny<string>())).Returns(pageViewId);
           
            ministryPlatformService.Setup(m => m.GetPageViewRecords(pageViewId, It.IsAny<string>(), searchString, It.IsAny<string>(), It.IsAny<int>()))
                .Returns(mpResponse);

            //Act
            var groups = fixture.GetGroupsForEvent(eventId);

            //Assert
            Assert.IsNotNull(groups);
            Assert.AreEqual(0, groups.Count);
        }
        [Test]
        public void GroupsByEventId()
        {
            //Arrange
            const int eventId = 123456;
            const int pageViewId = 999;
            var searchString = eventId + ",";

            configWrapper.Setup(m => m.GetConfigIntValue(It.IsAny<string>())).Returns(pageViewId);
            ministryPlatformService.Setup(m => m.GetPageViewRecords(pageViewId, It.IsAny<string>(), searchString, It.IsAny<string>(), It.IsAny<int>()))
                .Returns(GroupsByEventId_MpResponse());

            //Act
            var groups = fixture.GetGroupsForEvent(eventId);

            //Assert
            Assert.IsNotNull(groups);
            Assert.AreEqual(2, groups.Count);

            Assert.AreEqual(1, groups[0].GroupId);
            Assert.AreEqual("group-one", groups[0].Name);

            Assert.AreEqual(2, groups[1].GroupId);
            Assert.AreEqual("group-two", groups[1].Name);
        }

        

        private List<Dictionary<string, object>> GroupsByEventId_MpResponse()
        {
            return new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>()
                {
                    {"Group_ID", 1},
                    {"Group_Name", "group-one"},
                    {"Role_Title", "group-one-role"},
                    {"Primary_Contact", "me@aol.com"}
                },
                new Dictionary<string, object>()
                {
                    {"Group_ID", 2},
                    {"Group_Name", "group-two"},
                    {"Role_Title", "group-two-role"},
                    {"Primary_Contact", "me@aol.com"}
                }
            };
        }
    }
}