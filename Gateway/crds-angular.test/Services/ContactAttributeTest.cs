using NUnit.Framework;

namespace crds_angular.test.Services
{
    public class ContactAttributeTest
    {
        [Test]
        public void Given_An_AttributeType_That_Disallows_Multiselect_When_Attribute_Values_Changes_Then_Old_Value_Should_Be_End_Dated_And_A_New_Attribute_Added()
        {
            // Arrange
            // Setup list of AttributesTypes

            // Act
            // Modify the attribute value
            // Save Attributes

            // Assert
            // Assert old value has end date set
            // Assert new entry added
        }

        [Test]
        public void Given_An_AttributeType_That_Disallows_Multiselect_When_Attribute_Value_Does_Not_Change_Then_Ministry_Platform_Is_Not_Updated()
        {
            // Arrange
            // Setup list of AttributesTypes

            // Act
            // Save Attribute

            // Assert
            // Assert data 
        }

        // TODO: Should this be broken up into 2 tests
        // TODO: Would this be better to handle on the selection / assignment side of the fence?
        [Test]
        public void Given_An_AttributeType_That_Disallows_Multiselect_But_Somehow_Has_Multiple_Selected_When_Attribute_Value_Changes_Then_All_Existing_Contact_Attributes_Of_That_Type_Are_Enddated_And_New_Value_Is_Inserted()
        {
            // Arrange
            // Setup list of AttributesTypes 
            // Setup saved attrib on a Disallow_Mutliselect_item to have multiple entries

            // Act
            // Modify the value of the attribute
            // Save Attribute

            // Assert
            // Assert data 
        }

    }
}


