﻿using ContosoCrafts.WebSite.Models;
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
    public class NBAMatchReadTests
    {
        private NBAReadModel pageModel;
        private NBAReadModel invalidPageModel;

        private ILogger<SportsApiClient> logger;
        private ILogger<NBAReadModel> testLogger;

        private SportsApiClient sportsApiClient;

        [SetUp]
        public void Setup()
        {

            // Initialize logger

            logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<SportsApiClient>();
            testLogger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<NBAReadModel>();

            var apiKey = "b4ed364047f61b7a0ae7699c69c7ad57";
            sportsApiClient = new SportsApiClient(apiKey, logger);


            // Instantiate NFLMatches
            pageModel = new NBAReadModel(sportsApiClient, testLogger);
        }

        [Test]
        public void OnGet_Valid_GameId_Default_Year_Should_Fetch_Specific_2024_NBA_Game()
        {
            // Arrange
            string NBAleagueId = "standard";
            int seasonYear = 2024;
            string baseUrl = "https://v2.nba.api-sports.io";
            string baseHost = "v2.aenba.api-sports.io";
            string endPoint = "games";

            // Act
            var matches = sportsApiClient.GetGamesForSeason<NbaGameResponse>(NBAleagueId, seasonYear, baseUrl, baseHost, endPoint);
            var match = matches.FirstOrDefault();
            var matchId = match.Id.ToString();

            var result = pageModel.OnGet(matchId);

            // Assert
            Assert.That(pageModel.Match, Is.Not.Null);
            Assert.That(pageModel.Match.Id.ToString, Is.EqualTo(matchId));
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        [Test]
        public void OnGet_Valid_GameId_Year_2023_Should_Fetch_Specific_2023_NBA_Game()
        {
            // Arrange
            string NBAleagueId = "standard";
            int seasonYear = 2023;
            string baseUrl = "https://v2.nba.api-sports.io";
            string baseHost = "v2.aenba.api-sports.io";
            string endPoint = "games";

            // Act
            var matches = sportsApiClient.GetGamesForSeason<NbaGameResponse>(NBAleagueId, seasonYear, baseUrl, baseHost,endPoint);
            var match = matches.FirstOrDefault();
            var matchId = match.Id.ToString();

            var result = pageModel.OnGet(matchId, seasonYear);

            // Assert
            Assert.That(pageModel.Match, Is.Not.Null);
            Assert.That(pageModel.Match.Id.ToString, Is.EqualTo(matchId));
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        [Test]
        public void OnGet_Valid_GameId_Year_2022_Should_Fetch_Specific_2022_NBA_Game()
        {
            // Arrange
            string NBAleagueId = "standard";
            int seasonYear = 2022;
            string baseUrl = "https://v2.nba.api-sports.io";
            string baseHost = "v2.aenba.api-sports.io";
            string endPoint = "games";

            // Act
            var matches = sportsApiClient.GetGamesForSeason<NbaGameResponse>(NBAleagueId, seasonYear, baseUrl, baseHost,endPoint);
            var match = matches.FirstOrDefault();
            var matchId = match.Id.ToString();

            var result = pageModel.OnGet(matchId, seasonYear);

            // Assert
            Assert.That(pageModel.Match, Is.Not.Null);
            Assert.That(pageModel.Match.Id.ToString, Is.EqualTo(matchId));
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        [Test]
        public void OnGet_Invalid_GameId_Should_Return_NFLMatches_Page()
        {
            // Arrange
            string invalidGameId = "-12344";

            // Act
            var result = pageModel.OnGet(invalidGameId);

            // Assert
            Assert.That(pageModel.Match, Is.Null);
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());

        }


        [Test]
        public void OnGet_Valid_gameId_With_Overtime_Should_Set_HasOvertime_To_True()
        {
            // Arrange
            // Match with overtime: 1
            var overtimeMatchId = "14101";

            // Act
            var result = pageModel.OnGet(overtimeMatchId);

            // Assert: Verify HasOvertime is set correctly
            Assert.That(pageModel.HasOvertime, Is.True);
        }

        [Test]
        public void OnGet_Valid_gameId_WithOut_Overtime_Should_Set_HasOvertime_To_False()
        {
            // Arrange
            var noOvertimeMatchId = "14048";

            // Act
            var result = pageModel.OnGet(noOvertimeMatchId);

            // Assert: Verify HasOvertime is set correctly
            Assert.That(pageModel.HasOvertime, Is.False);
        }

        [Test]
        public void OnGet_Invalid_SportsApiClient_Should_Return_Valid_Page_With_No_Games()
        {
            // Arrange
            invalidPageModel = new NBAReadModel(null, testLogger);

            // Act

            // Invalid gameId will not give an error in the try/catch function.
            // Invalid sportsApiClient will break out of the try allowing us to test for the catch
            var result = invalidPageModel.OnGet("123");

            // Assert
            Assert.That(invalidPageModel.Match, Is.Null);
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());

        }

    }

}