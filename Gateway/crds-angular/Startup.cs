using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(crds_angular.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]

namespace crds_angular
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
