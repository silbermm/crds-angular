using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace crds_angular
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("http://localhost:8080,http://localhost:3000,https://int.crossroads.net,https://demo.crossroads.net,https://www.crossroads.net,https://prod.crossroads.net", "*", "*");
            cors.SupportsCredentials = true;
            config.EnableCors(cors);

            // Web API configuration and services
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


        }
    }
}
