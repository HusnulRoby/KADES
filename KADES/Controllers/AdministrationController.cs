using AspNetCoreHero.ToastNotification.Abstractions;
using KADES.Models;
using KADES.Models.Administrasi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using System.Data;
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
                            SK_BERHENTI = A.SK_BERHENTI,
                            KODE_JABATAN = A.KODE_JABATAN,
                            JABATAN = B.JABATAN,
                            NIK = A.NIK,
                            NO_TELP = A.NO_TELP,
                            ALAMAT = A.ALAMAT,
                            TGL_MASUK = DateTime.Parse(A.TGL_MASUK.ToString("dd/MM/yyyy")),
                            TGL_BERHENTI = A.TGL_BERHENTI.ToString()
                        };

            List<SelectListItem> JK = new List<SelectListItem>()
            {
                new SelectListItem { Value = "P", Text = "Perempuan" },
                new SelectListItem { Value = "L", Text = "Laki - laki" }
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
                    SK_BERHENTI = null,
                    NAMA = AparaturDesa.NAMA,
                    JENIS_KELAMIN = AparaturDesa.JENIS_KELAMIN,
                    NIK = AparaturDesa.NIK,
                    NO_TELP = AparaturDesa.NO_TELP,
                    ALAMAT = AparaturDesa.ALAMAT,
                    TGL_MASUK = AparaturDesa.TGL_MASUK,
                    TGL_BERHENTI = null,
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
                _context.AparaturDesa.Update(model);
                _context.SaveChanges();
                _notyf.Success("Update Data Sukses");
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
                _context.AparaturDesa.Update(model);
                _context.SaveChanges();
                _notyf.Success("Inactive Data Sukses");

            }
            catch (Exception ex)
            {
                _notyf.Error("Inactive Data Gagal");
            }


            return RedirectToAction("AparaturDesa");
        }

        public ActionResult DownloadExcelAPR()
        {
            DataTable dt = new DataTable();
            IQueryable<VW_AparaturDesa>? Query;


            string[] listHeaders = new string[]
            {
                "NIK",
                "NAMA",
                "SK PENGANGKATAN",
                "JENIS KELAMIN",
                "JABATAN",
                "ALAMAT",
                "NOMOR TELP",
                "TANGGAL PENGANGKATAN",
                "SK PEMBERHENTIAN",
                "TANGGAL PEMBERHENTIAN"
            };

            Query = from A in _context.AparaturDesa
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
                        TGL_MASUK = A.TGL_MASUK,
                        SK_BERHENTI=A.SK_BERHENTI,
                        TGL_BERHENTI = A.TGL_BERHENTI.ToString()
                    };

            for (int i = 0; i < listHeaders.Length; i++)
            {
                dt.Columns.Add(listHeaders[i]);
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Data");

                //Write column headers
                for (var col = 0; col < dt.Columns.Count; col++)
                {
                    worksheet.Cells[1, col + 1].Value = dt.Columns[col].ColumnName;
                }

                var JK = "";
                var skBerhenti = "";
                var tglBerhenti = "";
                foreach (var item in Query)
                {
                    JK = item.JENIS_KELAMIN.Equals('P') ? "Perempuan" : "Laki-laki";
                    skBerhenti = !string.IsNullOrEmpty(item.SK_BERHENTI) ? item.SK_BERHENTI : "";
                    tglBerhenti = string.IsNullOrEmpty(item.TGL_BERHENTI) ? "-" : string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(item.TGL_BERHENTI));
                    dt.Rows.Add(item.NIK, item.NAMA, item.SK, JK, item.JABATAN, item.ALAMAT, item.NO_TELP, item.TGL_MASUK.ToString("dd/MM/yyyy"), skBerhenti,tglBerhenti);
                }



                //Write data rows using LINQ query
                var dataRows = dt.AsEnumerable();
                int row = 2;
                foreach (var dataRow in dataRows)
                {
                    for (var col = 0; col < dt.Columns.Count; col++)
                    {
                        worksheet.Cells[row, col + 1].Value = dataRow[col];
                    }
                    row++;
                }

                // Generate the Excel file as a byte array
                var excelBytes = package.GetAsByteArray();

                // Return the Excel file as a downloadable file
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DataAparaturDesa" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx");
            }
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
                            NO_AKTA = A.NO_AKTA,
                            KK = A.KK,
                            POB = A.POB,
                            DOB = DateTime.Parse(A.DOB.ToString("dd/MM/yyyy")),
                            JENIS_KELAMIN = A.JENIS_KELAMIN,
                            ID_DLMKELUARGA = A.ID_DLMKELUARGA,
                            ID_AGAMA = A.ID_AGAMA,
                            AGAMA = B.AGAMA,
                            NIK_AYAH = A.NIK_AYAH,
                            NAMA_AYAH = A.NAMA_AYAH,
                            NIK_IBU = A.NIK_IBU,
                            NAMA_IBU = A.NAMA_IBU,
                            ALAMAT = A.ALAMAT,
                            NO_TELP = A.NO_TELP,
                            ID_DUSUN = A.ID_DUSUN,
                            DUSUN = D.DUSUN,
                            RW = A.RW,
                            RT = A.RT,
                            ID_PENDIDIKAN = A.ID_PENDIDIKAN,
                            PENDIDIKAN = E.PENDIDIKAN,
                            ID_PEKERJAAN = A.ID_PEKERJAAN,
                            PEKERJAAN = F.PEKERJAAN,
                            ID_KAWIN = A.ID_KAWIN,
                            ID_STATUS = A.ID_STATUS,

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
                    NIK = Penduduk.NIK,
                    NAMA = Penduduk.NAMA,
                    KK = Penduduk.KK,
                    NO_AKTA = Penduduk.NO_AKTA,
                    POB = Penduduk.POB,
                    DOB = Penduduk.DOB,
                    JENIS_KELAMIN = Penduduk.JENIS_KELAMIN,
                    ID_DLMKELUARGA = Penduduk.ID_DLMKELUARGA,
                    ID_AGAMA = Penduduk.ID_AGAMA,
                    NIK_AYAH = Penduduk.NIK_AYAH,
                    NAMA_AYAH = Penduduk.NAMA_AYAH,
                    NIK_IBU = Penduduk.NIK_IBU,
                    NAMA_IBU = Penduduk.NAMA_IBU,
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
                    data.NIK = model.NIK;
                    data.NAMA = model.NAMA;
                    data.KK = model.KK;
                    data.NO_AKTA = model.NO_AKTA;
                    data.POB = model.POB;
                    data.DOB = model.DOB;
                    data.JENIS_KELAMIN = model.JENIS_KELAMIN;
                    data.ID_DLMKELUARGA = model.ID_DLMKELUARGA;
                    data.ID_AGAMA = model.ID_AGAMA;
                    data.NIK_AYAH = model.NIK_AYAH;
                    data.NAMA_AYAH = model.NAMA_AYAH;
                    data.NIK_IBU = model.NIK_IBU;
                    data.NAMA_IBU = model.NAMA_IBU;
                    data.ALAMAT = model.ALAMAT;
                    data.NO_TELP = model.NO_TELP;
                    data.ID_DUSUN = model.ID_DUSUN;
                    data.RW = model.RW;
                    data.RT = model.RT;
                    data.ID_PENDIDIKAN = model.ID_PENDIDIKAN;
                    data.ID_PEKERJAAN = model.ID_PEKERJAAN;
                    data.ID_KAWIN = model.ID_KAWIN;
                    data.ID_STATUS = model.ID_STATUS;

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

        public ActionResult DownloadExcelSensus()
        {
            DataTable dt = new DataTable();
            IQueryable<VW_Penduduk>? Query;


            string[] listHeaders = new string[]
            {
                "NIK",
                "NAMA",
                "STATUS HIDUP",
                "NOMOR KK",
                "HUBUNGAN DALAM KELUARGA",
                "JENIS KELAMIN",
                "AGAMA",
                "STATUS PERKAWINAN",
                "NO AKTA",
                "TEMPAT LAHIR",
                "TANGGAL LAHIR",
                "PENDIDIKAN",
                "PEKERJAAN",
                "NIK AYAH",
                "NAMA AYAH",
                "NIK IBU",
                "NAMA IBU",
                "NOMOR TELP",
                "DUSUN",
                "RT",
                "RW",
                "ALAMAT"
            };

            Query = from A in _context.Penduduk
                    join B in _context.RfAgama on A.ID_AGAMA equals B.ID
                    join D in _context.RfDusun on A.ID_DUSUN equals D.ID
                    join E in _context.RfPendidikan on A.ID_PENDIDIKAN equals E.ID
                    join F in _context.RfPekerjaan on A.ID_PEKERJAAN equals F.ID
                    select new VW_Penduduk()
                    {
                        ID = A.ID,
                        NIK = A.NIK,
                        NAMA = A.NAMA,
                        NO_AKTA = A.NO_AKTA,
                        KK = A.KK,
                        POB = A.POB,
                        DOB = DateTime.Parse(A.DOB.ToString("dd/MM/yyyy")),
                        JENIS_KELAMIN = A.JENIS_KELAMIN,
                        ID_DLMKELUARGA = A.ID_DLMKELUARGA,
                        ID_AGAMA = A.ID_AGAMA,
                        AGAMA = B.AGAMA,
                        NIK_AYAH = A.NIK_AYAH,
                        NAMA_AYAH = A.NAMA_AYAH,
                        NIK_IBU = A.NIK_IBU,
                        NAMA_IBU = A.NAMA_IBU,
                        ALAMAT = A.ALAMAT,
                        NO_TELP = A.NO_TELP,
                        ID_DUSUN = A.ID_DUSUN,
                        DUSUN = D.DUSUN,
                        RW = A.RW,
                        RT = A.RT,
                        ID_PENDIDIKAN = A.ID_PENDIDIKAN,
                        PENDIDIKAN = E.PENDIDIKAN,
                        ID_PEKERJAAN = A.ID_PEKERJAAN,
                        PEKERJAAN = F.PEKERJAAN,
                        ID_KAWIN = A.ID_KAWIN,
                        ID_STATUS = A.ID_STATUS,
                    };

            for (int i = 0; i < listHeaders.Length; i++)
            {
                dt.Columns.Add(listHeaders[i]);
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Data");

                //Write column headers
                for (var col = 0; col < dt.Columns.Count; col++)
                {
                    worksheet.Cells[1, col + 1].Value = dt.Columns[col].ColumnName;
                }

                var JK = "";
                var dlmKeluarga = "";
                var kawin = "";
                var statHidup = "";
                foreach (var item in Query)
                {
                    JK = item.JENIS_KELAMIN.Equals(1) ? "P" : "L";
                    dlmKeluarga = item.ID_DLMKELUARGA.Equals(0) ? "Ayah" : item.DLMKELUARGA.Equals(1) ? "Ibu" : item.DLMKELUARGA.Equals(2) ? "Anak" : "";
                    kawin = item.ID_KAWIN.Equals(0) ? "Kawin" : "Belum Kawin";
                    statHidup = item.ID_STATUS.Equals(0) ? "Hidup" : "Mati";
                    dt.Rows.Add(item.NIK, item.NAMA, statHidup, item.KK, dlmKeluarga, JK, item.AGAMA, kawin, item.NO_AKTA,
                        item.POB, item.DOB.ToString("dd/MM/yyyy"), item.PENDIDIKAN, item.PEKERJAAN, item.NIK_AYAH, item.NAMA_AYAH, item.NIK_IBU,
                        item.NAMA_IBU, item.NO_TELP, item.DUSUN, item.RT, item.RW, item.ALAMAT);
                }



                //Write data rows using LINQ query
                var dataRows = dt.AsEnumerable();
                int row = 2;
                foreach (var dataRow in dataRows)
                {
                    for (var col = 0; col < dt.Columns.Count; col++)
                    {
                        worksheet.Cells[row, col + 1].Value = dataRow[col];
                    }
                    row++;
                }

                // Generate the Excel file as a byte array
                var excelBytes = package.GetAsByteArray();

                // Return the Excel file as a downloadable file
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DataPenduduk" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx");
            }
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
                    data.TGL_RAB = model.TGL_RAB;
                    data.KETERANGAN = model.KETERANGAN;
                    data.CREATED_BY = USERID;
                    data.CREATED_DATE = DateTime.Now;
                    data.FILENAME = FILE_UPLOAD.FileName;
                    var pathFolder = Path.Combine(_env.WebRootPath, "Upload/RAB/Lampiran/" + DateTime.Now.ToString("ddMMyyyyHHmmss"));
                    data.PATH_FILE = Path.Combine(pathFolder, data.FILENAME);

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

        public ActionResult DownloadExcelRAB()
        {
            DataTable dt = new DataTable();
            //IQueryable<RAB_DESA>? Query;


            string[] listHeaders = new string[]
            {
                "JENIS RAB",
                "TANGGAL RAB",
                "KETERANGAN"
            };

            var Query = _context.RAB_Desa.ToList();

            for (int i = 0; i < listHeaders.Length; i++)
            {
                dt.Columns.Add(listHeaders[i]);
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Data");

                //Write column headers
                for (var col = 0; col < dt.Columns.Count; col++)
                {
                    worksheet.Cells[1, col + 1].Value = dt.Columns[col].ColumnName;
                }

                foreach (var item in Query)
                {
                    dt.Rows.Add(item.JENIS_RAB, item.TGL_RAB.ToString("dd/MM/yyyy"), item.KETERANGAN);
                }



                //Write data rows using LINQ query
                var dataRows = dt.AsEnumerable();
                int row = 2;
                foreach (var dataRow in dataRows)
                {
                    for (var col = 0; col < dt.Columns.Count; col++)
                    {
                        worksheet.Cells[row, col + 1].Value = dataRow[col];
                    }
                    row++;
                }

                // Generate the Excel file as a byte array
                var excelBytes = package.GetAsByteArray();

                // Return the Excel file as a downloadable file
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LaporanRAB" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx");
            }
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
                            SK = A.SK,
                            SK_BERHENTI = A.SK_BERHENTI,
                            NO_TELP = A.NO_TELP,
                            ALAMAT = A.ALAMAT,
                            TGL_PENGANGKATAN = DateTime.Parse(A.TGL_PENGANGKATAN.ToString("dd/MM/yyyy")),
                            TGL_PEMBERHENTIAN = A.TGL_PEMBERHENTIAN.ToString()
                        };

            List<SelectListItem> JK = new List<SelectListItem>()
            {
                new SelectListItem { Value = "P", Text = "Perempuan" },
                new SelectListItem { Value = "L", Text = "Laki - laki" }
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
                    SK = BPD.SK,
                    SK_BERHENTI = null,
                    NO_TELP = BPD.NO_TELP,
                    ALAMAT = BPD.ALAMAT,
                    TGL_PENGANGKATAN = BPD.TGL_PENGANGKATAN,
                    TGL_PEMBERHENTIAN = null,
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
                _context.BPD.Update(model);
                _context.SaveChanges();
                _notyf.Success("Update Data Sukses");
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
                _context.BPD.Update(model);
                _context.SaveChanges();
                _notyf.Success("Inactive Data Sukses");
            }
            catch (Exception)
            {
                _notyf.Error("Inactive Data Gagal");
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

        public ActionResult DownloadExcelBPD()
        {
            DataTable dt = new DataTable();
            IQueryable<VW_BPD>? Query;


            string[] listHeaders = new string[]
            {
                "NIK",
                "SK PENGANGKATAN",
                "NAMA",
                "JENIS KELAMIN",
                "JABATAN",
                "ALAMAT",
                "NOMOR TELP",
                "TANGGAL PENGANGKATAN",
                "SK PEMBERHENTIAN",
                "TANGGAL PEMBERHENTIAN"
            };

            Query = from A in _context.BPD
                    join B in _context.RFJabatan on A.KODE_JABATAN equals B.KODE_JABATAN
                    select new VW_BPD()
                    {
                        ID = A.ID,
                        SK=A.SK,
                        SK_BERHENTI=A.SK_BERHENTI,
                        NAMA = A.NAMA,
                        JENIS_KELAMIN = A.JENIS_KELAMIN,
                        KODE_JABATAN = A.KODE_JABATAN,
                        JABATAN = B.JABATAN,
                        NIK = A.NIK,
                        NO_TELP = A.NO_TELP,
                        ALAMAT = A.ALAMAT,
                        TGL_PENGANGKATAN = A.TGL_PENGANGKATAN,
                        TGL_PEMBERHENTIAN = A.TGL_PEMBERHENTIAN.ToString()
                    };

            for (int i = 0; i < listHeaders.Length; i++)
            {
                dt.Columns.Add(listHeaders[i]);
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Data");

                //Write column headers
                for (var col = 0; col < dt.Columns.Count; col++)
                {
                    worksheet.Cells[1, col + 1].Value = dt.Columns[col].ColumnName;
                }

                var JK = "";
                var skBerhenti = "";
                var tglBerhenti = "";
                foreach (var item in Query)
                {
                    JK = item.JENIS_KELAMIN.Equals('P') ? "Perempuan" : "Laki-laki";
                    skBerhenti = !string.IsNullOrEmpty(item.SK_BERHENTI) ? item.SK_BERHENTI : "";
                    tglBerhenti = string.IsNullOrEmpty(item.TGL_PEMBERHENTIAN) ? "-" : string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(item.TGL_PEMBERHENTIAN));
                    dt.Rows.Add(item.NIK,item.SK, item.NAMA, JK, item.JABATAN, item.ALAMAT, item.NO_TELP, item.TGL_PENGANGKATAN.ToString("dd/MM/yyyy"),skBerhenti, tglBerhenti);
                }



                //Write data rows using LINQ query
                var dataRows = dt.AsEnumerable();
                int row = 2;
                foreach (var dataRow in dataRows)
                {
                    for (var col = 0; col < dt.Columns.Count; col++)
                    {
                        worksheet.Cells[row, col + 1].Value = dataRow[col];
                    }
                    row++;
                }

                // Generate the Excel file as a byte array
                var excelBytes = package.GetAsByteArray();

                // Return the Excel file as a downloadable file
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DataAnggotaBPD" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx");
            }
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
                if (data != null)
                {
                    data.KEGIATAN = model.KEGIATAN;
                    data.KOORDINATOR = model.KOORDINATOR;
                    data.TGL_MULAI = model.TGL_MULAI;
                    data.TGL_BERAKHIR = model.TGL_BERAKHIR;

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
                            SK=A.SK,
                            SK_BERHENTI=A.SK_BERHENTI,
                            NO_TELP = A.NO_TELP,
                            ALAMAT = A.ALAMAT,
                            TGL_PENGANGKATAN = DateTime.Parse(A.TGL_PENGANGKATAN.ToString("dd/MM/yyyy")),
                            TGL_PEMBERHENTIAN = A.TGL_PEMBERHENTIAN.ToString()
                        };

            List<SelectListItem> JK = new List<SelectListItem>()
            {
                new SelectListItem { Value = "P", Text = "Perempuan" },
                new SelectListItem { Value = "L", Text = "Laki - laki" }
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
                    SK=KarangTaruna.SK,
                    SK_BERHENTI=null,
                    NO_TELP = KarangTaruna.NO_TELP,
                    ALAMAT = KarangTaruna.ALAMAT,
                    TGL_PENGANGKATAN = KarangTaruna.TGL_PENGANGKATAN,
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
                _context.KarangTaruna.Update(model);
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
                _context.KarangTaruna.Update(model);
                _context.SaveChanges();
                _notyf.Success("Inactive Data Sukses");


            }
            catch (Exception)
            {
                _notyf.Error("Inactive Data Gagal");

            }


            return RedirectToAction("KarangTaruna");
        }

        public ActionResult DownloadExcelTaruna()
        {
            DataTable dt = new DataTable();
            IQueryable<VW_KarangTaruna>? Query;


            string[] listHeaders = new string[]
            {
                "NIK",
                "SK PENGANGKATAN",
                "NAMA",
                "JENIS KELAMIN",
                "JABATAN",
                "ALAMAT",
                "NOMOR TELP",
                "TANGGAL PENGANGKATAN",
                "SK PEMBERHENTIAN",
                "TANGGAL PEMBERHENTIAN"
            };

            Query = from A in _context.KarangTaruna
                    join B in _context.RFJabatan on A.KODE_JABATAN equals B.KODE_JABATAN
                    select new VW_KarangTaruna()
                    {
                        ID = A.ID,
                        NAMA = A.NAMA,
                        JENIS_KELAMIN = A.JENIS_KELAMIN,
                        KODE_JABATAN = A.KODE_JABATAN,
                        JABATAN = B.JABATAN,
                        NIK = A.NIK,
                        SK = A.SK,
                        SK_BERHENTI = A.SK_BERHENTI,
                        NO_TELP = A.NO_TELP,
                        ALAMAT = A.ALAMAT,
                        TGL_PENGANGKATAN = A.TGL_PENGANGKATAN,
                        TGL_PEMBERHENTIAN = A.TGL_PEMBERHENTIAN.ToString()
                    };

            for (int i = 0; i < listHeaders.Length; i++)
            {
                dt.Columns.Add(listHeaders[i]);
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Data");

                //Write column headers
                for (var col = 0; col < dt.Columns.Count; col++)
                {
                    worksheet.Cells[1, col + 1].Value = dt.Columns[col].ColumnName;
                }

                var JK = "";
                var skBerhenti = "";
                var tglBerhenti = "";
                foreach (var item in Query)
                {
                    JK = item.JENIS_KELAMIN.Equals(1) ? "P" : "L";
                    skBerhenti = string.IsNullOrEmpty(item.SK_BERHENTI) ? "" : item.SK_BERHENTI;
                    tglBerhenti = string.IsNullOrEmpty(item.TGL_PEMBERHENTIAN) ? "-" : string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(item.TGL_PEMBERHENTIAN));
                    dt.Rows.Add(item.NIK,item.SK, item.NAMA, JK, item.JABATAN, item.ALAMAT, item.NO_TELP, item.TGL_PENGANGKATAN.ToString("dd/MM/yyyy"),skBerhenti, tglBerhenti);
                }



                //Write data rows using LINQ query
                var dataRows = dt.AsEnumerable();
                int row = 2;
                foreach (var dataRow in dataRows)
                {
                    for (var col = 0; col < dt.Columns.Count; col++)
                    {
                        worksheet.Cells[row, col + 1].Value = dataRow[col];
                    }
                    row++;
                }

                // Generate the Excel file as a byte array
                var excelBytes = package.GetAsByteArray();

                // Return the Excel file as a downloadable file
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DataAnggotaKarangTaruna" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx");
            }
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
                data.KOORDINATOR = model.KOORDINATOR;
                data.TGL_MULAI = model.TGL_MULAI;
                data.TGL_BERAKHIR = model.TGL_BERAKHIR;

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
                            SK=A.SK,
                            SK_BERHENTI=A.SK_BERHENTI,
                            NO_TELP = A.NO_TELP,
                            ALAMAT = A.ALAMAT,
                            TGL_PENGANGKATAN = DateTime.Parse(A.TGL_PENGANGKATAN.ToString("dd/MM/yyyy")),
                            TGL_PEMBERHENTIAN = A.TGL_PEMBERHENTIAN.ToString()
                        };

            List<SelectListItem> JK = new List<SelectListItem>()
            {
                new SelectListItem { Value = "P", Text = "Perempuan" },
                new SelectListItem { Value = "L", Text = "Laki - laki" }
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
                    SK=PKK.SK,
                    SK_BERHENTI=null,
                    NO_TELP = PKK.NO_TELP,
                    ALAMAT = PKK.ALAMAT,
                    TGL_PENGANGKATAN = PKK.TGL_PENGANGKATAN,
                    TGL_PEMBERHENTIAN = null,
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
                _context.PKK.Update(model);
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
                _context.PKK.Update(model);
                _context.SaveChanges();
                _notyf.Success("Inactive Data Sukses");
            }
            catch (Exception)
            {
                _notyf.Error("Inactive Data Sukses");

            }


            return RedirectToAction("PKK");
        }

        public ActionResult DownloadExcelPKK()
        {
            DataTable dt = new DataTable();
            IQueryable<VW_PKK>? Query;


            string[] listHeaders = new string[]
            {
                "NIK",
                "SK PENGANGKATAN",
                "NAMA",
                "JENIS KELAMIN",
                "JABATAN",
                "ALAMAT",
                "NOMOR TELP",
                "TANGGAL PENGANGKATAN",
                "SK PEMBERHENTIAN",
                "TANGGAL PEMBERHENTIAN"
            };

            Query = from A in _context.PKK
                    join B in _context.RFJabatan on A.KODE_JABATAN equals B.KODE_JABATAN
                    select new VW_PKK()
                    {
                        ID = A.ID,
                        NAMA = A.NAMA,
                        JENIS_KELAMIN = A.JENIS_KELAMIN,
                        KODE_JABATAN = A.KODE_JABATAN,
                        JABATAN = B.JABATAN,
                        NIK = A.NIK,
                        SK=A.SK,
                        SK_BERHENTI=A.SK_BERHENTI,
                        NO_TELP = A.NO_TELP,
                        ALAMAT = A.ALAMAT,
                        TGL_PENGANGKATAN = A.TGL_PENGANGKATAN,
                        TGL_PEMBERHENTIAN = A.TGL_PEMBERHENTIAN.ToString()
                    };

            for (int i = 0; i < listHeaders.Length; i++)
            {
                dt.Columns.Add(listHeaders[i]);
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Data");

                //Write column headers
                for (var col = 0; col < dt.Columns.Count; col++)
                {
                    worksheet.Cells[1, col + 1].Value = dt.Columns[col].ColumnName;
                }

                var JK = "";
                var skBerhenti = "";
                var tglBerhenti = "";
                foreach (var item in Query)
                {
                    JK = item.JENIS_KELAMIN.Equals('P') ? "Perempuan" : "Laki-laki";
                    skBerhenti = string.IsNullOrEmpty(item.SK_BERHENTI) ? "" : item.SK_BERHENTI;
                    tglBerhenti = string.IsNullOrEmpty(item.TGL_PEMBERHENTIAN) ? "-" : string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(item.TGL_PEMBERHENTIAN));
                    dt.Rows.Add(item.NIK,item.SK, item.NAMA, JK, item.JABATAN, item.ALAMAT, item.NO_TELP, item.TGL_PENGANGKATAN.ToString("dd/MM/yyyy"),skBerhenti, tglBerhenti);
                }



                //Write data rows using LINQ query
                var dataRows = dt.AsEnumerable();
                int row = 2;
                foreach (var dataRow in dataRows)
                {
                    for (var col = 0; col < dt.Columns.Count; col++)
                    {
                        worksheet.Cells[row, col + 1].Value = dataRow[col];
                    }
                    row++;
                }

                // Generate the Excel file as a byte array
                var excelBytes = package.GetAsByteArray();

                // Return the Excel file as a downloadable file
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DataAnggotaPKK" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx");
            }
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
                var USERID = !string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")) ? HttpContext.Session.GetString("UserId").ToString() : "";
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

                data.KEGIATAN = model.KEGIATAN;
                data.KOORDINATOR = model.KOORDINATOR;
                data.TGL_MULAI = model.TGL_MULAI;
                data.TGL_BERAKHIR = model.TGL_BERAKHIR;

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