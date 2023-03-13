using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StoreFront.Helpers
{
    public class LogEntry : IAuthorizationFilter
    {
        public ControllerActionDescriptor? ControllerActionDescriptor { get; private set; }
        public string? ControllerName { get; private set; }
        public string? ActionName { get; private set; }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            ControllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            ControllerName = ControllerActionDescriptor?.ControllerName;
            ActionName = ControllerActionDescriptor?.ActionName;
        }
    }
}
