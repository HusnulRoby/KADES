using AspNetCoreHero.ToastNotification.Abstractions;
using KADES.Models;
using KADES.Models.Asset;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;

namespace KADES.Controllers
{
    public class AssetController : Controller
    {
        private readonly ILogger<AssetController> _logger;
        private readonly AppDbContext _context;
        private readonly INotyfService _notyf;
        public AssetController(ILogger<AssetController> logger, AppDbContext context, INotyfService notyf)
        {
            _logger = logger;
            _context = context;
            _notyf = notyf;
        }

        #region DataAsset
        public IActionResult DataAsset()
        {
            ViewBag.USERID = HttpContext.Session.GetString("UserId");

            var model = from A in _context.DataAset
                        join B in _context.RfJenisAset on A.ID_JNSASET equals B.ID
                        select new VW_DataAset()
                        {
                            ID = A.ID,
                            KODE_ASET=A.KODE_ASET,
                            NAMA_ASET=A.NAMA_ASET,
                            ID_JNSASET=A.ID_JNSASET,
                            JENIS_ASET=B.JENIS_ASET,
                            LOKASI=A.LOKASI,
                            NILAI_ASET=A.NILAI_ASET,
                            TGL_INPUT=A.TGL_INPUT,
                        };

            

            AssetModels assetModels = new AssetModels()
            {
                ListVW_Aset = model.ToList()
            };

            var getJnsAset = _context.RfJenisAset.Where(x => x.ACTIVE.Equals(true)).Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.JENIS_ASET.ToString(),
            });

            ViewBag.ddlJnsAset = getJnsAset;

            return View(assetModels);
        }

        [HttpPost]
        public IActionResult AddAsset(DataAset DataAset)
        {

            try
            {
                var USERID = HttpContext.Session.GetString("UserId").ToString();
                var getKode = from jnsAset in _context.RfJenisAset.Where(x => x.ID.Equals(DataAset.ID_JNSASET))
                                     select jnsAset.KODE_JNSASET;
                var cekData = _context.DataAset.FirstOrDefault();
                var getID = 0;

                var kodeJnsAset=getKode.FirstOrDefault();

                if (cekData != null) {
                    getID=_context.DataAset.Max(x => x.ID);
                }
                else
                {
                    getID= 1;
                }
                string formattedId = getID.ToString("D5");
                var kodeAset = getKode.FirstOrDefault() + formattedId + DateTime.Now.ToString("ddMMyy"); //TNH0000310523
                var getData = new DataAset
                {
                    ID_JNSASET=DataAset.ID_JNSASET,
                    NAMA_ASET=DataAset.NAMA_ASET,
                    KODE_ASET= kodeAset,
                    LOKASI=DataAset.LOKASI,
                    NILAI_ASET=DataAset.NILAI_ASET,
                    TGL_INPUT=DataAset.TGL_INPUT,
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
            return RedirectToAction("DataAsset");
        }

        [HttpPost]
        public IActionResult UpAsset(DataAset model)
        {
            try
            {
                var data = _context.DataAset.Where(x => x.ID.Equals(model.ID)).FirstOrDefault();

                data.NAMA_ASET=model.NAMA_ASET;
                data.ID_JNSASET = model.ID_JNSASET;
                data.LOKASI = model.LOKASI;
                data.NILAI_ASET= model.NILAI_ASET;
                data.TGL_INPUT = model.TGL_INPUT;

                _context.DataAset.Update(data);
                _context.SaveChanges();
                _notyf.Success("Update Data Sukses");
            }
            catch (Exception)
            {
                _notyf.Error("Update Data Gagal");

            }


            return RedirectToAction("DataAsset");
        }

        [HttpPost]
        public IActionResult DelAsset(int ID)
        {
            try
            {
                var getAcc = _context.DataAset.Find(ID);
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

            return RedirectToAction("DataAsset");

        }

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}