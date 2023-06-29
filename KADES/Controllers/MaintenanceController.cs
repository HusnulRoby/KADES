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

            List<SelectListItem> catJabatan = new List<SelectListItem>()
            {
                new SelectListItem { Value = "APR", Text = "Aparatur Desa" },
                new SelectListItem { Value = "BPD", Text = "BPD" },
                new SelectListItem { Value = "KTR", Text = "Karang Taruna" },
                new SelectListItem { Value = "PKK", Text = "PKK" }
            };

            ViewBag.ddlCat = catJabatan;

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
            try
            {
                _context.RFJabatan.Update(model);
                _context.SaveChanges();
                _notyf.Success("Update Data Sukses");
            }
            catch (Exception ex)
            {
                _notyf.Error("Update Data Gagal");

            }


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

        #region Maintenance Dusun
        public IActionResult Dusun()
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");

            MaintenanceModels maintenanceModel = new MaintenanceModels()
            {
                ListRfDusun = _context.RfDusun.ToList()
            };

            return View(maintenanceModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddDusun(RfDusun RfDusun)
        {
            var USERID = HttpContext.Session.GetString("UserId").ToString();

            try
            {
                var cekData = _context.RfDusun.Where(x => x.DUSUN.Equals(RfDusun.DUSUN)).Count();
                if (cekData > 0)
                {
                    _notyf.Error("Dusun Sudah Ada");
                }
                else
                {
                    var getData = new RfDusun
                    {
                        DUSUN = RfDusun.DUSUN,
                        ACTIVE = RfDusun.ACTIVE
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
            return RedirectToAction("Dusun");
        }

        [HttpPost]
        public IActionResult UpDusun(RfDusun model)
        {
            try
            {
                _context.RfDusun.Update(model);
                _context.SaveChanges();
                _notyf.Success("Update Data Sukses");
            }
            catch (Exception ex)
            {
                _notyf.Error("Update Data Gagal");

            }


            return RedirectToAction("Dusun");
        }

        [HttpPost]
        public IActionResult DelRfDusun(int ID)
        {
            try
            {
                var getAcc = _context.RfDusun.Find(ID);
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

            return RedirectToAction("Dusun");

        }

        #endregion

        #region Maitenance Agama

        public IActionResult Agama()
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");

            MaintenanceModels maintenanceModel = new MaintenanceModels()
            {
                ListRfAgama = _context.RfAgama.ToList()
            };

            return View(maintenanceModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddAgama(RfAgama RfAgama)
        {
            var USERID = HttpContext.Session.GetString("UserId").ToString();

            try
            {
                var cekData = _context.RfAgama.Where(x => x.AGAMA.Equals(RfAgama.AGAMA)).Count();
                if (cekData > 0)
                {
                    _notyf.Error("Data Sudah Ada");
                }
                else
                {
                    var getData = new RfAgama
                    {
                        AGAMA = RfAgama.AGAMA,
                        ACTIVE = RfAgama.ACTIVE
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
            return RedirectToAction("Agama");
        }

        [HttpPost]
        public IActionResult UpAgama(RfAgama model)
        {
            try
            {
                _context.RfAgama.Update(model);
                _context.SaveChanges();
                _notyf.Success("Update Data Sukses");
            }
            catch (Exception ex)
            {
                _notyf.Error("Update Data Gagal");

            }


            return RedirectToAction("Agama");
        }

        [HttpPost]
        public IActionResult DelRfAgama(int ID)
        {
            try
            {
                var getAcc = _context.RfAgama.Find(ID);
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

            return RedirectToAction("Agama");

        }

        #endregion

        #region Maitenance Pendidikan

        public IActionResult Pendidikan()
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");

            MaintenanceModels maintenanceModel = new MaintenanceModels()
            {
                ListRfPendidikan = _context.RfPendidikan.ToList()
            };

            return View(maintenanceModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddPendidikan(RfPendidikan RfPendidikan)
        {
            var USERID = HttpContext.Session.GetString("UserId").ToString();

            try
            {
                var cekData = _context.RfPendidikan.Where(x => x.PENDIDIKAN.Equals(RfPendidikan.PENDIDIKAN)).Count();
                if (cekData > 0)
                {
                    _notyf.Error("Jenis Pendidikan Sudah Ada");
                }
                else
                {
                    var getData = new RfPendidikan
                    {
                        PENDIDIKAN = RfPendidikan.PENDIDIKAN,
                        ACTIVE = RfPendidikan.ACTIVE
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
            return RedirectToAction("Pendidikan");
        }

        [HttpPost]
        public IActionResult UpPendidikan(RfPendidikan model)
        {
            try
            {
                _context.RfPendidikan.Update(model);
                _context.SaveChanges();
                _notyf.Success("Update Data Sukses");
            }
            catch (Exception ex)
            {
                _notyf.Error("Update Data Gagal");

            }


            return RedirectToAction("Pendidikan");
        }

        [HttpPost]
        public IActionResult DelRfPendidikan(int ID)
        {
            try
            {
                var getAcc = _context.RfPendidikan.Find(ID);
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

            return RedirectToAction("Pendidikan");

        }

        #endregion

        #region Maitenance Pekerjaan

        public IActionResult Pekerjaan()
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");

            MaintenanceModels maintenanceModel = new MaintenanceModels()
            {
                ListRfPekerjaan = _context.RfPekerjaan.ToList()
            };

            return View(maintenanceModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddPekerjaan(RfPekerjaan RfPekerjaan)
        {
            var USERID = HttpContext.Session.GetString("UserId").ToString();

            try
            {
                var cekData = _context.RfPekerjaan.Where(x => x.PEKERJAAN.Equals(RfPekerjaan.PEKERJAAN)).Count();
                if (cekData > 0)
                {
                    _notyf.Error("jenis Pekerjaan Sudah Ada");
                }
                else
                {
                    var getData = new RfPekerjaan
                    {
                        PEKERJAAN = RfPekerjaan.PEKERJAAN,
                        ACTIVE = RfPekerjaan.ACTIVE
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
            return RedirectToAction("Pekerjaan");
        }

        [HttpPost]
        public IActionResult UpPekerjaan(RfPekerjaan model)
        {
            try
            {
                _context.RfPekerjaan.Update(model);
                _context.SaveChanges();
                _notyf.Success("Update Data Sukses");
            }
            catch (Exception ex)
            {
                _notyf.Error("Update Data Gagal");

            }


            return RedirectToAction("Pekerjaan");
        }

        [HttpPost]
        public IActionResult DelRfPekerjaan(int ID)
        {
            try
            {
                var getAcc = _context.RfPekerjaan.Find(ID);
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

            return RedirectToAction("Pekerjaan");

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
                            ID=A.ID,
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
            try
            {
                _context.RFUsers.Update(model);
                _context.SaveChanges();
                _notyf.Success("Update Data Sukses");
            }
            catch (Exception ex)
            {
                _notyf.Error("Update Data Sukses");

            }


            return RedirectToAction("UserMaintenance");
        }

        [HttpPost]
        public IActionResult DeleteUsers(int ID)
        {
            try
            {
                var getAcc = _context.RFUsers.Find(ID);
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

            }

            return RedirectToAction("UserMaintenance");

        }

        #endregion

        #region Maitenance Jenis Asset

        public IActionResult JenisAset()
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");

            MaintenanceModels maintenanceModel = new MaintenanceModels()
            {
                ListJenisAset = _context.RfJenisAset.ToList()
            };

            return View(maintenanceModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddJnsAset(RfJenisAset JenisAset)
        {
            var USERID = HttpContext.Session.GetString("UserId").ToString();

            try
            {
                var cekData = _context.RfJenisAset.Where(x => x.KODE_JNSASET.Equals(JenisAset.KODE_JNSASET)).Count();
                if (cekData > 0)
                {
                    _notyf.Error("Kode Jenis Aset Sudah Ada");
                }
                else
                {
                    var getData = new RfJenisAset
                    {
                        KODE_JNSASET = JenisAset.KODE_JNSASET,
                        JENIS_ASET = JenisAset.JENIS_ASET,
                        ACTIVE = JenisAset.ACTIVE
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
            return RedirectToAction("JenisAset");
        }

        [HttpPost]
        public IActionResult UpJnsAset(RfJenisAset model)
        {
            try
            {
                _context.RfJenisAset.Update(model);
                _context.SaveChanges();
                _notyf.Success("Update Data Sukses");
            }
            catch (Exception ex)
            {
                _notyf.Error("Update Data Gagal");

            }


            return RedirectToAction("JenisAset");
        }

        [HttpPost]
        public IActionResult DelJnsAset(int ID)
        {
            try
            {
                var getAcc = _context.RfJenisAset.Find(ID);
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

            return RedirectToAction("JenisAset");

        }

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}