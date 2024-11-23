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

        public List<NbaGameResponse> Games { get; private set; }

        public IActionResult OnGet(string teamName = null)
        {
            try
            {
                //use "Standard for NBA"
                string NBAleagueId = "standard";
                int seasonYear = 2024;
                string baseUrl = "https://v2.nba.api-sports.io";
                string baseHost = "v2.aenba.api-sports.io";

                // Fetch game data for NBA 2024 season
                Games = _sportsApiClient.GetGamesForSeason<NbaGameResponse>(NBAleagueId, seasonYear, baseUrl, baseHost);

                if (teamName != null)
                {
                    Games = Games.Where(g => g.Teams.Home.Name == teamName || g.Teams.Visitors.Name == teamName).ToList();
                }
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
