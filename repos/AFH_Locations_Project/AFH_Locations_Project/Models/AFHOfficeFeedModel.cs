using System.Text.Json.Serialization;

namespace AFH_Locations_Project.Models
{
    public class AFHOfficeFeedModel
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("address1")]
        public required string Address1 { get; set; }

        [JsonPropertyName("address2")]
        public string? Address2 { get; set; }

        [JsonPropertyName("city")]
        public required string City { get; set; }

        [JsonPropertyName("postcode")]
        public required string PostCode { get; set; }
    }
}
