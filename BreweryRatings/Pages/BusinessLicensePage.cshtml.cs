using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BusinessLicenseOwnerNamespace;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

                foreach (BusinessLicenseOwnerNamespace.BusinessLicenseOwner businessLicense in businessLicenseOwners)
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