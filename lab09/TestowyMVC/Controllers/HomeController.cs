using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SimpleMVC.Models;

namespace SimpleMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    //Jeżeli poniższa metoda znajduje się w klasie HomeController, wywołanie jest postaci np.:

// http://localhost:5083/Home/LiczbaOdwiedzin?liczbaPowtorzen=5&napis=%22Akuku!%22

    public String LiczbaOdwiedzin(int liczbaPowtorzen,
        String napis){
        String napisPomoc = "";
        for (int a = 0; a < liczbaPowtorzen; a++)
        {
        napisPomoc += napis + "\n";
        }
        return napisPomoc;
}
}
