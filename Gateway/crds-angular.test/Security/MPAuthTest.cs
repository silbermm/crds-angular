using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using crds_angular.Security;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using System.Web.Http;
using System.Web.Http.Results;


namespace crds_angular.test.Security
{
    class MPAuthTest
    {
        private MPAuthTester fixture;

        private Mock<Func<string, IHttpActionResult>> actionWhenAuthorized;
        private Mock<Func<IHttpActionResult>> actionWhenNotAuthorized;
        private OkResult okResult;

        private string authType;
        private string authToken;

        [SetUp]
        public void SetUp()
        {
            fixture = new MPAuthTester();

            actionWhenAuthorized = new Mock<Func<string, IHttpActionResult>>(MockBehavior.Strict);
            actionWhenNotAuthorized = new Mock<Func<IHttpActionResult>>(MockBehavior.Strict);
            okResult = new OkResult(fixture);

            authType = "auth_type";
            authToken = "auth_token";
            fixture.Request = new HttpRequestMessage();
            fixture.Request.Headers.Authorization = new AuthenticationHeaderValue(authType, authToken);
            fixture.RequestContext = new HttpRequestContext();
        }

        [Test]
        public void testAuthorizedNotAuthorized()
        {
            fixture.Request = new HttpRequestMessage();
            fixture.Request.Headers.Authorization = null;

            var result = fixture.AuthTest(actionWhenAuthorized.Object);
            actionWhenAuthorized.VerifyAll();

            Assert.NotNull(result);
            Assert.IsInstanceOf(typeof(UnauthorizedResult), result);
        }

        [Test]
        public void testAuthorizedWhenAuthorized()
        {
            actionWhenAuthorized.Setup(mocked => mocked(authType + " " + authToken)).Returns(okResult);
            var result = fixture.AuthTest(actionWhenAuthorized.Object);
            actionWhenAuthorized.VerifyAll();

            Assert.NotNull(result);
            Assert.IsInstanceOf(typeof(OkResult), result);
            Assert.AreSame(okResult, result);
        }

        [Test]
        public void testOptionalAuthorizedNotAuthorized()
        {
            fixture.Request = new HttpRequestMessage();
            fixture.Request.Headers.Authorization = null;

            actionWhenNotAuthorized.Setup(mocked => mocked()).Returns(okResult);

            var result = fixture.AuthTest(actionWhenAuthorized.Object, actionWhenNotAuthorized.Object);
            actionWhenNotAuthorized.VerifyAll();
            actionWhenAuthorized.VerifyAll();

            Assert.NotNull(result);
            Assert.IsInstanceOf(typeof(OkResult), result);
            Assert.AreSame(okResult, result);
        }

        [Test]
        public void testOptionalAuthorizedWhenAuthorized()
        {
            actionWhenAuthorized.Setup(mocked => mocked(authType + " " + authToken)).Returns(okResult);
            var result = fixture.AuthTest(actionWhenAuthorized.Object, actionWhenNotAuthorized.Object);
            actionWhenAuthorized.VerifyAll();
            actionWhenNotAuthorized.VerifyAll();

            Assert.NotNull(result);
            Assert.IsInstanceOf(typeof(OkResult), result);
            Assert.AreSame(okResult, result);
        }

        private class MPAuthTester : MPAuth
        {
            public IHttpActionResult AuthTest(Func<string, IHttpActionResult> doIt)
            {
                return(base.Authorized(doIt));
            }

            public IHttpActionResult AuthTest(Func<string, IHttpActionResult> actionWhenAuthorized, Func<IHttpActionResult> actionWhenNotAuthorized)
            {
                return (base.Authorized(actionWhenAuthorized, actionWhenNotAuthorized));
            }
        }
    }
}
