using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ContosoCrafts.WebSite.Pages
{
    public class NBAMatches : PageModel
    {
        private readonly SportsApiClient _sportsApiClient;
        private readonly ILogger<NBAMatches> _logger;

        public NBAMatches(SportsApiClient sportsApiClient, ILogger<NBAMatches> logger)
        {
            _sportsApiClient = sportsApiClient;
            _logger = logger;
        }

        public List<GameResponse> Games { get; private set; }
        public void OnGet()
        {
            try
            {

                string NBAleagueId = "standard"; //use "Standard for NBA"
                int seasonYear = 2024;
                string baseUrl = "https://v2.nba.api-sports.io";
                string baseHost = "v2.nba.api-sports.io";

                // Fetch game data for NBA 2023 season
                Games = _sportsApiClient.GetGamesForSeason(NBAleagueId, seasonYear, baseUrl, baseHost);
            }

            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching game results.");
                Games = new List<GameResponse>(); // Handle error 
            }


        }
    }
}
