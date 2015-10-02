using System.Web.Http.Filters;
using log4net;

namespace crds_angular.Filters
{
    public class UnhandledExceptionFilter : ExceptionFilterAttribute, IExceptionFilter
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var ex = context.Exception;
            var logger = LogManager.GetLogger(context.ActionContext.ControllerContext.Controller.GetType());
            logger.Error(string.Format("Unhandled exception in controller: {0}", ex.Message), ex);
            if (logger.IsDebugEnabled)
            {
                var requestString = context.Request.Content == null ? string.Empty : context.Request.Content.ReadAsStringAsync().Result;
                var responseString = context.Response.Content == null ? string.Empty : context.Response.Content.ReadAsStringAsync().Result;
                logger.Debug(string.Format("Request  -- uri: {0}, message: {1}", context.Request.RequestUri, requestString));
                logger.Debug(string.Format("Response -- message: {0}", responseString));
            }
        }
    }
}