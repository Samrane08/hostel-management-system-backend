using ApplicantService.Model;
using Helper;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using System.Diagnostics;

namespace ApplicantService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            var a =  QueryStringEncryptDecrypt.EncryptQueryString("UID=" + "286311459796");
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
    }
}
