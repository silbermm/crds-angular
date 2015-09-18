using NUnit.Framework;
using MinistryPlatform.Models;

namespace crds_angular.test.Models
{
    [TestFixture]
    class Person
    {
        private crds_angular.Models.Person _person;

        [Test]
        public void ShouldReturnContactType()
        {
            var contact = _person.GetContact();
            Assert.That(contact, Is.TypeOf<MyContact>());
            Assert.AreEqual(_person.EmailAddress, contact.Email_Address);

        }

        [Test]
        public void ShouldReturnHouseholdType()
        {
            var household = _person.GetHousehold();
            Assert.That(household, Is.TypeOf<MinistryPlatform.Models.Household>());
            Assert.AreEqual(_person.CongregationId, household.Congregation_ID);

        }

        [Test]
        public void ShouldReturnAddressType()
        {
            var address = _person.GetAddress();
            Assert.That(address, Is.TypeOf<crds_angular.Models.MP.Address>());
            Assert.AreEqual(_person.AddressLine1, address.Address_Line_1);

        }

        [SetUp]
        public void SetUp()
        {
            _person = new crds_angular.Models.Person();
            _person.EmailAddress = "test@crossroads.net";
            _person.LastName = "Crossroads";
            _person.CongregationId = 6;
            _person.AddressLine1 = "1234 Madison Rd";

        }
    }
}
