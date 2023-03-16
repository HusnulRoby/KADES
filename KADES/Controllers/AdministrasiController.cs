using AspNetCoreHero.ToastNotification.Abstractions;
using KADES.Models;
using KADES.Models.Administrasi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Text;

namespace KADES.Controllers
{
    public class AdministrasiController : Controller
    {
        private readonly ILogger<AdministrasiController> _logger;
        private readonly AppDbContext _context;
        private readonly INotyfService _notyf;
        IWebHostEnvironment _env;

        public AdministrasiController(ILogger<AdministrasiController> logger, AppDbContext context, INotyfService notyf, IWebHostEnvironment env)
        {
            _logger = logger;
            _context = context;
            _notyf = notyf;
            _env = env;

        }

        public IActionResult Index()
        {
            return View();
        }

        #region APARATUR DESA
        public IActionResult AparaturDesa()
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");

            var model = from A in _context.AparaturDesa
                        join B in _context.RFJabatan on A.KODE_JABATAN equals B.KODE_JABATAN
                        select new VW_AparaturDesa()
                        {
                            ID = A.ID,
                            NAMA = A.NAMA,
                            JENIS_KELAMIN = A.JENIS_KELAMIN,
                            SK = A.SK,
                            KODE_JABATAN = A.KODE_JABATAN,
                            JABATAN = B.JABATAN,
                            NIK = A.NIK,
                            NO_TELP = A.NO_TELP,
                            ALAMAT = A.ALAMAT,
                            TGL_MASUK = DateTime.Parse(A.TGL_MASUK.ToString("dd/MM/yyyy")),
                            TGL_BERHENTI = A.TGL_BERHENTI.ToString(),
                        };

            List<SelectListItem> JK = new List<SelectListItem>()
            {
                new SelectListItem { Value = "1", Text = "Perempuan" },
                new SelectListItem { Value = "0", Text = "laki - laki" }
            };

            AdministrasiModels AdministrasiModels = new AdministrasiModels()
            {
                //TemplateSurat = new TemplateSurat(),
                ddlRFJabatan = _context.RFJabatan.Where(x => x.ACTIVE.Equals(true) && x.KODE_TYPE.Equals("APRT")).ToList(),
                ddlJK = JK,
                ListVW_AparaturDesa = model.ToList()
            };

            var getRoles = _context.RFJabatan.Where(x => x.ACTIVE.Equals(true) && x.KODE_TYPE.Equals("APRT")).Select(x => new SelectListItem
            {
                Value = x.KODE_JABATAN,
                Text = x.JABATAN.ToString(),
            });

            ViewBag.ddlJabatan = getRoles;
            ViewBag.ddlJK = JK;

            return View(AdministrasiModels);
        }


