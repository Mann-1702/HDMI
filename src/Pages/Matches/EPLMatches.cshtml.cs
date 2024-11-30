using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Matches
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

        public int seasonYear { get; private set; }

        // Fetching data and initializing the games property
        public IActionResult OnGet(string teamName = null, int year= 2024)
        {
            seasonYear = year;
            // EPL League ID
            string eplLeagueId = "39";    
            string baseUrl = "https://v3.football.api-sports.io";
            string baseHost = "v3.football.api-sports.io";
            string endPoint = "fixtures";

            try
            {
                if (_sportsApiClient == null)
                {
                    _logger.LogError("SportsApiClient is null. Cannot fetch game data.");

                    // Exit early if the client is null
                    return RedirectToPage("/Error");
                }

                // Fetch games for the 2024 season
                Games = _sportsApiClient.GetGamesForSeason<FixtureResponse>(eplLeagueId, seasonYear, baseUrl, baseHost,endPoint);

                // Return page if no specified team to filter for
                if (teamName == null)
                {
                    return Page();
                }

                // Filters for a specific team
                Games = Games.Where(g => g.Teams.Home.Name == teamName || g.Teams.Visitors.Name == teamName).ToList();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching EPL game data.");
                Games = new List<FixtureResponse>();
            }

            return Page();
        }
    }
}
