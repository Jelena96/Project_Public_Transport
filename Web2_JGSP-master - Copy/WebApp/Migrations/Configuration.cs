namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.SignalR.Hosting;
    using WebApp.Models;
    using WebApp.Persistence;

    public sealed class Configuration : DbMigrationsConfiguration<WebApp.Persistence.ApplicationDbContext>
    {
        ApplicationDbContext db = new ApplicationDbContext();
        
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebApp.Persistence.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            try
            {
                if (!context.Roles.Any(r => r.Name == "Admin"))
                {
                    var store = new RoleStore<IdentityRole>(context);
                    var manager = new RoleManager<IdentityRole>(store);
                    var role = new IdentityRole { Name = "Admin" };
                    manager.Create(role);
                }

                if (!context.Roles.Any(r => r.Name == "Controller"))
                {
                    var store = new RoleStore<IdentityRole>(context);
                    var manager = new RoleManager<IdentityRole>(store);
                    var role = new IdentityRole { Name = "Controller" };

                    manager.Create(role);
                }

                if (!context.Roles.Any(r => r.Name == "AppUser"))
                {
                    var store = new RoleStore<IdentityRole>(context);
                    var manager = new RoleManager<IdentityRole>(store);
                    var role = new IdentityRole { Name = "AppUser" };

                    manager.Create(role);
                }

                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                if (!context.Users.Any(u => u.UserName == "admin@yahoo.com"))
                {
                    var user = new ApplicationUser() { Id = "admin", UserName = "admin@yahoo.com", Email = "admin@yahoo.com", PasswordHash = ApplicationUser.HashPassword("Admin123!"), FirstName = "AdminFirstN", LastName = "AdminLastN", Address = "NeamPojma", Role = "Admin", BirthDate = "10.5.1994.", ImageUrl = "", VerificationStatus = "Valid" };
                    userManager.Create(user);
                    userManager.AddToRole(user.Id, "Admin");
                }

                if (!context.Users.Any(u => u.UserName == "aa@yahoo.com"))
                {
                    var user = new ApplicationUser() { Id = "aa", UserName = "aa@yahoo.com", Email = "aa@yahoo.com", PasswordHash = ApplicationUser.HashPassword("Maja123!"), FirstName = "MajaFirstN", LastName = "MajaLastN", Address = "MajaPojma", Role = "AppUser", BirthDate = "4.5.1994.", ImageUrl = "", VerificationStatus = "Valid" };
                    userManager.Create(user);
                    userManager.AddToRole(user.Id, "AppUser");
                }

                if (!context.Users.Any(u => u.UserName == "controller2@yahoo.com"))
                {
                    var user = new ApplicationUser() { Id = "controler2", UserName = "controller2@yahoo.com", Email = "controller2@yahoo.com", PasswordHash = ApplicationUser.HashPassword("controller2"), FirstName = "Mihajlo", LastName = "Mikic", Address = "bb", Role = "Controller", BirthDate = "4.5.1994.", ImageUrl = "", VerificationStatus = "Valid" };
                    userManager.Create(user);
                    userManager.AddToRole(user.Id, "Controller");
                }

            }
            catch (Exception e)
            {
                Trace.WriteLine("sta je problem" + e.InnerException);
            }
        }
    }
}
