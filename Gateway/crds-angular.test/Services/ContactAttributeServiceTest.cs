using crds_angular.Services;
using Moq;
using NUnit.Framework;
using MPServices = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.test.Services
{
    public class ContactAttributeServiceTest
    {

        private ContactAttributeService _contactAttributeService;
        private Mock<MPServices.IAttributeService> _mpAttributeService;

        [SetUp]
        public void Setup()
        {
            _mpAttributeService = new Mock<MPServices.IAttributeService>(MockBehavior.Strict);
            _contactAttributeService = new ContactAttributeService(_mpAttributeService.Object);
        }

        [Test]
        public void TestMethod1()
        {
        }
    }
}
