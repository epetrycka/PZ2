//Klasa oraz metoda modyfikująca standardowy routing 
//Wywołanie: http://localhost:5083/api/IO/index?id1=xxx&id2=yyy

using Microsoft.AspNetCore.Mvc;
namespace MvcMovie.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

[Route("api/[controller]")]
public class IO : Controller
{
    [Route("index/")]
    public String defaultmethod(
        [Bind(Prefix = "id1")] String napis1,
        [Bind(Prefix = "id2")] String napis2)
    {
        return "wynik= " + napis1 + " " + napis2;
    }

    public String IndexCookies()
    {
        if (!HttpContext.Request.Cookies.ContainsKey("pierwszy_request"))
        {
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddDays(7));
            HttpContext.Response.Cookies.Append("pierwszy_request",
                DateTime.Now.ToString(), cookieOptions);
            return "Pierwsze odwiedziny";
        }
        else
        {
            DateTime firstRequest = DateTime.Parse(HttpContext.Request.Cookies["pierwszy_request"]);
            return "Po raz pierwszy odwiedziłeś nas: " + firstRequest.ToString();
        }
    }

    public String IndexSession()
    {
        if (!HttpContext.Session.Keys.Contains("pierwszy_request"))
        {
            HttpContext.Session.SetString("pierwszy_request",
                (new DateTimeOffset(DateTime.Now.AddDays(7))).ToString());
            return "Pierwsze odwiedziny";
        }
        else
        {
            String firstRequest = HttpContext.Session.GetString("pierwszy_request");
            return "Po raz pierwszy odwiedziłeś nas: " + firstRequest;
        }
    }

    private readonly IMemoryCache _memoryCache;
    public IO(IMemoryCache memoryCache) =>
        _memoryCache = memoryCache;

    public String IndexCache()
    {
        DateTime CurrentDateTime = DateTime.Now;

        if (!_memoryCache.TryGetValue("pierwszy_request", out DateTime cacheValue))
        {
            cacheValue = CurrentDateTime;

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(60));

            _memoryCache.Set("pierwszy_request", cacheValue, cacheEntryOptions);
            return "Pierwsze odwiedziny";
        }
        else
        {
            return "Po raz pierwszy odwiedziłeś nas: " + cacheValue.ToString();
        }
    }

    public IActionResult IndexRazor()
    {
        return View();
    }

    public IActionResult Wyswietl(String tytul, String wiadomosc, int ile)
    {
        ViewData["Title"] = tytul;
        ViewData["Wiadomosc"] = wiadomosc;
        ViewData["Ile"] = ile;

        return View();
    }

    public IActionResult WczytajFormularz()
    {
        if (!HttpContext.Session.Keys.Contains("dane"))
            ViewData["dane"] += "brak danych";
        else
            ViewData["dane"] += HttpContext.Session.GetString("dane");
        return View();
    }

    //Obsługa metody POST
    [HttpPost]
    public IActionResult WczytajFormularz(IFormCollection form)
    {
        string dane = form["dane"].ToString();
        HttpContext.Session.SetString("dane", dane);
        ViewData["dane"] += dane;
        return View();
    }
}