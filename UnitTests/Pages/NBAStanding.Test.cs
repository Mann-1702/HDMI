using ContosoCrafts.WebSite.Pages.Matches;
using ContosoCrafts.WebSite.Services;
using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ContosoCrafts.WebSite.Pages.Standings;

namespace UnitTests.Pages
{
    [TestFixture]
    public class NBAMatchesTests
    {
        private NBAStandingsModel pageModel;
        private NBAStandingsModel invalidPageModel;

        private ILogger<SportsApiClient> logger;
        private ILogger<NBAMatchesModel> testLogger;

        private SportsApiClient sportsApiClient;

        [SetUp]
        public void Setup()
        {

            // Initialize logger

            logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<SportsApiClient>();
            testLogger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<NBAMatchesModel>();


            var apiKey = "b4ed364047f61b7a0ae7699c69c7ad57";
            sportsApiClient = new SportsApiClient(apiKey, logger);


            // Instantiate NFLMatches
            pageModel = new NBAStandingsModel(sportsApiClient, testLogger);
        }

        [Test]
        public void OnGet_Default_Should_Fetch_2024_NBA_Standings()
        {
            // Arrange

            // Act
            var result = pageModel.OnGet();
            var team = pageModel.Games.First();

            // Assert
            // Assert
            Assert.That(pageModel.Games, Is.Not.Empty);
            Assert.That(result, Is.InstanceOf<PageResult>());
        }


        [Test]
        public void OnGet_Invalid_SportsApiClient_Should_Return_Valid_Page_With_No_Games()
        {
            // Arrange
            invalidPageModel = new NBAStandingsModel(null, testLogger);

            // Act
            var result = invalidPageModel.OnGet();

            // Assert
            Assert.That(invalidPageModel.Games, Is.Empty, "Games list should be empty when API client is invalid.");
            Assert.That(result, Is.InstanceOf<PageResult>(), "Should return a valid PageResult.");
        }

        [Test]
        public void OnGet_Valid_Input_Lakers_Should_Return_Valid_Page_With_Lakers_Standings()
        {
            // Arrange
            string team = "Boston Celtics";

            // Act
            var result = pageModel.OnGet(team);
            var game = pageModel.Games.First();
           
            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>(), "Should return a valid PageResult.");
            Assert.That(pageModel.Games, Is.Not.Empty, "Games list should not be empty for the specified team.");
            Assert.That(team.Contains("Boston Celtics"), "The result should contain standings for the Lakers.");
        }

        
    }
}
