using System.Collections.Generic;

namespace loanRV
{
    public class ProfileViewModel
    {
        public User ProfileUser {get;set;}
        public User LoggedUser {get;set;}
        public List<Listing> Listings {get;set;}
    }
}