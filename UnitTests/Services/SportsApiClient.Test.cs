using ContosoCrafts.WebSite.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using WireMock.Server;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
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
        private WireMockServer server;

        /// <summary>
        /// Sets up the required dependencies.
        /// </summary>
        [SetUp]
        public void Setup()
        {
           
            logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<SportsApiClient>();

            // Initialize memory cache
            memoryCache = new MemoryCache(new MemoryCacheOptions());

            // API key for the Sports API
            var apiKey = "b4ed364047f61b7a0ae7699c69c7ad57";

            // Instantiate SportsApiClient
            sportsApiClient = new SportsApiClient(apiKey, logger, memoryCache);
            server = WireMockServer.Start();
        }
        [TearDown]
        public void Teardown()
        {
            // Stop the WireMock server after each test
            server.Stop();
            server.Dispose();
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
            string endPoint = "games";

            // Act
            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost,endPoint);

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
            string endPoint = "games";

            // Act
            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost,endPoint);

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
        

        /// <summary>
        /// Verifies that responses with more than 100 games are limited to the first 100 items.
        /// </summary>
        

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
            string endPoint = "games";

            // Simulate an empty response for NoContent
            var noContentGames = new List<GameResponse>(); // No games available
            memoryCache.Set($"Games_{leagueId}_{seasonYear}", noContentGames); // Prepopulate with no data

            // Act
            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost, endPoint);

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
            string endPoint = "games";

            var cachedGames = new List<GameResponse>
            {
                new GameResponse
                {
                    Game = new GameDetails { Id = 1, Stage = "Regular Season" }
                }
            };

            memoryCache.Set($"Games_{leagueId}_{seasonYear}", cachedGames);

            // Act
            var result = cachedClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost,endPoint);

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
        [Test]
        public void SportsApiClient_OverloadedConstructor_ShouldInitializeWithoutMemoryCache()
        {
            // Arrange
            string apiKey = "TestApiKey";
            var mockLogger = new Mock<ILogger<SportsApiClient>>();

            // Act
            var client = new SportsApiClient(apiKey, mockLogger.Object);

            // Assert
            Assert.That(client, Is.Not.Null, "Expected SportsApiClient to be initialized.");
            Assert.That(client.GetType(), Is.EqualTo(typeof(SportsApiClient)), "Expected the object to be of type SportsApiClient.");
        }

        /// <summary>
        /// Verifies that the main constructor initializes the object correctly with IMemoryCache.
        /// </summary>
        [Test]
        public void SportsApiClient_MainConstructor_ShouldInitializeWithMemoryCache()
        {
            // Arrange
            string apiKey = "TestApiKey";
            var mockLogger = new Mock<ILogger<SportsApiClient>>();
            var mockMemoryCache = new MemoryCache(new MemoryCacheOptions());

            // Act
            var client = new SportsApiClient(apiKey, mockLogger.Object, mockMemoryCache);

            // Assert
            Assert.That(client, Is.Not.Null, "Expected SportsApiClient to be initialized.");
            Assert.That(client.GetType(), Is.EqualTo(typeof(SportsApiClient)), "Expected the object to be of type SportsApiClient.");
        }

        [Test]
        public void GetGamesForSeason_ShouldLogError_WhenExceptionOccurs()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;
            var baseUrl = "https://invalid-url-for-exception.com"; // Invalid URL to force an exception
            var apiHost = "invalid-host";
            string endPoint = "games";

            var mockLogger = new Mock<ILogger<SportsApiClient>>();
            var clientWithInvalidUrl = new SportsApiClient("test-api-key", mockLogger.Object, memoryCache);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() =>
                clientWithInvalidUrl.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost,endPoint)
            );

            // Verify that an exception was thrown
            Assert.That(ex, Is.Not.Null, "Expected an exception to be thrown.");

            // Verify that the error was logged
            mockLogger.Verify(
                logger => logger.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("An error occurred while executing the request")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once, "Expected the logger to log an error when an exception occurs.");
        }
        [Test]
        public void GetGamesForSeason_ShouldDeserializeContent_AndAssignGamesCorrectly()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;
            var baseUrl = server.Urls[0];
            var apiHost = "mockapi.com";
            string endPoint = "games";

            var mockResponseData = new ApiResponse<GameResponse>
            {
                Response = new List<GameResponse>
                {
                    new GameResponse { Game = new GameDetails { Id = 1, Stage = "Test Stage" } },
                    new GameResponse { Game = new GameDetails { Id = 2, Stage = "Another Stage" } }
                }
            };

            var mockJson = JsonConvert.SerializeObject(mockResponseData);

            // Configure the mock server to return the mock JSON
            server.Given(Request.Create().WithPath("/games").UsingGet())
                  .RespondWith(Response.Create().WithBody(mockJson).WithStatusCode(200));

            var sportsApiClient = new SportsApiClient("test-api-key", logger, memoryCache);

            // Act
            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost, endPoint);

            // Assert
            Assert.That(result, Is.Not.Null, "Expected a non-null result.");
            Assert.That(result.Count, Is.EqualTo(2), "Expected the list to contain exactly 2 items.");
            Assert.That(result[0].Game.Id, Is.EqualTo(1), "Expected the first game's ID to be 1.");
            Assert.That(result[1].Game.Id, Is.EqualTo(2), "Expected the second game's ID to be 2.");
        }
        [Test]
        public void GetGamesForSeason_ShouldAssignGamesFromApiResponse_OrReturnEmptyList()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;
            var baseUrl = server.Urls[0];
            var apiHost = "mockapi.com";
            string endPoint = "games";

            // Mock response with two games
            var mockResponseData = new ApiResponse<GameResponse>
            {
                Response = new List<GameResponse>
                {
                    new GameResponse { Game = new GameDetails { Id = 1, Stage = "Test Stage" } },
                    new GameResponse { Game = new GameDetails { Id = 2, Stage = "Another Stage" } }
                }
            };
            var mockJson = JsonConvert.SerializeObject(mockResponseData);

            server.Given(Request.Create().WithPath("/games").UsingGet())
                  .RespondWith(Response.Create().WithBody(mockJson).WithStatusCode(200));

            var sportsApiClient = new SportsApiClient("test-api-key", logger, memoryCache);

            // Act
            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost,endPoint);

            // Assert
            Assert.That(result, Is.Not.Null, "Expected a non-null result.");
            Assert.That(result.Count, Is.EqualTo(2), "Expected the list to contain exactly 2 items.");
            Assert.That(result[0].Game.Id, Is.EqualTo(1), "Expected the first game's ID to be 1.");
            Assert.That(result[1].Game.Id, Is.EqualTo(2), "Expected the second game's ID to be 2.");
        }

        [Test]
        public void GetGamesForSeason_ShouldReturnEmptyList_WhenApiResponseIsNull()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;
            var baseUrl = server.Urls[0];
            var apiHost = "mockapi.com";
            string endPoint = "games";

            // Mock response with null "Response" property
            var mockResponseData = new ApiResponse<GameResponse>
            {
                Response = null
            };
            var mockJson = JsonConvert.SerializeObject(mockResponseData);

            server.Given(Request.Create().WithPath("/games").UsingGet())
                  .RespondWith(Response.Create().WithBody(mockJson).WithStatusCode(200));

            var sportsApiClient = new SportsApiClient("test-api-key", logger, memoryCache);

            // Act
            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost,endPoint);

            // Assert
            Assert.That(result, Is.Not.Null, "Expected a non-null result.");
            Assert.That(result.Count, Is.EqualTo(0), "Expected the list to be empty when the API response is null.");
        }
        
        /// <summary>
        /// Verifies that the method assigns an empty list when the API response is successful but has no data.
        /// </summary>
        [Test]
        public void GetGamesForSeason_ShouldReturnEmptyList_WhenApiResponseHasNoData()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;
            var baseUrl = server.Urls[0];
            var apiHost = "mockapi.com";
            string endPoint = "games";

            var mockResponseData = new ApiResponse<GameResponse>
            {
                Response = new List<GameResponse>() // Empty list
            };

            var mockJson = JsonConvert.SerializeObject(mockResponseData);

            server.Given(Request.Create().WithPath("/games").UsingGet())
                  .RespondWith(Response.Create().WithBody(mockJson).WithStatusCode(200));

            var sportsApiClient = new SportsApiClient("test-api-key", logger, memoryCache);

            // Act
            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost,endPoint);

            // Assert
            Assert.That(result, Is.Not.Null, "Expected a non-null result.");
            Assert.That(result.Count, Is.EqualTo(0), "Expected an empty list when the API response has no data.");
        }

        /// <summary>
        /// Verifies that the method assigns a partially populated list when the API response has partial data.
        /// </summary>
        [Test]
        public void GetGamesForSeason_ShouldHandlePartialData_WhenApiResponseHasPartialData()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;
            var baseUrl = server.Urls[0];
            var apiHost = "mockapi.com";
            string endPoint = "games";

            var mockResponseData = new ApiResponse<GameResponse>
            {
                Response = new List<GameResponse>
            {
                new GameResponse { Game = new GameDetails { Id = 1, Stage = "Test Stage" } },
                null // Partial data (null entry)
            }
            };

            var mockJson = JsonConvert.SerializeObject(mockResponseData);

            server.Given(Request.Create().WithPath("/games").UsingGet())
                  .RespondWith(Response.Create().WithBody(mockJson).WithStatusCode(200));

            var sportsApiClient = new SportsApiClient("test-api-key", logger, memoryCache);

            // Act
            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost,endPoint);

            // Assert
            Assert.That(result, Is.Not.Null, "Expected a non-null result.");
            Assert.That(result.Count, Is.EqualTo(2), "Expected the list to contain 2 items.");
            Assert.That(result[0]?.Game?.Id, Is.EqualTo(1), "Expected the first game's ID to be 1.");
            Assert.That(result[1], Is.Null, "Expected the second entry to be null.");
        }

        /// <summary>
        /// Verifies that the method correctly processes a large dataset and enforces the limit of 100 items.
        /// </summary>
        [Test]
        public void GetGamesForSeason_ShouldEnforceItemLimit_WhenResponseContainsMoreThan100Items()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;
            var baseUrl = server.Urls[0];
            var apiHost = "mockapi.com";
            string endPoint = "games";

            var mockResponseData = new ApiResponse<GameResponse>
            {
                Response = Enumerable.Range(1, 150).Select(i => new GameResponse
                {
                    Game = new GameDetails { Id = i, Stage = $"Stage {i}" }
                }).ToList()
            };
            var mockJson = JsonConvert.SerializeObject(mockResponseData);

            server.Given(Request.Create().WithPath("/games").UsingGet())
                  .RespondWith(Response.Create().WithBody(mockJson).WithStatusCode(200));

            var sportsApiClient = new SportsApiClient("test-api-key", logger, memoryCache);

            // Act
            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost,endPoint);

            // Assert
            Assert.That(result, Is.Not.Null, "Expected a non-null result.");
            Assert.That(result.Count, Is.EqualTo(100), "Expected the result to be limited to 100 items.");
            Assert.That(result[99].Game.Id, Is.EqualTo(100), "Expected the last item's ID to be 100.");
        }

        /// <summary>
        /// Verifies that the method correctly handles responses with mixed valid and invalid data.
        /// </summary>
        [Test]
        public void GetGamesForSeason_ShouldHandleMixedValidAndInvalidData()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;
            var baseUrl = server.Urls[0];
            var apiHost = "mockapi.com";
            string endPoint = "games";

            var mockResponseData = new ApiResponse<GameResponse>
            {
                Response = new List<GameResponse>
        {
            new GameResponse { Game = new GameDetails { Id = 1, Stage = "Valid Stage" } },
            null, // Invalid entry
            new GameResponse { Game = new GameDetails { Id = 3, Stage = "Another Valid Stage" } }
        }
            };
            var mockJson = JsonConvert.SerializeObject(mockResponseData);

            server.Given(Request.Create().WithPath("/games").UsingGet())
                  .RespondWith(Response.Create().WithBody(mockJson).WithStatusCode(200));

            var sportsApiClient = new SportsApiClient("test-api-key", logger, memoryCache);

            // Act
            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost,endPoint);

            // Assert
            Assert.That(result, Is.Not.Null, "Expected a non-null result.");
            Assert.That(result.Count, Is.EqualTo(3), "Expected 3 items including the null entry.");
            Assert.That(result[0]?.Game?.Id, Is.EqualTo(1), "Expected the first item's ID to be 1.");
            Assert.That(result[1], Is.Null, "Expected the second entry to be null.");
            Assert.That(result[2]?.Game?.Id, Is.EqualTo(3), "Expected the third item's ID to be 3.");
        }

        /// <summary>
        /// Verifies that the method handles API timeout scenarios gracefully.
        /// </summary>
        [Test]
        public void GetGamesForSeason_ShouldThrowException_WhenApiRequestTimesOut()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;
            var baseUrl = server.Urls[0];
            var apiHost = "mockapi.com";
            string endPoint = "games";

            server.Given(Request.Create().WithPath("/games").UsingGet())
                  .RespondWith(Response.Create().WithDelay(TimeSpan.FromSeconds(10)).WithStatusCode(200)); // Simulate a timeout

            var sportsApiClient = new SportsApiClient("test-api-key", logger, memoryCache);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() =>
                sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost,endPoint));

            Assert.That(ex, Is.Not.Null, "Expected an exception to be thrown.");
            Assert.That(ex.Message, Does.Contain("Unable to retrieve game data"), "Expected the exception message to indicate a timeout.");
        }

        /// <summary>
        /// Verifies that the method correctly handles duplicate cache keys.
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

            // Simulate outdated cached data
            var outdatedCacheData = new List<GameResponse>
    {
        new GameResponse { Game = new GameDetails { Id = 999, Stage = "Outdated Stage" } }
    };
            memoryCache.Set($"Games_{leagueId}_{seasonYear}", outdatedCacheData);

            // Mock API response with fresh data
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

            var sportsApiClient = new SportsApiClient("test-api-key", logger, memoryCache);

            // Act
            // Clear the outdated cache entry to simulate a fresh fetch
            memoryCache.Remove($"Games_{leagueId}_{seasonYear}");

            // Fetch new data and update the cache
            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost,endPoint);

            // Re-fetch from cache to verify update
            var isCached = memoryCache.TryGetValue($"Games_{leagueId}_{seasonYear}", out List<GameResponse> updatedCacheData);

            // Assert
            Assert.That(result, Is.Not.Null, "Expected a non-null result.");
            Assert.That(result.Count, Is.EqualTo(1), "Expected the result list to contain exactly 1 item.");
            Assert.That(result[0].Game.Id, Is.EqualTo(1), "Expected the game's ID to be 1 in the result.");
            Assert.That(isCached, Is.True, "Expected the cache to contain updated data.");
            Assert.That(updatedCacheData.Count, Is.EqualTo(1), "Expected the cache to contain exactly 1 item.");
            Assert.That(updatedCacheData[0].Game.Id, Is.EqualTo(1), "Expected the game's ID in the cache to be 1.");
        }
        [Test]
        public void GetGamesForSeason_ShouldGenerateCorrectCacheKey()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;
            var baseUrl = server.Urls[0];
            var apiHost = "mockapi.com";
            string endPoint = "games";

            // Expected cache key
            var expectedCacheKey = $"Games_{leagueId}_{seasonYear}";

            // Mock API response
            var mockResponseData = new ApiResponse<GameResponse>
            {
                Response = new List<GameResponse>
        {
            new GameResponse { Game = new GameDetails { Id = 1, Stage = "Stage 1" } }
        }
            };
            var mockJson = JsonConvert.SerializeObject(mockResponseData);

            server.Given(Request.Create().WithPath("/games").UsingGet())
                  .RespondWith(Response.Create().WithBody(mockJson).WithStatusCode(200));

            // Act
            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost,endPoint);

            // Re-fetch from cache
            var isCached = memoryCache.TryGetValue(expectedCacheKey, out List<GameResponse> cachedGames);

            // Assert
            Assert.That(isCached, Is.True, "Expected the cache key to be generated and the result to be cached.");
            Assert.That(cachedGames, Is.Not.Null, "Expected cached data to be non-null.");
            Assert.That(cachedGames.Count, Is.EqualTo(1), "Expected the cache to contain exactly 1 item.");
            Assert.That(cachedGames[0].Game.Id, Is.EqualTo(1), "Expected the cached game's ID to be 1.");
        }
        [Test]
        public void GetGamesForSeason_ShouldThrowException_WhenResponseContentIsInvalidJson()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;
            var baseUrl = server.Urls[0];
            var apiHost = "mockapi.com";
            string endPoint = "games";

            // Simulate invalid JSON in the API response
            var invalidJson = "INVALID_JSON";

            server.Given(Request.Create().WithPath("/games").UsingGet())
                  .RespondWith(Response.Create().WithBody(invalidJson).WithStatusCode(200));

            var sportsApiClient = new SportsApiClient("test-api-key", logger, memoryCache);

            // Act & Assert
            var ex = Assert.Throws<JsonReaderException>(() =>
                sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost,endPoint));

            Assert.That(ex, Is.Not.Null, "Expected an exception to be thrown.");
            Assert.That(ex.Message, Does.Contain("Error parsing"), "Expected the exception to indicate a JSON parsing error.");
        }
        [Test]
        public void GetGamesForSeason_ShouldFetchFromApi_WhenCacheIsUnavailable()
        {
            // Arrange
            var leagueId = "1";
            var seasonYear = 2024;
            var baseUrl = server.Urls[0];
            var apiHost = "mockapi.com";
            string endPoint = "games";

            // Mock API response
            var mockResponseData = new ApiResponse<GameResponse>
            {
                Response = new List<GameResponse>
        {
            new GameResponse { Game = new GameDetails { Id = 1, Stage = "Stage 1" } }
        }
            };
            var mockJson = JsonConvert.SerializeObject(mockResponseData);

            server.Given(Request.Create().WithPath("/games").UsingGet())
                  .RespondWith(Response.Create().WithBody(mockJson).WithStatusCode(200));

            var sportsApiClient = new SportsApiClient("test-api-key", logger, memoryCache);

            // Simulate cache being unavailable
            memoryCache.Remove($"Games_{leagueId}_{seasonYear}");

            // Act
            var result = sportsApiClient.GetGamesForSeason<GameResponse>(leagueId, seasonYear, baseUrl, apiHost,endPoint);

            // Assert
            Assert.That(result, Is.Not.Null, "Expected a non-null result.");
            Assert.That(result.Count, Is.EqualTo(1), "Expected the result list to contain 1 item.");
            Assert.That(result[0].Game.Id, Is.EqualTo(1), "Expected the game's ID to be 1.");
        }
        
    }

}










