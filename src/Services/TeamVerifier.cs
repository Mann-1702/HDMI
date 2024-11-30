using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

namespace ContosoCrafts.WebSite.Services
{
    /// <summary>
    /// Verifies if a given team name exists under the correct sport type.
    /// </summary>
    public class TeamVerifier
    {
        // A dictionary that maps sport types (e.g., "Soccer", "Basketball") to a list of team names
        private readonly Dictionary<string, List<string>> _sportsTeams;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamVerifier"/> class.
        /// Loads the sports teams data from a specified JSON file.
        /// </summary>
        /// <param name="jsonFilePath">The file path to the JSON data containing sports teams.</param>
        public TeamVerifier(string jsonFilePath)
        {
            // Load the JSON file into a string
            var jsonContent = File.ReadAllText(jsonFilePath);

            // Deserialize the JSON content into a strongly typed SportsData object
            var sportsData = JsonConvert.DeserializeObject<SportsData>(jsonContent);

            // The Sports property contains a dictionary of sport types and their respective teams
            _sportsTeams = sportsData.Sports;
        }

        /// <summary>
        /// Verifies if a team name exists under the correct sport type.
        /// </summary>
        /// <param name="sportType">The type of sport (e.g., "Soccer", "Basketball").</param>
        /// <param name="teamName">The name of the team to verify.</param>
        /// <returns>True if the team name exists under the given sport type, otherwise false.</returns>
        public bool IsValidName(string sportType, string teamName)
        {
            // Check if the sport type exists in the dictionary
            if (_sportsTeams.ContainsKey(sportType))
            {
                // Check if the team name exists under the specified sport type, ignoring case
                return _sportsTeams[sportType].Contains(teamName, StringComparer.OrdinalIgnoreCase);
            }

            // If the sport type is not found, log an error message
            Console.WriteLine($"Sport type '{sportType}' not found in the data.");
            return false;
        }
    }

    /// <summary>
    /// Represents the structure of sports data containing a dictionary of sport types and their respective teams.
    /// </summary>
    public class SportsData
    {
        // A dictionary where keys are sport types (e.g., "Soccer", "Basketball") and values are lists of team names
        public Dictionary<string, List<string>> Sports { get; set; }
    }
}