using Microsoft.EntityFrameworkCore;

namespace loanRV
{
    public class RVContext : DbContext
    {
        public RVContext(DbContextOptions options) : base(options) {}
        public DbSet<Amenity> Amenities {get;set;}
        public DbSet<Listing> Listings {get;set;}
        public DbSet<ListingAmenity> ListingAmenities {get;set;}
        public DbSet<Rental> Rentals {get;set;}
        public DbSet<User> Users {get;set;} 
    }
}