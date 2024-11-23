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
    public class NFLMatchesTests
    {
        private NFLMatchesModel pageModel;
        private NFLMatchesModel invalidPageModel;

        private ILogger<SportsApiClient> logger;
        private ILogger<NFLMatchesModel> testLogger;

        private SportsApiClient sportsApiClient;

        [SetUp]
        public void Setup()
        {

            // Initialize logger

            logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<SportsApiClient>();
            testLogger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<NFLMatchesModel>();


            var apiKey = "b4ed364047f61b7a0ae7699c69c7ad57";
            sportsApiClient = new SportsApiClient(apiKey, logger);


            // Instantiate NFLMatches
            pageModel = new NFLMatchesModel(sportsApiClient, testLogger);
        }

        [Test]
        public void Games_ShouldBeSetWithMockData()
        {

            // Prepare mock data
            var mockGames = new List<GameResponse>
            {
                new GameResponse
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
                },
                new GameResponse
                {
                    Game = new GameDetails { Id = 2, Stage = "Regular Season", Week = "2" },
                    Teams = new Teams
                    {
                        Home = new Team { Id = 103, Name = "Team C" },
                        Away = new Team { Id = 104, Name = "Team D" }
                    },
                    Scores = new Scores
                    {
                        Home = new ScoreDetails { Total = 28 },
                        Away = new ScoreDetails { Total = 20 }
                    }
                }
            };

            // Attempt to find the `Games` property
            var gamesProperty = typeof(NFLMatchesModel).GetProperty("Games", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);


            // Set the value
            gamesProperty.SetValue(pageModel, mockGames);


            // Assert that Games has been set correctly
            Assert.That(pageModel.Games, Is.Not.Null);
            Assert.That(pageModel.Games.Count, Is.EqualTo(mockGames.Count));
            Assert.That(pageModel.Games[0].Teams.Home.Name, Is.EqualTo("Team A"));
            Assert.That(pageModel.Games[1].Teams.Home.Name, Is.EqualTo("Team C"));
        }


        [Test]
        public void OnGet_Defaut_Should_Fetch_2024_NFL_Games()
        {
            // Arrange

            // Act
            var result = pageModel.OnGet();
            var game = pageModel.Games.First();

            // Assert
            Assert.That(pageModel.Games, Is.Not.Empty);
            Assert.That(game.Game.Date.Date.Contains("2024"));
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        [Test]
        public void OnGet_Valid_Input_Year_2023_Should_Fetch_2023_NFL_Games()
        {
            // Arrange

            // Act
            var result = pageModel.OnGet(year: 2023);
            var game = pageModel.Games.First();

            // Assert
            Assert.That(pageModel.Games, Is.Not.Empty);
            Assert.That(game.Game.Date.Date.Contains("2023"));
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        [Test]
        public void OnGet_Valid_Input_Year_2023_Should_Fetch_2022_NFL_Games()
        {
            // Arrange

            // Act
            var result = pageModel.OnGet(year: 2022);
            var game = pageModel.Games.First();

            // Assert
            Assert.That(pageModel.Games, Is.Not.Empty);
            Assert.That(game.Game.Date.Date.Contains("2022"));
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        [Test]
        public void OnGet_Invalid_SportsApiClient_Should_Return_Valid_Page_With_No_Games()
        {
            // Arrange
            invalidPageModel = new NFLMatchesModel(null, testLogger);

            // Act
            var result = invalidPageModel.OnGet();

            // Assert
            Assert.That(invalidPageModel.Games, Is.Empty);
            Assert.That(result, Is.InstanceOf<PageResult>());

        }

        [Test]
        public void OnGet_Valid_Input_Seahawks_Should_Return_Valid_Page_With_Seahawk_Games()
        {
            // Arrange
            string team = "Seattle Seahawks";

            // Act
            var result = pageModel.OnGet(team);

            // Assert
            Assert.That(pageModel.Games, Is.Not.Empty);
            Assert.That(result, Is.InstanceOf<PageResult>());

        }
    }

}