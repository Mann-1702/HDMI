using ContosoCrafts.WebSite.Pages.Matches;
using ContosoCrafts.WebSite.Pages.Standings;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Pages
{
    [TestFixture]
    public class NFLMatchesTests
    {
        private NFLStandingModel pageModel;
        private NFLStandingModel invalidPageModel;

        private ILogger<SportsApiClient> logger;
        private ILogger<NFLMatchesModel> testLogger;

        private SportsApiClient sportsApiClient;

        [SetUp]
        public void Setup()
        {
            // Initialize logger
            logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<SportsApiClient>();
            testLogger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<NFLMatchesModel>();

            // Initialize SportsApiClient
            var apiKey = "b4ed364047f61b7a0ae7699c69c7ad57";
            sportsApiClient = new SportsApiClient(apiKey, logger);

            // Instantiate NFLStandingModel
            pageModel = new NFLStandingModel(sportsApiClient, testLogger);
            invalidPageModel = new NFLStandingModel(null, testLogger);
        }

        [Test]
        public void OnGet_Default_Should_Fetch_2024_NFL_Standings()
        {
            // Act
            var result = pageModel.OnGet();
            var team = pageModel.Games.First();

            // Assert
            Assert.That(pageModel.Games, Is.Not.Empty, "Games list should not be empty for the default year.");
            Assert.That(team.Position, Is.GreaterThan(0), "The team position should be greater than 0.");
            Assert.That(result, Is.InstanceOf<PageResult>(), "Should return a valid PageResult.");
        }

        [Test]
        public void OnGet_Valid_Input_Year_2023_Should_Fetch_2023_NFL_Standings()
        {
            // Act
            var result = pageModel.OnGet(year: 2023);
            var team = pageModel.Games.First();

            // Assert
            Assert.That(pageModel.Games, Is.Not.Empty, "Games list should not be empty for the year 2023.");
            Assert.That(team.Won, Is.GreaterThan(0), "The number of games won should be greater than 0.");
            Assert.That(result, Is.InstanceOf<PageResult>(), "Should return a valid PageResult.");
        }

        [Test]
        public void OnGet_Invalid_SportsApiClient_Should_Return_Valid_Page_With_No_Games()
        {
            // Act
            var result = invalidPageModel.OnGet();

            // Assert
            Assert.That(invalidPageModel.Games, Is.Empty, "Games list should be empty when API client is invalid.");
            Assert.That(result, Is.InstanceOf<PageResult>(), "Should return a valid PageResult.");
        }

        [Test]
        public void OnGet_Valid_Input_TeamName_Should_Filter_Results()
        {
            // Arrange
            string teamName = "Buffalo Bills";

            // Act
            var result = pageModel.OnGet(teamName);
            var team = pageModel.Games.FirstOrDefault();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>(), "Should return a valid PageResult.");
            Assert.That(pageModel.Games, Is.Not.Empty, "Games list should not be empty for the specified team.");
            Assert.That(team?.Team1.Name, Is.EqualTo(teamName), "The result should contain standings for the specified team.");
        }

       
        [Test]
        public void OnGet_ApiThrowsException_Should_Return_Empty_Standings()
        {
            // Act
            var result = invalidPageModel.OnGet();

            // Assert
            Assert.That(invalidPageModel.Games, Is.Empty, "Games list should be empty when an exception occurs.");
            Assert.That(result, Is.InstanceOf<PageResult>(), "Should return a valid PageResult even on an exception.");
        }

        [Test]
        public void OnGet_Valid_Input_Year_2022_Should_Fetch_2022_NFL_Standings()
        {
            // Act
            var result = pageModel.OnGet(year: 2022);
            var team = pageModel.Games.First();

            // Assert
            Assert.That(pageModel.Games, Is.Not.Empty, "Games list should not be empty for the year 2022.");
            Assert.That(team.Won, Is.GreaterThan(0), "The number of games won should be greater than 0.");
            Assert.That(result, Is.InstanceOf<PageResult>(), "Should return a valid PageResult.");
        }
    }
}
