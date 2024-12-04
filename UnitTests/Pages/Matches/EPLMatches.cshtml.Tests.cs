using ContosoCrafts.WebSite.Pages.Matches;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.Pages.Matches
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

        #region TestSetup

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

        #endregion TestSetup

        #region OnGet Tests

        [Test]
        public void OnGet_Should_Fetch_EPL_Games()
        {
            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.Games, Is.Not.Empty, "Games should be fetched and not be empty.");
        }

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

        [Test]
        public void OnGet_Should_Handle_Null_TeamName_And_Return_All_Games()
        {
            // Act
            var result = pageModel.OnGet();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>(), "OnGet should return a valid PageResult.");
            Assert.That(pageModel.Games, Is.Not.Empty, "Games should not be empty when no team name is provided.");
        }

        [Test]
        public void OnGet_Valid_TeamName_Should_Return_Filtered_Games()
        {
            // Arrange
            string teamName = "Liverpool";

            // Act
            pageModel.OnGet(teamName);

            // Assert
            Assert.That(pageModel.Games, Is.Not.Empty, "Games should not be empty for a valid team name.");
            Assert.That(pageModel.Games.All(g => g.Teams.Home.Name == teamName || g.Teams.Visitors.Name == teamName), Is.True, "All games should involve the specified team.");
        }

        [Test]
        public void OnGet_Invalid_TeamName_Should_Return_Empty_Games_List()
        {
            // Arrange
            string teamName = "Invalid Team";

            // Act
            pageModel.OnGet(teamName);

            // Assert
            Assert.That(pageModel.Games, Is.Empty, "Games should be empty for an invalid team name.");
        }

        [Test]
        public void OnGet_Valid_Season_Should_Fetch_Correct_Season_Games()
        {
            // Arrange
            int year = 2023;

            // Act
            pageModel.OnGet(year: year);

            // Assert
            Assert.That(pageModel.seasonYear, Is.EqualTo(year), "The seasonYear should be set to the specified year.");
            Assert.That(pageModel.Games, Is.Not.Empty, "Games should not be empty for a valid season year.");
        }

        [Test]
        public void OnGet_Invalid_Season_Should_Handle_Gracefully()
        {
            // Arrange
            int year = 1800; // Invalid season year

            // Act
            var result = pageModel.OnGet(year: year);

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>(), "OnGet should return a PageResult for an invalid season year.");
            Assert.That(pageModel.Games, Is.Empty, "Games should be empty for an invalid season year.");
        }

        [Test]
        public void OnGet_Should_Handle_Exception_Gracefully()
        {
            // Arrange
            var faultyApiClient = new SportsApiClient("invalid-key", logger);
            var pageWithFaultyClient = new EPLMatches(faultyApiClient, testLogger);

            // Act & Assert
            Assert.DoesNotThrow(() => pageWithFaultyClient.OnGet(), "OnGet should handle exceptions from SportsApiClient gracefully.");
        }

        #endregion OnGet Tests

        #region Constructor Tests

        [Test]
        public void Constructor_Should_Initialize_Empty_Games_List()
        {
            // Arrange
            var newPageModel = new EPLMatches(null, testLogger);

            // Assert
            Assert.That(newPageModel.Games, Is.Empty, "Games should be initialized as an empty list in the constructor.");
        }

        [Test]
        public void OnGet_Should_Log_Error_If_SportsApiClient_Is_Null()
        {
            // Arrange
            invalidPageModel = new EPLMatches(null, testLogger);

            // Act & Assert
            Assert.DoesNotThrow(() => invalidPageModel.OnGet(), "OnGet should handle a null SportsApiClient gracefully.");
        }

        [Test]
        public void OnGet_Invalid_SportsApiClient_Should_Return_Valid_Page_With_No_Games1()
        {
            // Arrange
            invalidPageModel = new EPLMatches(null, testLogger);

            // Act
            invalidPageModel.OnGet();

            // Assert
            Assert.That(invalidPageModel.Games, Is.Empty, "Games should be empty when SportsApiClient is null.");
        }



        #endregion Constructor Tests
    }
}
