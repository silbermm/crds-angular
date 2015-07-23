using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using Crossroads.AsyncJobs.Application;
using Microsoft.Practices.Unity;
using Unity.WebApi;

namespace Crossroads.AsyncJobs
{
    public class ApplicationPreload : IProcessHostPreloadClient
    {
        public void Preload(string[] parameters)
        {
            Preload();
        }

        public static void Preload()
        {
            var uc = (IUnityContainer)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IUnityContainer));
            var processor = (JobProcessor)uc.Resolve(typeof(Crossroads.AsyncJobs.Application.JobProcessor), "JobProcessor");
            processor.Start();
        }
    }
}