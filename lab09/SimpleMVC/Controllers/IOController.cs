using Microsoft.AspNetCore.Mvc;
namespace MvcMovie.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;

[Route("IO")]
public class IO : Controller
{
    private readonly AppDbContext _context;

    public IO(AppDbContext context)
    {
        _context = context;
    }

    [Route("Logowanie/")]
    [HttpGet("Form")]
    public IActionResult Form()
    {
        if (HttpContext.Session.GetString("logged") == null)
        {
            return View("Form");
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

    [HttpPost("Form")]
    public IActionResult Form(IFormCollection form)
    {
        string login = form["login"].ToString();
        string haslo = form["haslo"].ToString();

        if (string.IsNullOrEmpty(login) || !Regex.IsMatch(login, @"^[a-zA-Z0-9]{3,20}$"))
        {
            ViewData["error"] = "Login musi mieć 3-20 znaków i zawierać tylko litery lub cyfry.";
            return View();
        }

        if (string.IsNullOrEmpty(haslo) || !Regex.IsMatch(haslo, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$"))
        {
            ViewData["error"] = "Hasło musi mieć co najmniej 6 znaków, zawierać małą, dużą literę i cyfrę.";
            return View();
        }

        string hasloMD5 = ObliczMD5(haslo);

        var user = _context.Loginy.FirstOrDefault(u => u.LoginName == login && u.Password == hasloMD5);

        if (user == null)
        {
            ViewData["error"] = "Nieprawidłowy login lub hasło.";
            return View();
        }

        HttpContext.Session.SetString("logged", "tak");
        ViewData["success"] = $"Zalogowano jako {login}";
        return View("LogOut");
    }

    [Route("Wyloguj")]
    [HttpPost]
    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Form");
    }

    [Route("Comments")]
    [HttpGet]
    public IActionResult Comments()
    {
        var dane = _context.Dane.ToList();
        return View(dane);
    }

    [HttpGet("AddComment")]
    public IActionResult AddCommentForm()
    {
        return View("AddComment");
    }

    [HttpPost("AddComment")]
    public IActionResult AddComment(IFormCollection form)
    {
        string comment = form["comment"].ToString();

        if (string.IsNullOrEmpty(comment))
        {
            ViewData["error"] = "Komentarz nie może być pusty";
            return View("AddComment");
        }

        var nowyKomentarz = new Dane { Tekst = comment };
        _context.Dane.Add(nowyKomentarz);
        _context.SaveChanges();

        ViewData["success"] = "Komentarz został dodany!";
        return View("AddComment");
    }

    [HttpGet("Register")]
    public IActionResult Register()
    {
        return View("Register");
    }
    
    [HttpPost("Register")]
    public IActionResult Register(IFormCollection form)
    {
        string name = form["name"].ToString();
        string login = form["login"].ToString();
        string haslo = form["haslo"].ToString();

        if (string.IsNullOrEmpty(login) || !Regex.IsMatch(login, @"^[a-zA-Z0-9]{3,20}$"))
        {
            ViewData["error"] = "Login musi mieć 3-20 znaków i zawierać tylko litery lub cyfry.";
            return View("Register");
        }

        if (string.IsNullOrEmpty(haslo) || !Regex.IsMatch(haslo, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$"))
        {
            ViewData["error"] = "Hasło musi mieć co najmniej 6 znaków, zawierać małą, dużą literę i cyfrę.";
            return View("Register");
        }

        var user = _context.Loginy.FirstOrDefault(u => u.LoginName == login);

        if (user != null)
        {
            ViewData["error"] = "User juz istnieje";
            return View("Register");
        }

        string hasloMD5 = ObliczMD5(haslo);
        var nowyUser = new Login { LoginName = login, Password = hasloMD5 };
        _context.Loginy.Add(nowyUser);
        _context.SaveChanges();

        HttpContext.Session.SetString("name", name);
        HttpContext.Session.SetString("logged", "tak");
        ViewData["success"] = $"Zarejestrowano jako {login}";
        return View("Registered");
    }
}