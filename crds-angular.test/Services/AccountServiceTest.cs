using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace crds_angular.test.Services
{
    [TestClass]
    public class testclass
    {
        [TestMethod]
        public void TestMethod1()
        {
            crds_angular.Services.AccountService.RegisterPerson();
        }
    }
}