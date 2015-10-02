using System.Web.Mvc;
using crds_angular.Filters;

namespace crds_angular
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new UnhandledExceptionFilter());
        }
    }
}
