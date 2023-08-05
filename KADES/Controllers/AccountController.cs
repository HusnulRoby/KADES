using AspNetCoreHero.ToastNotification.Abstractions;
using KADES.Models;
using KADES.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Asn1.Cms;
using System.Diagnostics;
using System.Security.Claims;

namespace KADES.Controllers
{

    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly AppDbContext _context;
        private readonly INotyfService _notyf;

        public AccountController(ILogger<AccountController> logger, AppDbContext context, INotyfService notyf)
        {
            _context = context;
            _logger = logger;
            _notyf = notyf;
        }

        [HttpGet]
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

                if (getAcc != null)
                {
                    HttpContext.Session.SetString("UserId", model.USERID.ToString());
                    HttpContext.Session.SetString("Password", model.PASSWORD.ToString());

                    var GROUPID = _context.RFUsers.FirstOrDefault(x => x.USERID == model.USERID && x.PASSWORD == model.PASSWORD).GROUPID;
                    HttpContext.Session.SetString("GroupId", GROUPID.ToString());
                    var a= HttpContext.Session.GetString("UserId");
                    //ViewBag.USERID = a;
                    return Redirect("/Home/Home");
                }
                else
                {
                    ViewBag.LoginStat = 0;
                    ViewBag.MsgError = "User Id/Password Salah";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return View();
        }

        [HttpGet]
        public IActionResult ForgotPass()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPass(ForgotPass model)
        {

            try
            {
                var getAcc = _context.RFUsers.Where(x => x.USERID == model.USERID).FirstOrDefault();

                if (getAcc != null)
                {
                    if (model.NEW_PASSWORD.Equals(model.CONFIRM_PASSWORD))
                    {
                        getAcc.PASSWORD= model.CONFIRM_PASSWORD;

                        _context.RFUsers.Update(getAcc);
                        _context.SaveChanges();
                        _notyf.Success("Tambah Data Sukses");
                    }
                    else
                    {
                        ViewBag.LoginStat = 0;
                        ViewBag.MsgError = "Password tidak sesuai.";
                    }
                    ViewBag.LoginStat = 0;
                    ViewBag.MsgError = "Forgot password sukses.";

                    return Redirect("/Account/Login");
                }
                else
                {
                    ViewBag.LoginStat = 0;
                    ViewBag.MsgError = "User Id tidak terdaftar.";

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