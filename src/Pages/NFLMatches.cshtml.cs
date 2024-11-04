using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContosoCrafts.WebSite.Pages
{
    public class NFLMatches : PageModel
    {
        private readonly SportsApiClient _sportsApiClient;
        private readonly ILogger<NFLMatches> _logger;

        public NFLMatches(SportsApiClient sportsApiClient, ILogger<NFLMatches> logger)
        {
            _sportsApiClient = sportsApiClient;
            _logger = logger;
        }

        public List<GameResponse> Games { get; private set; }

        public async Task OnGetAsync()
        {
            try
            {
               
                int nflLeagueId = 1; // Might need replacement  with actual NFL league ID
                int seasonYear = 2024;

                // Fetch game data for NFL 2023 season
                Games = _sportsApiClient.GetGamesForSeason(nflLeagueId, seasonYear);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching game results.");
                Games = new List<GameResponse>(); // Handle error 
            }
        }
    }
}
