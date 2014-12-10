using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(crds_angular.Startup))]
namespace crds_angular
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
