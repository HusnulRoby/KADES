using System.ComponentModel.DataAnnotations;

namespace KADES.Models.Maintenance
{
    public class MaintenanceModels
    {
        #region Parameter Maintenance
        public RFJabatan RFJabatan { get; set; }
        public List<RFJabatan> ListRFJabatan { get; set; }
        #endregion


    }
    #region Parameter Maintenance

    public class RFJabatan
    {
        [Key]
        public string KODE_JABATAN { get; set; }
        public string JABATAN { get; set; }
        public string KODE_TYPE { get; set; }
        public bool ACTIVE { get; set; }
    }
    #endregion
}
