namespace Yelp
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class BusinessWithReview : Business
    {
        [JsonProperty("reviews")]
        public ReviewNamespace.Review[] Reviews { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("possible_languages")]
        public string[] PossibleLanguages { get; set; }
    }

    public partial class BusinessWithReview
    {
        public static BusinessWithReview FromJson(string json) => JsonConvert.DeserializeObject<BusinessWithReview>(json, Yelp.Converter.Settings);
    }

    public static class Serialize2
    {
        public static string ToJson(this BusinessWithReview self) => JsonConvert.SerializeObject(self, Yelp.Converter.Settings);
    }
}