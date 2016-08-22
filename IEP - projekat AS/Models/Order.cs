namespace IEP___projekat_AS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string user_Id { get; set; }

        public int number_of_tokens { get; set; }

        public decimal price { get; set; }

        [Required]
        [StringLength(50)]
        public string status { get; set; }

        public DateTime date { get; set; }

        /// <summary>
        /// STANDARD,GOLD,PLATINUM
        /// </summary>
        [StringLength(50)]
        public string package { get; set; }
    }
}
