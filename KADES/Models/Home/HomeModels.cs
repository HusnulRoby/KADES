using KADES.Models.Account;
using System.ComponentModel.DataAnnotations;

namespace KADES.Models.Home
{
    public class HomeModels
    {
        public Note Note { get; set; }
        public List<Note> listNote { get; set; }
        public VW_Note VW_Note { get; set; }
        public List<VW_Note> listVwNote { get; set; }

        public RFUsers Users { get; set; }
    }

    public class Note
    {
        [Key]
        public int ID { get; set; }
        public string JUDUL { get; set; }
        public string NOTE { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
    }

    public class VW_Note
    {
        [Key]
        public int ID { get; set; }
        public string JUDUL { get; set; }
        public string NOTE { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string USERNAME { get; set; }
    }
}
