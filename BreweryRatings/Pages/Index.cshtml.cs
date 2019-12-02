﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ReviewNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Yelp;

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
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public string Location { get; set; }

        public void OnGet(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                location = Constant.DEFAULT_LOCATION;
            }

            string searchBusiness = "https://api.yelp.com/v3/businesses/search?categories=breweries&sort_by=rating&location=";

            
            Task<string> t = Http_Get(searchBusiness+location);
            var yelpRatingsString = t.Result;
            YelpRating yelpRating = YelpRating.FromJson(yelpRatingsString);

            
            var yelpBusinesses =yelpRating.Businesses.ToList();

            
            var businessesWithReviews = new List<BusinessWithReview>();

            
            foreach(Business business in yelpBusinesses)
            {
                Task<string> t2 = Http_Get("https://api.yelp.com/v3/businesses/" + business.Id + "/reviews");
                var businessReviewJsonString = t2.Result;
                BusinessReview businessReview = BusinessReview.FromJson(businessReviewJsonString);

                businessesWithReviews.Add(new BusinessWithReview
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
                }) ;
            }
            
            ViewData["BusinessesWithReviews"] = businessesWithReviews;
            ViewData["TitleCity"] = location;
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