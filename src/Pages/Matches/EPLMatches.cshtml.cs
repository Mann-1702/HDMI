using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Matches
{
    public class EPLMatches : PageModel
    {
        /// <summary>
        /// Client for interacting with the sports API to fetch match data.
        /// </summary>
        private readonly SportsApiClient _sportsApiClient;

        /// <summary>
        /// Logger for recording information or errors during the page's operations.
        /// </summary>
        private readonly ILogger<EPLMatches> _logger;

        /// <summary>
        /// Constructor to initialize EPLMatches with the required services.
        /// </summary>
        /// <param name="sportsApiClient">Service to fetch game data from the sports API.</param>
        /// <param name="logger">Logger for monitoring and debugging.</param>
        public EPLMatches(SportsApiClient sportsApiClient, ILogger<EPLMatches> logger)
        {
            _sportsApiClient = sportsApiClient;
            _logger = logger;

            // Initialize the list of games
            Games = new List<FixtureResponse>();
        }

        /// <summary>
        /// Holds the list of games fetched from the API for the specified season.
        /// </summary>
        public List<FixtureResponse> Games { get; private set; }

        /// <summary>
        /// The season year for which match data is being fetched.
        /// </summary>
        public int seasonYear { get; private set; }

        /// <summary>
        /// Handles the GET request to fetch and display matches for the EPL (English Premier League).
        /// </summary>
        /// <param name="teamName">Optional filter to display matches for a specific team.</param>
        /// <param name="year">The year of the season to fetch matches for (default: 2024).</param>
        /// <returns>
        /// Returns the page with match data or an error page if there is an issue with the API call.
        /// </returns>
        public IActionResult OnGet(string teamName = null, int year = 2024)
        {
            seasonYear = year;

            // Constants for the API request
            string eplLeagueId = "39"; // EPL league ID
            string baseUrl = "https://v3.football.api-sports.io"; // API base URL
            string baseHost = "v3.football.api-sports.io"; // API host
            string endPoint = "fixtures"; // API endpoint for fetching fixtures

            try
            {
                // Check if the API client is initialized
                if (_sportsApiClient == null)
                {
                    _logger.LogError("SportsApiClient is null. Cannot fetch game data.");

                    // Redirect to the error page if the client is null
                    return RedirectToPage("/Error");
                }

                // Fetch the games for the specified season
                Games = _sportsApiClient.GetGamesForSeason<FixtureResponse>(eplLeagueId, seasonYear, baseUrl, baseHost, endPoint);

                // If no team filter is specified, return the page with all games
                if (teamName == null)
                {
                    return Page();
                }

                // Filter games by the specified team name
                Games = Games.Where(g => g.Teams.Home.Name == teamName || g.Teams.Visitors.Name == teamName).ToList();
            }
            catch (System.Exception ex)
            {
                // Log the error and initialize an empty list of games
                _logger.LogError(ex, "Error fetching EPL game data.");
                Games = new List<FixtureResponse>();
            }

            // Return the page with the filtered or unfiltered list of games
            return Page();
        }
    }
}