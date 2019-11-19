namespace ReviewNamespace
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class BusinessReview
    {
        private Review[] reviews;
        private long total;
        private string[] possibleLanguages;

        [JsonProperty("reviews")]
        public Review[] Reviews { get => reviews; set => reviews = value; }

        [JsonProperty("total")]
        public long Total { get => total; set => total = value; }

        [JsonProperty("possible_languages")]
        public string[] PossibleLanguages { get => possibleLanguages; set => possibleLanguages = value; }
    }

    public partial class Review
    {
        private string id;
        private Uri url;
        private string text;
        private long rating;
        private DateTimeOffset timeCreated;
        private User user;

        [JsonProperty("id")]
        public string Id { get => id; set => id = value; }

        [JsonProperty("url")]
        public Uri Url { get => url; set => url = value; }

        [JsonProperty("text")]
        public string Text { get => text; set => text = value; }

        [JsonProperty("rating")]
        public long Rating { get => rating; set => rating = value; }

        [JsonProperty("time_created")]
        public DateTimeOffset TimeCreated { get => timeCreated; set => timeCreated = value; }

        [JsonProperty("user")]
        public User User { get => user; set => user = value; }
    }

    public partial class User
    {
        private string id;
        private Uri profileUrl;
        private Uri imageUrl;
        private string name;

        [JsonProperty("id")]
        public string Id { get => id; set => id = value; }

        [JsonProperty("profile_url")]
        public Uri ProfileUrl { get => profileUrl; set => profileUrl = value; }

        [JsonProperty("image_url")]
        public Uri ImageUrl { get => imageUrl; set => imageUrl = value; }

        [JsonProperty("name")]
        public string Name { get => name; set => name = value; }
    }

    public partial class BusinessReview
    {
        public static BusinessReview FromJson(string json) => JsonConvert.DeserializeObject<BusinessReview>(json, ReviewNamespace.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this BusinessReview self) => JsonConvert.SerializeObject(self, ReviewNamespace.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
