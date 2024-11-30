using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using ContosoCrafts.WebSite.Models;

using Microsoft.AspNetCore.Hosting;

namespace ContosoCrafts.WebSite.Services
{
    /// <summary>
    /// Service class responsible for handling match data stored in a JSON file.
    /// Provides methods to get match data, swap teams and scores, and validate match data.
    /// </summary>
    public class JsonFileMatchService
    {
        /// <summary>
        /// Initializes a new instance of the JsonFileMatchService class.
        /// </summary>
        /// <param name="webHostEnvironment">The IWebHostEnvironment used to get the web root path.</param>
        public JsonFileMatchService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Gets or sets the IWebHostEnvironment used to access the web root path.
        /// </summary>
        public IWebHostEnvironment WebHostEnvironment { get; }

        /// <summary>
        /// Gets the file path to the "matches.json" file stored in the "data" folder of the wwwroot directory.
        /// </summary>
        private string JsonFileName
        {
            get
            {
                // Combines the WebRootPath, the "data" folder, and the "matches.json" file name
                return Path.Combine(WebHostEnvironment.WebRootPath, "data", "matches.json");
            }
        }

        /// <summary>
        /// Reads match data from the JSON file and returns it as a collection of MatchModel objects.
        /// </summary>
        /// <returns>A collection of MatchModel objects containing match data.</returns>
        public IEnumerable<MatchModel> GetAllData()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                // Deserializes the JSON data into an array of MatchModel objects
                return JsonSerializer.Deserialize<MatchModel[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Allows case-insensitive property matching during deserialization
                    });
            }
        }

        /// <summary>
        /// Swaps Team1 and Team2 in the provided match data, including their respective scores.
        /// </summary>
        /// <param name="match">The match data containing the teams and scores to be swapped.</param>
        /// <returns>True if the teams and scores were successfully swapped, otherwise false.</returns>
        public bool SwapTeam1Team2(MatchModel match)
        {
            // Validates the match data before proceeding
            if (!IsValidMatch(match))
            {
                return false; // Returns false if the match is invalid
            }

            // Swaps the team names (Team1 and Team2)
            string tempTeam = match.Team2;
            match.Team2 = match.Team1;
            match.Team1 = tempTeam;

            // Swaps the scores of the teams (Team1_Score and Team2_Score)
            int tempScore = match.Team2_Score;
            match.Team2_Score = match.Team1_Score;
            match.Team1_Score = tempScore;

            return true; // Returns true indicating the swap was successful
        }

        /// <summary>
        /// Validates the match data to ensure it is valid.
        /// Checks for valid team names and non-negative scores for both teams.
        /// </summary>
        /// <param name="match">The match data to validate.</param>
        /// <returns>True if the match is valid, otherwise false.</returns>
        public bool IsValidMatch(MatchModel match)
        {
            // Returns false if the match object is null
            if (match == null)
            {
                return false;
            }

            // Returns false if Team1 or Team2 is null
            if (match.Team1 == null)
            {
                return false;
            }

            if (match.Team2 == null)
            {
                return false;
            }

            // Returns false if any of the scores are negative
            if (match.Team1_Score < 0)
            {
                return false;
            }

            if (match.Team2_Score < 0)
            {
                return false;
            }

            return true; // Returns true if all checks pass
        }
    }
}