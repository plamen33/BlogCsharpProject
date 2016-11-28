using BlogJuneMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BlogJuneMVC.Models
{
    public class Post
    {
       

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public DateTime Date { get; set; }

        //public string Author_Id { get; set; }
        //[ForeignKey("Author_Id")]
        public ApplicationUser Author { get; set; }

     
    }
}