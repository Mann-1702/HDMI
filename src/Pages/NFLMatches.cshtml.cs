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

        public IActionResult OnGet(string teamName = null)
        {
            try
            {
                //use "Standard for NBA"
                string nflLeagueId = "1";
                int seasonYear = 2024;
                string baseUrl = "https://v1.american-football.api-sports.io";
                string baseHost = "v1.american-football.api-sports.io";

                // Fetch game data for NFL 2024 season
                Games = _sportsApiClient.GetGamesForSeason<GameResponse>(nflLeagueId, seasonYear, baseUrl, baseHost);

                if (teamName != null)
                {
                    Games = Games.Where(g => g.Teams.Home.Name == teamName || g.Teams.Away.Name == teamName).ToList();
                }
            }

            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching game results.");

                // Handle error 
                Games = new List<GameResponse>(); 
            }

            return Page();
        }


    }

}