using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KADES.Models
{
    [Table ("Test")]
    public class Test
    {
        
        [Required]
        public int Id { get; set; }
        public string IdSurat { get; set; }
        public string NoSurat { get; set; }
        public string NamaPemohon { get; set; }
        public int NIK { get; set; }
        public int Reason { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
