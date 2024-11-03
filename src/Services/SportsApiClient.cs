namespace ContosoCrafts.WebSite.Services
{
    using ContosoCrafts.WebSite.Models;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SportsApiClient
    {
        private readonly string _apiKey;
        private readonly string _apiHost;
        private readonly string _baseUrl;
        private readonly ILogger<SportsApiClient> _logger;

        public SportsApiClient(string baseUrl, string apiKey, string apiHost, ILogger<SportsApiClient> logger)
        {
            _baseUrl = baseUrl;
            _apiKey = apiKey.Trim();
            _apiHost = apiHost.Trim();
            _logger = logger;
        }

        public List<GameResponse> GetGamesForSeason(int leagueId, int seasonYear, string timezone = "UTC")
        {
            var client = new RestClient(_baseUrl);
            var request = new RestRequest("games", Method.Get);
            request.AddQueryParameter("league", leagueId.ToString());
            request.AddQueryParameter("season", seasonYear.ToString());
            request.AddQueryParameter("timezone", timezone);
            request.AddHeader("x-rapidapi-key", _apiKey);
            request.AddHeader("x-rapidapi-host", _apiHost);
            request.Timeout = TimeSpan.FromMilliseconds(-1);

            try
            {
                var response = client.Execute(request);

                _logger.LogInformation("Status Code: {StatusCode}", response.StatusCode);
                _logger.LogInformation("Response Headers: {Headers}", JsonConvert.SerializeObject(response.Headers));
                _logger.LogInformation("Raw JSON Response Content: {Content}", response.Content);

                if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<GameResponse>>(response.Content);
                    var games = apiResponse?.Response;

                    // Limit to 50 results
                    return games?.Take(50).ToList() ?? new List<GameResponse>();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    _logger.LogInformation("No games data available.");
                    return new List<GameResponse>();
                }
                else
                {
                    _logger.LogError("Request failed with status code: {StatusCode}, Error Message: {ErrorMessage}", response.StatusCode, response.ErrorMessage);
                    throw new Exception($"Request failed with status code: {response.StatusCode}, Error Message: {response.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the request");
                throw;
            }
        }



    }

    public class ApiResponse<T>
    {
        public string Get { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public List<object> Errors { get; set; }
        public int Results { get; set; }
        public List<T> Response { get; set; }
    }
}
