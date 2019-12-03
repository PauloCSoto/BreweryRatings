using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BusinessLicenseOwnerNamespace;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

/// <summary>
/// WebClient.DownloadString allows us to download the Json stream from the other team
/// The foreach allows us to select every business license owner
/// We used list to create the new list of business license owners
/// In creating our code, we used one of our code reviews to help formulate this code in an efficient manner
/// </summary>

namespace BreweryRatings.Pages
{
    public class BusinessLicensePageModel : PageModel
    {
        public void OnGet()
        {
            List<BusinessLicenseOwnerNamespace.BusinessLicenseOwner> businessLicenseOwners = new List<BusinessLicenseOwnerNamespace.BusinessLicenseOwner>();

            using (WebClient webClient = new WebClient())
            {
                string licenseJsonString = webClient.DownloadString("https://licenseowners2019.azurewebsites.net/Privacy");
                BusinessLicenseOwner[] businessLicenses = BusinessLicenseOwner.FromJson(licenseJsonString);

                foreach (BusinessLicenseOwnerNamespace.BusinessLicenseOwner businessLicense in businessLicenses)
                {
                    businessLicenseOwners.Add(new BusinessLicenseOwner
                    {
                        AccountNumber = businessLicense.AccountNumber,
                        DoingBusinessAsName = businessLicense.DoingBusinessAsName,
                        OwnerFirstName = businessLicense.OwnerFirstName,
                        OwnerLastName = businessLicense.OwnerLastName,
                        LicenseNumber = businessLicense.LicenseNumber
                    });
                }

                ViewData["BusinessLicenseOwners"] = businessLicenseOwners;
            }

        }
    }
}