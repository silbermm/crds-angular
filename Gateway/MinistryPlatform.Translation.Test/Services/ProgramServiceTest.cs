using System;
using System.Collections.Generic;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class ProgramServiceTest
    {
        private ProgramService fixture;
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            fixture = new ProgramService(_ministryPlatformService.Object);
        }

        [Test]
        public void testGetOnlineGivingPrograms()
        {
            var GetPageViewRecordsResponse = new List<Dictionary<string, object>>();
            GetPageViewRecordsResponse.Add(new Dictionary<string, object>()
            {
                {"Program_Name", "Test Program Name"},
                {"Program_ID", 20},
            });

            GetPageViewRecordsResponse.Add(new Dictionary<string, object>()
            {
                {"Program_Name", "Test Fund Name"},
                {"Program_ID", 22},
            });

    
            const int OnlineGivingProgramsPageViewId = 1038;

            _ministryPlatformService.Setup(
            mocked => mocked.GetPageViewRecords(OnlineGivingProgramsPageViewId, It.IsAny<string>(), ",,,1", "", 0)).Returns(GetPageViewRecordsResponse);

            var programs = fixture.GetOnlineGivingPrograms(1);

            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(programs);
            
            Assert.AreEqual("Test Program Name", programs[0].Name);
            Assert.AreEqual(22, programs[1].ProgramId);
                
        }
        [Test]
        public void testGetOnlineGivingProgramsNullResponse()
        {
            const int OnlineGivingProgramsPageViewId = 1038;

            _ministryPlatformService.Setup(
            mocked => mocked.GetPageViewRecords(OnlineGivingProgramsPageViewId, It.IsAny<string>(), ",,,1", "", 0)).Returns((List<Dictionary<string,object>>)null);

            var programs = fixture.GetOnlineGivingPrograms(1);

            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(programs);
            Assert.AreEqual(0, programs.Count);
            
        }

        [Test]
        public void testGetProgram()
        {

           var GetRecordResponse = new Dictionary<string, object>()
            {
                {"Communication_ID", "1234"},
                {"Program_ID", 3},
                {"Program_Name","TEst Name" }
            };

            const int ProgramsPageId = 375;
            const int ProgramId = 3;

            _ministryPlatformService.Setup(
                mocked => mocked.GetRecordDict(ProgramsPageId, ProgramId, It.IsAny<string>(), false)).Returns(GetRecordResponse);

            var program = fixture.GetProgramById(ProgramId);

            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(program);
            Assert.AreEqual(1234, program.CommunicationTemplateId);
        }
    }
}
