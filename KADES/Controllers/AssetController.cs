using AspNetCoreHero.ToastNotification.Abstractions;
using KADES.Models;
using KADES.Models.Asset;
using KADES.Models.Pelayanan;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Org.BouncyCastle.Crypto.Utilities;
using System.Data;
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
        public IActionResult DataAsset(AsetSearchBydate searchBydate)
        {
            try
            {
                ViewBag.USERID = HttpContext.Session.GetString("UserId"); ViewBag.GROUPID = HttpContext.Session.GetString("GroupId");

            }
            catch (Exception ex)
            {

                return Redirect("/Account/Login");
            }

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

                var model = from A in _context.DataAset
                            where A.STATUS.Equals(true) && A.TGL_INPUT.Date >= DateTime.Parse(searchBydate.PERIODFROM).Date && A.TGL_INPUT.Date <= DateTime.Parse(searchBydate.PERIODTO).Date
                            join B in _context.RfJenisAset on A.ID_JNSASET equals B.ID
                            join C in _context.RfSumberAset on A.KODE_SUMBER equals C.KODE_SUMBER
                            select new VW_DataAset()
                            {
                                ID = A.ID,
                                KODE_ASET = A.KODE_ASET,
                                NAMA_ASET = A.NAMA_ASET,
                                ID_JNSASET = A.ID_JNSASET,
                                JENIS_ASET = B.JENIS_ASET,
                                KODE_SUMBER = A.KODE_SUMBER,
                                SUMBER_ASET = C.SUMBER_ASET,
                                KODE_KONDISI = A.KODE_KONDISI,
                                LOKASI = A.LOKASI,
                                NILAI_ASET = A.NILAI_ASET,
                                TGL_INPUT = A.TGL_INPUT,
                            };



                AssetModels assetModels = new AssetModels()
                {
                    ListVW_Aset = model.ToList(),
                    searchBydate = searchBydate
                };

                var getJnsAset = _context.RfJenisAset.Where(x => x.ACTIVE.Equals(true)).Select(x => new SelectListItem
                {
                    Value = x.ID.ToString(),
                    Text = x.JENIS_ASET.ToString(),
                });

                var getSumberAset = _context.RfSumberAset.Where(x => x.ACTIVE.Equals(true)).Select(x => new SelectListItem
                {
                    Value = x.KODE_SUMBER.ToString(),
                    Text = x.SUMBER_ASET.ToString(),
                });

                List<SelectListItem> kondisiAset = new List<SelectListItem>()
            {
                new SelectListItem { Value = "BK", Text = "Baik" },
                new SelectListItem { Value = "RR", Text = "Rusak Ringan" },
                new SelectListItem { Value = "RB", Text = "Rusak Berat" }
            };


                ViewBag.ddlJnsAset = getJnsAset;
                ViewBag.ddlSumberAset = getSumberAset;
                ViewBag.ddlKondisiAset = kondisiAset;

                return View(assetModels);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
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

                var kodeJnsAset = getKode.FirstOrDefault();

                if (cekData != null)
                {
                    getID = _context.DataAset.Max(x => x.ID);
                }
                else
                {
                    getID = 1;
                }
                string formattedId = getID.ToString("D5");
                var kodeAset = getKode.FirstOrDefault() + formattedId + DateTime.Now.ToString("ddMMyy"); //TNH0000310523
                var getData = new DataAset
                {
                    ID_JNSASET = DataAset.ID_JNSASET,
                    NAMA_ASET = DataAset.NAMA_ASET,
                    KODE_ASET = kodeAset,
                    KODE_SUMBER = DataAset.KODE_SUMBER,
                    KODE_KONDISI = DataAset.KODE_KONDISI,
                    LOKASI = DataAset.LOKASI,
                    NILAI_ASET = DataAset.NILAI_ASET,
                    TGL_INPUT = DataAset.TGL_INPUT,
                    TGL_HAPUS = DateTime.Now,
                    STATUS = true
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

                data.NAMA_ASET = model.NAMA_ASET;
                data.ID_JNSASET = model.ID_JNSASET;
                data.KODE_SUMBER = model.KODE_SUMBER;
                data.KODE_KONDISI = model.KODE_KONDISI;
                data.LOKASI = model.LOKASI;
                data.NILAI_ASET = model.NILAI_ASET;
                data.TGL_INPUT = model.TGL_INPUT;
                data.TGL_HAPUS = DateTime.Now;

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

                getAcc.STATUS = false;
                getAcc.TGL_HAPUS=DateTime.Now;
                _context.Update(getAcc);
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

        public ActionResult DownloadExcel(string PERIODFROM, string PERIODTO)
        {
            DataTable dt = new DataTable();
            IQueryable<VW_DataAset>? Query;


            string[] listHeaders = new string[]
            {
                "JENIS ASET",
                "KODE ASET",
                "NAMA ASET",
                "SUMBER ASET",
                "KONDISI ASET",
                "LOKASI",
                "NILAI ASET",
                "TANGGAL INPUT"
            };

            if (!string.IsNullOrEmpty(PERIODFROM)||!string.IsNullOrEmpty(PERIODTO))
            {
                Query = from a in _context.DataAset
                        where a.STATUS.Equals(true) && a.TGL_INPUT.Date >= DateTime.Parse(PERIODFROM).Date && a.TGL_INPUT.Date <= DateTime.Parse(PERIODTO).Date
                        join b in _context.RfJenisAset on a.ID_JNSASET equals b.ID
                        join c in _context.RfSumberAset on a.KODE_SUMBER equals c.KODE_SUMBER
                        select new VW_DataAset()
                        {
                            ID = a.ID,
                            ID_JNSASET = a.ID_JNSASET,
                            JENIS_ASET = b.JENIS_ASET,
                            KODE_ASET = a.KODE_ASET,
                            NAMA_ASET = a.NAMA_ASET,
                            KODE_SUMBER = a.KODE_SUMBER,
                            SUMBER_ASET = c.SUMBER_ASET,
                            KODE_KONDISI = a.KODE_KONDISI,
                            LOKASI = a.LOKASI,
                            NILAI_ASET = a.NILAI_ASET,
                            TGL_INPUT = a.TGL_INPUT
                        };
            }
            else
            {
                Query = from a in _context.DataAset
                        where a.STATUS.Equals(true)
                        join b in _context.RfJenisAset on a.ID_JNSASET equals b.ID
                        join c in _context.RfSumberAset on a.KODE_SUMBER equals c.KODE_SUMBER
                        select new VW_DataAset()
                        {
                            ID = a.ID,
                            ID_JNSASET = a.ID_JNSASET,
                            JENIS_ASET = b.JENIS_ASET,
                            KODE_ASET = a.KODE_ASET,
                            NAMA_ASET = a.NAMA_ASET,
                            KODE_SUMBER = a.KODE_SUMBER,
                            SUMBER_ASET = c.SUMBER_ASET,
                            KODE_KONDISI = a.KODE_KONDISI,
                            LOKASI = a.LOKASI,
                            NILAI_ASET = a.NILAI_ASET,
                            TGL_INPUT = a.TGL_INPUT
                        };
            }

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

                string kodisiAset = "";
                foreach (var item in Query)
                {
                    kodisiAset = item.KODE_KONDISI.Equals("BK") ? "Baik" : item.KODE_KONDISI.Equals("RR") ? "Rusak Ringan" : item.KODE_KONDISI.Equals("RB") ? "Rusak Berat" : "";
                    dt.Rows.Add(item.JENIS_ASET, item.KODE_ASET, item.NAMA_ASET, item.SUMBER_ASET, kodisiAset, item.LOKASI, item.NILAI_ASET, item.TGL_INPUT.ToString("dd/MM/yyyy"));
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
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DataAset" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx");
            }


        }

        public IActionResult DataAssetDeleted(AsetSearchBydate searchBydate)
        {
            IQueryable<VW_DataAset>? model;
            try
            {
                ViewBag.USERID = HttpContext.Session.GetString("UserId"); ViewBag.GROUPID = HttpContext.Session.GetString("GroupId");
            }
            catch (Exception ex)
            {
                return Redirect("/Account/Login");
            }

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

                model = from A in _context.DataAset
                        join B in _context.RfJenisAset on A.ID_JNSASET equals B.ID
                        join C in _context.RfSumberAset on A.KODE_SUMBER equals C.KODE_SUMBER
                        where A.TGL_HAPUS.Date >= DateTime.Parse(searchBydate.PERIODFROM).Date && A.TGL_HAPUS.Date <= DateTime.Parse(searchBydate.PERIODTO).Date && A.STATUS.Equals(false)
                        select new VW_DataAset()
                        {
                            ID = A.ID,
                            KODE_ASET = A.KODE_ASET,
                            NAMA_ASET = A.NAMA_ASET,
                            ID_JNSASET = A.ID_JNSASET,
                            JENIS_ASET = B.JENIS_ASET,
                            KODE_SUMBER = A.KODE_SUMBER,
                            SUMBER_ASET = C.SUMBER_ASET,
                            KODE_KONDISI = A.KODE_KONDISI,
                            LOKASI = A.LOKASI,
                            NILAI_ASET = A.NILAI_ASET,
                            TGL_INPUT = A.TGL_INPUT,
                            TGL_HAPUS = A.TGL_HAPUS,
                        };



            }
            catch (Exception ex)
            {

                throw ex;
            }


            AssetModels assetModels = new AssetModels()
            {
                ListVW_Aset = model.ToList(),
                searchBydate = searchBydate
            };

            return View(assetModels);
        }

        public ActionResult DownloadExcelDel(string PERIODFROM, string PERIODTO)
        {
            DataTable dt = new DataTable();
            IQueryable<VW_DataAset>? Query;


            string[] listHeaders = new string[]
            {
                "JENIS ASET",
                "KODE ASET",
                "NAMA ASET",
                "SUMBER ASET",
                "KONDISI ASET",
                "LOKASI",
                "NILAI ASET",
                "TANGGAL INPUT",
                "STATUS ASET",
                "TANGGAL ASET DIHAPUS"
            };

            if (!string.IsNullOrEmpty(PERIODFROM)||!string.IsNullOrEmpty(PERIODTO))
            {
                Query = from a in _context.DataAset
                        where a.STATUS.Equals(false)
                        join b in _context.RfJenisAset on a.ID_JNSASET equals b.ID
                        join c in _context.RfSumberAset on a.KODE_SUMBER equals c.KODE_SUMBER
                        where a.TGL_HAPUS.Date >= DateTime.Parse(PERIODFROM).Date && a.TGL_HAPUS.Date <= DateTime.Parse(PERIODTO).Date
                        select new VW_DataAset()
                        {
                            ID = a.ID,
                            ID_JNSASET = a.ID_JNSASET,
                            JENIS_ASET = b.JENIS_ASET,
                            KODE_ASET = a.KODE_ASET,
                            NAMA_ASET = a.NAMA_ASET,
                            KODE_SUMBER = a.KODE_SUMBER,
                            SUMBER_ASET = c.SUMBER_ASET,
                            KODE_KONDISI = a.KODE_KONDISI,
                            LOKASI = a.LOKASI,
                            NILAI_ASET = a.NILAI_ASET,
                            TGL_INPUT = a.TGL_INPUT,
                            TGL_HAPUS = a.TGL_HAPUS
                        };
            }
            else
            {
                Query = from a in _context.DataAset
                        where a.STATUS.Equals(false)
                        join b in _context.RfJenisAset on a.ID_JNSASET equals b.ID
                        join c in _context.RfSumberAset on a.KODE_SUMBER equals c.KODE_SUMBER
                        select new VW_DataAset()
                        {
                            ID = a.ID,
                            ID_JNSASET = a.ID_JNSASET,
                            JENIS_ASET = b.JENIS_ASET,
                            KODE_ASET = a.KODE_ASET,
                            NAMA_ASET = a.NAMA_ASET,
                            KODE_SUMBER = a.KODE_SUMBER,
                            SUMBER_ASET = c.SUMBER_ASET,
                            KODE_KONDISI = a.KODE_KONDISI,
                            LOKASI = a.LOKASI,
                            NILAI_ASET = a.NILAI_ASET,
                            TGL_INPUT = a.TGL_INPUT,
                            TGL_HAPUS = a.TGL_HAPUS
                        };
            }
            

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

                string kodisiAset = "";
                string statusAset = "";
                foreach (var item in Query)
                {
                    kodisiAset = item.KODE_KONDISI.Equals("BK") ? "Baik" : item.KODE_KONDISI.Equals("RR") ? "Rusak Ringan" : item.KODE_KONDISI.Equals("RB") ? "Rusak Berat" : "";
                    statusAset = item.STATUS.Equals(false) ? "Dihapus" : "";
                    dt.Rows.Add(item.JENIS_ASET, item.KODE_ASET, item.NAMA_ASET, item.SUMBER_ASET, kodisiAset, item.LOKASI, item.NILAI_ASET, item.TGL_INPUT.ToString("dd/MM/yyyy"), statusAset, item.TGL_HAPUS.ToString("dd/MM/yyyy"));
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
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DataAset" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx");
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