using KADES.Models.Maintenance;

namespace KADES.Models.Asset
{
    public class AssetModels
    {
        public DataAset DataAset { get; set; }
        public List<DataAset> ListAset { get; set; }

        public VW_DataAset VW_Aset { get; set; }
        public List<VW_DataAset> ListVW_Aset { get; set; }
    }

    public class DataAset 
    {
        public int ID { get; set; }
        public int ID_JNSASET { get; set; }
        public string KODE_ASET { get; set; }
        public string NAMA_ASET { get; set; }
        public string KODE_SUMBER { get; set; }
        public string KODE_KONDISI { get; set; }
        public string LOKASI { get;set; }
        public Double NILAI_ASET { get; set; }
        public DateTime TGL_INPUT { get; set; }
        public bool STATUS { get; set; }

    }

    public class VW_DataAset
    {
        public int ID { get; set; }
        public int ID_JNSASET { get; set; }
        public string JENIS_ASET { get; set; }
        public string KODE_ASET { get; set; }
        public string NAMA_ASET { get; set; }
        public string KODE_SUMBER { get; set; }
        public string SUMBER_ASET { get; set; }
        public string KODE_KONDISI { get; set; }
        public string LOKASI { get; set; }
        public Double NILAI_ASET { get; set; }
        public DateTime TGL_INPUT { get; set; }
        public bool STATUS { get; set; }
    }
}
