using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEP___projekat_AS.Models
{
    public class SearchAuctionViewContainerModel
    {
        public SearchAuctionViewModel searchAuctionViewModel { get; set; }
        public IPagedList<Auction> auctionList { get; set; } 

    }
}