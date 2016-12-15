﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogJuneMVC.Models;
using BlogJuneMVC.Extensions;
using PagedList;
using System.IO;

namespace BlogJuneMVC.Controllers
{
    [ValidateInput(false)]
    // we can validate text with html
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

          // GET: Posts
        public ActionResult Index(int? page, string sortBy)
        {
            ViewBag.SortDateParameter = string.IsNullOrEmpty(sortBy) ? "Date asc" : "";

            var posts = db.Posts.AsQueryable();
            posts = posts.Include(p => p.Author);
            int pageSize = 10; // pageSize is 10 - we will see 10 posts on a page
            int pageNumber = (page ?? 1); // means if the page is null use 1 or if not use whatever parameter page is

            switch (sortBy)
            {
                case "Date asc": posts = posts.OrderBy(x => x.Date); break;
                default:
                    posts = posts.OrderByDescending(x => x.Date); break;
                    
            }


            return View(posts.ToPagedList(page ?? pageNumber, pageSize));


            //return View(posts);
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Post post = db.Posts.Include(p => p.Author).SingleOrDefault(x => x.Id == id);
			Post post = db.Posts.Include(p => p.Author).Include(p => p.Comments).SingleOrDefault(x => x.Id == id);
            var comments = db.Comments.Include(p => p.Author).Where(cm => cm.PostId == post.Id).OrderByDescending(cm => cm.Date).ToList();
            post.Comments = comments;
            if (post == null)
            {
                return HttpNotFound();
            }
			post.Count = post.Count + 1;
            ViewBag.Player = post.Video;  // this option is needed to show and hide videos if null is in the video string
            db.SaveChanges();
            return View(post);
        }
         //GET: Search in Posts
        public ActionResult Search(string search, string TagSearch)
        {

            if (TagSearch == "Tags" && !String.IsNullOrEmpty(search))
            {
                var result = db.Posts.Include(p => p.Author).Where(p => p.Tags.Contains(search));

                return View(result);
            }
            else if (TagSearch != "Tags" && !String.IsNullOrEmpty(search))
            {
                var postsBody = db.Posts.Include(p => p.Author).Where(p => p.Body.Contains(search)).ToList();
                var postsTitle = db.Posts.Include(p => p.Author).Where(p => p.Title.Contains(search)).ToList();
                var result = postsBody.Concat(postsTitle).Distinct().ToList();  // without distinct 2 equal post could appear
                return View(result);
            }
            else
            {
                //var result = db.Posts.Where(p => p.Tags != "").ToList();
                //return View(result);
                var result = db.Posts.Where(p => p.Title.Equals("Title is empty")).ToList();
                return View(result);

            }
        }
        // GET: CategorySearch
        public ActionResult CategorySearch(string Category, string search, CatSearchPostViewModel model)
        {
            model.Categories = db.Categories.OrderBy(c => c.Id).ToList();

            if (search == null || search == String.Empty)

            { model.posts = db.Posts.Include(p => p.Author).Where(p => p.Category == Category).ToList(); }
            else
            {
                var postsBody = db.Posts.Include(p => p.Author).Where(p => p.Category == Category).Where(p => p.Body.Contains(search)).ToList();
                var postsTitle = db.Posts.Include(p => p.Author).Where(p => p.Category == Category).Where(p => p.Title.Contains(search)).ToList();
                model.posts = postsBody.Concat(postsTitle).Distinct().ToList();  // without distinct 2 equal post could appear
            }
            return View(model);
        }


