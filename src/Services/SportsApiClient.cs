using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;

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

        // In-memory cache instance for storing data temporarily
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="SportsApiClient"/> class.
        /// </summary>
        /// <param name="apiKey">The API key for authentication.</param>
        /// <param name="logger">Logger instance for logging messages and errors.</param>
        /// <param name="memoryCache">In-memory cache instance for caching API responses.</param>
        public SportsApiClient(string apiKey, ILogger<SportsApiClient> logger, IMemoryCache memoryCache)
        {
            _apiKey = apiKey.Trim(); // Trims any leading or trailing whitespace from the API key
            _logger = logger; // Assigns the logger instance
            _memoryCache = memoryCache; // Assigns the memory cache instance
        }

        /// <summary>
        /// Overloaded constructor for testing purposes, uses a null cache.
        /// </summary>
        /// <param name="apiKey">The API key for authentication.</param>
        /// <param name="logger">Logger instance for logging messages and errors.</param>
        public SportsApiClient(string apiKey, ILogger<SportsApiClient> logger)
            : this(apiKey, logger, null) // Constructor overload to allow for established testing
        {
        }

        /// <summary>
        /// Retrieves game data for a specified league and season.
        /// Uses caching to store results and avoid repeated API calls.
        /// </summary>
        /// <param name="leagueId">The ID of the league to retrieve games for.</param>
        /// <param name="seasonYear">The year of the season.</param>
        /// <param name="baseUrl">The base URL for the sports API.</param>
        /// <param name="apiHost">The API host for authentication.</param>
        /// <param name="endpoint">The API endpoint for the specific request (e.g., "fixtures" or "games").</param>
        /// <returns>A list of game data of type <see cref="T"/> for the specified league and season.</returns>
        public List<T> GetGamesForSeason<T>(string leagueId, int seasonYear, string baseUrl, string apiHost, string endpoint)
        {
            // Generate a unique cache key based on the league and season
            string cacheKey = $"Games_{leagueId}_{seasonYear}";

            // Check if the data is already cached
            if (_memoryCache != null)
            {
                if (_memoryCache.TryGetValue(cacheKey, out List<T> cachedGames))
                {
                    _logger.LogInformation("Cache hit for {CacheKey}. Returning cached data.", cacheKey);
                    return cachedGames; // Return cached data if found
                }
            }

            _logger.LogInformation("Cache miss for {CacheKey}. Fetching data from API.", cacheKey);

            // Initialize RestClient with the provided base URL
            var client = new RestClient(baseUrl);
            var request = new RestRequest(endpoint, Method.Get); // Create a GET request to the endpoint

            // Add query parameters for league and season, and API authentication headers
            request.AddQueryParameter("league", leagueId);
            request.AddQueryParameter("season", seasonYear.ToString());
            request.AddHeader("x-rapidapi-key", _apiKey);
            request.AddHeader("x-rapidapi-host", apiHost);

            try
            {
                // Execute the request to the API
                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        _logger.LogInformation("Response Content: {ResponseContent}", response.Content);
                        // Deserialize the response content into a strongly-typed API response
                        var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);

                        // Extract game data
                        var games = apiResponse.Response;

                        // Limit the number of games returned to 100
                        var limitedGames = games.Take(100).ToList();

                        // Cache the data for subsequent requests
                        if (_memoryCache != null)
                        {
                            _logger.LogInformation("Caching data for {CacheKey}.", cacheKey);

                            // Cache for 25 minutes
                            _memoryCache.Set(cacheKey, limitedGames, TimeSpan.FromMinutes(25));
                        }

                        // Return the games data
                        return limitedGames;
                    }
                }

                _logger.LogWarning("Unsuccessful response or empty content. Returning an empty list.");
                throw new Exception("Unsuccessful response or empty content. Unable to retrieve game data.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the request.");
                return null;
            }
        }
    }

    /// <summary>
    /// Represents the response from the sports API.
    /// </summary>
    /// <typeparam name="T">Type of the data in the response.</typeparam>
    public class ApiResponse<T>
    {
        // The query string parameters used in the API request
        public string Get { get; set; }

        // A dictionary of query parameters passed to the API
        public Dictionary<string, string> Parameters { get; set; }

        // A list of errors returned by the API, if any
        public List<object> Errors { get; set; }

        // The total number of results returned by the API
        public int Results { get; set; }

        // The actual data response from the API (e.g., list of games)
        public List<T> Response { get; set; }
    }
}