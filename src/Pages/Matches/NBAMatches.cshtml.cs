using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Matches
{
    public class NBAMatchesModel : PageModel
    {
        private readonly SportsApiClient _sportsApiClient;
        private readonly ILogger<NBAMatchesModel> _logger;

        public NBAMatchesModel(SportsApiClient sportsApiClient, ILogger<NBAMatchesModel> logger)
        {
            _sportsApiClient = sportsApiClient;
            _logger = logger;
        }

        // List of NBA Games
        public List<NbaGameResponse> Games { get; private set; }

        // Year of the NFL Season (Default = 2024)
        public int SeasonYear { get; private set; }

        public IActionResult OnGet(string teamName = null, int year = 2024)
        {
            SeasonYear = year;

            //use "Standard for NBA"
            string NBAleagueId = "standard";
            string baseUrl = "https://v2.nba.api-sports.io";
            string baseHost = "v2.aenba.api-sports.io";
            string endPoint = "games";

            try
            {
                // Fetch game data for NBA 2024 season
                Games = _sportsApiClient.GetGamesForSeason<NbaGameResponse>(NBAleagueId, SeasonYear, baseUrl, baseHost,endPoint);

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
                _logger.LogError(ex, "Error fetching game results.");
                Games = new List<NbaGameResponse>(); // Handle error 
            }

            return Page();
        }
    }
}
