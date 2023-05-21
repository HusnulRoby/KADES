using System.ComponentModel.DataAnnotations;

namespace KADES.Models.Account
{
    public class AccountModels
    {
        public RFUsers RFUsers { get; set; }
        public ForgotPass ForgotPass { get; set; }
        public List<RFUsers> ListRFUsers { get; set; }
        public List<VW_Users> ListVW_Users { get; set; }
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
        public int ID { get; set; }
        public string USERID { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string GROUPID { get; set; }
        public string EMAIL { get; set; }
    }

    public class ForgotPass
    {
        [Key]
        public string USERID { get; set; }
        public string NEW_PASSWORD { get; set; }
        public string CONFIRM_PASSWORD { get; set; }
    }

    public class VW_Users
    {
        [Key]
        public int ID { get; set; }
        public string USERID { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string GROUPID { get; set; }
        public string GROUP_NAME { get; set; }
        public string EMAIL { get; set; }
    }
}
