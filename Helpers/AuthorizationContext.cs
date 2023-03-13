using System.Web.Mvc;
using StoreFront.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using AllowAnonymousAttribute = Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;
using IAuthorizationFilter = Microsoft.AspNetCore.Mvc.Filters.IAuthorizationFilter;
using RedirectToRouteResult = Microsoft.AspNetCore.Mvc.RedirectToRouteResult;

namespace StoreFront.Helpers
{
    public partial class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["User"] = null)
            {
                //filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }

            var db = new StoreFrontContext();
            var session = filterContext.HttpContext.Session;
            var username = session.GetString("username");
            var role = session.GetString("Role");
            var actionName = filterContext.ActionDescriptor.ActionName;
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var tag = controllerName + actionName;

            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                // Don't check for authorization as AllowAnonymous filter is applied to the action or controller
                return;
            }

            // Check for authorization
            if (session.GetString("username") == null)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }

            if (username == "") return;
            var isPermitted = false;

            var viewPermission = db.RolePermission.SingleOrDefault(x => x.Role == role);
            if (viewPermission != null)
            {
                isPermitted = true;
            }
            if (isPermitted == false)
            {
                filterContext.Result = new RedirectResult(new RouteValueDictionary
                {
                   { "controller", "Home" },
                   { "action", "AccessDenied" }
                });
            }
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}
