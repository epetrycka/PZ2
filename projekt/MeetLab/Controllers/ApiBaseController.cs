using Microsoft.AspNetCore.Mvc;

namespace MvcMovie.Controllers;

[ApiController]
public abstract class ApiBaseController : ControllerBase
{
    protected readonly MeetLab.Data.AppDbContext _context;

    public ApiBaseController(MeetLab.Data.AppDbContext context)
    {
        _context = context;
    }

    protected MeetLab.Models.User? AuthorizeUser(HttpRequest request)
    {
        string? nick = request.Headers["username"];
        string? token = request.Headers["token"];

        if (string.IsNullOrEmpty(nick) || string.IsNullOrEmpty(token))
            return null;

        return _context.Users.FirstOrDefault(u => u.NickName == nick && u.Token == token);
    }
}
