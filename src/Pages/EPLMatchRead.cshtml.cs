using System;
using System.Linq;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ContosoCrafts.WebSite.Pages
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

        [BindProperty(SupportsGet = true)]
        public string HomeTeamId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string AwayTeamId { get; set; }

        public FixtureResponse Match { get; private set; }

        public void OnGet(string homeTeamId, string awayTeamId)
        {
            try
            {
                string leagueId = "39"; // EPL League ID
                int seasonYear = 2024;
                string baseUrl = "https://v3.football.api-sports.io";
                string apiHost = "v3.football.api-sports.io";

                // Fetch games for the given season
                var allGames = _sportsApiClient.GetGamesForSeason<FixtureResponse>(leagueId, seasonYear, baseUrl, apiHost);

                // Find the match between the specified teams
                Match = allGames.FirstOrDefault(game =>
                    game.Teams?.Home?.Id.ToString() == homeTeamId &&
                    game.Teams?.Visitors?.Id.ToString() == awayTeamId);

                if (Match == null)
                {
                    _logger.LogWarning($"Match between {homeTeamId} and {awayTeamId} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching details for match between {homeTeamId} and {awayTeamId}");
                Match = null;
            }
        }

    }
}