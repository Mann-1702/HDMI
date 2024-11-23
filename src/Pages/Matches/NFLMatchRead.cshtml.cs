using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Reflection;

namespace ContosoCrafts.WebSite.Pages.Matches
{
    public class NFLReadModel : PageModel
    {
        private readonly SportsApiClient _sportsApiClient;
        private readonly ILogger<NFLReadModel> _logger;

        public NFLReadModel(SportsApiClient sportsApiClient, ILogger<NFLReadModel> logger)
        {
            _sportsApiClient = sportsApiClient;
            _logger = logger;
        }

        public GameResponse Match { get; private set; }

        public bool HasOvertime { get; private set; }


        public IActionResult OnGet(string gameId, int year = 2024)
        {
            int matchId = int.Parse(gameId);

            // leagueId = 1 for NFL"
            string leagueId = "1";
            string baseUrl = "https://v1.american-football.api-sports.io";
            string baseHost = "v1.american-football.api-sports.io";


            try
            {
                // Fetch game data for NFL season with specified year
                List<GameResponse> Games = _sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, year, baseUrl, baseHost);

                // Find specific match
                Match = Games.FirstOrDefault(m => m.Game.Id == matchId);
            }

            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching game results.");
            }

            // Makes sure a match is found
            if (Match == null)
            {
                return RedirectToPage("/NFLMatches");
            }

            // Checks for overtime
            HasOvertime = Match.Scores.Home.Overtime > 0 || Match.Scores.Away.Overtime > 0;

            return Page();

        }

    }

}