// Filters/AuthorizeAdminAttribute.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication;

namespace AppCore.Filters;

public class AuthorizeAdminAttribute : Attribute, IAuthorizationFilter {
    public void OnAuthorization(AuthorizationFilterContext context) {
        if (!context.HttpContext.User.Identity.IsAuthenticated) {
            context.Result = new RedirectToActionResult("Login", "Admin", null);
            return;
        }
        if (!context.HttpContext.User.IsInRole("Admin")) {
            context.Result = new RedirectToActionResult("Login", "Admin", null);
        }
    }
}


