using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.Services;
using Moq;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class GroupServiceTest
    {
        private GroupService fixture;
        private Mock<IMinistryPlatformService> ministryPlatformService;
        private readonly int GroupsParticipantsPageId = 298;
        private readonly int GroupsPageId = 322;
        private readonly int GroupsEventsPageId = 302;
        private readonly int EventsGroupsPageId = 408;
        private readonly int GroupsSubGroupsPgeId = 299;

        [SetUp]
        public void SetUp()
        {
            ministryPlatformService = new Mock<IMinistryPlatformService>();
            fixture = new GroupService(ministryPlatformService.Object);
        }

        [Test]
        public void testAddParticipantToGroup()
        {
            var getGroupPageResponse = new Dictionary<string,object>
            {
                { "Group_ID", 456 },
                { "Group_Name", "Test Group" },
                { "Target_Size", (short)1 },
                { "Group_Is_Full", false },
            };

            ministryPlatformService.Setup(mocked => mocked.GetRecordDict(GroupsPageId, 456, It.IsAny<string>(), false)).Returns(getGroupPageResponse);

            ministryPlatformService.Setup(mocked => mocked.GetSubPageRecords(GroupsParticipantsPageId, 456, It.IsAny<string>())).Returns((List<Dictionary<string,object>>)null);

            ministryPlatformService.Setup(mocked => mocked.CreateSubRecord(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Dictionary<string, object>>(),
                It.IsAny<string>(), true)).Returns(987);

            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddDays(1);

            var expectedValues = new Dictionary<string, object>
            {
                { "Participant_ID", 123 },
                { "Group_Role_ID", 789 },
                { "Start_Date", startDate },
                { "End_Date", endDate},
                { "Employee_Role", true }
            };

            int groupParticipantId = fixture.addParticipantToGroup(123, 456, 789, startDate, endDate, true);

            ministryPlatformService.Verify(mocked => mocked.CreateSubRecord(
                GroupsParticipantsPageId, 456, expectedValues, It.IsAny<string>(), true));

            Assert.AreEqual(987, groupParticipantId);

        }

        [Test]
        public void testAddParticipantToGroupWhenGroupFull()
        {
            var getGroupPageResponse = new Dictionary<string, object>
            {
                { "Group_ID", 456 },
                { "Group_Name", "Test Group" },
                { "Target_Size", (short)1 },
                { "Group_Is_Full", true },
            };

            ministryPlatformService.Setup(mocked => mocked.GetRecordDict(GroupsPageId, 456, It.IsAny<string>(), false)).Returns(getGroupPageResponse);

            var groupParticipantsPageResponse = new List<Dictionary<string, object>>();
            groupParticipantsPageResponse.Add(new Dictionary<string, object>()
                {
                    { "Participant_ID", 42}
                });
            ministryPlatformService.Setup(mocked => mocked.GetSubPageRecords(GroupsParticipantsPageId, 456, It.IsAny<string>())).Returns(groupParticipantsPageResponse);

            try
            {
                fixture.addParticipantToGroup(123, 456, 789, DateTime.Now);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (GroupFullException e)
            {
                ministryPlatformService.Verify(mocked => mocked.CreateSubRecord(1, 1, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>(), true), Times.Never);
                Assert.NotNull(e.GroupDetails);
                Assert.AreEqual(456, e.GroupDetails.RecordId);
                Assert.AreEqual(1, e.GroupDetails.TargetSize);
                Assert.AreEqual(true, e.GroupDetails.Full);
                Assert.AreEqual("Test Group", e.GroupDetails.Name);
                Assert.NotNull(e.GroupDetails.Participants);
                Assert.AreEqual(1, e.GroupDetails.Participants.Count);
                Assert.AreEqual(42, e.GroupDetails.Participants[0]);
            }
        }

        [Test]
        public void testGetAllEventsForGroupNoGroupFound()
        {
            ministryPlatformService.Setup(mocked => mocked.GetSubPageRecords(GroupsEventsPageId, 456, It.IsAny<string>())).Returns((List<Dictionary<string, object>>)null);
            Assert.IsNull(fixture.getAllEventsForGroup(456));

            ministryPlatformService.VerifyAll();
        }

        [Test]
        public void testGetAllEventsForGroup()
        {
            List<Dictionary<string, object>> mpResult = new List<Dictionary<string, object>>();
            mpResult.Add(new Dictionary<string, object>() {
                {"dp_RecordID", 987},
            });
            mpResult.Add(new Dictionary<string, object>() {
                {"dp_RecordID", 654},
            });
            ministryPlatformService.Setup(mocked => mocked.GetSubPageRecords(GroupsEventsPageId, 456, It.IsAny<string>())).Returns(mpResult);

            ministryPlatformService.Setup(mocked => mocked.GetRecordDict(EventsGroupsPageId, 987, It.IsAny<string>(), false)).Returns(new Dictionary<string, object>()
            {
                { "Event_ID", 789 }
            });
            ministryPlatformService.Setup(mocked => mocked.GetRecordDict(EventsGroupsPageId, 654, It.IsAny<string>(), false)).Returns(new Dictionary<string, object>()
            {
                { "Event_ID", 456 }
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
                { "Group_ID", 456 },
                { "Group_Name", "Test Group" },
                { "Target_Size", (short)5 },
                { "Group_Is_Full", true },
                { "Enable_Waiting_List", true }
            };

            ministryPlatformService.Setup(mocked => mocked.GetRecordDict(GroupsPageId, 456, It.IsAny<string>(), false)).Returns(getGroupPageResponse);

            var groupParticipantsPageResponse = new List<Dictionary<string, object>>();
            for (int i = 42; i <= 46; i++) {
                groupParticipantsPageResponse.Add(new Dictionary<string, object>()
                {
                    { "Participant_ID", i},
                });
            }
            ministryPlatformService.Setup(mocked => mocked.GetSubPageRecords(GroupsParticipantsPageId, 456, It.IsAny<string>())).Returns(groupParticipantsPageResponse);

            var g = fixture.getGroupDetails(456);

            ministryPlatformService.VerifyAll();

            Assert.NotNull(g);
            Assert.AreEqual(456, g.RecordId);
            Assert.AreEqual(5, g.TargetSize);
            Assert.AreEqual(true, g.Full);
            Assert.AreEqual(true, g.WaitList);
            Assert.AreEqual("Test Group", g.Name);
            Assert.NotNull(g.Participants);
            Assert.AreEqual(5, g.Participants.Count);
            Assert.AreEqual(42, g.Participants[0]);
            Assert.AreEqual(43, g.Participants[1]);
            Assert.AreEqual(44, g.Participants[2]);
            Assert.AreEqual(45, g.Participants[3]);
            Assert.AreEqual(46, g.Participants[4]);

        }
    }
}
