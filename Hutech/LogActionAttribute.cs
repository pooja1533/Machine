using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualBasic;
using System.Diagnostics;

namespace Hutech
{
    public class LogActionAttribute : ActionFilterAttribute
    {
        private readonly ILogger<LogActionAttribute> _logger;
        public LogActionAttribute(ILogger<LogActionAttribute> logger)
        {
            _logger = logger;
        }
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var loggedinuser = filterContext.HttpContext.Session.GetString("LoogedInUser");
            var controllerName = filterContext.RouteData.Values["controller"];
            var actionName = filterContext.RouteData.Values["action"];
            var parameter = filterContext.ActionArguments;
            var message = String.Format("{0} controller:{1} action:{2} execeute by {3} at {4}", "onexecuting", controllerName, actionName,loggedinuser,DateTime.Now);
            if(parameter.Count > 0)
            {
                message = message+" " + parameter.Keys.First().ToString()+"-"+parameter.Values.First().ToString();
            }
            //Debug.WriteLine(message, "Action Filter Log");
            _logger.LogInformation(message);
            base.OnActionExecuting(filterContext);
        }
    }
}
