using System.ComponentModel.DataAnnotations.Schema;

namespace KADES.ModelClass
{
    public class TemplateSuratClass
    {
        [Table("TemplateSurat")]
        public class TemplateSuratTest
        {
            public string ID { get; set; }
            public string NO_SURAT { get; set; }
            public string NAMA_SURAT { get; set; }
            public string CREATED_BY { get; set; }
            public DateTime CREATED_DATE { get; set; }
            public string? UPDATED_BY { get; set; }
            public DateTime? UPDATED_DATE { get; set; }
        }
    }
}
