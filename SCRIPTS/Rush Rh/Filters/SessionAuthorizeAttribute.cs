using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

public class SessionAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var session = context.HttpContext.Session.GetString("session");

        if (string.IsNullOrEmpty(session))
        {
            context.Result = new RedirectToRouteResult(new RouteValueDictionary
            {
                { "controller", "Login" },
                { "action", "Index" }
            });
        }
    }
}
