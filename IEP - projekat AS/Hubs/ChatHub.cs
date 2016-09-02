using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using IEP___projekat_AS.Models;

namespace IEP___projekat_AS.Hubs
{
    public class ChatHub : Hub
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void Send(string name, string auctionID)
        {
            if (String.IsNullOrEmpty(name))
                return;
            int aID = Convert.ToInt32(auctionID);

            var user = db.Users.Where(m => m.Email == name).First();
            var auctionToBid = db.Auctions.Where(m => m.Id == aID).First();

            if (DateTime.Now > auctionToBid.closing
                || auctionToBid.status != "OPEN"
                || user.Credit == 0 || user.Credit < auctionToBid.price + 1)
                return; //LOSE

            // 1. Dodaj ponudu
            // 2. Povecaj trenutnu cenu
            // 3. Vrati kredit prethodnom user-u
            // 4. Uzmi kredit user-u i postavi ga kao trenutnog winnera
            // 5. Dodaj novi offer u offers tabelu

            //1
            Offer offer = new Offer();
            offer.auction_Id = aID;
            offer.Auction = auctionToBid;
            offer.time = DateTime.Now;
            offer.user_Id = user.Id;
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

            //5
            auctionToBid.Offers.Add(offer);

            db.SaveChanges();

            /// Any connection or hub wire up and configuration should go here
            Clients.All.addNewMessageToPage(name, auctionID); //pozovi addNewMessage
        }

        private void giveBackMoneyToUser(string winner_Id, int ammount)
        {
            var user = db.Users.Where(u => u.Id.Contains(winner_Id)).FirstOrDefault();
            user.Credit += ammount;
            db.SaveChanges();
        }
    }
}