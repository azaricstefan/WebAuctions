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
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(IEP___projekat_AS.Models.ApplicationDbContext context)
        {
            context.Auctions.AddOrUpdate(
                    new Auction
                    {
                        //Id = 1,
                        //winner_Id = "a44730fd-210c-4884-84d4-bed6dbe6d311",
                        name = "GoPro Hero 4",
                        length = 3600,
                        price = 1,
                        creation = DateTime.Now,
                        //opening = DateTime.Now,
                        //closing = DateTime.Now,
                        details = "Detalji o aukciji",
                        img = "https://www.bhphotovideo.com/images/images1000x1000/gopro_chdhy_401_hero4_silver_edition_adventure_1078007.jpg",
                        status = "READY",
                        start_offer = 1
                    }
                );
            context.Auctions.AddOrUpdate(
                    new Auction
                    {
                        //Id = 2,
                        //winner_Id = "a44730fd-210c-4884-84d4-bed6dbe6d311",
                        name = "Note 4",
                        length = 7200,
                        price = 1,
                        creation = DateTime.Now,
                        //opening = DateTime.Now,
                        //closing = DateTime.Now,
                        details = "Detalji o aukciji",
                        img = "http://drop.ndtv.com/TECH/product_database/images/12292014103136AM_635_samsung_galaxy_note_4_slte.jpeg",
                        status = "READY",
                        start_offer = 1
                    }
                );
            context.Auctions.AddOrUpdate(
                    new Auction
                    {
                        //Id = 3,
                        //winner_Id = "a44730fd-210c-4884-84d4-bed6dbe6d311",
                        name = "ASUS ROG",
                        length = 10800,
                        price = 1,
                        creation = DateTime.Now,
                        //opening = DateTime.Now,
                        //closing = DateTime.Now,
                        details = "Detalji o aukciji",
                        img = "https://dlcdnimgs.asus.com/websites/global/products/LYuLg2456gbkV5dw/img/03/14.png",
                        status = "READY",
                        start_offer = 1
                    }
                );
            context.Auctions.AddOrUpdate(
                    new Auction
                    {
                        //Id = 4,
                        //winner_Id = "a44730fd-210c-4884-84d4-bed6dbe6d311",
                        name = "Wireless AC5300",
                        length = 14400,
                        price = 1,
                        creation = DateTime.Now,
                        //opening = DateTime.Now,
                        //closing = DateTime.Now,
                        details = "Detalji o aukciji",
                        img = "http://images17.newegg.com/is/image/newegg/33-320-244-TS?$S640$",
                        status = "READY",
                        start_offer = 1
                    }
                );
            context.Auctions.AddOrUpdate(
                    new Auction
                    {
                        //Id = 5,
                        //winner_Id = "a44730fd-210c-4884-84d4-bed6dbe6d311",
                        name = "Gaming Keyboard",
                        length = 86399,
                        price = 1,
                        creation = DateTime.Now,
                        //opening = DateTime.Now,
                        //closing = DateTime.Now,
                        details = "Detalji o aukciji",
                        img = "http://www.corsair.com/~/media/corsair/product%20photos/keyboards/strafe/large/strafe_na_mx_red_01.png",
                        status = "READY",
                        start_offer = 1
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