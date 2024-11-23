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
        private readonly SportsApiClient _sportsApiClient;
        private readonly ILogger<NFLMatchesModel> _logger;

        public NFLMatchesModel(SportsApiClient sportsApiClient, ILogger<NFLMatchesModel> logger)
        {
            _sportsApiClient = sportsApiClient;
            _logger = logger;
        }

        // List of NFL Games
        public List<GameResponse> Games { get; private set; }

        // Year of the NFL Season (Default = 2024)
        public int SeasonYear { get; private set; }

        public IActionResult OnGet(string teamName = null, int year = 2024)
        {

            SeasonYear = year;

            // leagueId = 1 for NFL"
            string leagueId = "1";
            string baseUrl = "https://v1.american-football.api-sports.io";
            string baseHost = "v1.american-football.api-sports.io";

            try
            {
                // Fetch game data for NFL season with specified year
                Games = _sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, SeasonYear, baseUrl, baseHost);
                
                // Return page if no specified team to filter for
                if (teamName == null)
                {
                    return Page();
                }

                // Filters for a specific team
                Games = Games.Where(g => g.Teams.Home.Name == teamName || g.Teams.Away.Name == teamName).ToList();

            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching game results.");

                // Handle error 
                Games = new List<GameResponse>();
            }

            return Page();
        }


    }

}