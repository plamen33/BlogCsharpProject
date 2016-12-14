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
                    //Save category in DB
                    db.Categories.Add(category);
                    db.SaveChanges();

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
            if (ModelState.IsValid)
            {
             
                    //Save user in db
                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();

                    // redirect to index page
                    return RedirectToAction("Index");
              
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
