using System;
using System.Linq;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ContosoCrafts.WebSite.Pages.Matches
{
    public class EPLMatchReadModel : PageModel
    {
        /// <summary>
        /// Client for interacting with the sports API to fetch match data.
        /// </summary>
        private readonly SportsApiClient _sportsApiClient;

        /// <summary>
        /// Logger for recording information or errors during the page's operations.
        /// </summary>
        private readonly ILogger<EPLMatchReadModel> _logger;

        /// <summary>
        /// Constructor to initialize EPLMatchReadModel with the required services.
        /// </summary>
        /// <param name="sportsApiClient">Service to fetch match data from the sports API.</param>
        /// <param name="logger">Logger for monitoring and debugging.</param>
        public EPLMatchReadModel(SportsApiClient sportsApiClient, ILogger<EPLMatchReadModel> logger)
        {
            _sportsApiClient = sportsApiClient;
            _logger = logger;
        }

        /// <summary>
        /// Holds the details of the specific match being viewed.
        /// </summary>
        public FixtureResponse Match { get; private set; }

        /// <summary>
        /// Handles the GET request to fetch details of a specific EPL match.
        /// </summary>
        /// <param name="matchId">The ID of the match to be retrieved.</param>
        /// <param name="year">The year of the season for which the match data is being fetched (default: 2024).</param>
        /// <returns>
        /// The page displaying match details if found, or redirects to the matches list page if the match is not found.
        /// </returns>
        public IActionResult OnGet(int matchId, int year = 2024)
        {
            try
            {
                // Constants for the API request
                string leagueId = "39"; // EPL league ID
                string baseUrl = "https://v3.football.api-sports.io"; // API base URL
                string apiHost = "v3.football.api-sports.io"; // API host
                string endPoint = "fixture"; // API endpoint for fetching a specific fixture

                // Fetch all games for the specified season
                var allGames = _sportsApiClient.GetGamesForSeason<FixtureResponse>(leagueId, year, baseUrl, apiHost, endPoint);

                // Find the specific match by its MatchId
                Match = allGames.FirstOrDefault(game => game.Fixture.FixtureId == matchId);

            }
            catch (Exception ex)
            {
                // Log any exception that occurs and redirect to the matches list page
                _logger.LogError(ex, $"Error fetching details for match with MatchId {matchId}");
                Match = null;
                return RedirectToPage("/EPLMatches");
            }

            // Render the page displaying match details
            return Page();
        }
    }
}