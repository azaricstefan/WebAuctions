namespace IEP___projekat_AS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Auction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Auction()
        {
            Offers = new HashSet<Offer>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string user_Id { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        public int length { get; set; }

        public decimal price { get; set; }

        public DateTime creation { get; set; }

        public DateTime opening { get; set; }

        public DateTime closing { get; set; }

        [Required]
        [StringLength(500)]
        public string details { get; set; }

        [Required]
        [StringLength(255)]
        public string img { get; set; }

        [Required]
        [StringLength(50)]
        public string status { get; set; }

        public int start_offer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Offer> Offers { get; set; }
    }
}
