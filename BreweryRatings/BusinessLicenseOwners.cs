using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BreweryRatings
{
    public class BusinessLicenseOwners
    {
        public long AccountNumber { get; set; }
        public string DoingBusinessAsName { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public string OwnerTitle { get; set; }
        public long LicenseNumber { get; set; }
        public string State { get; set; }
        public string BusinessActivity { get; set; }
        public string City { get; set; }
    }
}
