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
using System.Data;
using OfficeOpenXml;
using System.Threading;

namespace KADES.Controllers
{
    public class SuratController : Controller
    {
        private readonly ILogger<SuratController> _logger;
        private readonly AppDbContext _context;
        private readonly INotyfService _notyf;
        IWebHostEnvironment _env;
        public SuratController(ILogger<SuratController> logger, AppDbContext context, INotyfService notyf, IWebHostEnvironment env)
        {
            _context = context;
            _logger = logger;
            _notyf = notyf;
            _env = env;
        }

        #region TEMPLATE SURAT
        public IActionResult TemplateSurat()
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");

            PelayananModels pelayananModel = new PelayananModels()
            {
                //TemplateSurat = new TemplateSurat(),
                ListTemplateSurat = _context.TemplateSurat.Where(x => x.ACTIVE.Equals(true)).ToList()
            };

            return View(pelayananModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddTempSurat(TemplateSurat TemplateSurat, IFormFile FILE_UPLOAD)
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
                    var pathFolder = Path.Combine(_env.WebRootPath, "Upload/Document/" + DateTime.Now.ToString("ddMMyyyy"));
                    var fullPath = Path.Combine(pathFolder, fileNama);

                    if (!Directory.Exists(pathFolder))
                    {
                        Directory.CreateDirectory(pathFolder);
                    }

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await FILE_UPLOAD.CopyToAsync(stream);
                    }


                    var getData = new TemplateSurat
                    {
                        ID = Guid.NewGuid().ToString(),
                        NO_SURAT = TemplateSurat.NO_SURAT,
                        NAMA_SURAT = TemplateSurat.NAMA_SURAT,
                        FILE_NAME = fileNama,
                        PATH_FILE = fullPath,
                        ACTIVE = true,
                        CREATED_BY = USERID,
                        CREATED_DATE = DateTime.Now
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
            return RedirectToAction("TemplateSurat");
        }

        //[HttpPost]
        //public IActionResult PrintSurat(TemplateSurat TemplateSurat, LogSurat LogSurat)
        //{


        //    var USERID = HttpContext.Session.GetString("UserId").ToString();
        //    var PATH_FILE = TemplateSurat.PATH_FILE;
        //    var FILENAME = TemplateSurat.FILE_NAME;

        //    try
        //    {
        //        var getData = new LogSurat
        //        {
        //            ID_SURAT = TemplateSurat.ID,
        //            NAMA_PEMOHON = LogSurat.NAMA_PEMOHON,
        //            NIK = LogSurat.NIK,
        //            NO_TELP = LogSurat.NO_TELP,
        //            ALASAN = LogSurat.ALASAN,
        //            ALAMAT=LogSurat.ALAMAT,
        //            GENERATE_BY = USERID,
        //            GENERATE_DATE = DateTime.Now
        //        };
        //        _context.Add(getData);
        //        _context.SaveChanges();
        //        _notyf.Success("Tambah Data Log Sukses");

        //        var net = new System.Net.WebClient();
        //        var data = net.DownloadData(PATH_FILE);

        //        var content = new System.IO.MemoryStream(data);

        //        return File(content, "application/octet-stream", FILENAME);

        //    }
        //    catch (Exception ex)
        //    {
        //        _notyf.Error("Tambah Data Log Gagal");
        //        throw ex;
        //    }
        //    //return RedirectToAction("TemplateSurat");
        //}

