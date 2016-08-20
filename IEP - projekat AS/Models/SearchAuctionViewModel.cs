using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEP___projekat_AS.Models
{
    public class SearchAuctionViewModel
    {
        public string name { get; set; }
        public string typeOfSearch { get; set; } //Exact All, Exact Keyword, Partial All, Partial Keywords
        public string status { get; set; } //Draft, Ready, Open, Sold, Expired
        public Decimal minPrice { get; set; }
        public Decimal maxPrice { get; set; }

    }

}