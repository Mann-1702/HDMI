using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace ContosoCrafts.WebSite.Pages
{
    public class MatchesModel : PageModel
    {
        public MatchesModel(JsonFileMatchService matchService)
        {
            MatchService = matchService;
        }

        // Data Service
        public JsonFileMatchService MatchService { get; }

        // Collection of the Data
        public IEnumerable<MatchModel> Matches { get; set; }


        public void OnGet(string teamName)
        {
            // Get all match data
            Matches = MatchService.GetAllData();

            // Filter for specific team
            if (teamName != null)
            {
                Matches = Matches.Where(m => m.Team1 == teamName || m.Team2 == teamName);
            }

            // Swap Team1 and Team2 data if the filtered team is in the Team2 field
			foreach (var match in Matches)
            {
                if (match.Team2 == teamName)
                {
                    MatchService.SwapTeam1Team2(match);
                }
            }

		}
    }
}
