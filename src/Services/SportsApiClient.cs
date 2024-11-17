using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace ContosoCrafts.WebSite.Services
{
    /// <summary>
    /// Client for interacting with the sports API to retrieve game data.
    /// </summary>
    public class SportsApiClient
    {
        // The API key for authenticating requests
        private readonly string _apiKey;

        // Logger instance for recording information and errors
        private readonly ILogger<SportsApiClient> _logger;

        // In memory instance for storing data to memory for a given time period
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="SportsApiClient"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL for the sports API.</param>
        /// <param name="apiKey">The API key for authentication.</param>
        /// <param name="apiHost">The API host for authentication.</param>
        /// <param name="logger">Logger instance for logging messages and errors.</param>
        public SportsApiClient( string apiKey, ILogger<SportsApiClient> logger, IMemoryCache memoryCache)
        {
            
            _apiKey = apiKey.Trim();
            _logger = logger;
            _memoryCache = memoryCache;
        }
        public SportsApiClient(string apiKey, ILogger<SportsApiClient> logger)
        : this(apiKey, logger,null) // constructor overload to allow already established testing
        {
        }

        /// <summary>
        /// Retrieves game data for a specified league and season.
        /// </summary>
        /// <param name="leagueId">The ID of the league to retrieve games for.</param>
        /// <param name="seasonYear">The year of the season.</param>
        /// <param name="timezone">Timezone for date and time fields (default is "UTC").</param>
        /// <returns>A list of <see cref="GameResponse"/> objects for the specified league and season.</returns>
        public List<T> GetGamesForSeason<T>(string leagueId, int seasonYear, string baseUrl, string apiHost)
        {
            // Unique cache key for retrieving games
            string cacheKey = $"Games_{leagueId}_{seasonYear}";

            // Check if the data is already in the cache
            if (_memoryCache != null && _memoryCache.TryGetValue(cacheKey, out List<T> cachedGames))
            {
                _logger.LogInformation("Cache hit for {CacheKey}. Returning cached data.", cacheKey);
                return cachedGames;
            }
            _logger.LogInformation("Cache miss for {CacheKey}. Fetching data from API.", cacheKey);

            // Initialize RestClient with the provided base URL
            var client = new RestClient(baseUrl);
            var endpoint = leagueId == "39" ? "fixtures" : "games";
            var request = new RestRequest(endpoint, Method.Get);

            // Add query parameters
            request.AddQueryParameter("league", leagueId);
            request.AddQueryParameter("season", seasonYear.ToString());
            request.AddHeader("x-rapidapi-key", _apiKey);
            request.AddHeader("x-rapidapi-host", apiHost);

            try
            {
                // Execute the request
                var response = client.Execute(request);

                if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
                    var games = apiResponse?.Response ?? new List<T>();

                    // Cache the data
                    if (_memoryCache != null)
                    {
                        _logger.LogInformation("Caching data for {CacheKey}.", cacheKey);
                        _memoryCache.Set(cacheKey, games, TimeSpan.FromMinutes(25));
                    }

                    return games.Take(100).ToList();
                }

                // Handle unsuccessful response
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    _logger.LogInformation("No games data available.");
                    return new List<T>();
                }

                throw new Exception($"Request failed with status code: {response.StatusCode}, Error: {response.ErrorMessage}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the request.");
                throw;
            }
        }


    }

    /// <summary>
    /// Represents the response from the sports API.
    /// </summary>
    /// <typeparam name="T">Type of the data in the response.</typeparam>
    public class ApiResponse<T>
    {
        public string Get { get; set; }

        public Dictionary<string, string> Parameters { get; set; }

        public List<object> Errors { get; set; }

        public int Results { get; set; }
  
        public List<T> Response { get; set; }
    }

}