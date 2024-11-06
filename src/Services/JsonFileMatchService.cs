using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using ContosoCrafts.WebSite.Models;

using Microsoft.AspNetCore.Hosting;

namespace ContosoCrafts.WebSite.Services
{
    public class JsonFileMatchService
    {

        public JsonFileMatchService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }


        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "matches.json"); }
        }


        public IEnumerable<MatchModel> GetAllData()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                return JsonSerializer.Deserialize<MatchModel[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
        }

        /// <summary>
        /// Swaps Team1 and Team2 in the match data including their scores
        /// 
        /// </summary>
        /// <param name="match"></param>
        public void SwapTeam1Team2(MatchModel match)
        {
			string tempTeam = match.Team2;
			match.Team2 = match.Team1;
			match.Team1 = tempTeam;

            int tempScore = match.Team2_Score;
			match.Team2_Score = match.Team1_Score;
			match.Team1_Score = tempScore;
        }

    }
}
