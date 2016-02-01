using System;
using System.IO;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Crossroads.Utilities.Services;
using log4net.Config;

namespace Crossroads.AsyncJobs
{
    public class Global : HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            AutoMapperConfig.RegisterMappings();
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            ApplicationPreload.StartJobProcessor();
            TlsHelper.AllowTls12();
            XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            ApplicationPreload.StopJobProcessor();
        }
    }
}