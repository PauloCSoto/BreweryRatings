using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using BreweryNamespace;
using System.Net;

namespace BreweryRatings.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            using (var webClient = new WebClient())
            {
                string jsonString = webClient.DownloadString("https://api.openbrewerydb.org/breweries?by_city=cincinnati");
                var brewery = Brewery.FromJson(jsonString);
                ViewData["Breweries"] = brewery;
            }
        }
    }
}
