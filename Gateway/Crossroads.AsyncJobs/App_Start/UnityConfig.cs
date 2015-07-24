using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using System.Web.Http;
using Unity.WebApi;


namespace Crossroads.AsyncJobs
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            var container = new UnityContainer();
            section.Configure(container);
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}