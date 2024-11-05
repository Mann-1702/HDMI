using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages
{
    public class CompareTeamsModel : PageModel
    {
        // Properties to hold selected sport and teams
        [BindProperty]
        public string SelectedSport { get; set; }

        [BindProperty]
        public string Team1 { get; set; }

        [BindProperty]
        public string Team2 { get; set; }

        // Lists for available sports and teams
        public List<string> Sports { get; set; } = new List<string> { "NFL", "NBA", "Soccer" };
        public Dictionary<string, List<string>> Teams { get; set; } = new Dictionary<string, List<string>>
        {
            { "NFL", new List<string> { "Patriots", "49ers", "Cowboys", "Packers" } },
            { "NBA", new List<string> { "Lakers", "Warriors", "Celtics", "Nets" } },
            { "Soccer", new List<string> { "Barcelona", "Real Madrid", "Liverpool", "Manchester United" } }
        };

        // Property to hold comparison result
        public string ComparisonResult { get; private set; }

        public void OnGet()
        {
            // This method would load the page without any pre-selected sport or teams
        }

        public IActionResult OnPostCompare()
        {
            // Validate the selected teams
            if (string.IsNullOrEmpty(Team1) || string.IsNullOrEmpty(Team2) || Team1 == Team2)
            {
                ComparisonResult = "Please select two different teams to compare.";
                return Page();
            }

            // Generate a comparison result (this is just a placeholder; replace with actual comparison logic if needed)
            ComparisonResult = $"Comparing {Team1} and {Team2} in {SelectedSport}...";
            ComparisonResult += $"\nBoth teams are well-matched in the {SelectedSport} league.";

            return Page();
        }

        public JsonResult OnGetTeamsBySport(string sport)
        {
            // Return the list of teams based on the selected sport
            if (Teams.ContainsKey(sport))
            {
                return new JsonResult(Teams[sport]);
            }

            return new JsonResult(new List<string>());
        }
    }
}
