using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

namespace ContosoCrafts.WebSite.Services
{
    public class TeamVerifier
    {
        private readonly Dictionary<string, List<string>> _sportsTeams;

        public TeamVerifier(string jsonFilePath)
        {
            // Load the JSON file into a dictionary
            var jsonContent = File.ReadAllText(jsonFilePath);
            var sportsData = JsonConvert.DeserializeObject<SportsData>(jsonContent);

            // Navigate to the "Sports" level
            _sportsTeams = sportsData.Sports;
        }

        // Verify if a team name exists under the correct sport type
        public bool IsValidName(string sportType, string teamName)
        {
            // Check if the sport type exists
            if (_sportsTeams.ContainsKey(sportType))
            {
                // Check if the team name exists under the specified sport type
                return _sportsTeams[sportType].Contains(teamName, StringComparer.OrdinalIgnoreCase);
            }

            // If sport type is not found
            Console.WriteLine($"Sport type '{sportType}' not found in the data.");
            return false;
        }

        // Check if a team name exists across all sports (for duplicate checking)
        public bool IsDuplicateTeam(string teamName)
        {
            if (string.IsNullOrWhiteSpace(teamName))
            {
                return false;
            }

            return _sportsTeams.Values
                .SelectMany(teams => teams) // Combine all team lists
                .Any(t => t.Equals(teamName, StringComparison.OrdinalIgnoreCase));
        }


    }
    public class SportsData
    {
        public Dictionary<string, List<string>> Sports { get; set; }
    }
}
