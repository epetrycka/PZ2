using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;

namespace MvcMovie.Controllers;

[Route("Admin")]
[ServiceFilter(typeof(AdminTokenAuthorizeAttribute))]
public class AdminController : Controller
{
    private readonly MeetLab.Data.AppDbContext _context;

    public AdminController(MeetLab.Data.AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("SearchUser")]
    public IActionResult SearchUser()
    {
        var allUsers = _context.Users
            .OrderBy(u => u.NickName)
            .Select(u => u.NickName)
            .ToList();

        ViewData["AllUsers"] = allUsers;

        return View("SearchUser");
    }

    [HttpPost("SearchUser")]
    public IActionResult SearchUser(IFormCollection form)
    {
        string nickname = form["nickname"].ToString();

        if (string.IsNullOrEmpty(nickname) || !Regex.IsMatch(nickname, @"^[a-zA-Z0-9]{3,20}$"))
        {
            ViewData["error"] = "Nickname musi mieć 3-20 znaków i zawierać tylko litery lub cyfry.";

            ViewData["AllUsers"] = _context.Users
                .OrderBy(u => u.NickName)
                .Select(u => u.NickName)
                .ToList();

            return View("SearchUser");
        }

        var user = _context.Users.FirstOrDefault(u => u.NickName == nickname);

        if (user == null)
        {
            ViewData["error"] = "User o podanym nickname nie istnieje";

            ViewData["AllUsers"] = _context.Users
                .OrderBy(u => u.NickName)
                .Select(u => u.NickName)
                .ToList();

            return View("SearchUser");
        }

        return RedirectToAction("UserDetails", new { nickname = user.NickName });
    }

    [HttpGet("UserDetails")]
    public IActionResult UserDetails(string nickname)
    {
        var user = _context.Users.FirstOrDefault(u => u.NickName == nickname);
        if (user == null)
        {
            TempData["error"] = "Użytkownik nie istnieje.";
            return RedirectToAction("SearchUser");
        }

        return View("UserDetails", user);
    }

    public static string GenerateToken()
    {
        var bytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }
        return Convert.ToBase64String(bytes);
    }

    [HttpPost("ResetToken")]
    public IActionResult ResetToken(string nickname)
    {
        var user = _context.Users.FirstOrDefault(u => u.NickName == nickname);
        if (user == null)
        {
            TempData["error"] = "Nie znaleziono użytkownika.";
            return RedirectToAction("SearchUser");
        }

        user.Token = GenerateToken();

        _context.Users.Update(user);
        _context.SaveChanges();

        TempData["nickname"] = nickname;
        return View("ResetToken");
    }

    private string ObliczMD5(string tekst)
    {
        using (var md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(tekst);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes);
        }
    }

    [HttpPost("ResetPassword")]
    public IActionResult ResetPassword(string nickname)
    {
        var user = _context.Users.FirstOrDefault(u => u.NickName == nickname);
        if (user == null)
        {
            TempData["error"] = "Nie znaleziono użytkownika.";
            return RedirectToAction("SearchUser");
        }

        user.Password = ObliczMD5("Haslo1234");

        _context.Users.Update(user);
        _context.SaveChanges();

        TempData["nickname"] = nickname;
        return View("ResetPassword");
    }

    [HttpPost("DeleteUser")]
    public IActionResult DeleteUser(string nickname)
    {
        var user = _context.Users.FirstOrDefault(u => u.NickName == nickname);
        if (user == null)
        {
            TempData["error"] = "Nie znaleziono użytkownika.";
            return RedirectToAction("SearchUser", "Admin");
        }

        if (user.Token == MeetLab.Credentials.AdminCredentials.AdminToken)
        {
            TempData["error"] = "Nie można usunąć administratora";
            return RedirectToAction("SearchUser", "Admin");
        }

        _context.Users.Remove(user);
        _context.SaveChanges();

        TempData["success"] = "Użytkownik został usunięty.";
        return RedirectToAction("SearchUser", "Admin");
    }
}