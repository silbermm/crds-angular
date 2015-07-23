using Microsoft.Owin;
using Owin;

[assembly: OwinStartup("Crossroads.AsyncJobs.Startup", typeof(Crossroads.AsyncJobs.Startup))]
namespace Crossroads.AsyncJobs
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
