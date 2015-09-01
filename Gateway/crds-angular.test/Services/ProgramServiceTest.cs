using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using crds_angular.App_Start;
using crds_angular.Models.Crossroads;
using crds_angular.Services;
using MinistryPlatform.Models;
using Moq;
using MPServices=MinistryPlatform.Translation.Services.Interfaces;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    public class ProgramServiceTest
    {
        private ProgramService _fixture;
        private Mock<MPServices.IProgramService> _mpProgramService;

        [SetUp]
        public void SetUp()
        {
            _mpProgramService = new Mock<MPServices.IProgramService>();
            _fixture = new ProgramService(_mpProgramService.Object);

            AutoMapperConfig.RegisterMappings();
        }

        [Test]
        public void TestGetOnlineGivingProgramsForProgramType()
        {
            var programs = new List<Program>
            {
                new Program
                {
                    CommunicationTemplateId = 1,
                    Name = "Program 1",
                    ProgramId = 2,
                    ProgramType = 3
                },
                new Program
                {
                    CommunicationTemplateId = 4,
                    Name = "Program 2",
                    ProgramId = 5,
                    ProgramType = 6
                },
            };

            _mpProgramService.Setup(mocked => mocked.GetOnlineGivingPrograms(3)).Returns(programs);

            var result = _fixture.GetOnlineGivingPrograms(3);
            _mpProgramService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            var expectedResult = programs.Select(Mapper.Map<ProgramDTO>).ToList();
            for(var i = 0; i < 2; i++)
            {
                Assert.AreEqual(expectedResult[i].ProgramType, result[i].ProgramType);
                Assert.AreEqual(expectedResult[i].CommunicationTemplateId, result[i].CommunicationTemplateId);
                Assert.AreEqual(expectedResult[i].Name, result[i].Name);
                Assert.AreEqual(expectedResult[i].ProgramId, result[i].ProgramId);
            }
        }

        [Test]
        public void TestGetOnlineGivingPrograms()
        {
            var programs = new List<Program>
            {
                new Program
                {
                    CommunicationTemplateId = 1,
                    Name = "Program 1",
                    ProgramId = 2,
                    ProgramType = 3
                },
                new Program
                {
                    CommunicationTemplateId = 4,
                    Name = "Program 2",
                    ProgramId = 5,
                    ProgramType = 6
                },
            };

            _mpProgramService.Setup(mocked => mocked.GetOnlineGivingPrograms(null)).Returns(programs);

            var result = _fixture.GetOnlineGivingPrograms();
            _mpProgramService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            var expectedResult = programs.Select(Mapper.Map<ProgramDTO>).ToList();
            for (var i = 0; i < 2; i++)
            {
                Assert.AreEqual(expectedResult[i].ProgramType, result[i].ProgramType);
                Assert.AreEqual(expectedResult[i].CommunicationTemplateId, result[i].CommunicationTemplateId);
                Assert.AreEqual(expectedResult[i].Name, result[i].Name);
                Assert.AreEqual(expectedResult[i].ProgramId, result[i].ProgramId);
            }
        }

        [Test]
        public void TestGetProgramByIdReturnsNull()
        {
            _mpProgramService.Setup(mocked => mocked.GetProgramById(3)).Returns((Program) null);

            var result = _fixture.GetProgramById(3);
            _mpProgramService.VerifyAll();
            Assert.IsNull(result);
        }

        [Test]
        public void TestGetProgramById()
        {
            var program = new Program
            {
                CommunicationTemplateId = 1,
                Name = "Program 1",
                ProgramId = 2,
                ProgramType = 3
            };

            _mpProgramService.Setup(mocked => mocked.GetProgramById(3)).Returns(program);

            var result = _fixture.GetProgramById(3);
            _mpProgramService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(program.ProgramType, result.ProgramType);
            Assert.AreEqual(program.CommunicationTemplateId, result.CommunicationTemplateId);
            Assert.AreEqual(program.Name, result.Name);
            Assert.AreEqual(program.ProgramId, result.ProgramId);
        }
    }
}
