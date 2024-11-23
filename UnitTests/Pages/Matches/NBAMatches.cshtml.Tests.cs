using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Matches;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnitTests.Pages.Matches
{
    [TestFixture]
    public class NBAMatchesTests
    {
        private NBAMatchesModel pageModel;
        private NBAMatchesModel invalidPageModel;

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
            pageModel = new NBAMatchesModel(sportsApiClient, testLogger);
        }



        [Test]
        public void OnGet_Defaut_Should_Fetch_2024_NBA_Games()
        {
            // Arrange

            // Act
            var result = pageModel.OnGet();
            var game = pageModel.Games.First();

            // Assert
            Assert.That(pageModel.Games, Is.Not.Empty);
            Assert.That(game.Date.Start.ToString().Contains("2024"));
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        [Test]
        public void OnGet_Valid_Input_Year_2023_Should_Fetch_2023_NBA_Games()
        {
            // Arrange

            // Act
            var result = pageModel.OnGet(year: 2023);
            var game = pageModel.Games.First();

            // Assert
            Assert.That(pageModel.Games, Is.Not.Empty);
            Assert.That(game.Date.Start.ToString().Contains("2023"));
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        [Test]
        public void OnGet_Valid_Input_Year_2022_Should_Fetch_2022_NBA_Games()
        {
            // Arrange

            // Act
            var result = pageModel.OnGet(year: 2022);
            var game = pageModel.Games.First();

            // Assert
            Assert.That(pageModel.Games, Is.Not.Empty);
            Assert.That(game.Date.Start.ToString().Contains("2022"));
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        [Test]
        public void OnGet_Invalid_SportsApiClient_Should_Return_Valid_Page_With_No_Games()
        {
            // Arrange
            invalidPageModel = new NBAMatchesModel(null, testLogger);

            // Act
            invalidPageModel.OnGet();

            // Assert
            Assert.That(invalidPageModel.Games, Is.Empty);
        }

        [Test]
        public void OnGet_Valid_Input_Celtics_Should_Return_Valid_Page_With_Celtics_Games()
        {
            // Arrange
            string team = "Boston Celtics";

            // Act
            var result = pageModel.OnGet(team);
            var game = pageModel.Games.First();
            var teams = game.Teams.Home.Name + game.Teams.Visitors.Name;

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.Games, Is.Not.Empty);
            Assert.That(teams.Contains("Boston"));

        }
    }

}