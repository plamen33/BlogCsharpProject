namespace MVCBlog.Migrations
{
    using BlogJuneMVC.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;

    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                // If the database is empty, populate sample data in it

                CreateUser(context, "Admin", "Admin", "OfBlog", "admin@gmail.com", "12345");
                CreateUser(context, "User1", "UserOne", "One", "userone@user.bg", "12345");
                CreateUser(context, "User2", "UserTwo", "Two", "usertwo@user.bg", "12345");

                CreateRole(context, "Administrators");
                AddUserToRole(context, "admin@gmail.com", "Administrators");

                CreatePost(context,
                    title: "Work Begins on HTML5.1",
                    body: @"<p>The World Wide Web Consortium (W3C) has begun work on <b>HTML5.1</b>, and this time it is handling the creation of the standard a little differently. The specification has its <b><a href=""https://w3c.github.io/html/"">own GitHub project</a></b> where anyone can see what is happening and propose changes.</p>
                    <p>The organization says the goal for the new specification is ""to <b>match reality better</b>, to make the specification as clear as possible to readers, and of course to make it possible for all stakeholders to propose improvements, and understand what makes changes to HTML successful.""</p>
                    <p>Creating HTML5 took years, but W3C hopes using GitHub will speed up the process this time around. It plans to release a candidate recommendation for HTML5.1 by <b>June</b> and a full recommendation in <b>September</b>.</p>",
                    date: new DateTime(2016, 03, 27, 17, 53, 48),
                    authorUsername: "Admin"
                );

                CreatePost(context,
                    title: "Windows 10 Preview with Bash Support Now Available",
                    body: @"<p>Microsoft has released a new <b>Windows 10 Insider Preview</b> that includes native support for <b>Bash running on Ubuntu Linux</b>. The company first announced the new feature at last week''s Build development conference, and it was one of the biggest stories of the event. The current process for installing Bash is a little complication, but Microsoft has a blog post that explains how the process works.</p>
                    <p>The preview build also includes <b>Cortana</b> upgrades, extensions support, the new <b>Skype</b> Universal Windows Platform app and some interface improvements.</p>",
                    date: new DateTime(2016, 05, 11, 08, 22, 03),
                    authorUsername: "User1"
                );

                CreatePost(context,
                    title: "Atom Text Editor Gets New Windows Features",
                    body: @"<p>GitHub has released <b>Atom 1.7</b>, and the updated version of the text editor offers improvements for Windows developers. Specifically, it is now easier to build in Visual Studio, and it now supports the Appveyor CI continuous integration service for Windows.</p>
                    <p>Other new features include improved tab switching, tree view and crash recovery. GitHub noted, ""Crashes are nobody''s idea of fun, but in case Atom does crash on you, it periodically saves your editor state. After relaunching Atom after a crash, you should find all your work saved and ready to go.""</p>
                    <p>GitHub has also released a beta preview of Atom 1.8.</p>",
                    date: new DateTime(2016, 03, 27, 17, 53, 48),
                    authorUsername: "User2"
                );



                context.SaveChanges();
            }
        }

        private void CreateUser(ApplicationDbContext context,
            string username, string firstname, string lastname, string email, string password)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            var user = new ApplicationUser
            {
                UserName = username,
                FirstName = firstname,
                LastName = lastname,
                Email = email,
            };

            var userCreateResult = userManager.Create(user, password);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }
        }

        private void CreateRole(ApplicationDbContext context, string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(roleName));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }
        }

        private void AddUserToRole(ApplicationDbContext context, string userName, string roleName)
        {
            var user = context.Users.First(u => u.UserName == userName);
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            var addAdminRoleResult = userManager.AddToRole(user.Id, roleName);
            if (!addAdminRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addAdminRoleResult.Errors));
            }
        }

        private void CreatePost(ApplicationDbContext context,
            string title, string body, DateTime date, string authorUsername)
        {
            var post = new Post();
            post.Title = title;
            post.Body = body;
            post.Date = date;
            post.Author = context.Users.Where(u => u.UserName == authorUsername).FirstOrDefault();
            context.Posts.Add(post);
        }
    }
}




//This was the OOB content here:
//namespace BlogJuneMVC.Migrations
//{
//    using System;
//    using System.Data.Entity;
//    using System.Data.Entity.Migrations;
//    using System.Linq;

//    internal sealed class Configuration : DbMigrationsConfiguration<BlogJuneMVC.Models.ApplicationDbContext>
//    {
//        public Configuration()
//        {
//            AutomaticMigrationsEnabled = true;
//            ContextKey = "BlogJuneMVC.Models.ApplicationDbContext";
//        }

//        protected override void Seed(BlogJuneMVC.Models.ApplicationDbContext context)
//        {
//            //  This method will be called after migrating to the latest version.

//            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
//            //  to avoid creating duplicate seed data. E.g.
//            //
//            //    context.People.AddOrUpdate(
//            //      p => p.FullName,
//            //      new Person { FullName = "Andrew Peters" },
//            //      new Person { FullName = "Brice Lambson" },
//            //      new Person { FullName = "Rowan Miller" }
//            //    );
//            //
//        }
//    }
//}
