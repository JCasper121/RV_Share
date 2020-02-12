using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace loanRV
{
    public class ListingAmenity
    {
        [Key]
        public int ListingAmenityId {get;set;}

        [Required]
        public int ListingId {get;set;}

        [Required]
        public int AmenityId {get;set;}

        [NotMapped]
        public bool Bool {get;set;}

        [NotMapped]
        public string AmenityName {get;set;}

        public Listing Listing {get;set;}
        public Amenity Amenity {get;set;}
    }
}