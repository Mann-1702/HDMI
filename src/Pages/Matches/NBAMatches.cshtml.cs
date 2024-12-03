using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Matches
{
    public class NBAMatchesModel : PageModel
    {
        // The SportsApiClient is used to fetch game data from an external sports API.
        private readonly SportsApiClient _sportsApiClient;

        // Logger instance for logging errors or other information related to the page.
        private readonly ILogger<NBAMatchesModel> _logger;

        // Public property to store the list of NBA game data.
        public List<NbaGameResponse> Games { get; private set; }

        // Public property to store the season year, default is 2024.
        public int SeasonYear { get; private set; }

        /// <summary>
        /// Constructor to initialize the SportsApiClient and Logger for the page model.
        /// </summary>
        /// <param name="sportsApiClient">SportsApiClient to interact with the external sports API.</param>
        /// <param name="logger">ILogger to log any issues during data fetching.</param>
        public NBAMatchesModel(SportsApiClient sportsApiClient, ILogger<NBAMatchesModel> logger)
        {
            _sportsApiClient = sportsApiClient;
            _logger = logger;
        }

        /// <summary>
        /// Handles the GET request to fetch NBA game data for a specific season and team (optional).
        /// </summary>
        /// <param name="teamName">Optional parameter to filter games by a specific team name.</param>
        /// <param name="year">Year of the NBA season (defaults to 2024 if not provided).</param>
        /// <returns>The page result with the list of NBA games, filtered by team if applicable.</returns>
        public IActionResult OnGet(string teamName = null, int year = 2024)
        {
            // Set the season year based on the parameter or default to 2024.
            SeasonYear = year;

            // Define the NBA league ID, base URL, and endpoint for fetching game data.
            string NBAleagueId = "standard";
            string baseUrl = "https://v2.nba.api-sports.io"; // API base URL.
            string baseHost = "v2.aenba.api-sports.io";    // Base host for the API request.
            string endPoint = "games";                      // Endpoint for fetching game data.

            try
            {
                // Fetch NBA game data for the specified season.
                Games = _sportsApiClient.GetGamesForSeason<NbaGameResponse>(NBAleagueId, SeasonYear, baseUrl, baseHost, endPoint);

                // If no team name is specified, return the page with all games.
                if (teamName == null)
                {
                    return Page();
                }

                // Filter the games to include only those where the specified team is playing.
                Games = Games.Where(g => g.Teams.Home.Name == teamName || g.Teams.Visitors.Name == teamName).ToList();
            }
            catch (System.Exception ex)
            {
                // Log any errors that occur while fetching the game data.
                _logger.LogError(ex, "Error fetching game results.");

                // Initialize the Games list as an empty list in case of an error.
                Games = new List<NbaGameResponse>();
            }

            // Return the page result with the fetched game data.
            return Page();
        }
    }
}