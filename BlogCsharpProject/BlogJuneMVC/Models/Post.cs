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

        public Post()
        {
            this.Date = DateTime.Now;
        } 
        
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        // // decided not to restrict Length of Body intentionally
        [Required]
        [DataType(DataType.MultilineText)] // making the field Body - multiline, not just line
        [Display(Name = "Content")]         // this will make Content to appear instead of Body
        public string Body { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        [StringLength(127)]
        public string Tags { get; set; }

        public ApplicationUser Author { get; set; }
        public List<Comment> Comments { get; set; }   // needed for comments

        public int Count { get; set; }   // hit counter

        public string Image { get; set; }
        public string Video { get; set; }

        public string VideoLink { get; set; }

        public virtual List<Category> Categories { get; set; }   // needed for categories 
    }
}