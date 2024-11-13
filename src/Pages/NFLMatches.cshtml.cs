using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoCrafts.WebSite.Pages
{
    public class NFLMatchesModel : PageModel
    {
        private readonly SportsApiClient _sportsApiClient;
        private readonly ILogger<NFLMatchesModel> _logger;

        public NFLMatchesModel(SportsApiClient sportsApiClient, ILogger<NFLMatchesModel> logger)
        {
            _sportsApiClient = sportsApiClient;
            _logger = logger;
        }

        public List<GameResponse> Games { get; private set; }

        public IActionResult OnGet()
        {
            try
            {

                string nflLeagueId = "1"; //use "Standard for NBA"
                int seasonYear = 2024;
                string baseUrl = "https://v1.american-football.api-sports.io";
                string baseHost = "v1.american-football.api-sports.io";

                // Fetch game data for NFL 2023 season
                Games = _sportsApiClient.GetGamesForSeason<GameResponse>(nflLeagueId, seasonYear, baseUrl, baseHost);

            }

            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching game results.");
                Games = new List<GameResponse>(); // Handle error 
            }

            return Page();
        }


    }

}