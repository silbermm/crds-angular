using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using log4net;
using log4net.Config;
using System.Reflection;

namespace crds_angular.Controllers
{
    public class HomeController : Controller
    {
        readonly log4net.ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            //log4net example
            logger.Debug("Entering application.");

            return View("Index");
        }        
    }
}
