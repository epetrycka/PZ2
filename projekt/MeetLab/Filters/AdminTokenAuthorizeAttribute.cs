using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AdminTokenAuthorizeAttribute : ActionFilterAttribute
{
    private readonly MeetLab.Data.AppDbContext _context;

    public AdminTokenAuthorizeAttribute(MeetLab.Data.AppDbContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;

        var sessionToken = httpContext.Session.GetString("logged");
        var adminUser = _context.Users.FirstOrDefault(u => u.Token == MeetLab.Credentials.AdminCredentials.AdminToken);

        if (adminUser == null || sessionToken == null || sessionToken != adminUser.Token)
        {
            context.Result = new RedirectToActionResult("AccessDenied", "Auth", null);
        }
    }
}