        [HttpPost]
        public IActionResult AddAparaturDesa(AparaturDesa AparaturDesa)
        {

            try
            {
                var USERID = HttpContext.Session.GetString("UserId").ToString();
                var getData = new AparaturDesa
                {
                    KODE_JABATAN = AparaturDesa.KODE_JABATAN,
                    SK = AparaturDesa.SK,
                    NAMA = AparaturDesa.NAMA,
                    JENIS_KELAMIN = AparaturDesa.JENIS_KELAMIN,
                    NIK = AparaturDesa.NIK,
                    NO_TELP = AparaturDesa.NO_TELP,
                    ALAMAT = AparaturDesa.ALAMAT,
                    TGL_MASUK = AparaturDesa.TGL_MASUK,
                    CREATED_BY = USERID,
                    CREATED_DATE = DateTime.Now,
                    ACTIVE = true,
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
            return RedirectToAction("AparaturDesa");
        }

        [HttpPost]
        public IActionResult UpdateAparaturDesa(AparaturDesa model)
        {

            _context.AparaturDesa.Update(model);
            _context.SaveChanges();
            _notyf.Success("Update Data Sukses");

            return RedirectToAction("AparaturDesa");
        }

        [HttpPost]
        public IActionResult InactIveAparaturDesa(AparaturDesa model)
        {
            model.ACTIVE = false;
            _context.AparaturDesa.Update(model);
            _context.SaveChanges();
            _notyf.Success("Inactive Data Sukses");

            return RedirectToAction("AparaturDesa");
        }

        #endregion

        public IActionResult DataPenduduk()
        {
            return View();
        }

        #region RAB DESA

        public IActionResult RABDesa()
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");

            var model = new AdministrasiModels()
            {
                ListRAB_DESA = _context.RAB_Desa.ToList(),

            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddRABDesa(RAB_DESA RAB_DESA, IFormFile FILE_UPLOAD)
        {
            var USERID = HttpContext.Session.GetString("UserId").ToString();

            try
            {
                if (FILE_UPLOAD == null)
                {
                    _notyf.Warning("FIle Document Tidak Boleh Kosong!");
                }
                else
                {
                    var fileNama = FILE_UPLOAD.FileName;
                    var pathFolder = Path.Combine(_env.WebRootPath, "Upload/RAB/Lampiran/" + DateTime.Now.ToString("ddMMyyyy"));
                    var fullPath = Path.Combine(pathFolder, fileNama);

                    if (!Directory.Exists(pathFolder))
                    {
                        Directory.CreateDirectory(pathFolder);
                    }

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await FILE_UPLOAD.CopyToAsync(stream);
                    }


                    var getData = new RAB_DESA
                    {
                        ID = Guid.NewGuid().ToString(),
                        JENIS_RAB = RAB_DESA.JENIS_RAB,
                        TGL_RAB = RAB_DESA.TGL_RAB,
                        FILENAME = fileNama,
                        PATH_FILE = fullPath,
                        KETERANGAN = RAB_DESA.KETERANGAN,
                        CREATED_BY = USERID,
                        CREATED_DATE = DateTime.Now,

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
            return RedirectToAction("RABDesa");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRABAsync(RAB_DESA model, IFormFile FILE_UPLOAD)
        {

            try
            {
                var USERID = HttpContext.Session.GetString("UserId").ToString();
                if (FILE_UPLOAD != null)
                {
                    var fileNama = FILE_UPLOAD.FileName;
                    var pathFolder = Path.Combine(_env.WebRootPath, "Upload/RAB/Lampiran/" + DateTime.Now.ToString("ddMMyyyyHHmmss"));
                    var fullPath = Path.Combine(pathFolder, fileNama);

                    if (!Directory.Exists(pathFolder))
                    {
                        Directory.CreateDirectory(pathFolder);
                    }

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await FILE_UPLOAD.CopyToAsync(stream);
                    }

                    model.FILENAME = fileNama;
                    model.PATH_FILE = fullPath;

                }
                model.CREATED_BY = USERID;
                model.CREATED_DATE = DateTime.Now;

                _context.RAB_Desa.Update(model);
                _context.SaveChanges();
                _notyf.Success("Update Data Sukses");

            }
            catch (Exception ex)
            {
                _notyf.Error("Update Data Gagal");
                throw ex;
            }

            return RedirectToAction("RABDesa");
        }

        [HttpPost]
        public IActionResult DeleteRAB(string ID)
        {
            try
            {
                var getAcc = _context.RAB_Desa.Find(ID);
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

            return RedirectToAction("RABDesa");

        }


        [HttpPost]
        public IActionResult DownloadRAB(string ID)
        {
            try
            {
                var FILENAME = "";
                var PATH_FILE = "";

                var getAcc = _context.RAB_Desa.Find(ID);
                if (getAcc == null)
                {
                    return NotFound();
                }

                FILENAME = getAcc.FILENAME;
                PATH_FILE = getAcc.PATH_FILE;

                var net = new System.Net.WebClient();
                var data = net.DownloadData(PATH_FILE);

                var content = new System.IO.MemoryStream(data);
                //byte[] bytes = Encoding.UTF8.GetBytes(PATH_FILE);
                return File(content, "application/octet-stream", FILENAME);
            }
            catch (Exception ex)
            {
                _notyf.Error("Download Data Gagal");

                throw ex;
            }
            //return RedirectToAction("RABDesa");
        }

        #endregion

        #region BPD
        public IActionResult BPD()
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");

            var model = from A in _context.BPD
                        join B in _context.RFJabatan on A.KODE_JABATAN equals B.KODE_JABATAN
                        select new VW_BPD()
                        {
                            ID = A.ID,
                            NAMA = A.NAMA,
                            JENIS_KELAMIN = A.JENIS_KELAMIN,
                            KODE_JABATAN = A.KODE_JABATAN,
                            JABATAN = B.JABATAN,
                            NIK = A.NIK,
                            NO_TELP = A.NO_TELP,
                            ALAMAT = A.ALAMAT,
                            TGL_PENGANGKATAN = DateTime.Parse(A.TGL_PENGANGKATAN.ToString("dd/MM/yyyy")),
                            TGL_PEMBERHENTIAN = A.TGL_PEMBERHENTIAN.ToString(),
                        };

            List<SelectListItem> JK = new List<SelectListItem>()
            {
                new SelectListItem { Value = "1", Text = "Perempuan" },
                new SelectListItem { Value = "0", Text = "laki - laki" }
            };

            AdministrasiModels AdministrasiModels = new AdministrasiModels()
            {
                //TemplateSurat = new TemplateSurat(),
                ddlRFJabatan = _context.RFJabatan.Where(x => x.ACTIVE.Equals(true) && x.KODE_TYPE.Equals("BPD")).ToList(),
                ddlJK = JK,
                ListVW_BPD = model.ToList()
            };

            var getRoles = _context.RFJabatan.Where(x => x.ACTIVE.Equals(true) && x.KODE_TYPE.Equals("BPD")).Select(x => new SelectListItem
            {
                Value = x.KODE_JABATAN,
                Text = x.JABATAN.ToString(),
            });

            ViewBag.ddlJabatan = getRoles;
            ViewBag.ddlJK = JK;

            return View(AdministrasiModels);
        }

        [HttpPost]
        public IActionResult AddBPD(BPD BPD)
        {

            try
            {
                var USERID = HttpContext.Session.GetString("UserId").ToString();
                var getData = new BPD
                {
                    KODE_JABATAN = BPD.KODE_JABATAN,
                    NAMA = BPD.NAMA,
                    JENIS_KELAMIN = BPD.JENIS_KELAMIN,
                    NIK = BPD.NIK,
                    NO_TELP = BPD.NO_TELP,
                    ALAMAT = BPD.ALAMAT,
                    TGL_PENGANGKATAN = BPD.TGL_PENGANGKATAN,
                    CREATED_BY = USERID,
                    CREATED_DATE = DateTime.Now,
                    ACTIVE = true,
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
            return RedirectToAction("BPD");
        }

        [HttpPost]
        public IActionResult UpdateBPD(BPD model)
        {

            _context.BPD.Update(model);
            _context.SaveChanges();
            _notyf.Success("Update Data Sukses");

            return RedirectToAction("BPD");
        }

        [HttpPost]
        public IActionResult InactiveBPD(BPD model)
        {
            model.ACTIVE = false;
            _context.BPD.Update(model);
            _context.SaveChanges();
            _notyf.Success("Inactive Data Sukses");

            return RedirectToAction("BPD");
        }

        public IActionResult KegiatanBPD()
        {
            AdministrasiModels AdministrasiModels = new AdministrasiModels()
            {
                ListKegiatanBPD = _context.KegiatanBPD.ToList(),
            };

            return View(AdministrasiModels);
        }

        [HttpPost]
        public IActionResult AddKegBPD(KegiatanBPD KegiatanBPD)
        {

            try
            {
                var USERID = HttpContext.Session.GetString("UserId").ToString();
                var getData = new KegiatanBPD
                {
                    KEGIATAN = KegiatanBPD.KEGIATAN,
                    KOORDINATOR = KegiatanBPD.KOORDINATOR,
                    DURASI = KegiatanBPD.DURASI,
                    TGL_MULAI = KegiatanBPD.TGL_MULAI,
                    TGL_BERAKHIR = KegiatanBPD.TGL_BERAKHIR
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
            return RedirectToAction("KegiatanBPD");
        }

        [HttpPost]
        public IActionResult UpKegBPD(KegiatanBPD model)
        {

            _context.KegiatanBPD.Update(model);
            _context.SaveChanges();
            _notyf.Success("Update Data Sukses");

            return RedirectToAction("KegiatanBPD");
        }

        [HttpPost]
        public IActionResult DelKegBPD(int ID)
        {
            try
            {
                var getAcc = _context.KegiatanBPD.Find(ID);
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

            return RedirectToAction("KegiatanBPD");

        }

        #endregion

        #region Taruna Karuna
        public IActionResult KarangTaruna()
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");

            var model = from A in _context.KarangTaruna
                        join B in _context.RFJabatan on A.KODE_JABATAN equals B.KODE_JABATAN
                        select new VW_KarangTaruna()
                        {
                            ID = A.ID,
                            NAMA = A.NAMA,
                            JENIS_KELAMIN = A.JENIS_KELAMIN,
                            KODE_JABATAN = A.KODE_JABATAN,
                            JABATAN = B.JABATAN,
                            NIK = A.NIK,
                            NO_TELP = A.NO_TELP,
                            ALAMAT = A.ALAMAT,
                            TGL_PENGANGKATAN = DateTime.Parse(A.TGL_PENGANGKATAN.ToString("dd/MM/yyyy")),
                            TGL_PEMBERHENTIAN = A.TGL_PEMBERHENTIAN.ToString(),
                        };

            List<SelectListItem> JK = new List<SelectListItem>()
            {
                new SelectListItem { Value = "1", Text = "Perempuan" },
                new SelectListItem { Value = "0", Text = "laki - laki" }
            };

            AdministrasiModels AdministrasiModels = new AdministrasiModels()
            {
                //TemplateSurat = new TemplateSurat(),
                ddlRFJabatan = _context.RFJabatan.Where(x => x.ACTIVE.Equals(true) && x.KODE_TYPE.Equals("KTR")).ToList(),
                ddlJK = JK,
                ListVW_KarangTaruna = model.ToList()
            };

            var getRoles = _context.RFJabatan.Where(x => x.ACTIVE.Equals(true) && x.KODE_TYPE.Equals("KTR")).Select(x => new SelectListItem
            {
                Value = x.KODE_JABATAN,
                Text = x.JABATAN.ToString(),
            });

            ViewBag.ddlJabatan = getRoles;
            ViewBag.ddlJK = JK;

            return View(AdministrasiModels);
        }

        [HttpPost]
        public IActionResult AddTaruna(KarangTaruna KarangTaruna)
        {

            try
            {
                var USERID = HttpContext.Session.GetString("UserId").ToString();
                var getData = new KarangTaruna
                {
                    KODE_JABATAN = KarangTaruna.KODE_JABATAN,
                    NAMA = KarangTaruna.NAMA,
                    JENIS_KELAMIN = KarangTaruna.JENIS_KELAMIN,
                    NIK = KarangTaruna.NIK,
                    NO_TELP = KarangTaruna.NO_TELP,
                    ALAMAT = KarangTaruna.ALAMAT,
                    TGL_PENGANGKATAN = KarangTaruna.TGL_PENGANGKATAN,
                    CREATED_BY = USERID,
                    CREATED_DATE = DateTime.Now,
                    ACTIVE = true,
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
            return RedirectToAction("KarangTaruna");
        }

        [HttpPost]
        public IActionResult UpdateTaruna(KarangTaruna model)
        {

            _context.KarangTaruna.Update(model);
            _context.SaveChanges();
            _notyf.Success("Update Data Sukses");

            return RedirectToAction("KarangTaruna");
        }

        [HttpPost]
        public IActionResult InactiveTaruna(KarangTaruna model)
        {
            model.ACTIVE = false;
            _context.KarangTaruna.Update(model);
            _context.SaveChanges();
            _notyf.Success("Inactive Data Sukses");

            return RedirectToAction("KarangTaruna");
        }
        public IActionResult KegiatanKarangTaruna()
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");
            AdministrasiModels AdministrasiModels = new AdministrasiModels()
            {
                ListKegiatanTaruna = _context.KegiatanTaruna.ToList(),
            };

            return View(AdministrasiModels);
        }

        [HttpPost]
        public IActionResult AddKegTaruna(KegiatanTaruna KegiatanTaruna)
        {

            try
            {
                var USERID = HttpContext.Session.GetString("UserId").ToString();
                var getData = new KegiatanTaruna
                {
                    KEGIATAN = KegiatanTaruna.KEGIATAN,
                    KOORDINATOR = KegiatanTaruna.KOORDINATOR,
                    DURASI = KegiatanTaruna.DURASI,
                    TGL_MULAI = KegiatanTaruna.TGL_MULAI,
                    TGL_BERAKHIR = KegiatanTaruna.TGL_BERAKHIR
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
            return RedirectToAction("KegiatanKarangTaruna");
        }

        [HttpPost]
        public IActionResult UpKegTaruna(KegiatanTaruna model)
        {

            _context.KegiatanTaruna.Update(model);
            _context.SaveChanges();
            _notyf.Success("Update Data Sukses");

            return RedirectToAction("KegiatanKarangTaruna");
        }

        [HttpPost]
        public IActionResult DelKegTaruna(int ID)
        {
            try
            {
                var getAcc = _context.KegiatanTaruna.Find(ID);
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

            return RedirectToAction("KegiatanKarangTaruna");

        }
        #endregion

        public IActionResult PKK()
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