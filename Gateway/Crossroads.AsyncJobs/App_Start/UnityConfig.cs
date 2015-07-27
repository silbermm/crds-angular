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
        private readonly static object Lock = new object();

        public static void RegisterComponents()
        {
            lock (Lock)
            {
                // Only initialize once
                if (GlobalConfiguration.Configuration.DependencyResolver != null &&
                    GlobalConfiguration.Configuration.DependencyResolver.GetType() == typeof (UnityDependencyResolver))
                {
                    return;
                }

                var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
                var container = new UnityContainer();
                section.Configure(container);
                GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
            }
        }
    }
}