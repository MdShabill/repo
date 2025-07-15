using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;
        int? userId = httpContext.Session.GetInt32("UserId");
        int? siteId = httpContext.Session.GetInt32("SelectedSiteId");

        if (userId == null || userId <= 0)
        {
            // Store message in TempData
            if (context.Controller is Controller controller)
            {
                controller.TempData["ErrorMessage"] = "Your session has expired. Please login again.";
            }

            // Redirect to Login
            context.Result = new RedirectToActionResult("Login", "Account", null);
            return;
        }

        string? controllerName = context.RouteData.Values["controller"]?.ToString();

        if (!string.Equals(controllerName, "Site", StringComparison.OrdinalIgnoreCase))
        {
            if (siteId == null || siteId <= 0)
            {
                if (context.Controller is Controller controller)
                    controller.TempData["ErrorMessage"] = "Please select a site before continuing.";

                context.Result = new RedirectToActionResult("NoSiteSelcted", "Site", null);
                return;
            }
        }

        base.OnActionExecuting(context);
    }
}
