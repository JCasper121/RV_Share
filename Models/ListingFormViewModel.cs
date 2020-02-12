using System.Collections.Generic;

namespace loanRV
{
    public class ListingFormViewModel
    {
        public int OwnerId { get; set; }
        public Listing Listing { get; set; }
        public List<Amenity> Amenities { get; set; }
        public List<ListingAmenity> listingamenities { get; set; }
    }
}