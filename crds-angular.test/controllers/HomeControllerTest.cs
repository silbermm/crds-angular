using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using crds_angular.Controllers;
using MvcContrib.TestHelper;

namespace crds_angular.test.controllers
{
    [TestFixture]
    class HomeControllerTest
    {

        [Test]
        public static void DummyTest()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public static void IndexPage()
        {
            var homeController = new crds_angular.Controllers.HomeController();
            System.Web.Mvc.ActionResult result = homeController.Index();

            result.AssertViewRendered().ForView("Index");
            
        }

    }
}
