using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Reflection;

namespace ContosoCrafts.WebSite.Pages.Matches
{
    public class NFLReadModel : PageModel
    {
        // SportsApiClient used to fetch NFL game data from the external API.
        private readonly SportsApiClient _sportsApiClient;

        // Logger instance to log errors or important information related to the page.
        private readonly ILogger<NFLReadModel> _logger;

        /// <summary>
        /// Constructor to initialize the SportsApiClient and Logger for the page model.
        /// </summary>
        /// <param name="sportsApiClient">SportsApiClient to interact with the external sports API.</param>
        /// <param name="logger">ILogger to log any issues during data fetching.</param>
        public NFLReadModel(SportsApiClient sportsApiClient, ILogger<NFLReadModel> logger)
        {
            _sportsApiClient = sportsApiClient;
            _logger = logger;
        }

        // Public property to store the specific NFL game data for the match.
        public GameResponse Match { get; private set; }

        // Public property to indicate whether the game had overtime (True if there was overtime, otherwise False).
        public bool HasOvertime { get; private set; }

        /// <summary>
        /// Handles the GET request to fetch NFL game data for a specific game (by gameId) and season year.
        /// </summary>
        /// <param name="gameId">Unique identifier for the game.</param>
        /// <param name="year">Year of the NFL season (defaults to 2024 if not provided).</param>
        /// <returns>The page result with the specific game data, including overtime status.</returns>
        public IActionResult OnGet(string gameId, int year = 2024)
        {
            // Parse the gameId to an integer for processing
            int matchId = int.Parse(gameId);

            // Define the NFL league ID, base URL, and endpoint for fetching game data.
            string leagueId = "1"; // NFL league ID
            string baseUrl = "https://v1.american-football.api-sports.io"; // API base URL.
            string baseHost = "v1.american-football.api-sports.io";    // Base host for the API request.
            string endPoint = "games";                                  // Endpoint for fetching game data.

            try
            {
                // Fetch NFL game data for the specified season year.
                List<GameResponse> Games = _sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, year, baseUrl, baseHost, endPoint);

                // Find the specific match from the list of games.
                Match = Games.FirstOrDefault(m => m.Game.Id == matchId);
            }
            catch (System.Exception ex)
            {
                // Log any errors that occur while fetching the game data.
                _logger.LogError(ex, "Error fetching game results.");
            }

            // If no match is found, redirect to the NFL matches page.
            if (Match == null)
            {
                return RedirectToPage("/NFLMatches");
            }

            // Check if the match had overtime based on the score data.
            HasOvertime = Match.Scores.Home.Overtime > 0 || Match.Scores.Away.Overtime > 0;

            // Return the page with the match data and overtime status.
            return Page();
        }
    }
}