        // GET: Posts/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.returnUrl = Request.UrlReferrer; // to return to page of Index
            var post = new Post();
            post.Categories = db.Categories.OrderBy(c => c.Id).ToList();
            return View(post);
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Title,Body,Date")] Post post)
        public ActionResult Create([Bind(Include = "Id,Title,Body,Category,Tags,Author,Count,Image,Categories,VideoLink")] Post post, HttpPostedFileBase upload, string returnUrl) // ", string returnUrl" neeeded to return to page number of Index
        {
            try
            {
                if (ModelState.IsValid)
                {
                    post.Author = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                    post.Date = DateTime.Now;
					post.Count = 0;
                    /////////////////////////// images upload ////

                    if (upload != null && (upload.ContentLength > 0 && upload.ContentLength < 33000))
                    {
                        var fileExt = Path.GetExtension(upload.FileName); // put it here not to have issues
                        if (fileExt.ToLower().EndsWith(".png") || fileExt.ToLower().EndsWith(".jpg") || fileExt.ToLower().EndsWith(".gif") || fileExt.ToLower().EndsWith(".jpeg"))// Important for security
                        {
                            var fileName = Path.GetFileName(upload.FileName);
                            var newFileName = Guid.NewGuid() + fileName;
                            var path = Path.Combine(Server.MapPath("~/images/posts/" + newFileName));
                            if (!Directory.Exists(HttpContext.Server.MapPath("~/images/posts/")))
                            {
                                Directory.CreateDirectory(HttpContext.Server.MapPath("~/images/posts/"));
                            }
                            upload.SaveAs(path);
                            post.Image = newFileName;
                        }
                    }

                    /////////////////////////////
                    // // Tag hack fix: ///
                    string tag = post.Tags;
                    List<string> taglist2 = tag.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                    post.Tags = string.Join(" ", taglist2).ToLower();
                    /// tag hack fix ////

                    /// /// // Video from youtube feature
                    string videoLink = post.VideoLink;
                    if (videoLink != null && videoLink.Contains("https://www.youtube.com/watch?v="))
                    { string video = videoLink.Substring(32); post.Video = video; }
                    else if (videoLink != null && videoLink.Contains("https://youtu.be/"))
                    { string video = videoLink.Substring(17); post.Video = video; }
                    else if (videoLink != null && videoLink.Contains("https://www.youtube.com/embed/"))
                    { string video = videoLink.Substring(30); post.Video = video; }
                    else { post.VideoLink = null; }
                    if (post.Video == "") { post.Video = null; }
                    /////////////  video youtube feature end///////////



                    db.Posts.Add(post);                   
                    db.SaveChanges();

                    this.AddNotification("New post created!", NotificationType.INFO);
                    if (returnUrl == null || returnUrl == "") { return RedirectToAction("Index"); }
                    return Redirect(returnUrl); // neeeded to return to page number of Index 
                    //return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes");
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        // [Authorize(Roles = "Administrators")]
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            post.Categories = db.Categories.OrderBy(c => c.Id).ToList();
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.returnUrl = Request.UrlReferrer; // to return to page of Index
            return View(post);
            
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        //[Authorize(Roles = "Administrators")]
        //[Authorize(Roles = "TrustedUser")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,Category,Date,Tags,Image,Count,Categories,Video,VideoLink")] Post post, string returnUrl, HttpPostedFileBase upload)  // ", string returnUrl" neeeded to return to page number of Index
        {
            if (ModelState.IsValid)
            {
                /// edit photo - delete old add new
                ///////////////////////////

                if (upload != null && (upload.ContentLength > 0 && upload.ContentLength < 33000))
                {
                    var fileExt = Path.GetExtension(upload.FileName); // put it here not to have issues
                    if (fileExt.ToLower().EndsWith(".png") || fileExt.ToLower().EndsWith(".jpg") || fileExt.ToLower().EndsWith(".gif") || fileExt.ToLower().EndsWith(".jpeg"))// Important for security
                    {
                        // delete old photo from System
                        string oldFileName = post.Image;
                        string deletePath = Request.MapPath("~/images/posts/" + oldFileName);
                        if (System.IO.File.Exists(deletePath))
                        {
                            System.IO.File.Delete(deletePath);
                        }

                        var fileName = Path.GetFileName(upload.FileName);
                        var newFileName = Guid.NewGuid() + fileName;
                        var path = Path.Combine(Server.MapPath("~/images/posts/" + newFileName));
                        if (!Directory.Exists(HttpContext.Server.MapPath("~/images/posts/")))
                        {
                            Directory.CreateDirectory(HttpContext.Server.MapPath("~/images/posts/"));
                        }
                        upload.SaveAs(path);

                        post.Image = newFileName;
                    }
                }
                /////////////////////////////
                /// /// // Video from youtube feature
                string videoLink = post.VideoLink;
                if (videoLink != null && videoLink.Contains("https://www.youtube.com/watch?v="))
                { string video = videoLink.Substring(32); post.Video = video; }
                else if (videoLink != null && videoLink.Contains("https://youtu.be/"))
                { string video = videoLink.Substring(17); post.Video = video; }
                else if (videoLink != null && videoLink.Contains("https://www.youtube.com/embed/"))
                { string video = videoLink.Substring(30); post.Video = video; }
                else
                {
                    if (post.Video == null) { post.VideoLink = null; }
                    else { post.VideoLink = "https://www.youtube.com/watch?v=" + post.Video; }
                }

                if (post.Video == "") { post.Video = null; post.VideoLink = null; }
                /////////////  video youtube feature end///////////

                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                this.AddNotification("Post edited!", NotificationType.INFO);
                if (returnUrl == null || returnUrl == "") { return RedirectToAction("Index"); }
                return Redirect(returnUrl); // neeeded to return to page number of Index 
                //return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        //[Authorize(Roles = "Administrators")]
        //[Authorize(Roles = "TrustedUser")]
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.returnUrl = Request.UrlReferrer; // to return to page of Index
            return View(post);
        }

        // POST: Posts/Delete/
        //[Authorize(Roles = "Administrators")]
        //[Authorize(Roles = "TrustedUser")]
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string returnUrl) // ", string returnUrl" neeeded to return to page number of Index
        {
            Post post = db.Posts.Find(id);
            // delete old photo from System
            string oldFileName = post.Image;
            string deletePath = Request.MapPath("~/images/posts/" + oldFileName);
            if (System.IO.File.Exists(deletePath))
            {
                System.IO.File.Delete(deletePath);
            }
            db.Posts.Remove(post);
            db.SaveChanges();
            this.AddNotification("Post deleted!", NotificationType.SUCCESS);
            if (returnUrl == null || returnUrl == "") { return RedirectToAction("Index"); }
            return Redirect(returnUrl); // neeeded to return to page number of Index 
            //return RedirectToAction("Index");
        }

		 public ActionResult DisplayTags()
        {
            var tagList = db.Posts.Where(p => p.Tags != "").ToList();

            return View(tagList);
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



//Post post = db.Posts.Include(p => p.Author).SingleOrDefault(p => p.Id == id);
//            if (post == null || post.Author == null || post.Author.UserName != User.Identity.Name)