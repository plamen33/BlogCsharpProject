using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogJuneMVC.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using BlogJuneMVC.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;

namespace BlogJuneMVC.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class RolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var roles = db.Roles.ToList();
            return View(roles);
        }

        // GET: /Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        public ActionResult Create(Microsoft.AspNet.Identity.EntityFramework.IdentityRole role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string roleName = role.Name;
                    if (roleName.Length > 33)
                    {
                        string roleLimited = role.Name.Substring(0, 33);
                        role.Name = roleLimited;
                        this.AddNotification("Role name was shortened to 33 characters. !", NotificationType.WARNING);
                    }
                    if (db.Roles.Any(r => r.Name == role.Name))
                    { this.AddNotification("You cannot have multiple Roles with the same name !", NotificationType.WARNING); }
                    db.Roles.Add(role);
                    db.SaveChanges();
                    ViewBag.ResultMessage = "Role created successfully !";
                    this.AddNotification("New Role created !", NotificationType.SUCCESS);

                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Roles/Edit/5
        public ActionResult Edit(string roleName)
        {
            var thisRole = db.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            return View(thisRole);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Microsoft.AspNet.Identity.EntityFramework.IdentityRole role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string roleName = role.Name;
                    if (roleName.Length > 33)
                    {
                        string roleLimited = role.Name.Substring(0, 33);
                        role.Name = roleLimited;
                        this.AddNotification("Role name was shortened to 33 characters. !", NotificationType.WARNING);
                    }

                    db.Entry(role).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // // Get Delete
        public ActionResult Delete(string roleName)
        {
            var thisRole = db.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            return View(thisRole);
        }
        // // Post Delete
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Microsoft.AspNet.Identity.EntityFramework.IdentityRole role)
        {

            db.Entry(role).State = System.Data.Entity.EntityState.Deleted;
            //db.Roles.Remove(role);
            db.SaveChanges();

            this.AddNotification("User Role deleted !", NotificationType.WARNING);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}


//Old functionality:
//    public ActionResult Delete(string RoleName)
//{
//    var thisRole = db.Roles.Where(r => r.Name.Equals(RoleName, System.StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
//    db.Roles.Remove(thisRole);
//    db.SaveChanges();
//    this.AddNotification("User Role deleted !", NotificationType.WARNING);
//    return RedirectToAction("Index");
//}

//using BlogJuneMVC.Models;
//using MVCBlog.Models;

//namespace BlogJuneMVC.Controllers

//Post post = db.Posts.Include(p => p.Author).SingleOrDefault(p => p.Id == id);
//            if (post == null || post.Author == null || post.Author.UserName != User.Identity.Name)