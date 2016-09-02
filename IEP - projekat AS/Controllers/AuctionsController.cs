using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IEP___projekat_AS.Models;
using PagedList;
using System.Data.Entity.Infrastructure;

namespace IEP___projekat_AS.Controllers
{
    public class AuctionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult OpenAuction(int? id)
        {
            var auction = db.Auctions.Find(id);
            auction.opening = DateTime.Now; //update opening
            bool saveFailed;
            do
            {
                saveFailed = false;
                try { db.SaveChanges(); }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;
                    // Update original values from the database 
                    var entry = ex.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                }
            } while (saveFailed);

            return View();
        }

        //// GET: Auctions NEW!
        //public ActionResult NewIndex()
        //{
        //    return View(db.Auctions.ToList());
        //}

        // POST: Auctions -> SEARCH!
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "name,typeOfSearch,minPrice,maxPrice,status")] SearchAuctionViewModel sa)
        {
            //search parameters
            var name = sa.name;
            var minPrice = sa.minPrice;
            var maxPrice = sa.maxPrice;
            var status = sa.status;
            var typeOfSearch = sa.typeOfSearch;

            var query = db.Auctions.Where(s => s.name.Contains(name)); //search for auctions with that name
            var aList = query.ToList();//.FirstOrDefault<Auction>();


            if (aList == null)
            {
                return HttpNotFound();
            }

            var savcm = new SearchAuctionViewContainerModel();
            savcm.searchAuctionViewModel = new SearchAuctionViewModel();
            savcm.auctionList = aList.ToPagedList(1, 3);
            return View(savcm);

            //return View(db.Auctions.ToList());
        }

        // GET: Auctions
        public ActionResult Index([Bind(Include = "name,typeOfSearch,minPrice,maxPrice,status")] SearchAuctionViewModel sa,
            string currentFilter, int? page, string sortOrder, string searchString)
        {
            var savcm = new SearchAuctionViewContainerModel(); //na pocetku kreiram dole postavljam
            savcm.searchAuctionViewModel = new SearchAuctionViewModel();

            int pageSize = 5; //elemenata po strani
            int pageNumber = (page ?? 1); //trenutna strana

            //search parameters
            var name = sa.name;
            var minPrice = sa.minPrice;
            var maxPrice = sa.maxPrice;
            var status = sa.status;
            var typeOfSearch = sa.typeOfSearch;

            //SVE AUCKIJE
            IQueryable<Auction> auctionsList = db.Auctions;
            //savcm.auctionList = db.Auctions.OrderBy(a => a.Id).ToPagedList(pageNumber, pageSize);

            //Filter minimal price
            if (minPrice != Decimal.Zero)
                auctionsList = auctionsList.Where(a => a.price >= minPrice);
            //Filter maximal price
            if (maxPrice != Decimal.Zero)
                auctionsList = auctionsList.Where(a => a.price <= maxPrice);
            //Filter status of auction
            if (!String.IsNullOrEmpty(status))
                auctionsList = auctionsList.Where(a => a.status.Equals(status));

            //EXACT ALL po default-u kada nema typeOfSearch
            if (String.IsNullOrEmpty(typeOfSearch) && !String.IsNullOrEmpty(name))
            {
                //EXACT ALL
                auctionsList = auctionsList.Where(a => a.name.Equals(name));
            }
            else if (!String.IsNullOrEmpty(name)) //NEKI OD OSTALIH VRSTA PRETRAGA
            {
                string[] keywords = name.Split(null); //pripremi kljucne reci
                var predicate = PredicateBuilder.False<Auction>();

                switch (typeOfSearch)
                {
                    case "EK": //Exact Keywords -> where Name like 'first' AND Name like 'second'...
                        foreach (string keyword in keywords)
                        {
                            string temp = keyword;
                            predicate = predicate.And(a => a.name.Equals(temp));
                        }
                        break;
                    case "PA": //Partial All -> where Name like 'Go He'
                        auctionsList = auctionsList.Where(a => a.name.Contains(name));
                        break;
                    case "PK": //Partial Keywords -> where Name like 'first' OR Name like 'second'...
                        foreach (string keyword in keywords)
                        {
                            string temp = keyword;
                            predicate = predicate.Or(a => a.name.Contains(temp));
                        }
                        break;
                }
                //isto za sva 3 slucaja
                auctionsList = auctionsList.Where(predicate);
            }
            savcm.auctionList = auctionsList.OrderByDescending(o => o.closing).ToPagedList(pageNumber, pageSize);
            return View(savcm);
        }

        //GET: Auctions/GetCentiliDetails
        public ActionResult GetCentiliDetails(string packageType, string clientId)
        {
            var user = db.Users.Where(u => u.Id.Equals(clientId)).FirstOrDefault();

            switch (packageType)
            {
                case "STANDARD":
                    user.Credit += 1;
                    break;
                case "GOLD":
                    user.Credit += 5;
                    break;
                case "PLATINUM":
                    user.Credit += 10;
                    break;
                default:
                    break;
            }

            db.SaveChanges();
            return RedirectToAction("Index", "Manage");
        }

        //GET: Auctions/AuctionsWon
        public ActionResult AuctionsWon()
        {
            var user = db.Users.Where(u => u.UserName.Equals(User.Identity.Name)).FirstOrDefault();
            var auctions = db.Auctions.Where(a => a.winner_Id.Equals(user.Id)).ToList();
            return View(auctions);
        }

        // GET: Auctions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auction auction = db.Auctions.Find(id);
            if (auction == null)
            {
                return HttpNotFound();
            }
            return View(auction);
        }

        // GET: Auctions/Create
        //TODO: NAKON pravljenja kompletno aukcija -> odkomentarisati dole
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auctions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,name,length,price,creation,opening,closing,details,img,status")] Auction auction)
        {
            if (ModelState.IsValid)
            {
                auction.winner_FullName = "";
                auction.start_offer = auction.price; //na pocetku je ista cena
                                                     /*auction.closing = auction.opening = */
                auction.creation = DateTime.Now; //moram odmah inicijalizovati opening i closing...
                db.Auctions.Add(auction);
                bool saveFailed;
                do
                {
                    saveFailed = false;
                    try { db.SaveChanges(); }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        saveFailed = true;
                        // Update original values from the database 
                        var entry = ex.Entries.Single();
                        entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    }
                } while (saveFailed);
                return RedirectToAction("Index");
            }

            return View(auction);
        }

        // GET: Auctions/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auction auction = db.Auctions.Find(id);
            if (auction.status.Equals("READY"))
            {
                // Ažuriranje i brisanje treba dozvoliti samo za aukcije koje su u stanju READY
                if (auction == null)
                {
                    return HttpNotFound();
                }
                return View(auction);
            }
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }

        // POST: Auctions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,winner_Id,name,length,price,details,img,status,start_offer")] Auction auction)
        {
            if (ModelState.IsValid && auction.status.Equals("READY"))
            {
                auction.creation = DateTime.Now;
                db.Entry(auction).State = EntityState.Modified;

                bool saveFailed;
                do
                {
                    saveFailed = false;
                    try { db.SaveChanges(); }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        saveFailed = true;
                        // Update original values from the database 
                        var entry = ex.Entries.Single();
                        entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    }
                } while (saveFailed);

                return RedirectToAction("Index");
            }
            return View(auction);
        }

        // GET: Auctions/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auction auction = db.Auctions.Find(id);
            if (auction == null)
            {
                return HttpNotFound();
            }
            return View(auction);
        }

        // POST: Auctions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Auction auction = db.Auctions.Find(id);
            auction.status = "DELETED"; //samo postavljanje fleg-a, ne radi se brisanje
            //db.Auctions.Remove(auction);
            bool saveFailed;
            do
            {
                saveFailed = false;
                try { db.SaveChanges(); }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;
                    // Update original values from the database 
                    var entry = ex.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                }
            } while (saveFailed);
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
