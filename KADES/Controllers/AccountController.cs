using KADES.Models;
using KADES.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;
using System.Diagnostics;
using System.Security.Claims;

namespace KADES.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly AppDbContext _context;

        public AccountController(ILogger<AccountController> logger, AppDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Login()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserId");
            return View();
        }

        [HttpPost]
        public IActionResult Login(RFUsers model)
        {

            try
            {
                var getAcc = _context.RFUsers.Where(x => x.USERID == model.USERID && x.PASSWORD == model.PASSWORD).FirstOrDefault();
                
                if (getAcc!=null)
                {
                    HttpContext.Session.SetString("UserId", model.USERID.ToString());
                    //var a= HttpContext.Session.GetString("UserId");
                    //ViewBag.USERID = a;
                    return Redirect("/Home/Dashboard");
                }
                else
                {
                    ViewBag.LoginStat = 0;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            
            return View();
        }

        ////Post Action
        //[HttpPost]
        //public ActionResult Login(User u)
        //{
        //    if (HttpContext.Session.GetString("UserName") == null)
        //    {

        //        if (ModelState.IsValid)
        //        {
        //            using (TestDBContext db = new TestDBContext())
        //            {
        //                var obj = db.Users.Where(a => a.UserName.Equals(u.UserName) && a.UserPassword.Equals(u.UserPassword)).FirstOrDefault();
        //                if (obj != null)
        //                {
        //                    HttpContext.Session.SetString("UserName", obj.UserName.ToString());
        //                    return RedirectToAction("Index");
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {



        //        return RedirectToAction("Login");
        //    }
        //    return View();


        //}


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