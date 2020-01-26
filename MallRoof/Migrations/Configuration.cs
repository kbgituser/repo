namespace MallRoof.Migrations
{
    using MallRoof.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using System.Web;
    
    using Microsoft.AspNet.Identity.Owin;

    internal sealed class Configuration : DbMigrationsConfiguration<MallRoof.DAL.MallContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        
        protected override void Seed(MallRoof.DAL.MallContext context)
        {

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "LandLord"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "LandLord" };
                manager.Create(role);
            }


            if (!context.Users.Any(u => u.UserName == "admin@ad.me"))
            {
                var store = new UserStore<User>(context);
                var manager = new UserManager<User>(store);
                var user = new User { UserName = "admin@ad.me", Email = "admin@ad.me", EmailConfirmed = true };

                manager.Create(user, "180884");
                manager.AddToRole(user.Id, "Admin");
            }

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Cities.AddOrUpdate(
                x=>x.Name,
                new City() { CityId = Guid.NewGuid(), Name = "Нур-Султан (Астана)"},
                new City() { CityId = Guid.NewGuid(), Name = "Алматы" },
                new City() { CityId = Guid.NewGuid(), Name = "Шымкент" },
                new City() { CityId = Guid.NewGuid(), Name = "Караганды" },
                new City() { CityId = Guid.NewGuid(), Name = "Орал" },
                new City() { CityId = Guid.NewGuid(), Name = "Актобе" },
                new City() { CityId = Guid.NewGuid(), Name = "Костанай" },
                new City() { CityId = Guid.NewGuid(), Name = "Петропавловск" },
                new City() { CityId = Guid.NewGuid(), Name = "Кокшетау" },
                new City() { CityId = Guid.NewGuid(), Name = "Павлодар" },
                new City() { CityId = Guid.NewGuid(), Name = "Семей" },
                new City() { CityId = Guid.NewGuid(), Name = "Усть-Каменогорск" },
                new City() { CityId = Guid.NewGuid(), Name = "Талдыкорган" },
                new City() { CityId = Guid.NewGuid(), Name = "Тараз" },
                new City() { CityId = Guid.NewGuid(), Name = "Кызылорда" },
                new City() { CityId = Guid.NewGuid(), Name = "Актау" },
                new City() { CityId = Guid.NewGuid(), Name = "Атырау" },
                new City() { CityId = Guid.NewGuid(), Name = "Туркестан" },
                new City() { CityId = Guid.NewGuid(), Name = "Темиртау" }
                );
            context.SaveChanges();
        }
    }
}
