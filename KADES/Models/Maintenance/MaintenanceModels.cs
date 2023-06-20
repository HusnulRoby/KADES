using System.ComponentModel.DataAnnotations;

namespace KADES.Models.Maintenance
{
    public class MaintenanceModels
    {
        #region Parameter Maintenance
        public RFJabatan RFJabatan { get; set; }
        public List<RFJabatan> ListRFJabatan { get; set; }

        public RfDusun RfDusun { get; set; }
        public List<RfDusun> ListRfDusun { get; set; }

        public RfAgama RfAgama { get; set; }
        public List<RfAgama> ListRfAgama { get; set; }

        public RfPendidikan RfPendidikan { get; set; }
        public List<RfPendidikan> ListRfPendidikan { get; set; }

        public RfPekerjaan RfPekerjaan { get; set; }
        public List<RfPekerjaan> ListRfPekerjaan { get; set; }

        public RfJenisAset JenisAset { get; set; }
        public List<RfJenisAset> ListJenisAset { get; set; }


        #endregion



    }
    #region Parameter Maintenance

    public class RFJabatan
    {

        [Key] public int ID { get; set; }
        public string KODE_JABATAN { get; set; }
        public string JABATAN { get; set; }
        public string KODE_TYPE { get; set; }
        public bool ACTIVE { get; set; }
    }

    public class RfDusun
    {
        [Key]
        public int ID { get; set; }
        public string DUSUN { get; set; }
        public bool ACTIVE { get; set; }
    }

    public class RfAgama
    {
        [Key]
        public int ID { get; set; }
        public string AGAMA { get; set; }
        public bool ACTIVE { get; set; }
    }
    public class RfPendidikan
    {
        [Key]
        public int ID { get; set; }
        public string PENDIDIKAN { get; set; }
        public bool ACTIVE { get; set; }
    }

    public class RfPekerjaan
    {
        [Key]
        public int ID { get; set; }
        public string PEKERJAAN { get; set; }
        public bool ACTIVE { get; set; }
    }

    public class RfJenisAset
    {
        public int ID { get; set; }
        public string KODE_JNSASET { get; set; }
        public string JENIS_ASET { get; set; }
        public bool ACTIVE { get; set; }
    }
    #endregion
}
    