using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using crds_angular.App_Start;
using NUnit.Framework;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads;
using Moq;
using crds_angular.Services.Interfaces;

namespace crds_angular.test.controllers
{
    [TestFixture]
    public class ProgramControllerTest

    {
        private ProgramController _fixture;
        private Mock<IProgramService> _programServiceMock;


        [SetUp]
        public void SetUp()
        {
            _programServiceMock = new Mock<IProgramService>();
            _fixture = new ProgramController(_programServiceMock.Object)
            {
                Request = new HttpRequestMessage(),
                RequestContext = new HttpRequestContext()
            };

            AutoMapperConfig.RegisterMappings();
        }

        [Test]
        public void TestGetPrograms()
        {
            int programType = 1;
            var programList = new List<ProgramDTO>();
            ProgramDTO program = new ProgramDTO
            {
                Name = "Test Fund",
                ProgramId = 1
            };
            programList.Add(program);

            _programServiceMock.Setup(mocked => mocked.GetOnlineGivingPrograms(programType)).Returns(programList);

            var httpResult = _fixture.Get(null, programType);
            var result = (OkNegotiatedContentResult<List<ProgramDTO>>) httpResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<List<ProgramDTO>>), result);
            _programServiceMock.VerifyAll();

            Assert.AreEqual(result.Content[0].Name, program.Name);
            Assert.AreEqual(result.Content[0].ProgramId, program.ProgramId);
        }
    }
}
