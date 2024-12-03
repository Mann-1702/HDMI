using ContosoCrafts.WebSite.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using WireMock.Server;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using System.Net.Http;
using Moq;

namespace UnitTests.Services
{
    /// <summary>
    /// Unit tests for the SportsApiClient class.
    /// Verifies the behavior of API interactions, caching, and error handling.
    /// </summary>
    [TestFixture]
    public class SportsApiClientTests
    {
        private ILogger<SportsApiClient> logger;
        private IMemoryCache memoryCache;
        private SportsApiClient sportsApiClient;
        private WireMockServer server;

        /// <summary>
        /// Sets up the required dependencies before each test.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // Initialize the logger for SportsApiClient
            logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<SportsApiClient>();

            // Initialize in-memory cache
            memoryCache = new MemoryCache(new MemoryCacheOptions());

            // API key for accessing the sports API
            var apiKey = "b4ed364047f61b7a0ae7699c69c7ad57";

            // Instantiate the SportsApiClient with its dependencies
            sportsApiClient = new SportsApiClient(apiKey, logger, memoryCache);

            // Start the WireMock server for simulating API responses
            server = WireMockServer.Start();
        }

        /// <summary>
        /// Tears down the WireMock server after each test.
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            // Stop and dispose of the WireMock server
            server.Stop();
            server.Dispose();
        }

        /// <summary>
        /// Test to verify that an empty list is returned when no games are available from the API.
        /// </summary>
        [Test]
        public void GetGamesForSeason_ShouldReturnEmptyList_WhenNoGamesAvailable()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2025;
            var baseUrl = "https://v1.american-football.api-sports.io";
            var apiHost = "v1.american-football.api-sports.io";
            string endPoint = "games";

            // Act
            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost, endPoint);

            // Assert
            Assert.That(result, Is.Empty, "Expected an empty list when no games are available.");
        }

        /// <summary>
        /// Test to ensure that successful API responses are cached properly.
        /// </summary>
        [Test]
        public void GetGamesForSeason_ShouldCacheResults_WhenResponseIsSuccessful()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;
            var baseUrl = "https://v1.american-football.api-sports.io";
            var apiHost = "v1.american-football.api-sports.io";
            string endPoint = "games";

            // Act
            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost, endPoint);

            // Re-fetch from cache
            var isCached = memoryCache.TryGetValue($"Games_{leagueId}_{seasonYear}", out List<GameResponse> cachedGames);

            // Assert
            Assert.That(isCached, Is.True, "Expected the result to be cached.");
            Assert.That(cachedGames, Is.Not.Null, "Expected cached games to be non-null.");
            Assert.That(cachedGames, Is.EqualTo(result), "Cached games should match the original API response.");
        }

        /// <summary>
        /// Test to ensure that an empty list is returned when the API response has no content.
        /// </summary>
        [Test]
        public void GetGamesForSeason_ShouldReturnEmptyList_WhenResponseIsNoContent()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;
            var baseUrl = "https://v1.american-football.api-sports.io";
            var apiHost = "v1.american-football.api-sports.io";
            string endPoint = "games";

            // Simulate an empty response from the API
            var noContentGames = new List<GameResponse>();
            memoryCache.Set($"Games_{leagueId}_{seasonYear}", noContentGames);

            // Act
            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost, endPoint);

            // Assert
            Assert.That(result, Is.Empty, "Expected an empty list when the API response has no content.");
        }

        /// <summary>
        /// Test to ensure the SportsApiClient handles API timeout scenarios gracefully.
        /// </summary>
        [Test]
        public void GetGamesForSeason_ApiRequestTimesOut_Should_Return_Null()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;

            // WireMock server URL
            var baseUrl = server.Urls[0];

            var apiHost = "mockapi.com";
            string endPoint = "games";

            // Simulate an API timeout by delaying the response beyond the default client timeout
            server.Given(Request.Create().WithPath("/games").UsingGet())
                  .RespondWith(Response.Create()
                      .WithDelay(TimeSpan.FromSeconds(10))
                      .WithStatusCode(200));

            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost, endPoint);

            // Assert
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Test to verify that duplicate cache keys are handled gracefully.
        /// </summary>
        [Test]
        public void GetGamesForSeason_ShouldHandleDuplicateCacheKeysGracefully()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;
            var baseUrl = server.Urls[0];
            var apiHost = "mockapi.com";
            string endPoint = "games";

            // populate the cache with outdated data
            var outdatedCacheData = new List<GameResponse>
            {
                new GameResponse { Game = new GameDetails { Id = 999, Stage = "Outdated Stage" } }
            };
            memoryCache.Set($"Games_{leagueId}_{seasonYear}", outdatedCacheData);

            // Simulate a fresh API response
            var mockApiResponse = new ApiResponse<GameResponse>
            {
                Response = new List<GameResponse>
                {
                    new GameResponse { Game = new GameDetails { Id = 1, Stage = "Updated Stage" } }
                }
            };
            var mockJson = JsonConvert.SerializeObject(mockApiResponse);

            server.Given(Request.Create().WithPath("/games").UsingGet())
                  .RespondWith(Response.Create().WithBody(mockJson).WithStatusCode(200));

            // Act
            // Clear the outdated cache
            memoryCache.Remove($"Games_{leagueId}_{seasonYear}");
            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost, endPoint);

            // Assert
            Assert.That(result, Is.Not.Null, "Expected a non-null result.");
            Assert.That(result.Count, Is.EqualTo(1), "Expected the result list to contain exactly 1 item.");
        }
    }
}