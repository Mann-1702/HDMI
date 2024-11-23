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
    public class NBAReadModel : PageModel
    {
        private readonly SportsApiClient _sportsApiClient;
        private readonly ILogger<NBAReadModel> _logger;

        public NBAReadModel(SportsApiClient sportsApiClient, ILogger<NBAReadModel> logger)
        {
            _sportsApiClient = sportsApiClient;
            _logger = logger;
        }

        public NbaGameResponse Match { get; private set; }
        public bool HasOvertime { get; private set; }


        public IActionResult OnGet(string gameId)
        {
            int matchId = int.Parse(gameId);

            //use "Standard for NBA"
            string NBAleagueId = "standard";
            int seasonYear = 2024;
            string baseUrl = "https://v2.nba.api-sports.io";
            string baseHost = "v2.aenba.api-sports.io";


            // Fetch game data for NBA 2024 season
            try
            {
                List<NbaGameResponse> Games = _sportsApiClient.GetGamesForSeason<NbaGameResponse>(NBAleagueId, seasonYear, baseUrl, baseHost);
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

            // Checks for overtime
            for (int i = 0; i < 4; i++)
            {
                homeTotal += int.Parse(Match.Scores.Home.Linescore[i]);
                visitorTotal += int.Parse(Match.Scores.Visitors.Linescore[i]);
            }

            HasOvertime = homeTotal != Match.Scores.Home.Points || visitorTotal != Match.Scores.Visitors.Points;

            return Page();

        }

    }

}