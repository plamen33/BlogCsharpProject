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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogJuneMVC.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var categories = db.Categories.OrderBy(c=>c.Id).ToList();
            return View(categories);
        }

         // GET: /Category/Create
         public ActionResult Create()
        {
            return View();
        }


       //POST: /Category/Create
       [HttpPost]
         public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {   
                if(db.Categories.Any(cat=>cat.Name == category.Name))
                { this.AddNotification("You cannot have multiple Categories with the same name ! Choose another name.", NotificationType.WARNING); return View(category); }
                //Save category in DB
                db.Categories.Add(category);
                db.SaveChanges();
                this.AddNotification("New post category created !", NotificationType.SUCCESS);
                return RedirectToAction("Index");
                
            }
            // If we got this far, something failed, redisplay form
            return View(category);
        }

        // GET: Category/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

                // Get the user from db
                var category = db.Categories
                    .FirstOrDefault(c => c.Id == id);

                if (category == null)
                {
                    return HttpNotFound();
                }

                return View(category);
            
        }
        // POST: User/Edit/


        [HttpPost]
       
        public ActionResult Edit(Category category)
        {
            try { 
                 if (ModelState.IsValid)
                 {
                  
                         //Save user in db
                         db.Entry(category).State = EntityState.Modified;
                         db.SaveChanges();
                
                         // redirect to index page
                         return RedirectToAction("Index");
                   
                 }
               }
            catch
            {
              this.AddNotification("You cannot have multiple Categories with the same name ! Choose another name.", NotificationType.WARNING); return View(category); 
             }
            // If we got this far, something failed, redisplay form
            return View(category);
        }
        // GET: Category/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


                // Get the category from db
                var category = db.Categories
                    .FirstOrDefault(c => c.Id == id);

                // Check if user exists
                if (category == null)
                {
                    return HttpNotFound();
                }

                // pass article to view
                return View(category);
            
        }

        // Post: Category/Delete

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
           
                // Get the category from db
                var category = db.Categories
                     .FirstOrDefault(c => c.Id == id);

                db.Categories.Remove(category);
                db.SaveChanges();
                this.AddNotification("Category was deleted !", NotificationType.WARNING);
            // redirect to index page
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
