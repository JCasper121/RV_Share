using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace loanRV
{
    public class Listing
    {
        [Key]
        public int ListingId {get;set;}

        [Required]
        public int OwnerId {get; set;}

        [Required]
        public string Make {get;set;}

        [Required]
        public string Model {get;set;}

        [Required]
        public int Year {get;set;}

        [Required]
        public string Type {get;set;}

        [Required]
        public int Capacity {get;set;}

        [Required]
        public bool Drivable {get;set;}

        [Required]
        public int Rate {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public List<ListingAmenity> Amenities {get;set;}
        public List<Rental> RentHistory {get;set;}
        public User Owner {get;set;}
    }
}