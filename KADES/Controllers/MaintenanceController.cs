using KADES.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System;
using KADES.Helper;
using System.Reflection.Metadata;
using Org.BouncyCastle.Asn1.Cms;
using System.Reflection.Metadata.Ecma335;
using KADES.Models.Pelayanan;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using MySqlX.XDevAPI;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.IO;
using System.Web;
using System.Text;
using KADES.Models.Maintenance;
using KADES.Models.Account;

namespace KADES.Controllers
{
    public class MaintenanceController : Controller
    {
        private readonly ILogger<MaintenanceController> _logger;
        private readonly AppDbContext _context;
        private readonly INotyfService _notyf;
        IWebHostEnvironment _env;
        public MaintenanceController(ILogger<MaintenanceController> logger, AppDbContext context, INotyfService notyf, IWebHostEnvironment env)
        {
            _context = context;
            _logger = logger;
            _notyf = notyf;
            _env = env;
        }

        #region Maintenance Jabatan
        public IActionResult Jabatan()
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");
            
            MaintenanceModels maintenanceModel = new MaintenanceModels()
            {
                ListRFJabatan = _context.RFJabatan.ToList()
            };

            return View(maintenanceModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddJabatan(RFJabatan RFJabatan)
        {
            var USERID = HttpContext.Session.GetString("UserId").ToString();

            try
            {
                var cekData = _context.RFJabatan.Where(x => x.KODE_JABATAN.Equals(RFJabatan.KODE_JABATAN)).Count();
                if (cekData>0)
                {
                    _notyf.Error("Kode Jabatan Sudah Ada");
                }
                else
                {
                    var getData = new RFJabatan
                    {
                        KODE_JABATAN = RFJabatan.KODE_JABATAN,
                        JABATAN = RFJabatan.JABATAN,
                        KODE_TYPE = RFJabatan.KODE_TYPE,
                        ACTIVE = true
                    };
                    _context.Add(getData);
                    _context.SaveChanges();
                    _notyf.Success("Tambah Data Sukses");
                }
                

            }
            catch (Exception ex)
            {
                _notyf.Error("Tambah Data Gagal");

                throw ex;

            }
            return RedirectToAction("Jabatan");
        }

        [HttpPost]
        public IActionResult UpJabatan(RFJabatan model)
        {

            _context.RFJabatan.Update(model);
            _context.SaveChanges();
            _notyf.Success("Update Data Sukses");

            return RedirectToAction("Jabatan");
        }

        [HttpPost]
        public IActionResult DelRFJabatan(int ID)
        {
            try
            {
                var getAcc = _context.RFJabatan.Find(ID);
                if (getAcc == null)
                {
                    _notyf.Error("Delete Data Gagal");
                    return NotFound();
                }
                _context.Remove(getAcc);
                _context.SaveChanges();
                _notyf.Success("Delete Data Sukses");
            }
            catch (Exception ex)
            {
                _notyf.Error("Delete Data Gagal");

                throw ex;
            }

            return RedirectToAction("Jabatan");

        }
        #endregion

        #region USER MAINTENANCE
        public IActionResult UserMaintenance()
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");

            var model = from A in _context.RFUsers
                        join B in _context.RFGroup on A.GROUPID equals B.GROUPID
                        select new VW_Users()
                        {
                            USERID = A.USERID,
                            USERNAME = A.USERNAME,
                            GROUPID = A.GROUPID,
                            GROUP_NAME = B.GROUP_NAME,
                            EMAIL = A.EMAIL,
                        };

            AccountModels AccountModels = new AccountModels()
            {
                //TemplateSurat = new TemplateSurat(),
                ListVW_Users = model.ToList()
            };

            var getGroup = _context.RFGroup.Select(x => new SelectListItem
            {
                Value = x.GROUPID,
                Text = x.GROUP_NAME.ToString(),
            });

            ViewBag.ddlGroup = getGroup;

            return View(AccountModels);
        }

        [HttpPost]
        public IActionResult AddUsers(RFUsers RFUsers)
        {

            try
            {
                var USERID = HttpContext.Session.GetString("UserId").ToString();
                var getData = new RFUsers
                {
                    USERID= RFUsers.USERID,
                    USERNAME= RFUsers.USERNAME,
                    GROUPID= RFUsers.GROUPID,
                    PASSWORD= RFUsers.PASSWORD,
                    EMAIL= RFUsers.EMAIL,
                };



                _context.Add(getData);
                _context.SaveChanges();
                _notyf.Success("Tambah Data Sukses");

            }
            catch (Exception ex)
            {
                _notyf.Error("Tambah Data Gagal");
                throw ex;
            }
            return RedirectToAction("UserMaintenance");
        }

        [HttpPost]
        public IActionResult UpdateUsers(RFUsers model)
        {

            _context.RFUsers.Update(model);
            _context.SaveChanges();
            _notyf.Success("Update Data Sukses");

            return RedirectToAction("UserMaintenance");
        }

        [HttpPost]
        public IActionResult DeleteUsers(string USERID)
        {
            try
            {
                var getAcc = _context.RFUsers.Find(USERID);
                if (getAcc == null)
                {
                    return NotFound();
                }
                _context.Remove(getAcc);
                _context.SaveChanges();
                _notyf.Success("Delete Data Sukses");
            }
            catch (Exception ex)
            {
                _notyf.Error("Delete Data Gagal");

                throw ex;
            }

            return RedirectToAction("UserMaintenance");

        }

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}