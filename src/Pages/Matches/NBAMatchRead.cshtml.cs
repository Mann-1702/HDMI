using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.VisualBasic;

namespace ContosoCrafts.WebSite.Pages.Matches
{
    public class NBAReadModel : PageModel
    {
        // SportsApiClient used to fetch game data from an external API.
        private readonly SportsApiClient _sportsApiClient;

        // Logger instance to log errors or important information related to the page.
        private readonly ILogger<NBAReadModel> _logger;

        /// <summary>
        /// Constructor to initialize the SportsApiClient and Logger for the page model.
        /// </summary>
        /// <param name="sportsApiClient">SportsApiClient to interact with the external sports API.</param>
        /// <param name="logger">ILogger to log any issues during data fetching.</param>
        public NBAReadModel(SportsApiClient sportsApiClient, ILogger<NBAReadModel> logger)
        {
            _sportsApiClient = sportsApiClient;
            _logger = logger;
        }

        // Public property to store the specific NBA match details.
        public NbaGameResponse Match { get; private set; }

        // Public property to indicate if the match went into overtime (true if overtime).
        public bool HasOvertime { get; private set; }

        /// <summary>
        /// Handles the GET request to fetch details of a specific NBA match by game ID.
        /// </summary>
        /// <param name="gameId">Game ID of the match to fetch.</param>
        /// <param name="year">Year of the NBA season (defaults to 2024 if not provided).</param>
        /// <returns>The page result with the match details.</returns>
        public IActionResult OnGet(string gameId, int year = 2024)
        {
            // Parse the game ID into an integer.
            int matchId = int.Parse(gameId);

            // Define the NBA league ID, base URL, and endpoint for fetching game data.
            string NBAleagueId = "standard";
            string baseUrl = "https://v2.nba.api-sports.io"; // API base URL.
            string baseHost = "v2.aenba.api-sports.io";    // Base host for the API request.
            string endPoint = "games";                      // Endpoint for fetching game data.

            try
            {
                // Fetch the list of NBA games for the specified season year.
                List<NbaGameResponse> Games = _sportsApiClient.GetGamesForSeason<NbaGameResponse>(NBAleagueId, year, baseUrl, baseHost, endPoint);

                // Find the specific match from the list of games using the match ID.
                Match = Games.FirstOrDefault(m => m.Id == matchId);
            }
            catch (System.Exception ex)
            {
                // Log any errors that occur while fetching the game data.
                _logger.LogError(ex, "Error fetching game results.");
            }

            // If no match is found, redirect to the NBA matches page.
            if (Match == null)
            {
                return RedirectToPage("/NBAMatches");
            }

            // Variables to hold the total scores for the home and visitor teams.
            int homeTotal = 0;
            int visitorTotal = 0;

            // Calculate the sum of scores from the first 4 quarters of the game.
            for (int i = 0; i < 4; i++)
            {
                homeTotal += int.Parse(Match.Scores.Home.Linescore[i]);
                visitorTotal += int.Parse(Match.Scores.Visitors.Linescore[i]);
            }

            // Check if there was overtime by comparing the total score with the points in the game.
            HasOvertime = homeTotal != Match.Scores.Home.Points || visitorTotal != Match.Scores.Visitors.Points;

            // Return the page with the match details.
            return Page();
        }
    }
}