        [HttpPost]
        public IActionResult PrintSurat(string ID)
        {


            var USERID = HttpContext.Session.GetString("UserId").ToString();
            var FILENAME = "";
            var PATH_FILE = "";

            try
            {
                var getAcc = _context.TemplateSurat.Find(ID);
                if (getAcc == null)
                {
                    return NotFound();
                }

                var getData = new LogSurat
                {
                    ID_SURAT = getAcc.ID,
                    GENERATE_BY = USERID,
                    GENERATE_DATE = DateTime.Now
                };

                FILENAME = getAcc.FILE_NAME;
                PATH_FILE=getAcc.PATH_FILE;

                _context.Add(getData);
                _context.SaveChanges();
                //_notyf.Success("Tambah Data Log Sukses");

                var net = new System.Net.WebClient();
                var data = net.DownloadData(PATH_FILE);

                var content = new System.IO.MemoryStream(data);

                return File(content, "application/octet-stream", FILENAME);

            }
            catch (Exception ex)
            {
                _notyf.Error("Tambah Data Log Gagal");
                return RedirectToAction("TemplateSurat");

            }
            //return RedirectToAction("TemplateSurat");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTempSuratAsync(TemplateSurat model, IFormFile FILE_UPLOAD)
        {

            try
            {
                var data = _context.TemplateSurat.Where(x => x.ID.Equals(model.ID)).FirstOrDefault();
                if (FILE_UPLOAD != null)
                {
                    data.NO_SURAT = model.NO_SURAT;
                    data.NAMA_SURAT=model.NAMA_SURAT;

                    data.FILE_NAME = FILE_UPLOAD.FileName;
                    var pathFolder = Path.Combine(_env.WebRootPath, "Upload/Document/" + DateTime.Now.ToString("ddMMyyyy"));
                    data.PATH_FILE = Path.Combine(pathFolder, FILE_UPLOAD.FileName);

                    if (!Directory.Exists(pathFolder))
                    {
                        Directory.CreateDirectory(pathFolder);
                    }

                    using (var stream = new FileStream(data.PATH_FILE, FileMode.Create))
                    {
                        await FILE_UPLOAD.CopyToAsync(stream);
                    }

                }
                _context.TemplateSurat.Update(data);
                _context.SaveChanges();
                _notyf.Success("Update Data Sukses");

            }
            catch (Exception ex)
            {
                _notyf.Error("Update Data Gagal");
            }

            return RedirectToAction("TemplateSurat");
        }

        [HttpPost]
        public IActionResult DeleteTempSurat(string ID)
        {
            try
            {
                var getAcc = _context.TemplateSurat.Find(ID);
                if (getAcc == null)
                {
                    return NotFound();
                }
               // getAcc.ACTIVE = false;

                _context.Remove(getAcc);
                _context.SaveChanges();
                _notyf.Success("Delete Data Sukses");
            }
            catch (Exception ex)
            {
                _notyf.Error("Delete Data Gagal");

            }

            return RedirectToAction("TemplateSurat");

        }
        #endregion


        #region LOG SURAT
        public IActionResult LogSurat(searchBydate searchBydate)
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");

            IQueryable<VW_LogSurat>? Query;
            try
            {
                if (string.IsNullOrEmpty(searchBydate.PERIODFROM))
                {
                    searchBydate.PERIODFROM = DateTime.Now.ToString();
                }

                if (string.IsNullOrEmpty(searchBydate.PERIODTO))
                {
                    searchBydate.PERIODTO = DateTime.Now.ToString();
                }

                Query = from a in _context.TemplateSurat
                        join b in _context.LogSurat on a.ID equals b.ID_SURAT
                        where b.GENERATE_DATE.Date >= DateTime.Parse(searchBydate.PERIODFROM).Date && b.GENERATE_DATE.Date <= DateTime.Parse(searchBydate.PERIODTO).Date
                        select new VW_LogSurat()
                        {
                            ID = b.ID,
                            ID_SURAT = a.ID,
                            NO_SURAT = a.NO_SURAT,
                            NAMA_SURAT = a.NAMA_SURAT,
                            ACTIVE = a.ACTIVE,
                            GENERATE_BY = b.GENERATE_BY,
                            GENERATE_DATE = b.GENERATE_DATE,

                        };
            }
            catch (Exception ex)
            {

                throw ex;
            }

            PelayananModels PelayananModels = new PelayananModels()
            {
                ListVW_LogSurat = Query.ToList(),
                searchBydate= searchBydate
            };

            return View(PelayananModels);
        }

        public ActionResult DownloadExcel(string PERIODFROM, string PERIODTO )
        {
            DataTable dt = new DataTable();
            IQueryable<VW_LogSurat>? Query;

            if (string.IsNullOrEmpty(PERIODFROM))
            {
                PERIODFROM = DateTime.Now.ToString();
            }

            if (string.IsNullOrEmpty(PERIODTO))
            {
                PERIODTO = DateTime.Now.ToString();
            }

            string[] listHeaders = new string[]
            {
                "NO SURAT",
                "NAMA SURAT", 
                "NAMA FILE",
                "TANGGAL GENERATE"
            };

            Query = from a in _context.TemplateSurat
                    join b in _context.LogSurat on a.ID equals b.ID_SURAT
                    where b.GENERATE_DATE.Date >= DateTime.Parse(PERIODFROM).Date && b.GENERATE_DATE.Date <= DateTime.Parse(PERIODTO).Date
                    select new VW_LogSurat()
                    {
                        ID = b.ID,
                        ID_SURAT = a.ID,
                        NO_SURAT = a.NO_SURAT,
                        NAMA_SURAT = a.NAMA_SURAT,
                        ACTIVE = a.ACTIVE,
                        GENERATE_BY = b.GENERATE_BY,
                        GENERATE_DATE = b.GENERATE_DATE,

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

                foreach (var item in Query)
                {
                    dt.Rows.Add(item.NO_SURAT,item.NAMA_SURAT,item.FILE_NAME,item.GENERATE_DATE.ToString("dd/MM/yyyy"));
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
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LogSurat"+DateTime.Now.ToString("ddMMyyyy")+".xlsx");
            }

            
        }

        #endregion


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}