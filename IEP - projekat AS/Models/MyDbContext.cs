//namespace IEP___projekat_AS.Models
//{
//    using System;
//    using System.Data.Entity;
//    using System.ComponentModel.DataAnnotations.Schema;
//    using System.Linq;

//    public partial class MyDbContext : DbContext
//    {
//        public MyDbContext()
//        //: base("name=DefaultConnection")
//        : base("name=MyDbContext")
//        {
//        }

//        public virtual DbSet<Auction> Auctions { get; set; }
//        public virtual DbSet<Offer> Offers { get; set; }
//        public virtual DbSet<Order> Orders { get; set; }

//        protected override void OnModelCreating(DbModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Auction>()
//                .HasMany(e => e.Offers)
//                .WithRequired(e => e.Auction)
//                .HasForeignKey(e => e.auction_Id)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Order>()
//                .Property(e => e.price)
//                .HasPrecision(18, 0);
//        }
//    }
//}
