using System.Collections.Generic;
using crds_angular.Services;
using MinistryPlatform.Models;
using Moq;
using NUnit.Framework;
using MPServices = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.test.Services
{
    public class AttributeServiceTest
    {
        private AttributeService _fixture;
        private Mock<MPServices.IAttributeService> _mpAttributeService;

        [SetUp]
        public void SetUp()
        {
            _mpAttributeService = new Mock<MPServices.IAttributeService>(MockBehavior.Strict);

            _fixture = new AttributeService(_mpAttributeService.Object);
        }

        [Test]
        public void Given_An_Attribute_List_When_Translated_To_AttributeTypes_Should_Create_Hierarchy_By_AttributeType()
        {
            var attriubuteResults = GetAttributesResults();
            _mpAttributeService.Setup(mocked => mocked.GetAttributes(null)).Returns(attriubuteResults);

            var result = _fixture.GetAttributeTypes(null);
            _mpAttributeService.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 2, "Records are not grouped by attributeType");
            Assert.AreEqual(result[0].Name, "AttributeType #1", "attributeType name not correct");
            Assert.AreEqual(result[0].AllowMultipleSelections, true);
            Assert.AreEqual(result[0].Attributes.Count, 2, "Number of attributes for attributeType not correct");


            Assert.AreEqual(result[1].Name, "AttributeType #2", "attributeType name not correct");
            Assert.AreEqual(result[1].AllowMultipleSelections, false);
            Assert.AreEqual(result[1].Attributes.Count, 1, "Number of attributes for attributeType not correct");
        }

        private List<Attribute> GetAttributesResults()
        {
            return new List<Attribute>
            {
                new Attribute()
                {
                    AttributeId = 1,
                    Name=  "Attribute #1",
                    CategoryId = 2,
                    Category = "Category #1",
                    AttributeTypeId = 3,
                    AttributeTypeName = "AttributeType #1",
                    PreventMultipleSelection = false
                },
                new Attribute()
                {
                    AttributeId = 4,
                    Name=  "Attribute #2",
                    CategoryId = 5,
                    Category = "Category #2",
                    AttributeTypeId = 3,
                    AttributeTypeName = "AttributeType #1",
                    PreventMultipleSelection = false
                },
                new Attribute()
                {
                    AttributeId = 7,
                    Name=  "Attribute #3",
                    CategoryId = null,
                    Category = null,
                    AttributeTypeId = 9,
                    AttributeTypeName = "AttributeType #2",
                    PreventMultipleSelection = true
                }
            };
        }
    }
}