using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class ParticipantServiceTest
    {
        private ParticipantService _fixture;
        private Mock<IMinistryPlatformService> _mpServiceMock;

        [SetUp]
        public void SetUp()
        {
            _mpServiceMock = new Mock<IMinistryPlatformService>();

            _fixture = new ParticipantService(_mpServiceMock.Object);
        }

        [Test]
        public void GetParticipantByParticipantId()
        {
            const int contactId = 99999;

            const string viewKey = "ParticipantByContactId";
            var searchString = contactId.ToString();
            var mockDictionaryList = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>{
                {"dp_RecordID", 100},
                {"Email Address", "email-address"},
                {"Nickname", "nick-name"}}
            };

            _mpServiceMock.Setup(m => m.GetPageViewRecords(viewKey, It.IsAny<string>(), searchString, "", 0)).Returns(mockDictionaryList);

            var participant = _fixture.GetParticipant(contactId);

            _mpServiceMock.VerifyAll();

            Assert.IsNotNull(participant);
            Assert.AreEqual("nick-name", participant.PreferredName);
            Assert.AreEqual("email-address", participant.EmailAddress);
            Assert.AreEqual(100, participant.ParticipantId);
        }
    }
}
