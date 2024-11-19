using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
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
            Games = new List<FixtureResponse>();
        }

        // This property holds the list of games
        public List<FixtureResponse> Games { get; private set; }

        // Fetching data and initializing the games property
        public void OnGet()
        {
            string eplLeagueId = "39"; // EPL League ID
            int seasonYear = 2024;
            string baseUrl = "https://v3.football.api-sports.io";
            string baseHost = "v3.football.api-sports.io";

            try
            {
                if (_sportsApiClient == null)
                {
                    _logger.LogError("SportsApiClient is null. Cannot fetch game data.");
                    return; // Exit early if the client is null
                }
                // Fetch games for the 2024 season
                Games = _sportsApiClient.GetGamesForSeason<FixtureResponse>(eplLeagueId, seasonYear, baseUrl, baseHost);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching EPL game data.");
            }
        }
    }
}
