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
		// by this we make exact time to be put like in the real blogs - in Posts Controller we will remove the Date field from Create
		// so by this we won't be able to put Date at Create - it will be an actual one.
        public Post()
        {
            this.Date = DateTime.Now;
        } 

        [Key]
        public int Id { get; set; }

        [Required]
		[StringLength(200)] // we set the value of Title max symbols
        public string Title { get; set; }

        [Required]
		[DataType(DataType.MultilineText)] // making the field Body - multiline, not just line
		[Display(Name = "Content")]         // this will make Content to appear instead of Body
        public string Body { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Category { get; set; }
        [Required]
        public string Tags { get; set; }

        //public string Author_Id { get; set; }
        //[ForeignKey("Author_Id")]
        public ApplicationUser Author { get; set; }
		
		 public int Count { get; set; }   // hit counter

     
    }
}