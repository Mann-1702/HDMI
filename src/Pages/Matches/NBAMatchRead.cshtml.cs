using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.VisualBasic;

namespace ContosoCrafts.WebSite.Pages.Matches
{
    public class NBAReadModel : PageModel
    {
        private readonly SportsApiClient _sportsApiClient;
        private readonly ILogger<NBAReadModel> _logger;

        public NBAReadModel(SportsApiClient sportsApiClient, ILogger<NBAReadModel> logger)
        {
            _sportsApiClient = sportsApiClient;
            _logger = logger;
        }

        // NBA Match
        public NbaGameResponse Match { get; private set; }

        // True is the game has overtime, False otherwise
        public bool HasOvertime { get; private set; }


        public IActionResult OnGet(string gameId, int year = 2024)
        {
            int matchId = int.Parse(gameId);

            //use "Standard for NBA"
            string NBAleagueId = "standard";
            string baseUrl = "https://v2.nba.api-sports.io";
            string baseHost = "v2.aenba.api-sports.io";
            string endPoint = "games";

            try
            {
                // Fetch game data for NBA season with specified year
                List<NbaGameResponse> Games = _sportsApiClient.GetGamesForSeason<NbaGameResponse>(NBAleagueId, year, baseUrl, baseHost,endPoint);

                // Find specific match
                Match = Games.FirstOrDefault(m => m.Id == matchId);
            }

            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching game results.");
            }

            // Makes sure a match is found
            if (Match == null)
            {
                return RedirectToPage("/NBAMatches");
            }

            int homeTotal = 0;
            int visitorTotal = 0;

            // Calculates sum of scores from first 4 quarters
            for (int i = 0; i < 4; i++)
            {
                homeTotal += int.Parse(Match.Scores.Home.Linescore[i]);
                visitorTotal += int.Parse(Match.Scores.Visitors.Linescore[i]);
            }

            // Checks for overtime: Checks if total score matches the score from first 4 quarters
            HasOvertime = homeTotal != Match.Scores.Home.Points || visitorTotal != Match.Scores.Visitors.Points;

            return Page();

        }

    }

}