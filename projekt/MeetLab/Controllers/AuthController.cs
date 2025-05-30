using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;

namespace MvcMovie.Controllers;

[Route("Auth")]
public class AuthController : Controller
{
    private readonly MeetLab.Data.AppDbContext _context;

    public AuthController(MeetLab.Data.AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("Register")]
    public IActionResult Register()
    {
        return View("Register");
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

    [HttpPost("Register")]
    public IActionResult Register(IFormCollection form)
    {
        string name = form["name"].ToString();
        string login = form["login"].ToString();
        string password = form["password"].ToString();

        if (string.IsNullOrEmpty(login) || !Regex.IsMatch(login, @"^[a-zA-Z0-9]{3,20}$"))
        {
            ViewData["error"] = "Login musi mieć 3-20 znaków i zawierać tylko litery lub cyfry.";
            return View("Register");
        }

        if (string.IsNullOrEmpty(password) || !Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$"))
        {
            ViewData["error"] = $"Hasło musi mieć co najmniej 6 znaków, zawierać małą, dużą literę i cyfrę.";
            return View("Register");
        }

        var user = _context.Users.FirstOrDefault(u => u.NickName == login);

        if (user != null)
        {
            ViewData["error"] = "User juz istnieje";
            return View("Register");
        }

        string passwordMD5 = ObliczMD5(password);
        string token = GenerateToken();
        var nowyUser = new MeetLab.Models.User { NickName = login, FirstName = name, Password = passwordMD5, Token = token, RegistrationDate = DateTime.Now };
        _context.Users.Add(nowyUser);
        _context.SaveChanges();

        HttpContext.Session.SetString("user", login);
        HttpContext.Session.SetString("logged", token);
        ViewData["success"] = $"Zarejestrowano jako {login}";
        return View("Registered");
    }

    [HttpGet("Login")]
    public IActionResult Login()
    {
        if (HttpContext.Session.GetString("logged") == null)
        {
            return View("Login");
        }
        else
        {
            return View("LogOut");
        }
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

    [HttpPost("Login")]
    public IActionResult Login(IFormCollection form)
    {
        string login = form["login"].ToString();
        string password = form["password"].ToString();

        if (string.IsNullOrEmpty(login) || !Regex.IsMatch(login, @"^[a-zA-Z0-9]{3,20}$"))
        {
            ViewData["error"] = "Login musi mieć 3-20 znaków i zawierać tylko litery lub cyfry.";
            return View();
        }

        if (string.IsNullOrEmpty(password) || !Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$"))
        {
            ViewData["error"] = "Hasło musi mieć co najmniej 6 znaków, zawierać małą, dużą literę i cyfrę.";
            return View();
        }

        string passwordMD5 = ObliczMD5(password);

        var user = _context.Users.FirstOrDefault(u => u.NickName == login && u.Password == passwordMD5);

        if (user == null)
        {
            ViewData["error"] = "Nieprawidłowy login lub hasło.";
            return View();
        }

        string token = user.Token ?? "tak";
        HttpContext.Session.SetString("logged", token);
        HttpContext.Session.SetString("user", login);
        ViewData["success"] = $"Zalogowano jako {login}";

        return View("LogOut");
    }

    [Route("LogOut")]
    [HttpPost]
    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
    
    [HttpGet("AccessDenied")]
    public IActionResult AccessDenied()
    {
        return View();
    }
}