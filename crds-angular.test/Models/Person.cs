using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using crds_angular.Models;

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
            Assert.That(contact, Is.TypeOf<crds_angular.Models.MP.Contact>());
            Assert.AreEqual(_person.Email_Address, contact.Email_Address);

        }

        [Test]
        public void ShouldReturnHouseholdType()
        {
            var household = _person.GetHousehold();
            Assert.That(household, Is.TypeOf<crds_angular.Models.MP.Household>());
            Assert.AreEqual(_person.Congregation_ID, household.Congregation_ID);

        }

        [Test]
        public void ShouldReturnAddressType()
        {
            var address = _person.GetAddress();
            Assert.That(address, Is.TypeOf<crds_angular.Models.MP.Address>());
            Assert.AreEqual(_person.Address_Line_1, address.Address_Line_1);

        }

        [SetUp]
        public void SetUp()
        {
            _person = new crds_angular.Models.Person();
            _person.Email_Address = "test@crossroads.net";
            _person.Last_Name = "Crossroads";
            _person.Congregation_ID = 6;
            _person.Address_Line_1 = "1234 Madison Rd";

        }
    }
}
