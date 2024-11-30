using ContosoCrafts.WebSite.Pages.Matches;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Standings
{
    public class NFLStandingModel : PageModel
    {
        private readonly SportsApiClient _sportsApiClient;
        private readonly ILogger<NFLMatchesModel> _logger;

        public NFLStandingModel(SportsApiClient sportsApiClient, ILogger<NFLMatchesModel> logger)
        {
            _sportsApiClient = sportsApiClient;
            _logger = logger;
        }

        // List of NFL Games
        public List<TeamStanding> Games { get; private set; }

        // Year of the NFL Season (Default = 2024)
        public int SeasonYear { get; private set; }

        public IActionResult OnGet(string teamName = null, int year = 2024)
        {

            SeasonYear = year;

            // leagueId = 1 for NFL"
            string leagueId = "1";
            string baseUrl = "https://v1.american-football.api-sports.io";
            string baseHost = "v1.american-football.api-sports.io";
            string endPoint = "standings";

            try
            {
                // Fetch game data for NFL season with specified year
                Games = _sportsApiClient.GetGamesForSeason<TeamStanding>(leagueId, SeasonYear, baseUrl, baseHost, endPoint).ToList();

                // Return page if no specified team to filter for
                if (teamName == null)
                {
                    return Page();
                }


            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching game results.");

                // Handle error 
                Games = new List<TeamStanding>();
            }

            return Page();
        }
    }
}
