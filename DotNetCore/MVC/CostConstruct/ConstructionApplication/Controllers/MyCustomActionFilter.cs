using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;
        var userId = httpContext.Session.GetInt32("UserId");

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
        base.OnActionExecuting(context);
    }
}
