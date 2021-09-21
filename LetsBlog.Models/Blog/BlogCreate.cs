using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsBlog.Models.Blog
{
    public class BlogCreate
    {
        public int BlogId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MinLength(10, ErrorMessage = "Title should be 10-50 characters long!")]
        [MaxLength(50, ErrorMessage = "Title should be 10-50 characters long!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [MinLength(300, ErrorMessage = "Content should be 300-5000 characters long!")]
        [MaxLength(5000, ErrorMessage = "Content should be 300-5000 characters long!")]
        public string Content { get; set; }

        public int? PhotoId { get; set; }
    }
}
