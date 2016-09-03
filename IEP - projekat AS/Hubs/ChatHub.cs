using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using IEP___projekat_AS.Models;
using System.Data.Entity.Infrastructure;
using Hangfire;

namespace IEP___projekat_AS.Hubs
{
    public class ChatHub : Hub
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Send(string name, string auctionID)
        {
            if (String.IsNullOrEmpty(name))
                return;
            int aID = Convert.ToInt32(auctionID);

            var user = db.Users.Where(m => m.Email == name).First();
            var auctionToBid = db.Auctions.Where(m => m.Id == aID).First();

            if (DateTime.Now > auctionToBid.closing
                || auctionToBid.status != "OPEN"
                || user.Credit == 0)
                return; //LOSE
            //Ukoliko je isti bider vidi jel ima dovoljno tokena
            if (!String.IsNullOrEmpty(auctionToBid.winner_Id)
                && user.Id.Equals(auctionToBid.winner_Id))
            {
                if (user.Credit + auctionToBid.price < auctionToBid.price + 1)
                    return; //nema dovoljno kredita
            }
            else if (user.Credit < auctionToBid.price + 1)
                 return;
            // 1. Dodaj ponudu
            // 2. Povecaj trenutnu cenu
            // 3. Vrati kredit prethodnom user-u
            // 4. Uzmi kredit user-u i postavi ga kao trenutnog winnera (id i ime)
            // 5. Dodaj novi offer u offers tabelu
            // 6. Proveri vreme ako je manje od 10 produzi!

            //1
            Offer offer = new Offer();
            offer.auction_Id = aID;
            offer.Auction = auctionToBid;
            offer.time = DateTime.Now;
            offer.user_Id = user.Id; offer.user_Fullname = user.Name + " " + user.Surname;
            offer.value = auctionToBid.price++; //2

            //3
            if (!String.IsNullOrEmpty(auctionToBid.winner_Id))
            {
                giveBackMoneyToUser(auctionToBid.winner_Id, auctionToBid.price);
            }
            //4
            int costOfOffer = auctionToBid.price + 1;
            user.Credit -= costOfOffer;
            auctionToBid.winner_Id = user.Id;
            auctionToBid.winner_FullName = user.Name + " " + user.Surname;

            //5
            auctionToBid.Offers.Add(offer);

            //6
            checkTime(auctionToBid);

            saveContextManual();

            logger.Info("Client BID. FullName:" + user.Name + " " + user.Surname + " auctionID: " + auctionID);
            /// Any connection or hub wire up and configuration should go here
            Clients.All.addNewMessageToPage(user.Name + " " + user.Surname, auctionID); //pozovi addNewMessage
        }

        private void giveBackMoneyToUser(string winner_Id, int ammount)
        {
            var user = db.Users.Where(u => u.Id.Contains(winner_Id)).FirstOrDefault();
            user.Credit += ammount;

            saveContextManual();
        }


        private void checkTime(Auction a)
        {
            var duration = TimeSpan.FromSeconds(a.length);
            TimeSpan time = duration - (DateTime.Now - (DateTime)a.opening);
            if (time.Seconds < 10 && time.Seconds > 0) //OPEN i GOTOVA
            {
                //OBAVESTI SVE
                var offers = a.Offers;
                var lastOffer = offers.LastOrDefault();
                if (lastOffer != null)
                {
                    TimeSpan tlo = (DateTime)a.closing - lastOffer.time;
                    if (tlo.Seconds < 10 && tlo.Seconds > 0)
                    {
                        //produzi
                        a.closing = a.closing.Value.AddSeconds(10); a.length += 10;
                        var tmp = a.Offers;
                        a.Offers = null;
                        BackgroundJob.Schedule(() => closeAuctionTask(a), new DateTimeOffset((DateTime)a.closing));
                        a.Offers = tmp;
                    }
                }
            }
            saveContextManual();
        }

        //jos jednom
        public void closeAuctionTask(Auction tmp)
        {
            var a = db.Auctions.Where(m => m.Id == tmp.Id).FirstOrDefault();
            if (a == null) return;
            if (DateTime.Now > a.closing && a.status != "EXPIRED")
            {
                var offers = a.Offers;
                var lastOffer = offers.LastOrDefault();
                if (lastOffer != null)
                {
                    TimeSpan tlo = (DateTime)a.closing - lastOffer.time;
                    if (tlo.Seconds < 10 && tlo.Seconds > 0)
                    {
                        //DONT CLOSE 
                        return;
                    }
                }

                //DODATI ONO SA 10 SEKUNDI!
                if (a.price > 1)
                    a.status = "SOLD";
                else a.status = "EXPIRED";
                //winnerID?
            }
            saveContextManual();
        }
        private void saveContextManual()
        {
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
        }
    }
}