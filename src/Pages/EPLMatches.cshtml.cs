using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ContosoCrafts.WebSite.Pages
{
    public class EPLMatches : PageModel
    {
        private readonly SportsApiClient _sportsApiClient;
        private readonly ILogger<EPLMatches> _logger;

        public EPLMatches(SportsApiClient sportsApiClient, ILogger<EPLMatches> logger)
        {
            _sportsApiClient = sportsApiClient;
            _logger = logger;
        }

        public List<FixtureResponse> Games { get; private set; }
        public void OnGet()
        {
            try
            {

                string NBAleagueId = "39"; //use "Standard for NBA"
                int seasonYear = 2022;
                string baseUrl = "https://v3.football.api-sports.io";
                string baseHost = "v3.football.api-sports.io";

                // Fetch game data for NBA 2023 season
                Games = _sportsApiClient.GetGamesForSeason<FixtureResponse>(NBAleagueId, seasonYear, baseUrl, baseHost);
            }

            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching game results.");
                Games = new List<FixtureResponse>(); // Handle error 
            }


        }
    }
}
