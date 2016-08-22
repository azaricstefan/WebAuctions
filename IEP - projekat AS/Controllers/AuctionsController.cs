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

namespace IEP___projekat_AS.Controllers
{
    public class AuctionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult OpenAuction(int ?id)
        {
            var auction = db.Auctions.Find(id);
            auction.opening = DateTime.Now; //update opening

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
            var name         = sa.name;
            var minPrice     = sa.minPrice;
            var maxPrice     = sa.maxPrice;
            var status       = sa.status;
            var typeOfSearch = sa.typeOfSearch;

            var query = db.Auctions.Where(s => s.name.Contains(name)); //search for auctions with that name
            var aList     = query.ToList();//.FirstOrDefault<Auction>();


            if (aList == null)
            {
                return HttpNotFound();
            }

            var savcm = new SearchAuctionViewContainerModel();
            savcm.searchAuctionViewModel = new SearchAuctionViewModel();
            savcm.auctionList = aList.ToPagedList(1,3);
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
            savcm.auctionList = db.Auctions.OrderBy(a => a.Id).ToPagedList(pageNumber, pageSize);

            if (String.IsNullOrEmpty(name) && String.IsNullOrEmpty(status) && String.IsNullOrEmpty(typeOfSearch)
                && minPrice == Decimal.Zero && maxPrice == Decimal.Zero)
            {
                //NEMA PARAMETARA POSALJI SVE POSTOJECE AUKCIJE
                //postavi auctionList
                //savcm.auctionList = db.Auctions.OrderBy(a => a.Id).ToPagedList(pageNumber, pageSize);
                String a = "ndas";
            }
            else
            {
                //EXACT ALL po default-u kada nema typeOfSearch
                if (String.IsNullOrEmpty(typeOfSearch))
                {
                    //EXACT ALL
                    savcm.auctionList = savcm.auctionList.Where(a => a.name.Equals(name))
                        .OrderBy(a => a.Id)
                        .ToPagedList(pageNumber, pageSize);
                }
                else
                {
                    string[] keywords = name.Split(null); //pripremi kljucne reci
                    var predicate = PredicateBuilder.False<Auction>();

                    switch (typeOfSearch)
                    {
                        case "EK": //Exact Keywords
                            foreach (string keyword in keywords)
                            {
                                string temp = keyword;
                                predicate = predicate.Or(a => a.name.Equals(temp));
                            }
                            break;
                        case "PA": //Partial All
                            foreach (string keyword in keywords)
                            {
                                string temp = keyword;
                                predicate = predicate.Or(a => a.name.Contains(temp));
                            }
                            break;
                        case "PK": //Partial Keywords
                            foreach (string keyword in keywords)
                            {
                                string temp = keyword;
                                predicate = predicate.Or(a => a.name.Contains(temp));
                            }
                            break;
                    }
                    //isto za sva 3 slucaja
                    savcm.auctionList = db.Auctions.Where(predicate)
                        .OrderBy(a => a.Id)
                        .ToPagedList(pageNumber, pageSize);
                }
                //*********NAME SEARCH*********************************
                //string[] keywords = name.Split(null);
                //var predicate = PredicateBuilder.False<Auction>();

                //foreach (string keyword in keywords)
                //{
                //    string temp = keyword;
                //    predicate = predicate.Or(a => a.name.Contains(temp));
                //}
                //savcm.auctionList = db.Auctions.Where(predicate)
                //    .OrderBy(a => a.Id)
                //    .ToPagedList(pageNumber, pageSize);
                //*******************************PREDICATE TEST
                //Filter minimal price
                if (minPrice != Decimal.Zero)
                    savcm.auctionList = savcm.auctionList.Where(a => a.price >= minPrice)
                        .OrderBy(a => a.Id)
                        .ToPagedList(pageNumber, pageSize);
                //Filter maximal price
                if (maxPrice != Decimal.Zero)
                    savcm.auctionList = savcm.auctionList.Where(a => a.price <= maxPrice)
                        .OrderBy(a => a.Id)
                        .ToPagedList(pageNumber, pageSize);
                //Filter status of auction
                if (!String.IsNullOrEmpty(status))
                    savcm.auctionList = savcm.auctionList.Where(a => a.status.Equals(status))
                        .OrderBy(a => a.Id)
                        .ToPagedList(pageNumber, pageSize);



            }

            //savcm.auctionList = db.Auctions.
            //    Where(a => a.name.Contains("Aukcija") && a.price < 1000).
            //    OrderBy(a => a.Id).
            //    ToPagedList(pageNumber,pageSize);

            //*********************************** SETTING UP*******************
            //ViewBag.CurrentSort = sortOrder;

            return View(savcm);
            //return View(db.Auctions.ToList()); //OLD
        }

        public ActionResult GetCentiliDetails()
        {
            //TODO!
            return View();
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
                auction.start_offer = auction.price; //na pocetku je ista cena
                auction.closing = auction.opening = auction.creation = DateTime.Now; //moram odmah inicijalizovati opening i closing...
                db.Auctions.Add(auction);
                db.SaveChanges();
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
        public ActionResult Edit([Bind(Include = "Id,winner_Id,name,length,price,creation,opening,closing,details,img,status,start_offer")] Auction auction)
        {
            if (ModelState.IsValid && auction.status.Equals("READY"))
            {
                db.Entry(auction).State = EntityState.Modified;
                db.SaveChanges();
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
            db.SaveChanges();
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
