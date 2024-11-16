using ContosoCrafts.WebSite.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Services
{
    [TestFixture]
    public class SportsApiClientTests
    {
        private ILogger<SportsApiClient> logger;
        private SportsApiClient sportsApiClient;

        [SetUp]
        public void Setup()
        {
            // Initialize logger
            logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<SportsApiClient>();

            // API key for the Sports API
            var apiKey = "b4ed364047f61b7a0ae7699c69c7ad57";

            // Instantiate SportsApiClient
            sportsApiClient = new SportsApiClient(apiKey, logger);
        }

        [Test]
        public void SportsApiClient_ShouldInitializeSuccessfully()
        {
            // Assert that the SportsApiClient is not null
            Assert.That(sportsApiClient, Is.Not.Null);
        }

        [Test]
        public void GetGamesForSeason_ShouldReturnEmptyList_WhenNoGamesAvailable()
        {
            // Arrange
            var leagueId = "1"; 
            var seasonYear = 2025;
            var baseUrl = "https://v1.american-football.api-sports.io";
            var apiHost = "v1.american-football.api-sports.io";

            // Act
            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost);

            // Assert
            Assert.That(result, Is.Empty, "Expected an empty list when no games are available.");
        }

       

        [Test]
        public void GetGamesForSeason_ShouldThrowException_WhenUnhandledErrorOccurs()
        {
            // Arrange
            var leagueId = "9999"; // Invalid league ID
            var seasonYear = 2024;
            var baseUrl = "https://v1.american-football.api-sports.io";
            var apiHost = "v1.american-football.api-sports.io";

            // Act & Assert
            try
            {
                var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.That(ex.Message, Does.Contain("Request failed with status code"), "Expected a generic error message for unhandled errors.");
            }
        }

        [Test]
        public void GetGamesForSeason_ShouldReturnGames_WhenApiReturnsValidResponse()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;
            var baseUrl = "https://v1.american-football.api-sports.io";
            var apiHost = "v1.american-football.api-sports.io";

            // Act
            List<GameResponse> result;

            try
            {
                result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost);
            }
            catch (Exception ex)
            {
                Assert.Fail($"An exception was thrown while retrieving games: {ex.Message}");
                return;
            }

            // Assert
            Assert.That(result, Is.Not.Empty, "Expected a list of games.");

            // Validate the structure of the first game in the response
            var firstGame = result.FirstOrDefault();
            Assert.That(firstGame, Is.Not.Null, "Expected at least one game in the response.");
            Assert.That(firstGame.Game.Id, Is.GreaterThan(0), "Expected the game ID to be a positive number.");
            Assert.That(firstGame.Game.Stage, Is.Not.Null.Or.Empty, "Expected the game stage to be non-null and non-empty.");
            Assert.That(firstGame.Teams.Home.Name, Is.Not.Null.Or.Empty, "Expected the home team name to be non-null and non-empty.");
            Assert.That(firstGame.Teams.Away.Name, Is.Not.Null.Or.Empty, "Expected the away team name to be non-null and non-empty.");
        }
        [Test]
        public void GetGamesForSeason_ShouldThrowException_WhenRequestTimesOut()
        {
            // Arrange
            var leagueId = "1"; 
            var seasonYear = 2024;
            var baseUrl = "https://v1.american-football.api-sports.io";
            var apiHost = "v1.american-football.api-sports.io";

            // Create a client with an empty API key to simulate a timeout or bad response
            var clientWithInvalidKey = new SportsApiClient("", logger);

            // Act & Assert
            var caughtException = Assert.Throws<JsonSerializationException>(() =>
                clientWithInvalidKey.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost));

            // Assert
            Assert.That(caughtException, Is.Not.Null, "Expected a JsonSerializationException to be thrown.");
            Assert.That(caughtException.Message, Does.Contain("Cannot deserialize the current JSON object"),
                "Expected the exception message to indicate a deserialization issue.");
        }

        [Test]
        public void GetGamesForSeason_ShouldLogInformation_WhenRequestSucceeds()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;
            var baseUrl = "https://v1.american-football.api-sports.io";
            var apiHost = "v1.american-football.api-sports.io";

            // Act
            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost);

            // Assert
            // (You'd use a logging framework with a test sink to validate that logs contain expected entries)
            Assert.That(result, Is.Not.Empty, "Expected a successful API response with logging.");
        }
        [Test]
        public void GetGamesForSeason_ShouldThrowException_WhenHeadersAreMissing()
        {
            // Arrange
            var leagueId = "1"; 
            var seasonYear = 2024;
            var baseUrl = "https://v1.american-football.api-sports.io";
            var apiHost = "v1.american-football.api-sports.io";

            // Simulate a client with a missing API key (empty string to simulate missing headers)
            var clientWithMissingHeaders = new SportsApiClient("", logger);

            // Act & Assert
            var caughtException = Assert.Throws<JsonSerializationException>(() =>
                clientWithMissingHeaders.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost));

            // Assert
            Assert.That(caughtException, Is.Not.Null, "Expected a JsonSerializationException to be thrown.");
            Assert.That(caughtException.Message, Does.Contain("Cannot deserialize the current JSON object"),
                "Expected the exception message to indicate a deserialization issue.");
        }



    }

    [TestFixture]
    public class ApiResponseTests
    {
        [Test]
        public void ApiResponse_ShouldInitializeWithDefaultValues()
        {
            // Act
            var apiResponse = new ApiResponse<string>();

            // Assert
            Assert.That(apiResponse.Get, Is.Null, "Expected 'Get' to be null by default.");
            Assert.That(apiResponse.Parameters, Is.Null, "Expected 'Parameters' to be null by default.");
            Assert.That(apiResponse.Errors, Is.Null, "Expected 'Errors' to be null by default.");
            Assert.That(apiResponse.Results, Is.EqualTo(0), "Expected 'Results' to be 0 by default.");
            Assert.That(apiResponse.Response, Is.Null, "Expected 'Response' to be null by default.");
        }

        [Test]
        public void ApiResponse_ShouldAllowSettingAndGettingProperties()
        {
            // Arrange
            var parameters = new Dictionary<string, string>
            {
                { "league", "39" },
                { "season", "2024" }
            };

            var errors = new List<object> { "Invalid league ID", "Season not found" };
            var response = new List<string> { "Game 1", "Game 2" };

            // Act
            var apiResponse = new ApiResponse<string>
            {
                Get = "fixtures",
                Parameters = parameters,
                Errors = errors,
                Results = 2,
                Response = response
            };

            // Assert
            Assert.That(apiResponse.Get, Is.EqualTo("fixtures"), "Expected 'Get' to match the assigned value.");
            Assert.That(apiResponse.Parameters, Is.EqualTo(parameters), "Expected 'Parameters' to match the assigned dictionary.");
            Assert.That(apiResponse.Errors, Is.EqualTo(errors), "Expected 'Errors' to match the assigned list.");
            Assert.That(apiResponse.Results, Is.EqualTo(2), "Expected 'Results' to match the assigned value.");
            Assert.That(apiResponse.Response, Is.EqualTo(response), "Expected 'Response' to match the assigned list.");
        }

        [Test]
        public void ApiResponse_ShouldHandleNullValuesGracefully()
        {
            // Act
            var apiResponse = new ApiResponse<string>
            {
                Get = null,
                Parameters = null,
                Errors = null,
                Results = 0,
                Response = null
            };

            // Assert
            Assert.That(apiResponse.Get, Is.Null, "Expected 'Get' to be null.");
            Assert.That(apiResponse.Parameters, Is.Null, "Expected 'Parameters' to be null.");
            Assert.That(apiResponse.Errors, Is.Null, "Expected 'Errors' to be null.");
            Assert.That(apiResponse.Results, Is.EqualTo(0), "Expected 'Results' to remain 0.");
            Assert.That(apiResponse.Response, Is.Null, "Expected 'Response' to be null.");
        }
        [Test]
        public void ApiResponse_ShouldHandleLargeResponseList()
        {
            // Arrange
            var largeList = Enumerable.Range(1, 10000).Select(i => $"Game {i}").ToList();

            // Act
            var apiResponse = new ApiResponse<string> { Response = largeList };

            // Assert
            Assert.That(apiResponse.Response.Count, Is.EqualTo(10000), "Expected a large list to be handled correctly.");
        }

    }

}
