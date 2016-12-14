using BlogJuneMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BlogJuneMVC.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)] //the name to be unique (there won't be categories with the same name) 
        [StringLength(20)]
        public string Name { get; set; }

    }
}