using BlogJuneMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogJuneMVC.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        [DataType(DataType.MultilineText)]  // making the field Text - multiline
        public string Text { get; set; }

        [Required]
        public int PostId { get; set; }

        //[Required]
        //public int AuthorId { get; set; }  // this is not needed

        //[Required]
        //public string AuthorName { get; set; } // this is not needed


        [Required]
        public DateTime Date { get; set; }

        public ApplicationUser Author { get; set; }
        public Post Post { get; set; }
      
    }
}