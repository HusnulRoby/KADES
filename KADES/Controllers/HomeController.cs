using AspNetCoreHero.ToastNotification.Abstractions;
using KADES.Models;
using KADES.Models.Asset;
using KADES.Models.Home;
using KADES.Models.Maintenance;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using System.Diagnostics;

namespace KADES.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly INotyfService _notyf;
        public HomeController(ILogger<HomeController> logger, AppDbContext context, INotyfService notyf)
        {
            _logger = logger;
            _context = context;
            _notyf = notyf;
        }

        public IActionResult Dashboard()
        {
            var monthNow = DateTime.Now.Month;
            var yearNow = DateTime.Now.Year;

            var getNote=_context.Note.ToList();
            var getCountJiwa = _context.Penduduk.Count();
            var getLogThisDay=_context.LogSurat.Count(x=>x.GENERATE_DATE.Date.Equals(DateTime.Now.Date));
            var getLogThisMonth=_context.LogSurat.Count(y=>y.GENERATE_DATE.Month.Equals(monthNow));
            var getLogThisYear=_context.LogSurat.Count(x=>x.GENERATE_DATE.Year.Equals(yearNow));
            var getCountAsset=_context.DataAset.Count();

            ViewBag.countJiwa=getCountJiwa;
            ViewBag.logThisDay=getLogThisDay;
            ViewBag.logThisMonth=getLogThisMonth;
            ViewBag.logThisYear=getLogThisYear;
            ViewBag.countAsset= getCountAsset;

            ViewBag.USERID = HttpContext.Session.GetString("UserId");

            HomeModels homeModels = new HomeModels() {
                listNote = getNote
            };

            return View(homeModels);
        }

        #region Maitenance Notes
        public IActionResult Notes()
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");
            var a = DateTime.Now.ToString("dd/MM/yyyy");

            var model = from A in _context.Note
                        join B in _context.RFUsers on A.CREATED_BY equals B.USERID
                        select new VW_Note()
                        {
                            ID = A.ID,
                            JUDUL=A.JUDUL,
                            NOTE=A.NOTE,
                            CREATED_DATE=A.CREATED_DATE,
                            CREATED_BY=A.CREATED_BY,
                            USERNAME=B.USERNAME
                        };

            HomeModels homeModel = new HomeModels()
            {
                listVwNote = model.ToList()
            };

            return View(homeModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddNote(Note Note)
        {
            var USERID = HttpContext.Session.GetString("UserId").ToString();
             var a= DateTime.Now;
            try
            {
                var getData = new Note
                {
                   JUDUL=Note.JUDUL,
                   NOTE=Note.NOTE,
                   CREATED_DATE=DateTime.Now,
                   CREATED_BY= USERID
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
            return RedirectToAction("Notes");
        }

        [HttpPost]
        public IActionResult UpNote(Note model)
        {
            try
            {
                var data = _context.Note.Where(x => x.ID.Equals(model.ID)).FirstOrDefault();

                data.JUDUL=model.JUDUL;
                data.NOTE=model.NOTE;
                

                _context.Note.Update(data);
                _context.SaveChanges();
                _notyf.Success("Update Data Sukses");
            }
            catch (Exception ex)
            {
                _notyf.Error("Update Data Gagal");

            }


            return RedirectToAction("Notes");
        }

        [HttpPost]
        public IActionResult DelNote(int ID)
        {
            try
            {
                var getAcc = _context.Note.Find(ID);
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

            return RedirectToAction("Notes");

        }

        #endregion


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public JsonResult CheckSession()
        {
            if (HttpContext.Session.IsAvailable)
            {

            }
            string userID = Convert.ToString(HttpContext.Session.GetString("UserId"));
            int check = 0;
            if (!string.IsNullOrEmpty(userID))
                check = 1;
            return Json(new { check });
        }

        
    }
}