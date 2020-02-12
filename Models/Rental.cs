using System;
using System.ComponentModel.DataAnnotations;

namespace loanRV
{
    public class Rental
    {
        [Key]
        public int RentalId {get;set;}
        [Required]
        public int RenterId {get;set;}
        [Required]
        public int ListingId {get;set;}
        [Required]
        public  DateTime PickUpDate {get;set;}
        [Required]
        public DateTime DropOffDate {get;set;}

        
        public User Owner {get;set;}
        public User Renter {get;set;}
    }
}