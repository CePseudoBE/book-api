using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

public class UserIdFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.HttpContext.User.Identity is ClaimsIdentity identity)
        {
            var userIdClaim = identity.FindFirst(ClaimTypes.Name);
            if (userIdClaim != null)
            {
                context.HttpContext.Items["UserId"] = userIdClaim.Value;
            }
        }

        await next();
    }
}
