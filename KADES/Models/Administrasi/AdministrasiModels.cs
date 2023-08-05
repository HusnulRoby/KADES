using KADES.Models.Home;
using KADES.Models.Maintenance;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace KADES.Models.Administrasi
{
    public class AdministrasiModels
    {
        public HomeModels HomeModels { get; set; }
        #region APARATUR DESA
        public AparaturDesa AparaturDesa { get; set; }
        public List<AparaturDesa> ListAparaturDesa { get; set; }
        public VW_AparaturDesa VW_AparaturDesa { get; set; } 
        public List<VW_AparaturDesa>? ListVW_AparaturDesa { get; set; }
        public List<RFJabatan> ddlRFJabatan { get; set; }
        

        public JK JK { get; set; }
        public List<SelectListItem> ddlJK { get; set; }
        public List<SelectListItem> ddlDlmKeluarga { get; set; }
        public List<SelectListItem> ddlStatusHidup { get; set; }
        public List<SelectListItem> ddlStatusKawin { get; set; }
        
        #endregion

        #region RAB DESA
        public RAB_DESA RAB_DESA { get; set; }
        public List<RAB_DESA> ListRAB_DESA { get; set; }
        public VW_RAB_DESA VW_RAB_DESA { get; set; }
        public List<VW_RAB_DESA> ListVWRAB_DESA { get; set; }

        public REALISASI_RAB REALISASI_RAB { get; set; }
        public List<REALISASI_RAB> ListREALISASI_RAB { get; set; }

        public VWREALISASI_RAB VWREALISASI_RAB { get; set; }
        public List<VWREALISASI_RAB> ListVWREALISASI_RAB { get; set; }
        #endregion

        #region BPD
        public BPD BPD { get; set; }
        public VW_BPD VW_BPD { get; set; }
        public List<VW_BPD>? ListVW_BPD { get; set; }
        public KegiatanBPD? KegiatanBPD { get; set; }
        public List<KegiatanBPD> ListKegiatanBPD { get; set; }
        #endregion

        #region Karang Taruna
        public KarangTaruna KarangTaruna { get; set; }
        public List<VW_KarangTaruna>? ListVW_KarangTaruna { get; set; }
        public KegiatanTaruna KegiatanTaruna { get; set; }
        public List<KegiatanTaruna> ListKegiatanTaruna { get; set; }
        #endregion

        #region PKK
        public PKK PKK { get; set; }
        public List<VW_PKK>? ListVW_PKK { get; set; }
        public KegiatanPKK KegiatanPKK { get; set; }
        public List<KegiatanPKK> ListKegiatanPKK { get; set; }
        #endregion

        #region Penduduk
        public Penduduk Penduduk { get; set; }
        public List<VW_Penduduk>? ListVW_Penduduk { get; set; }
        public List<RfDusun> ddlRfDusun { get; set; }
        public List<RfAgama> ddlRfAgama { get; set; }
        public List<RfPendidikan> ddlRfPendidikan { get; set; }
        public List<RfPekerjaan> ddlRfPekerjaan { get; set; }
        #endregion

        public AdmSearchBydate? AdmSearchBydate { get; set; }
    }

    public class AdmSearchBydate
    {
        public string PERIODFROM { get; set; }
        public string PERIODTO { get; set; }
    }

    #region APARATUR DESA
    public class AparaturDesa
    {
        [Key]
        public int ID { get; set; }
        public string KODE_JABATAN { get; set; }
        public string SK { get; set; }
        public string? SK_BERHENTI { get; set; }
        public string NAMA { get; set; }
        [Required(ErrorMessage ="Nama harus diisi!")]
        public char JENIS_KELAMIN { get; set; }
        [Required(ErrorMessage = "Nama harus diisi!")]
        public string NIK { get; set; }
        [Required(ErrorMessage = "NIK harus diisi!")]
        public string NO_TELP { get; set; }
        [Required(ErrorMessage = "No telp harus diisi!")]
        public string ALAMAT { get; set; }
        [Required(ErrorMessage = "Alamat harus diisi!")]
        public DateTime TGL_MASUK { get; set; }
        [Required(ErrorMessage = "Tanggal masuk harus diisi!")]

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? TGL_BERHENTI { get; set; }
        public bool ACTIVE { get; set; }
    }

    //public class RFJabatan
    //{
    //    [Key]
    //    public string KODE_JABATAN { get; set; }
    //    public string JABATAN { get; set; }
    //    public string KODE_TYPE { get; set; }
    //    public bool ACTIVE { get; set; }
    //}

    public class JK
    {
        [Key]
        public int ID { get; set; }
        public string JENIS_KELAMIN { get; set; }
    }
    public class VW_AparaturDesa
    {
        [Key]
        public int ID { get; set; }
        public string KODE_JABATAN { get; set; }
        public string SK { get; set; }
        public string SK_BERHENTI { get; set; }
        public string JABATAN { get; set; }
        public string NAMA { get; set; }
        public char JENIS_KELAMIN { get; set; }
        public string NIK { get; set; }
        public string NO_TELP { get; set; }
        public string ALAMAT { get; set; }
        public DateTime TGL_MASUK { get; set; }
        public string TGL_BERHENTI { get; set; }
        public bool ACTIVE { get; set; }

    }
    #endregion

    #region RAB DESA
    public class RAB_DESA
    {
        public int ID { get; set; }
        public string JENIS_RAB { get; set; }
        public DateTime TGL_RAB { get; set; }
        public string IDSUMBER_DANA { get; set; }
        public double SALDO_AWAL { get; set; }
        public double SALDO_AKHIR { get; set; }
        public string KETERANGAN { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
    }

    public class VW_RAB_DESA
    {
        public int ID { get; set; }
        public string JENIS_RAB { get; set; }
        public DateTime TGL_RAB { get; set; }
        public string IDSUMBER_DANA { get; set; }
        public string SUMBER_DANA { get; set; }
        public double SALDO_AWAL { get; set; }
        public double SALDO_AKHIR { get; set; }
        public string KETERANGAN { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
    }

    public class REALISASI_RAB
    {
        public int ID { get; set; }
        public int ID_RAB { get; set; }
        public string KEGIATAN { get; set; }
        public DateTime TGL_REALISASI { get; set; }
        public double BIAYA { get; set; }
        public string FILENAME { get; set; }
        public string PATH { get; set; }
    }

    public class VWREALISASI_RAB
    {
        public int ID { get; set; }
        public int ID_RAB { get; set; }
        public string JENIS_RAB { get; set; }
        public string KEGIATAN { get; set; }
        public DateTime TGL_REALISASI { get; set; }
        public double BIAYA { get; set; }
        public string FILENAME { get; set; }
        public string PATH { get; set; }
    }

    #endregion

    #region BPD
    public class BPD
    {
        [Key]
        public int ID { get; set; }
        public string KODE_JABATAN { get; set; }
        public string NAMA { get; set; }
        [Required(ErrorMessage = "Nama harus diisi!")]
        public char JENIS_KELAMIN { get; set; }
        [Required(ErrorMessage = "Nama harus diisi!")]
        public string NIK { get; set; }
        public string SK { get; set; }
        public string? SK_BERHENTI { get; set; }

        public string NO_TELP { get; set; }
        [Required(ErrorMessage = "No telp harus diisi!")]
        public string ALAMAT { get; set; }
        [Required(ErrorMessage = "Alamat harus diisi!")]
        public DateTime TGL_PENGANGKATAN { get; set; }
        [Required(ErrorMessage = "Tanggal masuk harus diisi!")]
        
        public DateTime? TGL_PEMBERHENTIAN { get; set; }
        public bool ACTIVE { get; set; }
    }

    public class VW_BPD
    {
        [Key]
        public int ID { get; set; }
        public string KODE_JABATAN { get; set; }
        public string JABATAN { get; set; }
        public string NAMA { get; set; }
        public char JENIS_KELAMIN { get; set; }
        public string NIK { get; set; }
        public string SK { get; set; }
        public string SK_BERHENTI { get; set; }
        public string NO_TELP { get; set; }
        public string ALAMAT { get; set; }
        public DateTime TGL_PENGANGKATAN { get; set; }
        public string TGL_PEMBERHENTIAN { get; set; }
        public bool ACTIVE { get; set; }

    }

    public class KegiatanBPD
    {
        [Key] public int ID { get; set; }
        public string KEGIATAN { get; set; }
        public string KOORDINATOR { get; set; }
        public DateTime TGL_MULAI { get; set; }
        public DateTime TGL_BERAKHIR { get; set; }
        //public string STATUS { get; set; }
    }
    #endregion

    #region Karang Taruna
    public class KarangTaruna
    {
        [Key]
        public int ID { get; set; }
        public string KODE_JABATAN { get; set; }
        public string NAMA { get; set; }
        [Required(ErrorMessage = "Nama harus diisi!")]
        public char JENIS_KELAMIN { get; set; }
        [Required(ErrorMessage = "Nama harus diisi!")]
        public string NIK { get; set; }
        public string SK { get; set; }
        public string? SK_BERHENTI { get; set; }
        
        public string NO_TELP { get; set; }
        [Required(ErrorMessage = "No telp harus diisi!")]
        public string ALAMAT { get; set; }
        [Required(ErrorMessage = "Alamat harus diisi!")]
        public DateTime TGL_PENGANGKATAN { get; set; }
        [Required(ErrorMessage = "Tanggal masuk harus diisi!")]

        public DateTime? TGL_PEMBERHENTIAN { get; set; }

        public bool ACTIVE { get; set; }
    }

    public class VW_KarangTaruna
    {
        [Key]
        public int ID { get; set; }
        public string KODE_JABATAN { get; set; }
        public string JABATAN { get; set; }
        public string NAMA { get; set; }
        public char JENIS_KELAMIN { get; set; }
        public string NIK { get; set; }
        public string SK { get; set; }
        public string SK_BERHENTI { get; set; }
        public string NO_TELP { get; set; }
        public string ALAMAT { get; set; }
        public DateTime TGL_PENGANGKATAN { get; set; }

        public string TGL_PEMBERHENTIAN { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public bool ACTIVE { get; set; }

    }

    public class KegiatanTaruna
    {
        [Key] public int ID { get; set; }
        public string KEGIATAN { get; set; }
        public string KOORDINATOR { get; set; }
        public DateTime TGL_MULAI { get; set; }
        public DateTime TGL_BERAKHIR { get; set; }
        //public string STATUS { get; set; }
    }
    #endregion

    #region PKK
    public class PKK
    {
        [Key]
        public int ID { get; set; }
        public string KODE_JABATAN { get; set; }
        public string NAMA { get; set; }
        public string SK { get; set; }
        public string? SK_BERHENTI { get; set; }
        public char JENIS_KELAMIN { get; set; }
        [Required(ErrorMessage = "Nama harus diisi!")]
        public string NIK { get; set; }
        [Required(ErrorMessage = "NIK harus diisi!")]
        public string NO_TELP { get; set; }
        [Required(ErrorMessage = "No telp harus diisi!")]
        public string ALAMAT { get; set; }
        [Required(ErrorMessage = "Alamat harus diisi!")]
        public DateTime TGL_PENGANGKATAN { get; set; }
        [Required(ErrorMessage = "Tanggal masuk harus diisi!")]

        public DateTime? TGL_PEMBERHENTIAN { get; set; }

        public bool ACTIVE { get; set; }
    }

    public class VW_PKK
    {
        [Key]
        public int ID { get; set; }
        public string KODE_JABATAN { get; set; }
        public string JABATAN { get; set; }
        public string NAMA { get; set; }
        public char JENIS_KELAMIN { get; set; }
        public string NIK { get; set; }
        public string SK_BERHENTI { get; set; }
        public string SK { get; set; }
        public string NO_TELP { get; set; }
        public string ALAMAT { get; set; }
        public DateTime TGL_PENGANGKATAN { get; set; }

        public string TGL_PEMBERHENTIAN { get; set; }
        public bool ACTIVE { get; set; }

    }

    public class KegiatanPKK
    {
        [Key] public int ID { get; set; }
        public string KEGIATAN { get; set; }
        public string KOORDINATOR { get; set; }
        public DateTime TGL_MULAI { get; set; }
        public DateTime TGL_BERAKHIR { get; set; }
        //public string STATUS { get; set; }
    }
    #endregion

    #region Penduduk

    public class Penduduk
    {
        [Key]
        public int ID { get; set; }
        public string NIK { get; set; }
        public string? NO_AKTA { get; set; }
        public string NAMA { get; set; }
        public string KK { get; set; }
        public string POB { get; set; }
        public DateTime DOB { get; set; }
        public char JENIS_KELAMIN { get; set; }
        public int ID_DLMKELUARGA { get; set; }
        public int ID_AGAMA { get; set; }
        public string? NAMA_AYAH { get; set; }
        public string? NAMA_IBU { get; set; }
        public string? NO_TELP { get; set; }
        public int ID_DUSUN { get; set; }
        public string? RW { get; set; }
        public string? RT { get; set; }
        public int ID_PENDIDIKAN { get; set; }
        public int ID_PEKERJAAN { get; set; }
        public int ID_KAWIN { get; set; }
    }

    public class VW_Penduduk
    {
        [Key]
        public int ID { get; set; }
        public string NIK { get; set; }
        public string NAMA { get; set; }
        public string KK { get; set; }
        public string? NO_AKTA { get; set; }
        public string POB { get; set; }
        public DateTime DOB { get; set; }
        public char JENIS_KELAMIN { get; set; }
        public int ID_DLMKELUARGA { get; set; }
        public int DLMKELUARGA { get; set; }
        public int ID_AGAMA { get; set; }
        public string AGAMA { get; set; }
        public string? NAMA_AYAH { get; set; }
        public string? NAMA_IBU { get; set; }
        public string? NO_TELP { get; set; }
        public int ID_DUSUN { get; set; }
        public string DUSUN { get; set; }
        public string? RW { get; set; }
        public string? RT { get; set; }
        public int ID_PENDIDIKAN { get; set; }
        public int ID_PEKERJAAN { get; set; }
        public string PENDIDIKAN { get; set; }
        public string PEKERJAAN { get; set; }
        public int ID_KAWIN { get; set; }
        public string KAWIN { get; set; }
    }
    #endregion
}
