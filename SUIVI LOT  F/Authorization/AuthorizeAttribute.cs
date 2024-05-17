namespace SUIVI.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SUIVI.Models.AllModels;
using SUIVI.Models.AllModels.UserModels;
using System.Security.Claims;

[AttributeUsage(AttributeTargets.Method)]
public class AllowAnonymousAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly IList<Role> _roles;

    public AuthorizeAttribute(params Role[] roles)
    {
        _roles = roles ?? Array.Empty<Role>();
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // skip authorization if action is decorated with [AllowAnonymous] attribute
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous) { return; }

        // get role
        string? role = context.HttpContext.User.Claims.FirstOrDefault(
                c => c.Type == ClaimTypes.Role)?.Value;
        Role myrole = new();
        if (!string.IsNullOrEmpty(role))
        {
            string[] arrayString = role.Split('|');
            foreach (string str in arrayString)
            {
                if (str == "RESP") { myrole = Role.RESP; break; }
                else { myrole = Role.INPUT; }
            }
            if (_roles.Any() && !_roles.Contains(myrole))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary{
                {"controller", "Account"},
                {"action", "Logout"}
            });
            }
        }
        else
        {
            context.Result = new RedirectToRouteResult(new RouteValueDictionary{
                {"controller", "Account"},
                {"action", "Logout"}
            });
        }
    }
}
