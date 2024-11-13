using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContosoCrafts.WebSite.Services
{
   
    /// <summary>
    /// Client for interacting with the sports API to retrieve game data.
    /// </summary>
    public class SportsApiClient
    {

        // The API key for authenticating requests
        private readonly string _apiKey;

        // The API host for authenticating requests
        private readonly string _defaultapiHost;

        // The base URL for the sports API
        private readonly string _defaultbaseUrl;

        // Logger instance for recording information and errors
        private readonly ILogger<SportsApiClient> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SportsApiClient"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL for the sports API.</param>
        /// <param name="apiKey">The API key for authentication.</param>
        /// <param name="apiHost">The API host for authentication.</param>
        /// <param name="logger">Logger instance for logging messages and errors.</param>
        public SportsApiClient( string apiKey, ILogger<SportsApiClient> logger)
        {
            
            _apiKey = apiKey.Trim();
            _logger = logger;
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
            // Initialize RestClient with the provided base URL
            var client = new RestClient(baseUrl);

            // Determine the endpoint based on the league ID (use "fixtures" for NBA or "games" for others)
            var endpoint = leagueId == "39" ? "fixtures" : "games";
            var request = new RestRequest(endpoint, Method.Get);

            // Add query parameters for the league ID and season year
            request.AddQueryParameter("league", leagueId.ToString());
            request.AddQueryParameter("season", seasonYear.ToString());

            // Add required headers for API authentication
            request.AddHeader("x-rapidapi-key", _apiKey);
            request.AddHeader("x-rapidapi-host", apiHost);

            // Set timeout to infinite to prevent timeout issues for long - running requests
            request.Timeout = TimeSpan.FromMilliseconds(-1);

            try
            {
                // Execute the request and capture the response
                var response = client.Execute(request);

                // Log the response details (status code, headers, and content)
                _logger.LogInformation("Status Code: {StatusCode}", response.StatusCode);
                _logger.LogInformation("Response Headers: {Headers}", JsonConvert.SerializeObject(response.Headers));
                _logger.LogInformation("Raw JSON Response Content: {Content}", response.Content);

                // Check if the response is successful and contains content
                if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
                    var games = apiResponse?.Response;
                    return games?.Take(100).ToList() ?? new List<T>();
                }

                // Log a message and return an empty list if the response status is NoContent
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    _logger.LogInformation("No games data available.");
                    return new List<T>();
                }

                // If the response was unsuccessful, log the error and throw an exception
                if (!response.IsSuccessful)
                {
                    _logger.LogError("Request failed with status code: {StatusCode}, Error Message: {ErrorMessage}",
                                     response.StatusCode, response.ErrorMessage);
                    throw new Exception($"Request failed with status code: {response.StatusCode}, Error Message: {response.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {  
                // Log any exception that occurs during the request execution and rethrow the exception
                _logger.LogError(ex, "An error occurred while executing the request");
                throw;
            }
            // Return an empty list as a fallback if no valid response was processed
            return new List<T>();
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