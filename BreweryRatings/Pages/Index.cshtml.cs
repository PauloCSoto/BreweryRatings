using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using BreweryNamespace;
using System.Net;
using System.Net.Http;
using System.IO;
using Yelp;
using BusinessNamespace;

namespace BreweryRatings.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

//        public void OnGet()
        public JsonResult OnGet()

        {
            IDictionary<string, Business> allBusinesses = new Dictionary<string, Business>();

            Task<string> t = Http_Get();

            var yelpRatingsString = t.Result;

            YelpRating yelpRating = YelpRating.FromJson(yelpRatingsString);

            ViewData["YelpRatings"] = yelpRating;

            Business[] businesses = yelpRating.Businesses;

            foreach(Business business in businesses)
            {
                allBusinesses.Add(business.Name, business);
            }

            using (var webClient = new WebClient())
            {
                List<Brewery> breweriesWithRatings = new List<Brewery>();
                List<BusinessWithRating> businessesWithRatings = new List<BusinessWithRating>();
                string jsonString = webClient.DownloadString("https://api.openbrewerydb.org/breweries?by_city=cincinnati");
                var brewery = Brewery.FromJson(jsonString);

                foreach(Brewery brewbar in brewery)
                {
                    if (allBusinesses.ContainsKey(brewbar.Name))
                    {
                        var addBiz = new BusinessWithRating();
                        addBiz.Id = brewbar.Id;
                        addBiz.Name = brewbar.Name;
//                        addBiz.BreweryType = brewbar.BreweryType;
                        addBiz.Street = brewbar.Street;
                        addBiz.City = brewbar.City;
                        addBiz.Latitude = brewbar.Latitude;
                        addBiz.Longitude = brewbar.Longitude;
                        //                        addBiz.Rating = allBusinesses.Ratings

                        foreach (Business business in businesses)
                        {
                            if (business.Name == brewbar.Name)
                            {
                                addBiz.Rating = business.Rating;
                                break;
                            }
                        }

                        businessesWithRatings.Add(addBiz);

//                        breweriesWithRatings.Add(addBiz);
                    }
                }

                return new JsonResult(businessesWithRatings);

//                return new JsonResult(breweriesWithRatings);

//                ViewData["Breweries"] = breweriesWithRatings;
            }
        }

        static async Task<string> Http_Get()
        {
            var targetURL = "https://api.yelp.com/v3/businesses/search?location=cincinnati&categories=breweries";

            String myYelpKey = System.IO.File.ReadAllText("YelpAPIKey.txt");

            HttpClientHandler handler = new HttpClientHandler();

            HttpClient client = new HttpClient(handler);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", myYelpKey);

            HttpResponseMessage response = await client.GetAsync(targetURL);

            HttpContent content = response.Content;

            string result = await content.ReadAsStringAsync();

            return result;
        }
    }
}
