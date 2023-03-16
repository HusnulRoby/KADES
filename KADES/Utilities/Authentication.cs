using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace KADES.Utilities
{
    public class Authentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.GetString("UserId") == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary {
                    { "Controller", "Account" },
                    { "Action", "Login" }
                });
            }
        }
    }
}
