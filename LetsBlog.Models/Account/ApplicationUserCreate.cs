using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsBlog.Models.Account
{
    public class ApplicationUserCreate : ApplicationUserLogin
    {
        [MinLength(10, ErrorMessage = "Fullname should be 10-30 characters long")]
        [MaxLength(30, ErrorMessage = "Fullname should be 10-30 characters long")]
        public string Fullname { get; set; }

        [MaxLength(30, ErrorMessage = "Email should be at least 30 characters long")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }
}
