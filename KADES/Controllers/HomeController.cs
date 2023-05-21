using KADES.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using System.Diagnostics;

namespace KADES.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Dashboard()
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");
            return View();
        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public JsonResult CheckSession()
        {
            if (HttpContext.Session.IsAvailable)
            {

            }
            string userID = Convert.ToString(HttpContext.Session.GetString("UserId"));
            int check = 0;
            if (!string.IsNullOrEmpty(userID))
                check = 1;
            return Json(new { check });
        }

        
    }
}