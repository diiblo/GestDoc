using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelotransApp.Model
{
    public class UserModel
    {
        public int IdUser { get; set; }
        public int IdType { get; set; }
        public int IdFonction { get; set; }
        public string NomUser { get; set; }
        public string PrenomUser { get; set; }
        public string PhotoProfilUser { get; set; }
        public string EmailUser { get; set; }
        public int PhoneUser { get; set; }
        public string MotdepasseUser { get; set; }
        public int EmailVerifyCodeUser { get; set; }
        public int VerifyUser { get; set; }
        public int EnableUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
