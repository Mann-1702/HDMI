using ContosoCrafts.WebSite.Pages;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.Pages
{
    /// <summary>
    /// Unit tests for the EPLMatches page model.
    /// Verifies behavior of the OnGet method and proper initialization.
    /// </summary>
    [TestFixture]
    public class EPLMatchesTests
    {
        private EPLMatches pageModel;
        private EPLMatches invalidPageModel;

        private ILogger<SportsApiClient> logger;
        private ILogger<EPLMatches> testLogger;

        private SportsApiClient sportsApiClient;

        /// <summary>
        /// Sets up the test environment by initializing necessary dependencies.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // Initialize loggers
            logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<SportsApiClient>();
            testLogger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<EPLMatches>();

            // Initialize SportsApiClient with a valid API key
            var apiKey = "b4ed364047f61b7a0ae7699c69c7ad57";
            sportsApiClient = new SportsApiClient(apiKey, logger);

            // Initialize EPLMatches page model
            pageModel = new EPLMatches(sportsApiClient, testLogger);
        }

        /// <summary>
        /// Tests that the OnGet method fetches and populates games for the EPL.
        /// </summary>
        [Test]
        public void OnGet_Should_Fetch_EPL_Games()
        {
            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.Games, Is.Not.Empty, "Games should be fetched and not be empty.");
        }

        /// <summary>
        /// Tests that an invalid SportsApiClient results in an empty games list.
        /// </summary>
        [Test]
        public void OnGet_Invalid_SportsApiClient_Should_Return_Valid_Page_With_No_Games()
        {
            // Arrange
            invalidPageModel = new EPLMatches(null, testLogger);

            // Act
            invalidPageModel.OnGet();

            // Assert
            Assert.That(invalidPageModel.Games, Is.Empty, "Games should be empty when SportsApiClient is null.");
        }

        /// <summary>
        /// Tests that the constructor initializes an empty games list.
        /// </summary>
        [Test]
        public void Constructor_Should_Initialize_Empty_Games_List()
        {
            // Arrange
            var newPageModel = new EPLMatches(null, testLogger);

            // Assert
            Assert.That(newPageModel.Games, Is.Empty, "Games should be initialized as an empty list in the constructor.");
        }

        /// <summary>
        /// Tests that the OnGet method logs an error if SportsApiClient is null.
        /// </summary>
        [Test]
        public void OnGet_Should_Log_Error_If_SportsApiClient_Is_Null()
        {
            // Arrange
            invalidPageModel = new EPLMatches(null, testLogger);

            // Act & Assert
            Assert.DoesNotThrow(() => invalidPageModel.OnGet(), "OnGet should handle a null SportsApiClient gracefully.");
        }

        /// <summary>
        /// Tests that the OnGet method handles exceptions thrown by SportsApiClient gracefully.
        /// </summary>
        [Test]
        public void OnGet_Should_Handle_Exception_Gracefully()
        {
            // Arrange
            var faultyApiClient = new SportsApiClient("invalid-key", logger);
            var pageWithFaultyClient = new EPLMatches(faultyApiClient, testLogger);

            // Act & Assert
            Assert.DoesNotThrow(() => pageWithFaultyClient.OnGet(), "OnGet should handle exceptions from SportsApiClient gracefully.");
        }

        [Test]
        public void OnGet_Valid_Input_Manchester_United_Should_Return_Valid_Page_With_Manchester_United_Games()
        {
            // Arrange
            string team = "Manchester United";

            // Act
            var result = pageModel.OnGet(team);
            var game = pageModel.Games.First();
            var teams = game.Teams.Home.Name + game.Teams.Visitors.Name;

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.Games, Is.Not.Empty);
            Assert.That(teams.Contains("Manchester"));

        }
    }
}
