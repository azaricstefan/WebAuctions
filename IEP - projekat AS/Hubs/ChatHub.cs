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
            int aID = Convert.ToInt32(auctionID);

            var user = db.Users.Where(m => m.Email == name).First();
            var auctionToBid = db.Auctions.Where(m => m.Id == aID).First();

            if (auctionToBid.status != "OPEN")
                return; //LOSE
            Offer offer = new Offer();
            offer.auction_Id = aID;
            offer.Auction = auctionToBid;
            offer.time = DateTime.Now;
            offer.user_Id = user.Id;
            offer.value = auctionToBid.price++;

            auctionToBid.Offers.Add(offer);
            db.SaveChanges();

            /// Any connection or hub wire up and configuration should go here
            Clients.All.addNewMessageToPage(name, auctionID); //pozovi addNewMessage
        }
    }
}