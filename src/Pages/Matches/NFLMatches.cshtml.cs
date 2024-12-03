using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoCrafts.WebSite.Pages.Matches
{
    public class NFLMatchesModel : PageModel
    {
        // SportsApiClient used to fetch NFL game data from the external API.
        private readonly SportsApiClient _sportsApiClient;

        // Logger instance to log errors or important information related to the page.
        private readonly ILogger<NFLMatchesModel> _logger;

        /// <summary>
        /// Constructor to initialize the SportsApiClient and Logger for the page model.
        /// </summary>
        /// <param name="sportsApiClient">SportsApiClient to interact with the external sports API.</param>
        /// <param name="logger">ILogger to log any issues during data fetching.</param>
        public NFLMatchesModel(SportsApiClient sportsApiClient, ILogger<NFLMatchesModel> logger)
        {
            _sportsApiClient = sportsApiClient;
            _logger = logger;
        }

        // Public property to store the list of NFL game data.
        public List<GameResponse> Games { get; private set; }

        // Public property to store the season year, default is 2024.
        public int SeasonYear { get; private set; }

        /// <summary>
        /// Handles the GET request to fetch NFL game data for a specific season and team (optional).
        /// </summary>
        /// <param name="teamName">Optional parameter to filter games by a specific team name.</param>
        /// <param name="year">Year of the NFL season (defaults to 2024 if not provided).</param>
        /// <returns>The page result with the list of NFL games, filtered by team if applicable.</returns>
        public IActionResult OnGet(string teamName = null, int year = 2024)
        {
            // Set the season year based on the parameter or default to 2024.
            SeasonYear = year;

            // Define the NFL league ID, base URL, and endpoint for fetching game data.
            string leagueId = "1"; // NFL league ID
            string baseUrl = "https://v1.american-football.api-sports.io"; // API base URL.
            string baseHost = "v1.american-football.api-sports.io";    // Base host for the API request.
            string endPoint = "games";                                  // Endpoint for fetching game data.

            try
            {
                // Fetch NFL game data for the specified season year.
                Games = _sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, SeasonYear, baseUrl, baseHost, endPoint);

                // If no team name is specified, return the page with all games.
                if (teamName == null)
                {
                    return Page();
                }

                // Filter the games to include only those where the specified team is playing.
                Games = Games.Where(g => g.Teams.Home.Name == teamName || g.Teams.Away.Name == teamName).ToList();
            }
            catch (Exception ex)
            {
                // Log any errors that occur while fetching the game data.
                _logger.LogError(ex, "Error fetching game results.");

                // Initialize the Games list as an empty list in case of an error.
                Games = new List<GameResponse>();
            }

            // Return the page with the fetched game data.
            return Page();
        }
    }
}