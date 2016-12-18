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

                CreateUser(context, "Admin", "Admin", "OfBlog", "admin@gmail.com", "123");
                CreateUser(context, "User1", "UserOne", "One", "userone@user.bg", "123");
                CreateUser(context, "User2", "UserTwo", "Two", "usertwo@user.bg", "123");
                CreateUser(context, "DocBrown", "Emmett", "Brown", "emmett@brown.com", "123");
                CreateUser(context, "MechoPuh", "Mecho", "Puh", "mecho@puh.com", "123");


                CreateRole(context, "Administrators");
                CreateRole(context, "TrustedUser");
                CreateRole(context, "User");
                AddUserToRole(context, "Admin", "Administrators");
                AddUserToRole(context, "User1", "TrustedUser");
                AddUserToRole(context, "DocBrown", "TrustedUser");
                AddUserToRole(context, "User2", "User");
                AddUserToRole(context, "MechoPuh", "User");

                CreateCategory(context, "Programming");
                CreateCategory(context, "Science");
                CreateCategory(context, "Space");
                CreateCategory(context, "News");
                CreateCategory(context, "Other");

                CreatePost(context,
                    title: "Work Begins on HTML5.1",
                    body: @"The World Wide Web Consortium (W3C) has begun work on HTML5.1, and this time it is handling the creation of the standard a little differently. The specification has its https://w3c.github.io/html/ own GitHub project where anyone can see what is happening and propose changes.
                    The organization says the goal for the new specification is ""to match reality better, to make the specification as clear as possible to readers, and of course to make it possible for all stakeholders to propose improvements, and understand what makes changes to HTML successful.""
                    Creating HTML5 took years, but W3C hopes using GitHub will speed up the process this time around. It plans to release a candidate recommendation for HTML5.1 by June and a full recommendation in September.",
                    date: new DateTime(2016, 03, 27, 17, 53, 48),
                    category: "Programming",
                    tag: "html",
                    count: 0,
                    image: "aspnet.png",
                    authorUsername: "Admin",
                    video: null,
                    VideoLink: null
                );

                CreatePost(context,
                    title: "Windows 10 Preview with Bash Support Now Available",
                    body: @"Microsoft has released a new Windows 10 Insider Preview that includes native support for Bash running on Ubuntu Linux. The company first announced the new feature at last week''s Build development conference, and it was one of the biggest stories of the event. The current process for installing Bash is a little complication, but Microsoft has a blog post that explains how the process works.
                    The preview build also includes Cortana upgrades, extensions support, the new Skype Universal Windows Platform app and some interface improvements.",
                    date: new DateTime(2016, 05, 11, 08, 22, 03),
                    category: "News",
                    tag: "windows",
                    count: 0,
                    image: "aspnet.png",
                    authorUsername: "User1",
                    video: null,
                    VideoLink: null
                );

                CreatePost(context,
                    title: "Atom Text Editor Gets New Windows Features",
                    body: @"GitHub has released Atom 1.7, and the updated version of the text editor offers improvements for Windows developers. Specifically, it is now easier to build in Visual Studio, and it now supports the Appveyor CI continuous integration service for Windows.
                    Other new features include improved tab switching, tree view and crash recovery. GitHub noted, ""Crashes are nobody''s idea of fun, but in case Atom does crash on you, it periodically saves your editor state. After relaunching Atom after a crash, you should find all your work saved and ready to go.""
                    GitHub has also released a beta preview of Atom 1.8.",
                    date: new DateTime(2016, 03, 27, 17, 53, 48),
                    category: "Other",
                    tag: "java",
                    count: 0,
                    image: "aspnet.png",
                    authorUsername: "Admin",
                    video: null,
                    VideoLink: null
                );
                CreatePost(context,
                 title: "New Lambda features show Amazon is serious about serverless computing",
                 body: @" Some call it “lambda architecture,” others call it “serverless computing.” Whatever the name, the idea is the same: Break applications into clusters of single functions that run on demand and on automatically managed resources.
                 At Amazon, it’s called AWS Lambda, and the latest Lambda-related features indicate serverless computing is becoming a more important part 
                 of Amazon's portfolio.",
                 date: new DateTime(2016, 03, 27, 17, 53, 48),
                 category: "Programming",
                 tag: "java",
                 count: 0,
                 image: "aspnet.png",
                 authorUsername: "User2",
                 video: null,
                 VideoLink: null
                );

                CreatePost(context,
                title: "JavaScript insiders predict its future features",
                body: @" At one time, JavaScript evolved slowly. But its pace of evolution has stepped up considerably, particularly with last year's ECMAScript 2015 specification. In the coming years,  developers could see innovations like a type system and multithreading.
                In a discussion on JavaScript at the QCon conference in San Francisco on Monday, Stefan Penner and Jafar Husain elaborated on what they see potentially happening with JavaScript. Penner and Husain are both insiders key to the ECMAScript specification process; ECMAScript is the official specification that underlies JavaScript, and Technical Committee 39 handles it.
                Asked how TypeScript, Microsoft's typed superset of JavaScript, affects JavaScript development, Penner, of TC39 participant LinkedIn, said he was excited about it.",
             date: new DateTime(2016, 11, 27, 17, 53, 48),
             category: "Programming",
             tag: "java",
             count: 0,
             image: "aspnet.png",
             authorUsername: "User1",
             video: null,
             VideoLink: null
         );
                CreatePost(context,
                   title: "European satellite navigation system Galileo",
                   body: @"Galileo, which launched on Thursday, has 18 satellites in place so far and will initially be boosted by satellites from the American GPS system.
                   But its signal will grow stronger and more independent as satellites are added to the network 14,430 miles above the Earth.
                   The EU Commission and European Space Agency, which created the system, said Galileo should be fully operational by 2020, and will provide positioning data of unprecedented accuracy.
                   Elzbieta Bienkowska, EU industry commissioner, said in a statement: Today, Galileo, the most accurate satellite navigation system in the world, becomes a reality.",
                   date: new DateTime(2016, 12, 15, 17, 33, 21),
                   category: "Space",
                   tag: "space",
                   count: 0,
                   image: "aspnet.png",
                   authorUsername: "User1",
                   video: null,
                   VideoLink: null
               );
                CreatePost(context,
             title: "Red Hat links Java to Microsoft's Visual Studio Code",
             body: @"Using a newly minted protocol for language and IDE interoperability, Red Hat is expanding Java developers' ability to use Microsoft's Visual Studio Code editor.
                   This functionality was enabled by last week's release of Red Hat's Java language support extension to the Visual Studio Code marketplace. The extension is based on Red Hat's Java language server, an implementation of the Language Server Protocol recently forged by Red Hat, Microsoft, and CodEnvy. Based on JSON-RPC 2.0, the protocol defines calls and data structures to implement common language functionality on IDEs and editors.",
             date: new DateTime(2016, 11, 28, 17, 33, 21),
             category: "Programming",
             tag: "java c#",
             count: 0,
             image: "aspnet.png",
             authorUsername: "Admin",
             video: null,
             VideoLink: null
              );
                CreatePost(context,
                  title: "Convert JSON and XML markup into C# classes using Visual Studio",
                  body: @"Visual Studio provides a plethora of features that make you more productive. One such feature is converting XML or JSON markup into C# classes. This article discusses this feature with a few examples. XML and JSON are the two commonly used data formats for serializing data over the wire. Many a times you need to map XML or JSON markup to C# classes.",
                  date: new DateTime(2016, 11, 28, 17, 33, 21),
                  category: "Programming",
                  tag: "c#",
                  count: 0,
                  image: "aspnet.png",
                  authorUsername: "User1",
                  video: null,
                  VideoLink: null
              );
                CreatePost(context,
              title: "Bulgarian Yogurt",
              body: @"Bulgarian yogurt is the most popular variety of yogurt in the world and is one of the things that make Bulgarians proud to call themselves Bulgarians; it is their exclusive invention and heritage that dates back many centuries. A mildly sour-tasting yogurt, kiselo mlyako is undoubtedly the best and the healthiest of all dairy products that are available to consumers nowadays. The western world calls it Bulgarian yogurt but in its homeland, Bulgaria, it’s called sour milk. Whatever the name, this wonderful probiotic food has impeccable ancestry - it is believed to have been known for at least 4000 years.",
              date: new DateTime(2016, 11, 28, 15, 53, 33),
              category: "Other",
              tag: "yogurt",
              count: 0,
              image: "aspnet.png",
              authorUsername: "User2",
              video: "cUR52elXxBw",
              VideoLink: "https://www.youtube.com/watch?v=cUR52elXxBw"
             );
                CreatePost(context,
        title: "New stars discovery shed new light on Galaxy's formation",
        body: @"An astronomer has discovered a new family of stars in the core of the Milky Way Galaxy which provides new insights into the early stages of the Galaxy's formation. ",
        date: new DateTime(2016, 10, 27, 10, 17, 33),
        category: "Space",
        tag: "stars",
        count: 0,
        image: "aspnet.png",
        authorUsername: "Admin",
        video: null,
        VideoLink: null
       );
                CreatePost(context,
                  title: "Our brains have a basic algorithm that enables our intelligence",
                  body: @"Our brains have a basic algorithm that enables us to not just recognize a traditional Thanksgiving meal, but the intelligence to ponder the broader implications of a bountiful harvest as well as good family and friends.
                        A relatively simple mathematical logic underlies our complex brain computations - said Dr. Joe Z. Tsien, neuroscientist at the Medical College of Georgia at Augusta University, co-director of the Augusta University Brain and Behavior Discovery Institute and Georgia Research Alliance Eminent Scholar in Cognitive and Systems Neurobiology.
                  Tsien is talking about his Theory of Connectivity, a fundamental principle for how our billions of neurons assemble and align not just to acquire knowledge, but to generalize and draw conclusions from it.",
                  date: new DateTime(2016, 11, 28, 17, 33, 21),
                  category: "Science",
                  tag: "brain",
                  count: 0,
                  image: "aspnet.png",
                  authorUsername: "DocBrown",
                  video: null,
                  VideoLink: null
              );
                CreatePost(context,
               title: "About Honey",
               body: @"Honey is a sweet food made by bees foraging nectar from flowers. The variety produced by honey bees (the genus Apis)
                       is the one most commonly referred to, as it is the type of honey collected by most beekeepers and consumed by people.
                       Honey is also produced by bumblebees, stingless bees, and other hymenopteran insects such as honey wasps, though the
                       quantity is generally lower and they have slightly different properties compared with honey from the genus Apis. Honey
                       bees convert nectar into honey by a process of regurgitation and evaporation: they store it as a primary food source in
                       wax honeycombs inside the beehive.
                       Honey gets its sweetness from the monosaccharides fructose and glucose, and has about the same relative sweetness
                       as granulated sugar.It has attractive chemical properties for baking and a distinctive flavor that leads some people to
                       prefer it to sugar and other sweeteners.",
               date: new DateTime(2016, 12, 12, 12, 33, 21),
               category: "Other",
               tag: "honey",
               count: 0,
               image: "aspnet.png",
               authorUsername: "MechoPuh",
               video: "iT6IQx26eHk",
               VideoLink: "https://www.youtube.com/watch?v=iT6IQx26eHk"
           );
                CreatePost(context,
                 title: "Original BTTF DeLorean getting museum-quality restoration",
                 body: @"Of all the vehicles created for the three films, the most detailed hero car, otherwise known as the A Car, was left on display outside. The ensuing years of suffering in the elements weren't kind to the DMC-12, and Bob Gale, co-writer for the BTTF saga, is now working to have the vehicle completely restored to its original condition.
                  Joe Walser, J Ryan and Terry Matalas are currently working to get the car back to its former glory, but the project needs help. The craftsmen may not need roads where they're going, but they do need anyone with photos of the original A Car or parts from any of the movie DeLorean models to reach out via TimeMachineRestoration@gmail.com or the restoration's Facebook page.",
                 date: new DateTime(2016, 11, 28, 17, 33, 21),
                 category: "News",
                 tag: "dmc12",
                 count: 0,
                 image: "aspnet.png",
                 authorUsername: "Admin",
                 video: null,
                 VideoLink: null
             );
                CreatePost(context,
           title: "Astronomers use light from X-ray source to study nearby stellar cloud",
           body: @"A snapshot of the stellar life cycle has been captured in a new portrait from NASA's Chandra X-ray Observatory and the Smithsonian's Submillimeter Array (SMA). A cloud that is giving birth to stars has been observed to reflect X-rays from Cygnus X-3, a source of X-rays produced by a system where a massive star is slowly being eaten by its companion black hole or neutron star. This discovery provides a new way to study how stars form.",
           date: new DateTime(2016, 11, 22, 07, 33, 21),
           category: "Space",
           tag: "astronomy",
           count: 0,
           image: "aspnet.png",
           authorUsername: "User1",
           video: null,
           VideoLink: null
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
                RequiredLength = 3,
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
           string title, string body, DateTime date, string category, string tag, int count, string authorUsername, string image, string video, string VideoLink)
        {
            var post = new Post();
            post.Title = title;
            post.Body = body;
            post.Date = date;
            post.Category = category;
            post.Tags = tag;
            post.Author = context.Users.Where(u => u.UserName == authorUsername).FirstOrDefault();
            post.Count = count;
            post.Image = image;
            post.Video = video;
            post.VideoLink = VideoLink;
            context.Posts.Add(post);
        }
        private void CreateCategory(ApplicationDbContext context, string name)
        {
            var category = new Category();
            category.Name = name;
            context.Categories.Add(category);

        }
    }
}

