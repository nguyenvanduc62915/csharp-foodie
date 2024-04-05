using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppCore.Filters;

public class AuthorizeUserAttribute : Attribute, IAuthorizationFilter {
    public void OnAuthorization(AuthorizationFilterContext context) {
        if (!context.HttpContext.User.Identity.IsAuthenticated) {
            context.Result = new RedirectToActionResult("Login", "User", null);
            return;
        }
        // if (!context.HttpContext.User.IsInRole("User")) {
        //     context.Result = new RedirectToActionResult("Login", "User", null);
        // }
    }
}


