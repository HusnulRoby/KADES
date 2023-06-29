using KADES.Models;
using KADES.Models.Account;
using KADES.Models.Administrasi;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;
using System.Diagnostics;
using System.Security.Claims;

namespace KADES.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly AppDbContext _context;

        public UserController(ILogger<UserController> logger, AppDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult UserMaintenance()
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");

            var model = from A in _context.RFUsers
                        join B in _context.RFGroup on A.GROUPID equals B.GROUPID
                        select new VW_Users()
                        {
                            USERID=A.USERID,
                            USERNAME=A.USERNAME,
                            GROUPID=A.GROUPID,
                            GROUP_NAME=B.GROUP_NAME,
                            EMAIL=A.EMAIL,
                        };

            AccountModels AccountModels = new AccountModels()
            {
                //TemplateSurat = new TemplateSurat(),
                //ddlRFJabatan = _context.RFJabatan.Where(x => x.ACTIVE.Equals(true) && x.KODE_TYPE.Equals("APRT")).ToList(),
                ListVW_Users = model.ToList()
            };

            return View(AccountModels);
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
                    return Redirect("/Home/Home");
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