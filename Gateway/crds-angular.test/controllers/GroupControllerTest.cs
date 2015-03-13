using crds_angular.Controllers.API;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;

namespace crds_angular.test.controllers
{
    [TestFixture]
    public class GroupControllerTest
    {
        private GroupController fixture;
        private Mock<IGroupService> groupServiceMock;
        private Mock<IEventService> eventServiceMock;
        private string authType;
        private string authToken;
        private readonly string groupRoleId = ConfigurationManager.AppSettings["Group_Role_Default_ID"];

        [SetUp]
        public void SetUp()
        {
            groupServiceMock = new Mock<IGroupService>();
            eventServiceMock = new Mock<IEventService>();

            fixture = new GroupController(groupServiceMock.Object, eventServiceMock.Object);

            authType = "auth_type";
            authToken = "auth_token";
            fixture.Request = new HttpRequestMessage();
            fixture.Request.Headers.Authorization = new AuthenticationHeaderValue(authType, authToken);
            fixture.RequestContext = new HttpRequestContext();
        }

    //    [Test]
    //    public void testCallGroupServiceIsSuccessful()
    //    {
    //        //groupServiceMock.Setup(mocked => mocked.addParticipantToGroup(authType + " " + authToken, "3", groupRoleId, It.IsAny<DateTime>(), null, false)).Returns(999);
    //        //eventServiceMock.Setup(mocked => mocked.getUpcomingEventsForCommunityGroup(3));

    //        IHttpActionResult result = fixture.Post("3");
    //        //groupServiceMock.Verify(mocked => mocked.addParticipantToGroup(authType + " " + authToken, "3", groupRoleId, It.IsAny<DateTime>(), null, false));

    //        Assert.IsNotNull(result);
    //        Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<Dictionary<string, object>>), result);
    //        OkNegotiatedContentResult<Dictionary<string, object>> okResult = (OkNegotiatedContentResult<Dictionary<string, object>>)result;
    //        Assert.IsNotNull(okResult.Content);
    //        Assert.AreEqual(1, okResult.Content.Count);
    //        Assert.AreEqual(999, okResult.Content["groupParticipantId"]);
    //    }

    //    [Test]
    //    public void testCallGroupServiceFails()
    //    {
    //        Exception ex = new Exception();
    //        //groupServiceMock.Setup(mocked => mocked.addParticipantToGroup(authType + " " + authToken, "3", groupRoleId, It.IsAny<DateTime>(), null, false)).Throws(ex);

    //        IHttpActionResult result = fixture.Post("3");
    //        //groupServiceMock.Verify(mocked => mocked.addParticipantToGroup(authType + " " + authToken, "3", groupRoleId, It.IsAny<DateTime>(), null, false));

    //        Assert.IsNotNull(result);
    //        Assert.IsInstanceOf(typeof(BadRequestResult), result);
    //    }

    //    [Test]
    //    public void testCallGroupServiceFailsUnauthorized()
    //    {
    //        fixture.Request.Headers.Authorization = null;
    //        IHttpActionResult result = fixture.Post("3");
    //        Assert.IsNotNull(result);
    //        Assert.IsInstanceOf(typeof(UnauthorizedResult), result);
    //        groupServiceMock.VerifyAll();
    //    }
    //}
}