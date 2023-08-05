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
            ViewBag.USERID = HttpContext.Session.GetString("UserId"); ViewBag.GROUPID = HttpContext.Session.GetString("GroupId");

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