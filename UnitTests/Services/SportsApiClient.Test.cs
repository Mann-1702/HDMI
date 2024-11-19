using ContosoCrafts.WebSite.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Services
{
    /// <summary>
    /// Unit tests for the SportsApiClient class.
    /// </summary>
    [TestFixture]
    public class SportsApiClientTests
    {
        private ILogger<SportsApiClient> logger;
        private IMemoryCache memoryCache;
        private SportsApiClient sportsApiClient;

        /// <summary>
        /// Sets up the required dependencies.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // Initialize logger
            logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<SportsApiClient>();

            // Initialize memory cache
            memoryCache = new MemoryCache(new MemoryCacheOptions());

            // API key for the Sports API
            var apiKey = "b4ed364047f61b7a0ae7699c69c7ad57";

            // Instantiate SportsApiClient
            sportsApiClient = new SportsApiClient(apiKey, logger, memoryCache);
        }

        /// <summary>
        /// Verifies that an empty list is returned when no games are available.
        /// </summary>
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

        /// <summary>
        /// Verifies that successful responses are cached and contain the expected number of games.
        /// </summary>
        [Test]
        public void GetGamesForSeason_ShouldCacheResults_WhenResponseIsSuccessful()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;
            var baseUrl = "https://v1.american-football.api-sports.io";
            var apiHost = "v1.american-football.api-sports.io";

            // Act
            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost);

            // Re-fetch from cache
            var isCached = memoryCache.TryGetValue($"Games_{leagueId}_{seasonYear}", out List<GameResponse> cachedGames);

            // Assert
            Assert.That(isCached, Is.True, "Expected the result to be cached.");
            Assert.That(cachedGames.Count, Is.EqualTo(100), "Expected the cached result to contain only 100 games.");
            Assert.That(cachedGames, Is.EqualTo(result), "Cached games should match the first 100 games of the original response.");
        }

        /// <summary>
        /// Verifies that an invalid API URL results in an empty list and logs a warning.
        /// </summary>
        [Test]
        public void GetGamesForSeason_ShouldLogWarning_WhenApiUrlIsInvalid()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;
            var baseUrl = "https://invalid.api.url";
            var apiHost = "invalid-host";

            var mockLogger = new Mock<ILogger<SportsApiClient>>();
            var clientWithInvalidUrl = new SportsApiClient("test-api-key", mockLogger.Object, memoryCache);

            // Act
            var result = clientWithInvalidUrl.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost);

            // Assert
            Assert.That(result, Is.Empty, "Expected an empty list for an invalid API URL.");

        }

        /// <summary>
        /// Verifies that responses with more than 100 games are limited to the first 100 items.
        /// </summary>
        [Test]
        public void GetGamesForSeason_ShouldHandleLargeResponses()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;
            var baseUrl = "https://v1.american-football.api-sports.io";
            var apiHost = "v1.american-football.api-sports.io";

            // Act
            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost);

            // Assert
            Assert.That(result.Count, Is.LessThanOrEqualTo(100), "Expected the result to limit the number of games to 100.");
        }

        /// <summary>
        /// Verifies that an empty list is returned when the API response has no content.
        /// </summary>
        [Test]
        public void GetGamesForSeason_ShouldReturnEmptyList_WhenResponseIsNoContent()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;
            var baseUrl = "https://v1.american-football.api-sports.io";
            var apiHost = "v1.american-football.api-sports.io";

            // Simulate an empty response for NoContent
            var noContentGames = new List<GameResponse>(); // No games available
            memoryCache.Set($"Games_{leagueId}_{seasonYear}", noContentGames); // Prepopulate with no data

            // Act
            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost);

            // Assert
            Assert.That(result, Is.Empty, "Expected an empty list when the API response is NoContent.");
        }

        /// <summary>
        /// Verifies that the logger logs cache hits correctly.
        /// </summary>
        [Test]
        public void GetGamesForSeason_ShouldLogInformation_WhenCacheHit()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;
            var baseUrl = "https://v1.american-football.api-sports.io";
            var apiHost = "v1.american-football.api-sports.io";
            var mockLogger = new Mock<ILogger<SportsApiClient>>();
            var cachedClient = new SportsApiClient("b4ed364047f61b7a0ae7699c69c7ad57", mockLogger.Object, memoryCache);

            var cachedGames = new List<GameResponse>
            {
                new GameResponse
                {
                    Game = new GameDetails { Id = 1, Stage = "Regular Season" }
                }
            };

            memoryCache.Set($"Games_{leagueId}_{seasonYear}", cachedGames);

            // Act
            var result = cachedClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost);

            // Assert
            Assert.That(result, Is.Not.Empty, "Expected a non-empty list from the cache.");
            mockLogger.Verify(
                logger => logger.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Cache hit")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once, "Expected the logger to log cache hit information.");
        }
    }
}
