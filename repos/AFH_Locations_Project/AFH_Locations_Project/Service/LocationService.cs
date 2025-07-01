using System.Text.Json;
using System.IO;
using AFH_Locations_Project.Models;
using Microsoft.Extensions.Caching.Memory;

namespace AFHOfficeFeedApi.Services
{
    public class LocationService
    {
        private readonly IMemoryCache _cache;
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://www.afhwm.co.uk/api/v1/locations";
        private const string FallbackJsonFileName = "locations-fallback.json";

        public LocationService(IMemoryCache cache, HttpClient httpClient)
        {
            _cache = cache;
            _httpClient = httpClient;
        }

        public async Task<List<AFHOfficeFeedModel>> GetLocationsAsync()
        {
            const string cacheKey = "office_locations";

            if (_cache.TryGetValue(cacheKey, out List<AFHOfficeFeedModel> cachedLocations))
            {
                return cachedLocations;
            }

            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<AFHOfficeFeedModel>>(ApiUrl);
                var locations = result?.Take(12).ToList() ?? new List<AFHOfficeFeedModel>();

                _cache.Set(cacheKey, locations, TimeSpan.FromMinutes(1));

                return locations;
            }
            catch (Exception)
            {
                var fallbackLocations = LoadFallbackLocations();

                _cache.Set(cacheKey, fallbackLocations, TimeSpan.FromMinutes(1));

                return fallbackLocations;
            }
        }

        private List<AFHOfficeFeedModel> LoadFallbackLocations()
        {
            try
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, FallbackJsonFileName);
                if (!File.Exists(filePath))
                {
                    return new List<AFHOfficeFeedModel>();
                }

                var json = File.ReadAllText(filePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var locations = JsonSerializer.Deserialize<List<AFHOfficeFeedModel>>(json,options);

                return locations?.Take(12).ToList() ?? new List<AFHOfficeFeedModel>();
            }
            catch
            {
                return new List<AFHOfficeFeedModel>();
            }
        }
    }
}
