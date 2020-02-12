using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using loanRV.Models;
using Microsoft.EntityFrameworkCore;

namespace loanRV.Controllers
{
    public class HomeController : Controller
    {
        private RVContext dbContext;


        [HttpGet("")]
        public IActionResult Home()
        {
            List<Listing> listings = dbContext.Listings.ToList();
            HomeViewModel model = new HomeViewModel()
            {
                Listings = listings
            };
            return View(model);
        }

        [HttpGet("profile/{profileid}")]
        public IActionResult Profile(int profileid)
        {
            User profile = dbContext.Users.FirstOrDefault(u => u.UserId == profileid);
            List<Listing> listings = dbContext.Listings.Where(l => l.OwnerId == profileid).ToList();
            int loggeduserid = (int)HttpContext.Session.GetInt32("UserId");
            User loggeduser = dbContext.Users.FirstOrDefault(u => u.UserId == loggeduserid);
            ProfileViewModel model = new ProfileViewModel()
            {
                ProfileUser = profile,
                LoggedUser = loggeduser,
                Listings = listings
            };

            if (profile.Owner)
            {
                return View("OwnerProfile", model);
            }
            else
            {
                return View("RenterProfile", model);
            }
        }

        [HttpGet]
        public IActionResult AccountProfile()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("LogRegPage", "LogReg");
            }
            else
            {
                int loggedid = (int)HttpContext.Session.GetInt32("UserId");
                return Redirect($"/profile/{loggedid}");
            }
        }

        [HttpGet("/listing/{listid}")]
        public IActionResult ListingPage(int listid)
        {
            Listing listing = dbContext.Listings.Where(l => l.ListingId == listid)
                .Include(l => l.Amenities)
                .ThenInclude(a => a.Amenity)
                .FirstOrDefault();
            User owner = dbContext.Users.FirstOrDefault(u => u.UserId == listing.OwnerId);

            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                int loggedid = (int)HttpContext.Session.GetInt32("UserId");
                User loggeduser = dbContext.Users.FirstOrDefault(u => u.UserId == loggedid);

                ListingViewModel model = new ListingViewModel()
                {
                    LoggedUser = loggeduser,
                    Owner = owner,
                    Listing = listing
                };

                return View("ListingPage", model);
            }

            ListingViewModel model2 = new ListingViewModel()
            {
                Owner = owner,
                Listing = listing
            };
            return View("ListingPage", model2);

            // Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(listing));
        }

        [HttpGet("/newlisting")]
        public IActionResult ListingForm()
        {
            int ownerid = (int)HttpContext.Session.GetInt32("UserId");
            List<Amenity> amenities = dbContext.Amenities.ToList();
            List<ListingAmenity> listingamenities = new List<ListingAmenity>();
            amenities.ForEach(a => listingamenities.Add(new ListingAmenity()
            {
                Amenity = a,
                AmenityId = a.AmenityId,
                Bool = false,
                AmenityName = a.AmenityName
            }));
            ListingFormViewModel model = new ListingFormViewModel()
            {
                OwnerId = ownerid,
                Listing = new Listing(),
                Amenities = amenities,
                listingamenities = listingamenities
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult AddListing(ListingFormViewModel model)
        {
            Listing listing = model.Listing;
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(model));
            dbContext.Listings.Add(listing);
            List<ListingAmenity> listingamens = model.listingamenities;
            foreach (ListingAmenity l in listingamens)
            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(l));
                if (l.Bool)
                {

                    ListingAmenity newamen = new ListingAmenity()
                    {
                        ListingId = listing.ListingId,

                        AmenityId = l.AmenityId,
                    };
                    // Console.WriteLine("***********************************************");
                    // Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(newamen));
                    // Console.WriteLine("***********************************************");


                    dbContext.ListingAmenities.Add(newamen);
                }
            }
            int listid = listing.ListingId;
            Console.WriteLine(listid);
            dbContext.SaveChanges();
            Listing list = dbContext.Listings.Last();
            int id = list.ListingId;
            return Redirect($"/listing/{id}");
        }

        [HttpGet("newamenity")]
        public IActionResult AmenityForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddAmenity(Amenity amenity)
        {
            dbContext.Amenities.Add(amenity);
            dbContext.SaveChanges();
            return RedirectToAction("ListingForm");
        }

        [HttpGet("edit/{listid}")]
        public IActionResult Edit(int listid)
        {
            int loggedid = (int)HttpContext.Session.GetInt32("UserId");
            Listing toedit = dbContext.Listings.Include(l => l.Amenities).FirstOrDefault(l => l.ListingId == listid);
            List<ListingAmenity> has = dbContext.ListingAmenities.Where(a => a.ListingId == toedit.ListingId).ToList();
            List<ListingAmenity> listingamenities = new List<ListingAmenity>();
            List<Amenity> amenities = dbContext.Amenities.ToList();
            if (toedit.OwnerId != loggedid)
            {
                return RedirectToAction("LogRegPage", "LogReg");
            }
            for (int i = 0; i < amenities.Count; i++)
            {
                listingamenities.Add(new ListingAmenity()
                {
                    Amenity = amenities[i],
                    AmenityId = amenities[i].AmenityId,
                    Bool = false,
                    AmenityName = amenities[i].AmenityName
                });
            }

            for (int k = 0; k < has.Count; k++)
            {
                for (int i = 0; i < listingamenities.Count; i++)
                {
                    if ( has[k].AmenityId == listingamenities[i].AmenityId)
                    {
                        listingamenities[i].Bool = true;
                    }
                }
            }

            ListingFormViewModel editlist = new ListingFormViewModel
            {
                OwnerId = toedit.OwnerId,
                Listing = toedit,
                Amenities = amenities,
                listingamenities = listingamenities
            };
            return View(editlist);
        }

        [HttpPost]
        public IActionResult Update(ListingFormViewModel model)
        {
            Listing listing = model.Listing;
            Listing updlist = dbContext.Listings.FirstOrDefault(l => l.ListingId == listing.ListingId);

            //Updating fields to new values
            updlist.Make = listing.Make;
            updlist.Model = listing.Model;
            updlist.Year = listing.Year;
            updlist.Type = listing.Type;
            updlist.Capacity = listing.Capacity;
            updlist.Drivable = listing.Drivable;
            updlist.Rate = listing.Rate;

            Console.WriteLine($"ListingId: {updlist.ListingId}, Year:{updlist.Year}, OwnerId:{updlist.OwnerId}");
            
            dbContext.Listings.Update(updlist);


            List<ListingAmenity> listamens = dbContext.ListingAmenities.Where(l => l.ListingId == listing.ListingId).ToList();

            //Removing all previous Listing Amenities
            foreach(ListingAmenity l in listamens)
            {
                dbContext.ListingAmenities.Remove(l);
            }
            
            //Creating new set of Listing Amenities
            foreach (ListingAmenity l in model.listingamenities)
            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(l));
                if (l.Bool)
                {

                    ListingAmenity newamen = new ListingAmenity()
                    {
                        ListingId = listing.ListingId,

                        AmenityId = l.AmenityId,
                    };
                    dbContext.ListingAmenities.Add(newamen);
                }

            }
            
            dbContext.SaveChanges();
            int id = model.Listing.ListingId;
            return Redirect($"/listing/{id}");
        }

        [HttpGet("delete/{listid}")]
        public IActionResult Delete(int listid)
        {
            Listing toremove = dbContext.Listings.FirstOrDefault(l => l.ListingId == listid);
            dbContext.Remove(toremove);
            dbContext.SaveChanges();
            return RedirectToAction("AccountProfile");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public HomeController(RVContext context) => dbContext = context;
    }
}
