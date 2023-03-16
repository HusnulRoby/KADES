using System.ComponentModel.DataAnnotations;

namespace KADES.Models.Pelayanan
{
    public class PelayananModels
    {
        public TemplateSurat TemplateSurat { get; set; }
        public LogSurat LogSurat { get; set; }
        public List<LogSurat> ListLogSurat { get; set; }
        public List<TemplateSurat> ListTemplateSurat { get; set; }
        public VW_LogSurat VW_LogSurat { get; set; }
        public List<VW_LogSurat>? ListVW_LogSurat { get; set; }

    }

    public class TemplateSurat
    {
        public string? ID { get; set; }
        public string NO_SURAT { get; set; }
        public string NAMA_SURAT { get; set; }
        public string FILE_NAME { get; set; }
        public string PATH_FILE { get; set; }
        public bool ACTIVE { get; set; }
        public string? CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string? UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
    }

    public class LogSurat
    {
        [Required]
        public int ID { get; set; }
        public string ID_SURAT { get; set; }
        public string GENERATE_BY { get; set; }
        public DateTime GENERATE_DATE { get; set; }

    }

    public class VW_LogSurat
    {
        [Required]
        public int ID { get; set; }
        public string ID_SURAT { get; set; }
        public string NO_SURAT { get; set; }
        public string NAMA_SURAT { get; set; }
        public string FILE_NAME { get; set; }
        public string PATH_FILE { get; set; }
        public bool ACTIVE { get; set; }
        public string GENERATE_BY { get; set; }
        public DateTime GENERATE_DATE { get; set; }

    }

    public class LogSurat1
    {
        [Required]
        public int ID { get; set; }
        public string ID_SURAT { get; set; }
        public string NAMA_PEMOHON { get; set; }
        public string NIK { get; set; }
        public string NO_TELP { get; set; }
        public string ALAMAT { get; set; }
        public string ALASAN { get; set; }
        public string GENERATE_BY { get; set; }
        public DateTime GENERATE_DATE { get; set; }

    }

    public class VW_LogSurat1
    {
        [Required]
        public int ID { get; set; }
        public string ID_SURAT { get; set; }
        public string NO_SURAT { get; set; }
        public string NAMA_SURAT { get; set; }
        public string FILE_NAME { get; set; }
        public string PATH_FILE { get; set; }
        public bool ACTIVE { get; set; }
        public string NAMA_PEMOHON { get; set; }
        public string NIK { get; set; }
        public string NO_TELP { get; set; }
        public string ALAMAT { get; set; }
        public string ALASAN { get; set; }
        public string GENERATE_BY { get; set; }
        public DateTime GENERATE_DATE { get; set; }

    }

}
