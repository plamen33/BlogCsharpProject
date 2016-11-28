using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlogJuneMVC.Models
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<BlogJuneMVC.Models.Post> Posts { get; set; }

        //public System.Data.Entity.DbSet<BlogJuneMVC.Models.ApplicationUser> ApplicationUsers { get; set; }
        // public System.Data.Entity.DbSet<MVCBlog.Models.Post> Posts { get; set; }
    }
}