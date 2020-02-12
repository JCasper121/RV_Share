using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace loanRV
{
    public class User
    {
        [Key]
        public int UserId {get;set;}

        [Required]
        public string Name {get;set;}

        [Required]
        [System.ComponentModel.Bindable(true)]
        [System.ComponentModel.SettingsBindable(true)]
        public bool Owner {get;set;}

        [Required]
        [EmailAddress]
        public string Email {get;set;}

        [Required]
        [DataType(DataType.PhoneNumber)]
        public int Phone {get;set;}

        [Required]
        public string City {get;set;}

        [Required]
        public string State {get;set;}

        [Required]
        public string Address {get;set;}

        [Required]
        [DataType(DataType.Password)]
        public string Password {get;set;}

        [Required]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public List<Listing> Listings {get;set;} 
    }
}