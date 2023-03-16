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
                throw ex;
            }
            //return RedirectToAction("TemplateSurat");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTempSuratAsync(TemplateSurat model, IFormFile FILE_UPLOAD)
        {

            try
            {
                if (FILE_UPLOAD != null)
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

                    model.FILE_NAME = fileNama;
                    model.PATH_FILE = fullPath;
                }

                model.UPDATED_BY = HttpContext.Session.GetString("UserId").ToString();
                model.UPDATED_DATE = DateTime.Now;
                _context.TemplateSurat.Update(model);
                _context.SaveChanges();
                _notyf.Success("Update Data Sukses");

            }
            catch (Exception ex)
            {
                _notyf.Error("Update Data Gagal");
                throw ex;
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

                throw ex;
            }

            return RedirectToAction("TemplateSurat");

        }
        #endregion


        #region LOG SURAT
        public IActionResult LogSurat()
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");

            IQueryable<VW_LogSurat>? Query;
            try
            {
                Query = from a in _context.TemplateSurat
                        join b in _context.LogSurat on a.ID equals b.ID_SURAT
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


            return View(Query.ToList());
        }
        #endregion


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}