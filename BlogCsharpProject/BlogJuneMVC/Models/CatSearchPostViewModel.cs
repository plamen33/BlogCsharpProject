using BlogJuneMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogJuneMVC.Models
{
    public class CatSearchPostViewModel
    {
        public List<Post> posts { get; set; }
        public List<Category> Categories { get; set; }   
    }
}
