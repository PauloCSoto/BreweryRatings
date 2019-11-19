namespace Yelp
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class YelpRating
    {
        private Business[] businesses;
        private long total;
        private Region region;

        [JsonProperty("businesses")]
        public Business[] Businesses { get => businesses; set => businesses = value; }

        [JsonProperty("total")]
        public long Total { get => total; set => total = value; }

        [JsonProperty("region")]
        public Region Region { get => region; set => region = value; }
    }

    public partial class Business
    {
        private string id;
        private string alias;
        private string name;
        private Uri imageUrl;
        private bool isClosed;
        private Uri url;
        private long reviewCount;
        private Category[] categories;
        private double rating;
        private Center coordinates;
        private string[] transactions;
        private string price;
        private Location location;
        private string phone;
        private string displayPhone;
        private double distance;

        [JsonProperty("id")]
        public string Id { get => id; set => id = value; }

        [JsonProperty("alias")]
        public string Alias { get => alias; set => alias = value; }

        [JsonProperty("name")]
        public string Name { get => name; set => name = value; }

        [JsonProperty("image_url")]
        public Uri ImageUrl { get => imageUrl; set => imageUrl = value; }

        [JsonProperty("is_closed")]
        public bool IsClosed { get => isClosed; set => isClosed = value; }

        [JsonProperty("url")]
        public Uri Url { get => url; set => url = value; }

        [JsonProperty("review_count")]
        public long ReviewCount { get => reviewCount; set => reviewCount = value; }

        [JsonProperty("categories")]
        public Category[] Categories { get => categories; set => categories = value; }

        [JsonProperty("rating")]
        public double Rating { get => rating; set => rating = value; }

        [JsonProperty("coordinates")]
        public Center Coordinates { get => coordinates; set => coordinates = value; }

        [JsonProperty("transactions")]
        public string[] Transactions { get => transactions; set => transactions = value; }

        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public string Price { get => price; set => price = value; }

        [JsonProperty("location")]
        public Location Location { get => location; set => location = value; }

        [JsonProperty("phone")]
        public string Phone { get => phone; set => phone = value; }

        [JsonProperty("display_phone")]
        public string DisplayPhone { get => displayPhone; set => displayPhone = value; }

        [JsonProperty("distance")]
        public double Distance { get => distance; set => distance = value; }
    }

    public partial class Category
    {
        private string alias;
        private string title;

        [JsonProperty("alias")]
        public string Alias { get => alias; set => alias = value; }

        [JsonProperty("title")]
        public string Title { get => title; set => title = value; }
    }

    public partial class Center
    {
        private double latitude;
        private double longitude;

        [JsonProperty("latitude")]
        public double Latitude { get => latitude; set => latitude = value; }

        [JsonProperty("longitude")]
        public double Longitude { get => longitude; set => longitude = value; }
    }

    public partial class Location
    {
        private string address1;
        private string address2;
        private string address3;
        private string city;
        private long zipCode;
        private string country;
        private string state;
        private string[] displayAddress;

        [JsonProperty("address1")]
        public string Address1 { get => address1; set => address1 = value; }

        [JsonProperty("address2")]
        public string Address2 { get => address2; set => address2 = value; }

        [JsonProperty("address3")]
        public string Address3 { get => address3; set => address3 = value; }

        [JsonProperty("city")]
        public string City { get => city; set => city = value; }

        [JsonProperty("zip_code")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long ZipCode { get => zipCode; set => zipCode = value; }

        [JsonProperty("country")]
        public string Country { get => country; set => country = value; }

        [JsonProperty("state")]
        public string State { get => state; set => state = value; }

        [JsonProperty("display_address")]
        public string[] DisplayAddress { get => displayAddress; set => displayAddress = value; }
    }

    public partial class Region
    {
        private Center center;

        [JsonProperty("center")]
        public Center Center { get => center; set => center = value; }
    }

    public partial class YelpRating
    {
        public static YelpRating FromJson(string json) => JsonConvert.DeserializeObject<YelpRating>(json, Yelp.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this YelpRating self) => JsonConvert.SerializeObject(self, Yelp.Converter.Settings);
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

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
