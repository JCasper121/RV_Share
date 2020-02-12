using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace loanRV
{
    public class Amenity
    {
        [Key]
        public int AmenityId {get;set;}

        [Required]
        public string AmenityName {get;set;}

        public List<ListingAmenity> RvsHave {get;set;}
    }
}