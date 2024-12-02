using ContosoCrafts.WebSite.Services;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace UnitTests.Services
{
    /// <summary>
    /// Unit tests for the TeamVerifier class, which verifies if a given team name is valid for a specific sport type.
    /// </summary>
    [TestFixture]
    public class TeamVerifierTests
    {
        private string _testJsonFilePath;

        /// <summary>
        /// Sets up the test environment by creating a temporary JSON file with sports data.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // Create a dictionary of sports and teams
            var sportsData = new SportsData
            {
                Sports = new Dictionary<string, List<string>>
                {
                    { "NFL", new List<string> { "Patriots", "Packers", "Cowboys" } },
                    { "Basketball", new List<string> { "Lakers", "Bulls", "Celtics" } }
                }
            };

            // Generate a temporary file path and write the sports data to the file
            _testJsonFilePath = Path.Combine(Path.GetTempPath(), "sportsData.json");
            File.WriteAllText(_testJsonFilePath, JsonConvert.SerializeObject(sportsData));
        }

        /// <summary>
        /// Cleans up by deleting the temporary JSON file after each test.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_testJsonFilePath))
            {
                File.Delete(_testJsonFilePath);
            }
        }

        /// <summary>
        /// Verifies that a valid team name returns true.
        /// </summary>
        [Test]
        public void IsValidName_ShouldReturnTrue_ForValidTeam()
        {
            // Arrange
            var teamVerifier = new TeamVerifier(_testJsonFilePath);

            // Act
            var result = teamVerifier.IsValidName("NFL", "Patriots");

            // Assert
            Assert.That(result, Is.True, "Expected 'Patriots' to be a valid team in 'NFL'.");
        }

        /// <summary>
        /// Verifies that an invalid team name returns false.
        /// </summary>
        [Test]
        public void IsValidName_ShouldReturnFalse_ForInvalidTeam()
        {
            // Arrange
            var teamVerifier = new TeamVerifier(_testJsonFilePath);

            // Act
            var result = teamVerifier.IsValidName("Football", "Redskins");

            // Assert
            Assert.That(result, Is.False, "Expected 'Redskins' to be an invalid team in 'Football'.");
        }

        /// <summary>
        /// Verifies that an invalid sport type returns false.
        /// </summary>
        [Test]
        public void IsValidName_ShouldReturnFalse_ForInvalidSportType()
        {
            // Arrange
            var teamVerifier = new TeamVerifier(_testJsonFilePath);

            // Act
            var result = teamVerifier.IsValidName("Baseball", "Yankees");

            // Assert
            Assert.That(result, Is.False, "Expected 'Baseball' to be an invalid sport type.");
        }

        /// <summary>
        /// Verifies that the validation is case-insensitive for team names.
        /// </summary>
        [Test]
        public void IsValidName_ShouldIgnoreCase_ForTeamNames()
        {
            // Arrange
            var teamVerifier = new TeamVerifier(_testJsonFilePath);

            // Act
            var result = teamVerifier.IsValidName("NFL", "patriots");

            // Assert
            Assert.That(result, Is.True, "Expected 'patriots' to be valid regardless of case.");
        }

        /// <summary>
        /// Verifies that an empty team name returns false.
        /// </summary>
        [Test]
        public void IsValidName_ShouldReturnFalse_ForEmptyTeamName()
        {
            // Arrange
            var teamVerifier = new TeamVerifier(_testJsonFilePath);

            // Act
            var result = teamVerifier.IsValidName("Football", "");

            // Assert
            Assert.That(result, Is.False, "Expected empty team name to be invalid.");
        }
    }
}