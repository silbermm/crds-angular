using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class AttributeServiceTest
    {
        private AttributeService _fixture;
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IAuthenticationService> _authService;
        private Mock<IConfigurationWrapper> _configWrapper;

        private readonly string _tokenValue = "ABC";

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _authService = new Mock<IAuthenticationService>();
            _configWrapper = new Mock<IConfigurationWrapper>();

            var authenticateResults =
                new Dictionary<string, object>()
                {
                    {"token", _tokenValue},
                    {"exp", "123"}
                };
            _authService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(authenticateResults);
            _fixture = new AttributeService(_ministryPlatformService.Object, _authService.Object, _configWrapper.Object);
        }

        [Test]
        public void Given_An_AttributeTypeId_When_Queried_It_Should_Filter_Records_And_Return_Records()
        {
            int? attributeTypeId = 123456;
            var response = GetPageViewRecordsResponse();
            _ministryPlatformService.Setup(
                mocked =>
                    mocked.GetPageViewRecords("AttributesPageView",
                                              _tokenValue,
                                              It.Is<string>((x => x.Contains(string.Format("\"{0}\"", attributeTypeId)))),
                                              string.Empty,
                                              0))
                .Returns(response);

            var attributes = _fixture.GetAttributes(attributeTypeId).ToList();

            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(attributes);
            Assert.AreEqual(3, attributes.Count());

            var attribute = attributes[0];
            Assert.AreEqual(1, attribute.AttributeId);
            Assert.AreEqual("Attribute #1", attribute.Name);
            Assert.AreEqual(2, attribute.CategoryId);
            Assert.AreEqual("Category #1", attribute.Category);
            Assert.AreEqual(3, attribute.AttributeTypeId);
            Assert.AreEqual("AttributeType #1", attribute.AttributeTypeName);

            attribute = attributes[1];
            Assert.AreEqual(4, attribute.AttributeId);
            Assert.AreEqual("Attribute #2", attribute.Name);
            Assert.AreEqual(5, attribute.CategoryId);
            Assert.AreEqual("Category #2", attribute.Category);
            Assert.AreEqual(6, attribute.AttributeTypeId);
            Assert.AreEqual("AttributeType #1", attribute.AttributeTypeName);

            attribute = attributes[2];
            Assert.AreEqual(7, attribute.AttributeId);
            Assert.AreEqual("Attribute #3", attribute.Name);
            Assert.AreEqual(null, attribute.CategoryId);
            Assert.AreEqual(null, attribute.Category);
            Assert.AreEqual(9, attribute.AttributeTypeId);
            Assert.AreEqual("AttributeType #2", attribute.AttributeTypeName);
        }

        [Test]
        public void Given_An_Null_AttributeTypeId_When_Queried_It_Should_Not_Filter_Records_And_Return_Records()
        {
            var response = GetPageViewRecordsResponse();
            _ministryPlatformService.Setup(
                mocked =>
                    mocked.GetPageViewRecords("AttributesPageView", _tokenValue, string.Empty, string.Empty, 0))
                .Returns(response);

            var attributes = _fixture.GetAttributes(null).ToList();

            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(attributes);
            Assert.AreEqual(3, attributes.Count());

            var attribute = attributes[0];
            Assert.AreEqual(1, attribute.AttributeId);
            Assert.AreEqual("Attribute #1", attribute.Name);
            Assert.AreEqual(2, attribute.CategoryId);
            Assert.AreEqual("Category #1", attribute.Category);
            Assert.AreEqual(3, attribute.AttributeTypeId);
            Assert.AreEqual("AttributeType #1", attribute.AttributeTypeName);
            Assert.AreEqual(false, attribute.PreventMultipleSelection);

            attribute = attributes[1];
            Assert.AreEqual(4, attribute.AttributeId);
            Assert.AreEqual("Attribute #2", attribute.Name);
            Assert.AreEqual(5, attribute.CategoryId);
            Assert.AreEqual("Category #2", attribute.Category);
            Assert.AreEqual(6, attribute.AttributeTypeId);
            Assert.AreEqual("AttributeType #1", attribute.AttributeTypeName);
            Assert.AreEqual(false, attribute.PreventMultipleSelection);

            attribute = attributes[2];
            Assert.AreEqual(7, attribute.AttributeId);
            Assert.AreEqual("Attribute #3", attribute.Name);
            Assert.AreEqual(null, attribute.CategoryId);
            Assert.AreEqual(null, attribute.Category);
            Assert.AreEqual(9, attribute.AttributeTypeId);
            Assert.AreEqual("AttributeType #2", attribute.AttributeTypeName);
            Assert.AreEqual(true, attribute.PreventMultipleSelection);
        }

        private static List<Dictionary<string, object>> GetPageViewRecordsResponse()
        {
            return new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>()
                {
                    {"Attribute_ID", 1},
                    {"Attribute_Name", "Attribute #1"},
                    {"Attribute_Category_ID", 2},
                    {"Attribute_Category", "Category #1"},
                    {"Attribute_Type_ID", 3},
                    {"Attribute_Type", "AttributeType #1"},
                    {"Prevent_Multiple_Selection", "False"},
                    {"Sort_Order", "0"}
                },
                new Dictionary<string, object>()
                {
                    {"Attribute_ID", 4},
                    {"Attribute_Name", "Attribute #2"},
                    {"Attribute_Category_ID", 5},
                    {"Attribute_Category", "Category #2"},
                    {"Attribute_Type_ID", 6},
                    {"Attribute_Type", "AttributeType #1"},
                    {"Prevent_Multiple_Selection", "False"},
                    {"Sort_Order", "0"}
                },
                new Dictionary<string, object>()
                {
                    {"Attribute_ID", 7},
                    {"Attribute_Name", "Attribute #3"},
                    {"Attribute_Category_ID", null},
                    {"Attribute_Category", null},
                    {"Attribute_Type_ID", 9},
                    {"Attribute_Type", "AttributeType #2"},
                    {"Prevent_Multiple_Selection", "True"},
                    {"Sort_Order", "0"}
                }
            };
        }
    }
}