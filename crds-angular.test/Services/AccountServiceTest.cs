using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace crds_angular.test.Services
{
    [TestClass]
    public class testclass
    {
        [TestMethod]
        //TODO When DeleteRecord is available cleanup and delete this test record
        public void shouldRegisterPerson()
        {
            crds_angular.Services.AccountService.RegisterPerson();
        }
    }
}