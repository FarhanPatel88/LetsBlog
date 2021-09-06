using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsBlog.Models.Account
{
    public class ApplicationUserLogin
    {
        [Required(ErrorMessage = "Username is required")]
        [MinLength(5, ErrorMessage = "Username should be 5-20 characters long!")]
        [MaxLength(20, ErrorMessage = "Username should be 5-20 characters long!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(10, ErrorMessage = "Password should be 10-50 characters long!")]
        [MaxLength(50, ErrorMessage = "Password should be 10-50 characters long!")]
        public string Password { get; set; }
    }
}
