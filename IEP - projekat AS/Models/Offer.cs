namespace IEP___projekat_AS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Offer
    {
        //[Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //POTREBNO DA BI SQL MANAGEMENT STUDIO GENERISAO ID...
        public int Id { get; set; }

        //[Key]
        //[Column(Order = 1)]
        public string user_Id { get; set; }

        //[Key]
        //[Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int auction_Id { get; set; }

        public decimal value { get; set; }

        public DateTime time { get; set; }

        public virtual Auction Auction { get; set; }
    }
}
