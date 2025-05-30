public class TokenAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly MeetLab.Data.AppDbContext _context;

    public TokenAuthenticationMiddleware(RequestDelegate next, MeetLab.Data.AppDbContext context)
    {
        _next = next;
        _context = context;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("Username", out var username) &&
            context.Request.Headers.TryGetValue("Token", out var token))
        {
            var user = _context.Users.FirstOrDefault(u => u.NickName == username && u.Token == token);
            if (user != null)
            {
                // User is authenticated
                context.Items["User"] = user;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid token.");
                return;
            }
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Username and Token headers are required.");
            return;
        }

        await _next(context);
    }
}
