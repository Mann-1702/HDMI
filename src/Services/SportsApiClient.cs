namespace ContosoCrafts.WebSite.Services
{
    using ContosoCrafts.WebSite.Models;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
        public SportsApiClient(string baseUrl, string apiKey, string apiHost, ILogger<SportsApiClient> logger)
        {
            _defaultbaseUrl = baseUrl;
            _apiKey = apiKey.Trim();
            _defaultapiHost = apiHost.Trim();
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

            // Create the RestClient with the base URL
            var client = new RestClient(baseUrl);

            // Create the GET request to retrieve games

            
            var endpoint = leagueId == "39" ? "fixtures" : "games";
            var request = new RestRequest(endpoint, Method.Get);

            // Add query parameters for league ID, season year, and timezone
            request.AddQueryParameter("league", leagueId.ToString()); //use league for NBA
            request.AddQueryParameter("season", seasonYear.ToString());
            //request.AddQueryParameter("timezone", timezone);

            // Add required headers for API authentication
            request.AddHeader("x-rapidapi-key", _apiKey);
            request.AddHeader("x-rapidapi-host", apiHost);

            // Set timeout to infinite 
            request.Timeout = TimeSpan.FromMilliseconds(-1);

            try
            {

                // Execute the request and store the response
                var response = client.Execute(request);

                // Log response details for debugging can remove whenever not need 
                _logger.LogInformation("Status Code: {StatusCode}", response.StatusCode);
                _logger.LogInformation("Response Headers: {Headers}", JsonConvert.SerializeObject(response.Headers));
                _logger.LogInformation("Raw JSON Response Content: {Content}", response.Content);

                // Check if the response is successful and contains content
                if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
                {

                    // Deserialize the JSON response to ApiResponse<GameResponse>
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);

                    // Extract and limit to 100 results
                    var games = apiResponse?.Response;
                    return games?.Take(100).ToList() ?? new List<T>();
                }

                else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {

                    // Log a message if no content was returned ment for debugging purposes, can remove
                    _logger.LogInformation("No games data available.");
                    return new List<T>();
                }

                else
                {

                    // Log an error and throw an exception if the request failed also ment for debuggin purposes
                    _logger.LogError("Request failed with status code: {StatusCode}, Error Message: {ErrorMessage}", response.StatusCode, response.ErrorMessage);
                    throw new Exception($"Request failed with status code: {response.StatusCode}, Error Message: {response.ErrorMessage}");
                }

            }

            catch (Exception ex)
            {

                // Log any exceptions that occur during execution
                _logger.LogError(ex, "An error occurred while executing the request");
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