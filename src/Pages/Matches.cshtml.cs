using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ContosoCrafts.WebSite.Pages
{
    public class MatchesModel : PageModel
    {
        public List<MatchModel> Matches { get; set; }

        public void OnGet()
        {
            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "matches.json");
            var jsonData = System.IO.File.ReadAllText(jsonFilePath);
            Matches = JsonSerializer.Deserialize<List<MatchModel>>(jsonData);
        }
    }
}
