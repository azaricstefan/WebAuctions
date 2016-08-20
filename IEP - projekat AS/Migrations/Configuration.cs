namespace IEP___projekat_AS.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<IEP___projekat_AS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(IEP___projekat_AS.Models.ApplicationDbContext context)
        {
            context.Auctions.AddOrUpdate(
                    new Auction
                    {
                        //Id = 1,
                        //winner_Id = "a44730fd-210c-4884-84d4-bed6dbe6d311",
                        name = "Aukcija 1",
                        length = 10000,
                        price = 100,
                        creation = DateTime.Now,
                        opening = DateTime.Now,
                        closing = DateTime.Now,
                        details = "Detalji o aukciji",
                        img = "http://drop.ndtv.com/TECH/product_database/images/12292014103136AM_635_samsung_galaxy_note_4_slte.jpeg",
                        status = "READY",
                        start_offer = 100
                    }
                );
            context.Auctions.AddOrUpdate(
                    new Auction
                    {
                        //Id = 2,
                        //winner_Id = "a44730fd-210c-4884-84d4-bed6dbe6d311",
                        name = "Aukcija 2",
                        length = 10000,
                        price = 200,
                        creation = DateTime.Now,
                        opening = DateTime.Now,
                        closing = DateTime.Now,
                        details = "Detalji o aukciji",
                        img = "http://drop.ndtv.com/TECH/product_database/images/12292014103136AM_635_samsung_galaxy_note_4_slte.jpeg",
                        status = "OPEN",
                        start_offer = 200
                    }
                );
            context.Auctions.AddOrUpdate(
                    new Auction
                    {
                        //Id = 3,
                        //winner_Id = "a44730fd-210c-4884-84d4-bed6dbe6d311",
                        name = "Aukcija 3",
                        length = 10000,
                        price = 300,
                        creation = DateTime.Now,
                        opening = DateTime.Now,
                        closing = DateTime.Now,
                        details = "Detalji o aukciji",
                        img = "http://drop.ndtv.com/TECH/product_database/images/12292014103136AM_635_samsung_galaxy_note_4_slte.jpeg",
                        status = "DRAFT",
                        start_offer = 300
                    }
                );
            context.Auctions.AddOrUpdate(
                    new Auction
                    {
                        //Id = 4,
                        //winner_Id = "a44730fd-210c-4884-84d4-bed6dbe6d311",
                        name = "Aukcija 4",
                        length = 10000,
                        price = 400,
                        creation = DateTime.Now,
                        opening = DateTime.Now,
                        closing = DateTime.Now,
                        details = "Detalji o aukciji",
                        img = "http://drop.ndtv.com/TECH/product_database/images/12292014103136AM_635_samsung_galaxy_note_4_slte.jpeg",
                        status = "SOLD",
                        start_offer = 400
                    }
                );
            context.Auctions.AddOrUpdate(
                    new Auction
                    {
                        //Id = 5,
                        //winner_Id = "a44730fd-210c-4884-84d4-bed6dbe6d311",
                        name = "Aukcija 5",
                        length = 10000,
                        price = 500,
                        creation = DateTime.Now,
                        opening = DateTime.Now,
                        closing = DateTime.Now,
                        details = "Detalji o aukciji",
                        img = "http://drop.ndtv.com/TECH/product_database/images/12292014103136AM_635_samsung_galaxy_note_4_slte.jpeg",
                        status = "EXPIRED",
                        start_offer = 500
                    }
                );

            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //context.Users.AddOrUpdate(
            //        new ApplicationUser
            //        {
            //            Name          = "Test",
            //            Surname       = "Testic",
            //            Email         = "test@asp.net",
            //            UserName      = "test@asp.net",
            //            Credit        = 1000000,
            //            PasswordHash  = UserManager.PasswordHasher.HashPassword("2607290"),
            //            SecurityStamp = Guid.NewGuid().ToString()
            //        }
            //    );

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}