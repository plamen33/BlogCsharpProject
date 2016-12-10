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

// done
// check:   used here:  using Microsoft.AspNet.Identity.Owin;

namespace BlogJuneMVC.Controllers
{
    [ValidateInput(false)]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UsersController()
        {
        }

        public UsersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Users
        [Authorize(Roles = "Administrators")]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Create
        [Authorize(Roles = "Administrators")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overusering attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrators")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,

                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    // add user to User Role - User from the Users Admin Panel:
                    UserManager.AddToRole(user.Id, "User");

                    this.AddNotification("Successful user creation!", NotificationType.SUCCESS);
                    return RedirectToAction("Index", "Users");
                }

                // AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "Administrators")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var roles = db.Roles.ToList();
            ViewBag.Roles = roles;
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overusering attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrators")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,RoleId")] EditUserViewModel userViewModel)
        {
            ApplicationUser user = null;
            if (ModelState.IsValid)
            {
                user = userViewModel.Save();
                this.AddNotification("User edited!", NotificationType.INFO);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "Administrators")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [Authorize(Roles = "Administrators")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {

            //// if the Admin User delete himself
            if (id == User.Identity.GetUserId())
            {
                this.AddNotification("You cannot delete yourself - Code Forbidden!", NotificationType.ERROR);
                this.AddNotification("You want to break my code - NOT THIS TIME !", NotificationType.WARNING);
                return RedirectToAction("Index");
            }
            else
            {
                ApplicationUser user = db.Users.Find(id);

                // remove posts from users
                foreach (var item in db.Posts)
                {
                    if (item.Author == user)
                    {
                        // delete photo before deleting the user /////////////
                        string oldFileName = item.Image;
                        string deletePath = Request.MapPath("~/images/posts/" + oldFileName);
                        if (System.IO.File.Exists(deletePath))
                        {
                            System.IO.File.Delete(deletePath);
                        }
                        /////////////
                        db.Posts.Remove(item);
                    }
                }
                // remove comments by the user:
                foreach (var item in db.Comments)
                {
                    if (item.Author == user)
                    { db.Comments.Remove(item); }
                }

                // now remove the user himself
                db.Users.Remove(user);
                db.SaveChanges();
                this.AddNotification("User deleted!", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }

        }

		 // GET: /EditPassword
        public ActionResult EditUserPassword(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResetPasswordViewModel model = new ResetPasswordViewModel() { Id = id };
            return View(model);
        }

        //
        // POST: /EditPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var removePassword = UserManager.RemovePassword(model.Id);
            if (removePassword.Succeeded)
            {
                //Removed Password Success
                var AddPassword = UserManager.AddPassword(model.Id, model.Password);
                if (AddPassword.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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


//using BlogJuneMVC.Models;
//using MVCBlog.Models;

//namespace BlogJuneMVC.Controllers

//Post post = db.Posts.Include(p => p.Author).SingleOrDefault(p => p.Id == id);
//            if (post == null || post.Author == null || post.Author.UserName != User.Identity.Name)