using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages;
using ContosoCrafts.WebSite.Pages.Matches;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnitTests.Pages.Matches
{
    [TestFixture]
    public class EPLMatchReadTests
    {
        private EPLMatchReadModel pageModel;
        private EPLMatchReadModel invalidPageModel;

        private ILogger<SportsApiClient> logger;
        private ILogger<EPLMatchReadModel> testLogger;

        private SportsApiClient sportsApiClient;

        [SetUp]
        public void Setup()
        {

            // Initialize logger

            logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<SportsApiClient>();
            testLogger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<EPLMatchReadModel>();

            var apiKey = "b4ed364047f61b7a0ae7699c69c7ad57";
            sportsApiClient = new SportsApiClient(apiKey, logger);


            // Instantiate NFLMatches
            pageModel = new EPLMatchReadModel(sportsApiClient, testLogger);
        }


        [Test]
        public void OnGet_Valid_gameId_Should_Fetch_Specific_2024_NFL_Game()
        {
            // Arrange

            // EPL League ID = 39
            string leagueId = "39";
            int seasonYear = 2024;
            string baseUrl = "https://v3.football.api-sports.io";
            string apiHost = "v3.football.api-sports.io";
            string endPoint = "fixtures";

            // Act
            var matches = sportsApiClient.GetGamesForSeason<FixtureResponse>(leagueId, seasonYear, baseUrl, apiHost,endPoint);
            var match = matches.LastOrDefault();
            var matchId = match.Fixture.FixtureId;

            var result = pageModel.OnGet(matchId);

            // Assert
            Assert.That(pageModel.Match, Is.Not.Null); // Ensure Match is not null
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        [Test]
        public void OnGet_Invalid_GameId_Should_Return_EPLMatches_Page()
        {
            // Arrange
            int invalidGameId = -12344;

            // Act
            var result = pageModel.OnGet(invalidGameId);

            // Assert
            Assert.That(pageModel.Match, Is.Null);
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());

        }


        [Test]
        public void OnGet_Invalid_SportsApiClient_Should_Return_Valid_Page_With_No_Games()
        {
            // Arrange
            invalidPageModel = new EPLMatchReadModel(null, testLogger);

            // Act

            // Invalid gameId will not give an error in the try/catch function.
            // Invalid sportsApiClient will break out of the try allowing us to test for the catch
            var result = invalidPageModel.OnGet(123);

            // Assert
            Assert.That(invalidPageModel.Match, Is.Null);
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());

        }

    }

}