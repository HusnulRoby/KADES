using AspNetCoreHero.ToastNotification.Abstractions;
using KADES.Models;
using KADES.Models.Administrasi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Text;

namespace KADES.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly ILogger<AdministrationController> _logger;
        private readonly AppDbContext _context;
        private readonly INotyfService _notyf;
        IWebHostEnvironment _env;

        public AdministrationController(ILogger<AdministrationController> logger, AppDbContext context, INotyfService notyf, IWebHostEnvironment env)
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
                            CREATED_BY=A.CREATED_BY,
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
                    TGL_BERHENTI=null,
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
            try
            {
                var data = _context.AparaturDesa.Where(x => x.ID.Equals(model.ID)).FirstOrDefault();
                if (data != null)
                {
                    data.NAMA=model.NAMA;
                    data.KODE_JABATAN=model.KODE_JABATAN;
                    data.SK=model.SK;
                    data.JENIS_KELAMIN=model.JENIS_KELAMIN;
                    data.NIK=model.NIK;
                    data.NO_TELP=model.NO_TELP;
                    data.ALAMAT=model.ALAMAT;
                    data.TGL_MASUK=model.TGL_MASUK;

                    _context.AparaturDesa.Update(data);
                    _context.SaveChanges();
                    _notyf.Success("Update Data Sukses");
                }
                
            }
            catch (Exception ex)
            {
                _notyf.Error("Update data gagal.");
                //throw ex;
            }
            

            return RedirectToAction("AparaturDesa");
        }

        [HttpPost]
        public IActionResult InactIveAparaturDesa(AparaturDesa model)
        {
            try
            {
                var data = _context.AparaturDesa.Where(x => x.ID.Equals(model.ID)).FirstOrDefault();
                if (data != null)
                {
                    data.ACTIVE = false;
                    data.TGL_BERHENTI=model.TGL_BERHENTI;

                    _context.AparaturDesa.Update(data);
                    _context.SaveChanges();
                    _notyf.Success("Inactive Data Sukses");
                }
            }
            catch (Exception ex)
            {
                _notyf.Success("Inactive Data Gagal");
            }
            

            return RedirectToAction("AparaturDesa");
        }

        #endregion

        #region PENDUDUK
        public IActionResult DataPenduduk()
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");

            var model = from A in _context.Penduduk
                        join B in _context.RfAgama on A.ID_AGAMA equals B.ID
                        join D in _context.RfDusun on A.ID_DUSUN equals D.ID
                        join E in _context.RfPendidikan on A.ID_PENDIDIKAN equals E.ID
                        join F in _context.RfPekerjaan on A.ID_PEKERJAAN equals F.ID
                        select new VW_Penduduk()
                        {
                            ID = A.ID,
                            NIK = A.NIK,
                            NAMA = A.NAMA,
                            NO_AKTA= A.NO_AKTA,
                            KK= A.KK,
                            POB=A.POB,
                            DOB= DateTime.Parse(A.DOB.ToString("dd/MM/yyyy")),
                            JENIS_KELAMIN = A.JENIS_KELAMIN,
                            ID_DLMKELUARGA = A.ID_DLMKELUARGA,
                            ID_AGAMA = A.ID_AGAMA,
                            AGAMA = B.AGAMA,
                            NIK_AYAH=A.NIK_AYAH,
                            NAMA_AYAH=A.NAMA_AYAH,
                            NIK_IBU=A.NIK_IBU,
                            NAMA_IBU=A.NAMA_IBU,
                            ALAMAT = A.ALAMAT,
                            NO_TELP = A.NO_TELP,
                            ID_DUSUN = A.ID_DUSUN,
                            DUSUN=D.DUSUN,
                            RW = A.RW,
                            RT = A.RT,
                            ID_PENDIDIKAN= A.ID_PENDIDIKAN,
                            PENDIDIKAN = E.PENDIDIKAN,
                            ID_PEKERJAAN = A.ID_PEKERJAAN,
                            PEKERJAAN = F.PEKERJAAN,
                            ID_KAWIN=A.ID_KAWIN,
                            ID_STATUS= A.ID_STATUS,

                        };

            List<SelectListItem> JK = new List<SelectListItem>()
            {
                new SelectListItem { Value = "1", Text = "Perempuan" },
                new SelectListItem { Value = "0", Text = "laki - laki" }
            };

            List<SelectListItem> DlmKeluarga = new List<SelectListItem>()
            {
                new SelectListItem { Value = "0", Text = "Ayah" },
                new SelectListItem { Value = "1", Text = "Ibu" },
                new SelectListItem { Value = "2", Text = "Anak" }
            };

            List<SelectListItem> StatusHidup = new List<SelectListItem>()
            {
                new SelectListItem { Value = "0", Text = "Hidup" },
                new SelectListItem { Value = "1", Text = "Mati" }
            };

            List<SelectListItem> StatusKawin = new List<SelectListItem>()
            {
                new SelectListItem { Value = "0", Text = "Kawin" },
                new SelectListItem { Value = "1", Text = "Belum Kawin" }
            };

            AdministrasiModels AdministrasiModels = new AdministrasiModels()
            {
                //TemplateSurat = new TemplateSurat(),
                ddlRfAgama = _context.RfAgama.Where(x => x.ACTIVE.Equals(true)).ToList(),
                ddlDlmKeluarga = DlmKeluarga,
                ddlRfDusun = _context.RfDusun.Where(x => x.ACTIVE.Equals(true)).ToList(),
                ddlStatusHidup = StatusHidup,
                ddlStatusKawin = StatusKawin,
                ddlJK = JK,
                ddlRfPendidikan = _context.RfPendidikan.Where(x => x.ACTIVE.Equals(true)).ToList(),
                ddlRfPekerjaan = _context.RfPekerjaan.Where(x => x.ACTIVE.Equals(true)).ToList(),
                ListVW_Penduduk = model.ToList()
            };

            var getAgama = _context.RfAgama.Where(x => x.ACTIVE.Equals(true)).Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.AGAMA.ToString(),
            });

            var getDusun = _context.RfDusun.Where(x => x.ACTIVE.Equals(true)).Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.DUSUN.ToString(),
            });

            var getPendidikan = _context.RfPendidikan.Where(x => x.ACTIVE.Equals(true)).Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.PENDIDIKAN.ToString(),
            });

            var getPekerjaan = _context.RfPekerjaan.Where(x => x.ACTIVE.Equals(true)).Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.PEKERJAAN.ToString(),
            });

            ViewBag.ddlAgama = getAgama;
            ViewBag.ddlDlmKeluarga = DlmKeluarga;
            ViewBag.ddlDusun = getDusun;
            ViewBag.ddlStatusHidup = StatusHidup;
            ViewBag.ddlStatusKawin = StatusKawin;
            ViewBag.ddlRfPendidikan = getPendidikan;
            ViewBag.ddlRfPekerjaan = getPekerjaan;
            ViewBag.ddlJK = JK;

            return View(AdministrasiModels);
        }

        [HttpPost]
        public IActionResult AddPenduduk(Penduduk Penduduk)
        {

            try
            {
                var USERID = HttpContext.Session.GetString("UserId").ToString();
                var getData = new Penduduk
                {
                    NIK= Penduduk.NIK,
                    NAMA= Penduduk.NAMA,
                    KK= Penduduk.KK,
                    NO_AKTA= Penduduk.NO_AKTA,
                    POB= Penduduk.POB,
                    DOB= Penduduk.DOB,
                    JENIS_KELAMIN=Penduduk.JENIS_KELAMIN,
                    ID_DLMKELUARGA= Penduduk.ID_DLMKELUARGA,
                    ID_AGAMA=Penduduk.ID_AGAMA,
                    NIK_AYAH=Penduduk.NIK_AYAH,
                    NAMA_AYAH= Penduduk.NAMA_AYAH,
                    NIK_IBU= Penduduk.NIK_IBU,
                    NAMA_IBU= Penduduk.NAMA_IBU,
                    ALAMAT = Penduduk.ALAMAT,
                    NO_TELP = Penduduk.NO_TELP,
                    ID_DUSUN = Penduduk.ID_DUSUN,
                    RT = Penduduk.RT,
                    RW = Penduduk.RW,
                    ID_PENDIDIKAN = Penduduk.ID_PENDIDIKAN,
                    ID_PEKERJAAN = Penduduk.ID_PEKERJAAN,
                    ID_KAWIN = Penduduk.ID_KAWIN,
                    ID_STATUS = Penduduk.ID_STATUS,
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
            return RedirectToAction("Penduduk");
        }

        [HttpPost]
        public IActionResult UpdatePenduduk(Penduduk model)
        {
            try
            {
                var data = _context.Penduduk.Where(x => x.ID.Equals(model.ID)).FirstOrDefault();
                if (data != null)
                {
                    data.ID = model.ID;
                    data.NIK= model.NIK;
                    data.NAMA= model.NAMA;
                    data.KK= model.KK;
                    data.NO_AKTA= model.NO_AKTA;
                    data.POB= model.POB;
                    data.DOB= model.DOB;
                    data.JENIS_KELAMIN= model.JENIS_KELAMIN;
                    data.ID_DLMKELUARGA = model.ID_DLMKELUARGA;
                    data.ID_AGAMA= model.ID_AGAMA;
                    data.NIK_AYAH= model.NIK_AYAH;
                    data.NAMA_AYAH = model.NAMA_AYAH;
                    data.NIK_IBU= model.NIK_IBU;
                    data.NAMA_IBU= model.NAMA_IBU;
                    data.ALAMAT= model.ALAMAT;
                    data.NO_TELP= model.NO_TELP;
                    data.ID_DUSUN= model.ID_DUSUN;
                    data.RW= model.RW;
                    data.RT= model.RT;
                    data.ID_PENDIDIKAN= model.ID_PENDIDIKAN;
                    data.ID_PEKERJAAN= model.ID_PEKERJAAN;
                    data.ID_KAWIN= model.ID_KAWIN;
                    data.ID_STATUS= model.ID_STATUS;

                    _context.Penduduk.Update(data);
                    _context.SaveChanges();
                    _notyf.Success("Update Data Sukses");
                }
            }
            catch (Exception)
            {
                _notyf.Error("Update Data Gagal");
            }


            return RedirectToAction("Penduduk");
        }

        #endregion

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
                var data = _context.RAB_Desa.Where(x => x.ID.Equals(model.ID)).FirstOrDefault();

                if (FILE_UPLOAD != null)
                {

                    data.JENIS_RAB = model.JENIS_RAB;
                    data.TGL_RAB= model.TGL_RAB;
                    data.KETERANGAN= model.KETERANGAN;
                    data.CREATED_BY = USERID;
                    data.CREATED_DATE = DateTime.Now;
                    data.FILENAME= FILE_UPLOAD.FileName;
                    var pathFolder = Path.Combine(_env.WebRootPath, "Upload/RAB/Lampiran/" + DateTime.Now.ToString("ddMMyyyyHHmmss"));
                    data.PATH_FILE= Path.Combine(pathFolder, data.FILENAME);

                    if (!Directory.Exists(pathFolder))
                    {
                        Directory.CreateDirectory(pathFolder);
                    }

                    using (var stream = new FileStream(data.PATH_FILE, FileMode.Create))
                    {
                        await FILE_UPLOAD.CopyToAsync(stream);
                    }

                }

                _context.RAB_Desa.Update(data);
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
                            CREATED_BY=A.CREATED_BY,
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
                    TGL_PEMBERHENTIAN=null,
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
            try
            {
                var data = _context.BPD.Where(x => x.ID.Equals(model.ID)).FirstOrDefault();
                if (data != null)
                {
                    data.KODE_JABATAN=model.KODE_JABATAN;
                    data.NAMA=model.NAMA;
                    data.JENIS_KELAMIN=model.JENIS_KELAMIN;
                    data.NIK=model.NIK;
                    data.NO_TELP=model.NO_TELP;
                    data.ALAMAT=model.ALAMAT;
                    data.TGL_PENGANGKATAN = model.TGL_PENGANGKATAN;

                    _context.BPD.Update(data);
                    _context.SaveChanges();
                    _notyf.Success("Update Data Sukses");
                }
            }
            catch (Exception)
            {
                _notyf.Error("Update Data Gagal");
            }
            

            return RedirectToAction("BPD");
        }

        [HttpPost]
        public IActionResult InactiveBPD(BPD model)
        {
            try
            {
                var data = _context.BPD.Where(x => x.ID.Equals(model.ID)).FirstOrDefault();
                if (data != null)
                {
                    data.ACTIVE = false;
                    data.TGL_PEMBERHENTIAN = model.TGL_PEMBERHENTIAN;

                    _context.BPD.Update(data);
                    _context.SaveChanges();
                    _notyf.Success("Inactive Data Sukses");
                }
            }
            catch (Exception)
            {
                _notyf.Error("Inactive Data Sukses");
            }
            

            return RedirectToAction("BPD");
        }

        public IActionResult KegiatanBPD()
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");
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
            try
            {
                var data = _context.KegiatanBPD.Where(x => x.ID.Equals(model.ID)).FirstOrDefault();
                if (data!=null)
                {
                    data.KEGIATAN= model.KEGIATAN;
                    data.KOORDINATOR= model.KOORDINATOR;
                    data.TGL_MULAI= model.TGL_MULAI;
                    data.TGL_BERAKHIR= model.TGL_BERAKHIR;

                    _context.KegiatanBPD.Update(data);
                    _context.SaveChanges();
                    _notyf.Success("Update Data Sukses");
                }
            }
            catch (Exception ex)
            {
                _notyf.Error("Update Data Gagal");

            }



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
                            CREATED_BY=A.CREATED_BY,
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
            try
            {
                var data = _context.KarangTaruna.Where(x => x.ID.Equals(model.ID)).FirstOrDefault();

                data.KODE_JABATAN = model.KODE_JABATAN;
                data.NAMA=model.NAMA;
                data.JENIS_KELAMIN=model.JENIS_KELAMIN;
                data.NIK=model.NIK;
                data.NO_TELP=model.NO_TELP;
                data.ALAMAT=model.ALAMAT;
                data.TGL_PENGANGKATAN = model.TGL_PENGANGKATAN;

                _context.KarangTaruna.Update(data);
                _context.SaveChanges();
                _notyf.Success("Update Data Sukses");
            }
            catch (Exception)
            {
                _notyf.Error("Update Data Gagal");

            }


            return RedirectToAction("KarangTaruna");
        }

        [HttpPost]
        public IActionResult InactiveTaruna(KarangTaruna model)
        {
            try
            {
                var data = _context.KarangTaruna.Where(x => x.ID.Equals(model.ID)).FirstOrDefault();

                data.ACTIVE = false;
                data.TGL_PEMBERHENTIAN = model.TGL_PEMBERHENTIAN;

                _context.KarangTaruna.Update(data);
                _context.SaveChanges();
                _notyf.Success("Inactive Data Sukses");
            }
            catch (Exception)
            {
                _notyf.Error("Inactive Data Gagal");

            }


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
            }
            return RedirectToAction("KegiatanKarangTaruna");
        }

        [HttpPost]
        public IActionResult UpKegTaruna(KegiatanTaruna model)
        {
            try
            {
                var data = _context.KegiatanTaruna.Where(x => x.ID.Equals(model.ID)).FirstOrDefault();

                data.KEGIATAN = model.KEGIATAN;
                data.KOORDINATOR= model.KOORDINATOR;
                data.TGL_MULAI= model.TGL_MULAI;
                data.TGL_BERAKHIR= model.TGL_BERAKHIR;

                _context.KegiatanTaruna.Update(data);
                _context.SaveChanges();
                _notyf.Success("Update Data Sukses");
            }
            catch (Exception)
            {
                _notyf.Error("Update Data Sukses");

            }


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

        #region PKK
        public IActionResult PKK()
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");

            var model = from A in _context.PKK
                        join B in _context.RFJabatan on A.KODE_JABATAN equals B.KODE_JABATAN
                        select new VW_PKK()
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
                            CREATED_BY=A.CREATED_BY,
                        };

            List<SelectListItem> JK = new List<SelectListItem>()
            {
                new SelectListItem { Value = "1", Text = "Perempuan" },
                new SelectListItem { Value = "0", Text = "laki - laki" }
            };

            AdministrasiModels AdministrasiModels = new AdministrasiModels()
            {
                //TemplateSurat = new TemplateSurat(),
                ddlRFJabatan = _context.RFJabatan.Where(x => x.ACTIVE.Equals(true) && x.KODE_TYPE.Equals("PKK")).ToList(),
                ddlJK = JK,
                ListVW_PKK = model.ToList()
            };

            var getRoles = _context.RFJabatan.Where(x => x.ACTIVE.Equals(true) && x.KODE_TYPE.Equals("PKK")).Select(x => new SelectListItem
            {
                Value = x.KODE_JABATAN,
                Text = x.JABATAN.ToString(),
            });

            ViewBag.ddlJabatan = getRoles;
            ViewBag.ddlJK = JK;

            return View(AdministrasiModels);
        }

        [HttpPost]
        public IActionResult AddPKK(PKK PKK)
        {

            try
            {
                var USERID = HttpContext.Session.GetString("UserId").ToString();
                var getData = new PKK
                {
                    KODE_JABATAN = PKK.KODE_JABATAN,
                    NAMA = PKK.NAMA,
                    JENIS_KELAMIN = PKK.JENIS_KELAMIN,
                    NIK = PKK.NIK,
                    NO_TELP = PKK.NO_TELP,
                    ALAMAT = PKK.ALAMAT,
                    TGL_PENGANGKATAN = PKK.TGL_PENGANGKATAN,
                    TGL_PEMBERHENTIAN=null,
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
            return RedirectToAction("PKK");
        }

        [HttpPost]
        public IActionResult UpdatePKK(PKK model)
        {
            try
            {
                var data = _context.PKK.Where(x => x.ID.Equals(model.ID)).FirstOrDefault();

                data.KODE_JABATAN = model.KODE_JABATAN;
                data.NAMA=model.NAMA;
                data.JENIS_KELAMIN=model.JENIS_KELAMIN;
                data.NIK=model.NIK;
                data.NO_TELP=model.NO_TELP;
                data.ALAMAT=model.ALAMAT;
                data.TGL_PENGANGKATAN = model.TGL_PENGANGKATAN;

                _context.PKK.Update(data);
                _context.SaveChanges();
                _notyf.Success("Update Data Sukses");
            }
            catch (Exception)
            {
                _notyf.Error("Update Data Gagal");

            }


            return RedirectToAction("PKK");
        }

        [HttpPost]
        public IActionResult InactivePKK(PKK model)
        {
            try
            {
                var data = _context.PKK.Where(x => x.ID.Equals(model.ID)).FirstOrDefault();

                data.ACTIVE = false;
                data.TGL_PEMBERHENTIAN = model.TGL_PEMBERHENTIAN;

                _context.PKK.Update(data);
                _context.SaveChanges();
                _notyf.Success("Inactive Data Sukses");
            }
            catch (Exception)
            {
                _notyf.Error("Inactive Data Sukses");

            }


            return RedirectToAction("PKK");
        }

        public IActionResult KegiatanPKK()
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");
            AdministrasiModels AdministrasiModels = new AdministrasiModels()
            {
                ListKegiatanPKK = _context.KegiatanPKK.ToList(),
            };

            return View(AdministrasiModels);
        }

        [HttpPost]
        public IActionResult AddKegPKK(KegiatanPKK KegiatanPKK)
        {

            try
            {
                var USERID = !string.IsNullOrEmpty(HttpContext.Session.GetString("UserId"))? HttpContext.Session.GetString("UserId").ToString():"";
                var getData = new KegiatanPKK
                {
                    KEGIATAN = KegiatanPKK.KEGIATAN,
                    KOORDINATOR = KegiatanPKK.KOORDINATOR,
                    TGL_MULAI = KegiatanPKK.TGL_MULAI,
                    TGL_BERAKHIR = KegiatanPKK.TGL_BERAKHIR
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
            return RedirectToAction("KegiatanPKK");
        }

        [HttpPost]
        public IActionResult UpKegPKK(KegiatanPKK model)
        {
            try
            {
                var data = _context.KegiatanPKK.Where(x => x.ID.Equals(model.ID)).FirstOrDefault();

                data.KEGIATAN= model.KEGIATAN;
                data.KOORDINATOR= model.KOORDINATOR;
                data.TGL_MULAI= model.TGL_MULAI;
                data.TGL_BERAKHIR=  model.TGL_BERAKHIR;

                _context.KegiatanPKK.Update(data);
                _context.SaveChanges();
                _notyf.Success("Update Data Sukses");
            }
            catch (Exception)
            {
                _notyf.Error("Update Data Sukses");

            }


            return RedirectToAction("KegiatanPKK");
        }

        [HttpPost]
        public IActionResult DelKegPKK(int ID)
        {
            try
            {
                var getAcc = _context.KegiatanPKK.Find(ID);
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

            return RedirectToAction("KegiatanPKK");

        }

        #endregion

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}