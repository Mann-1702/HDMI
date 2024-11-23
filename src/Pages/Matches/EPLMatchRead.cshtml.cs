using System;
using System.Linq;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ContosoCrafts.WebSite.Pages.Matches
{
    public class EPLMatchReadModel : PageModel
    {
        private readonly SportsApiClient _sportsApiClient;
        private readonly ILogger<EPLMatchReadModel> _logger;

        public EPLMatchReadModel(SportsApiClient sportsApiClient, ILogger<EPLMatchReadModel> logger)
        {
            _sportsApiClient = sportsApiClient;
            _logger = logger;
        }


        public FixtureResponse Match { get; private set; }

        public IActionResult OnGet(int matchId)
        {
            try
            {
                // EPL League ID
                string leagueId = "39";
                int seasonYear = 2024;
                string baseUrl = "https://v3.football.api-sports.io";
                string apiHost = "v3.football.api-sports.io";

                // Fetch games for the given season
                var allGames = _sportsApiClient.GetGamesForSeason<FixtureResponse>(leagueId, seasonYear, baseUrl, apiHost);

                // Find the match between the specified teams
                Match = allGames.FirstOrDefault(game => game.Fixture.FixtureId == matchId);

                if (Match == null)
                {
                    _logger.LogWarning($"Match with MatchId: {matchId} not found.");
                    return RedirectToPage("/EPLMatches");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching details for match with matchId {matchId}");
                Match = null;
                return RedirectToPage("/NFLMatches");
            }

            return Page();
        }

    }
}