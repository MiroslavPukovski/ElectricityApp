using ElectricityApp.Models;
using ElectricityApp.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ElectricityApp.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    { 
      public IActionResult Index()
        {
            return View();
        }

    }
}