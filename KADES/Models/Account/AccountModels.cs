using System.ComponentModel.DataAnnotations;

namespace KADES.Models.Account
{
    public class AccountModels
    {
        public RFUsers RFUsers { get; set; }
        public RFGroup RFGroup { get; set; }
    }

    public class RFGroup
    {
        [Key]
        public string GROUPID { get; set; }
        public string GROUP_NAME { get; set; }
    }

    public class RFUsers
    {
        [Key]
        public string USERID { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string GROUPID { get; set; }
        public string EMAIL { get; set; }
    }
}
