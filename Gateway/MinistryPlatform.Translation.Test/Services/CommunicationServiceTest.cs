using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class CommunicationServiceTest
    {
        private CommunicationService _fixture;
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IAuthenticationService> _authService;
        private Mock<IConfigurationWrapper> _configWrapper;

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _authService = new Mock<IAuthenticationService>();
            _configWrapper = new Mock<IConfigurationWrapper>();

            _authService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new Dictionary<string, object> {{"token", "ABC"}, {"exp", "123"}});
            _fixture = new CommunicationService(_ministryPlatformService.Object, _authService.Object, _configWrapper.Object);
        }

        [Test]
        public void GetTemplateAsCommunication()
        {
            const int templateId = 3;
            const int fromContactId = 6;
            const string fromEmailAddress = "brady@minon.com";
            const int replyContactId = 5;
            const string replyEmailAddress = "bob@minon.com";
            const int toContactId = 4;
            const string toEmailAddress = "help@me.com";
            const string mockBody = "mock email body";
            const string mockSubject = "mock subject";

            _configWrapper.Setup(m => m.GetConfigIntValue("DefaultAuthorUser")).Returns(99);

            //template
            var templateDictionary = new Dictionary<string, object> {{"Body", mockBody}, {"Subject", mockSubject}};
            _ministryPlatformService.Setup(m => m.GetRecordDict(341, templateId, It.IsAny<string>(), false)).Returns(templateDictionary);

            var communication = _fixture.GetTemplateAsCommunication(templateId, fromContactId, fromEmailAddress, replyContactId, replyEmailAddress, toContactId, toEmailAddress);

            _configWrapper.VerifyAll();
            _ministryPlatformService.VerifyAll();

            Assert.AreEqual(99, communication.AuthorUserId);
            Assert.AreEqual(mockBody, communication.EmailBody);
            Assert.AreEqual(mockSubject, communication.EmailSubject);
            Assert.AreEqual(fromContactId, communication.FromContactId);
            Assert.AreEqual(fromEmailAddress, communication.FromEmailAddress);
            Assert.IsNull(communication.MergeData);
            Assert.AreEqual(replyContactId, communication.ReplyContactId);
            Assert.AreEqual(replyEmailAddress, communication.ReplyToEmailAddress);
            Assert.AreEqual(toContactId, communication.ToContactId);
            Assert.AreEqual(toEmailAddress, communication.ToEmailAddress);
        }

        [Test]
        public void TestParseTemplateBody()
        {
            var mergeData = new Dictionary<string, object>
            {
                {"DavidsGame", "Global Thermonuclear War"},
                {"WoprsGame", "Chess"},
                {"WhenToPlayChess", null}
            };

            var parsed = _fixture.ParseTemplateBody("David: Would you like to play a game of [DavidsGame]? / WOPR: Not right now, wouldn't you like to play a game of [WoprsGame] instead? / David: No, maybe some other time, [WhenToPlayChess]",
                                       mergeData);

            Assert.AreEqual("David: Would you like to play a game of Global Thermonuclear War? / WOPR: Not right now, wouldn't you like to play a game of Chess instead? / David: No, maybe some other time, ", parsed);
        }
    }
}