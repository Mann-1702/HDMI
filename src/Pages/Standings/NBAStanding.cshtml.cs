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
    public class NBAStandingsModel : PageModel
    {
        private readonly SportsApiClient _sportsApiClient;
        private readonly ILogger<NBAMatchesModel> _logger;

        public NBAStandingsModel(SportsApiClient sportsApiClient, ILogger<NBAMatchesModel> logger)
        {
            _sportsApiClient = sportsApiClient;
            _logger = logger;
        }

        // List of NBA Games
        public List<NBAStanding> Games { get; private set; }

        // Year of the NBA Season (Default = 2024)
        public int SeasonYear { get; private set; }

        public IActionResult OnGet(string teamName = null, int year = 2024)
        {

            SeasonYear = year;

            string leagueId = "standard";
            string baseUrl = "https://v2.nba.api-sports.io";
            string baseHost = "v2.aenba.api-sports.io";
            string endPoint = "standings";

            try
            {
                // Fetch game data for NFL season with specified year
                Games = _sportsApiClient.GetGamesForSeason<NBAStanding>(leagueId, SeasonYear, baseUrl, baseHost, endPoint).ToList();

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
                Games = new List<NBAStanding>();
            }

            return Page();
        }
    }
}