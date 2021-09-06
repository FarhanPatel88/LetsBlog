using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsBlog.Models.Account
{
    public class ApplicationUser
    {
        public int ApplicationUserID { get; set; }

        public string Username { get; set; }
                                                          
        public string Email { get; set; }

        public string Fullname { get; set; }

        public string Token { get; set; }
    }
}
