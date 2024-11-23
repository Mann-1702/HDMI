using ContosoCrafts.WebSite.Models;
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
    public class NFLMatchesReadTests
    {
        private NFLReadModel pageModel;
        private NFLReadModel invalidPageModel;

        private ILogger<SportsApiClient> logger;
        private ILogger<NFLReadModel> testLogger;

        private SportsApiClient sportsApiClient;

        [SetUp]
        public void Setup()
        {

            // Initialize logger

            logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<SportsApiClient>();
            testLogger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<NFLReadModel>();

            var apiKey = "b4ed364047f61b7a0ae7699c69c7ad57";
            sportsApiClient = new SportsApiClient(apiKey, logger);


            // Instantiate NFLMatches
            pageModel = new NFLReadModel(sportsApiClient, testLogger);
        }

        [Test]
        public void Games_GetSet_ValidInput_Should_Store_Valid_GameResponse_Data()
        {

            // Prepare mock data
            var mockGame = new GameResponse
            {
                Game = new GameDetails { Id = 1, Stage = "Regular Season", Week = "1" },
                Teams = new Teams
                {
                    Home = new Team { Id = 101, Name = "Team A" },
                    Away = new Team { Id = 102, Name = "Team B" }
                },
                Scores = new Scores
                {
                    Home = new ScoreDetails { Total = 21 },
                    Away = new ScoreDetails { Total = 14 }
                }
            };

            // Attempt to find the `Games` property
            var matchProperty = typeof(NFLReadModel).GetProperty("Match", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            // Set the value
            matchProperty.SetValue(pageModel, mockGame);



            // Assert that Games has been set correctly
            Assert.That(pageModel.Match, Is.Not.Null);
            Assert.That(pageModel.Match.Teams.Home.Name, Is.EqualTo("Team A"));
            Assert.That(pageModel.Match.Teams.Away.Name, Is.EqualTo("Team B"));
        }


        [Test]
        public void OnGet_Valid_gameId_Should_Fetch_Specific_2024_NFL_Game()
        {
            // Arrange
            string nflLeagueId = "1";
            int seasonYear = 2024;
            string baseUrl = "https://v1.american-football.api-sports.io";
            string baseHost = "v1.american-football.api-sports.io";

            // Act
            var matches = sportsApiClient.GetGamesForSeason<GameResponse>(nflLeagueId, seasonYear, baseUrl, baseHost);
            var match = matches.FirstOrDefault();
            var matchId = match.Game.Id.ToString();

            var result = pageModel.OnGet(matchId);

            // Assert
            Assert.That(pageModel.Match, Is.Not.Null); // Ensure Match is not null
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
            string nflLeagueId = "1";
            int seasonYear = 2024;
            string baseUrl = "https://v1.american-football.api-sports.io";
            string baseHost = "v1.american-football.api-sports.io";

            // OBtain a matchId from a match that contains an overtime score
            var matches = sportsApiClient.GetGamesForSeason<GameResponse>(nflLeagueId, seasonYear, baseUrl, baseHost);
            var match = matches.FirstOrDefault(m => m.Scores.Home.Overtime > 0 || m.Scores.Away.Overtime > 0);
            var matchId = match.Game.Id.ToString();

            // Act
            var result = pageModel.OnGet(matchId);

            // Assert: Verify HasOvertime is set correctly
            Assert.That(pageModel.HasOvertime, Is.True);
        }

        [Test]
        public void OnGet_Valid_gameId_WithOut_Overtime_Should_Set_HasOvertime_To_False()
        {
            // Arrange
            string nflLeagueId = "1";
            int seasonYear = 2024;
            string baseUrl = "https://v1.american-football.api-sports.io";
            string baseHost = "v1.american-football.api-sports.io";

            // matchId from Patriots vs Panthers 8-8-24. No overtime
            var matchId = "13147";

            // Act
            var result = pageModel.OnGet(matchId);

            // Assert: Verify HasOvertime is set correctly
            Assert.That(pageModel.HasOvertime, Is.False);
        }

        [Test]
        public void OnGet_Invalid_SportsApiClient_Should_Return_Valid_Page_With_No_Games()
        {
            // Arrange
            invalidPageModel = new NFLReadModel(null, testLogger);

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