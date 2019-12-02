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
using BusinessLicenseOwnerNamespace;
using System.Net;

/// <summary>
/// Http Get search business: Get Yelp Business Search Json data for breweries in City
/// [] business: Define Business array to hold Yelp Business data with ratings
/// List business: Convert array to List for better compatibility
/// var business reviews: Create new List to hold new Json output
/// first foreach: Loop through businesses in Yelp Business Search results to find matching reviews
/// businesslicense search is bringing in the json stream from another group and we do similar tasks similar to the business 
/// </summary>
namespace BreweryRatings.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }


        public JsonResult OnGet(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                location = "Cincinnati";
            }

            string searchBusiness = "https://api.yelp.com/v3/businesses/search?categories=breweries&sort_by=rating&location=";

            
            Task<string> yelpSearch = Http_Get(searchBusiness + location);
            var yelpRatingsString = yelpSearch.Result;
            YelpRating yelpRating = YelpRating.FromJson(yelpRatingsString);
            
            Yelp.Business[] businesses = yelpRating.Businesses;

            List<Yelp.Business> yelpBusinesses = businesses.OfType<Yelp.Business>().ToList();

            var businessesWithReviews = new List<Yelp.BusinessWithReview>();

            foreach (Yelp.Business business in yelpBusinesses)
            {
                Task<string> businessSearch = Http_Get("https://api.yelp.com/v3/businesses/" + business.Id + "/reviews");
                var businessReviewJsonString = businessSearch.Result;
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
