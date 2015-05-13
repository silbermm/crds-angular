using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using NUnit.Framework;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using MinistryPlatform.Models;

namespace crds_angular.test.controllers
{
    [TestFixture]
    public class ProgramControllerTest

    {
        private ProgramController fixture;
        private Mock<IProgramService> programServiceMock;


        [SetUp]
        public void SetUp()
        {
            programServiceMock = new Mock<IProgramService>();
            fixture = new ProgramController(programServiceMock.Object);
            fixture.Request = new HttpRequestMessage();
            fixture.RequestContext = new HttpRequestContext();
        }

        [Test]
        public void testGetPrograms()
        {
            int programType = 1;
            var programList = new List<Program>();
            Program program = new Program();
            program.Name = "Test Fund";
            program.ProgramId = 1;
            programList.Add(program);

            programServiceMock.Setup(mocked => mocked.GetOnlineGivingPrograms(programType)).Returns(programList);

            IHttpActionResult httpResult = fixture.Get(programType);
            OkNegotiatedContentResult<List<ProgramDTO>> result = (OkNegotiatedContentResult<List<ProgramDTO>>) httpResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<List<ProgramDTO>>), result);
            programServiceMock.VerifyAll();

            Assert.AreEqual(result.Content[0].Name, program.Name);
            Assert.AreEqual(result.Content[0].ProgramId, program.ProgramId);
        }
    }
}
