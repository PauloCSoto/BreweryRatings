using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Yelp;
using ReviewNamespace;
using System.Net.Http;

namespace BreweryRatings.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        //[BindProperty]
        //public string SearchCity { get; set; }

        public JsonResult OnGet(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                location = "Cincinnati";
            }

            string searchBusiness = "https://api.yelp.com/v3/businesses/search?categories=breweries&sort_by=rating&location=";

            //Get Yelp Business Search Json data for breweries in City
            Task<string> t = Http_Get(searchBusiness + location);
            var yelpRatingsString = t.Result;
            YelpRating yelpRating = YelpRating.FromJson(yelpRatingsString);

            //Define Business array to hold Yelp Business data with ratings
            Yelp.Business[] businesses = yelpRating.Businesses;

            //Convert array to List for better compatibility
            List<Yelp.Business> yelpBusinesses = businesses.OfType<Yelp.Business>().ToList();

            //Create new List to hold new Json output
            var businessesWithReviews = new List<Yelp.BusinessWithReview>();

            //Loop through businesses in Yelp Business Search results to find matching reviews
            foreach (Yelp.Business business in yelpBusinesses)
            {
                Task<string> t2 = Http_Get("https://api.yelp.com/v3/businesses/" + business.Id + "/reviews");
                var businessReviewJsonString = t2.Result;
                BusinessReview businessReview = BusinessReview.FromJson(businessReviewJsonString);

                businessesWithReviews.Add(new Yelp.BusinessWithReview
                {
                    Id = business.Id,
                    Alias = business.Alias,
                    Name = business.Name,
                    ImageUrl = business.ImageUrl,
                    IsClosed = business.IsClosed,
                    Url = business.Url,
                    ReviewCount = business.ReviewCount,
                    Categories = business.Categories,
                    Rating = business.Rating,
                    Coordinates = business.Coordinates,
                    Transactions = business.Transactions,
                    Price = business.Price,
                    Location = business.Location,
                    Phone = business.Phone,
                    DisplayPhone = business.DisplayPhone,
                    Distance = business.Distance,
                    Reviews = businessReview.Reviews,
                    Total = businessReview.Total,
                    PossibleLanguages = businessReview.PossibleLanguages
                });
            }
            //Uncomment this line and comment out the following one to output Json stream on the Index view
            //return new JsonResult(businessesWithReviews);
            //ViewData["BusinessesWithReviews"] = businessesWithReviews;
            //ViewData["TitleCity"] = SearchCity;
            return new JsonResult(businessesWithReviews);
        }
        static async Task<string> Http_Get(string uri)
        {
            String myYelpKey = System.IO.File.ReadAllText("YelpAPIKey.txt");
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", myYelpKey);
            HttpResponseMessage response = await client.GetAsync(uri);
            HttpContent content = response.Content;
            string result = await content.ReadAsStringAsync();
            return result;
        }
    }
}
