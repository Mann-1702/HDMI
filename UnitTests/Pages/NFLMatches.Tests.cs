using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;

namespace UnitTests.Pages
{
    [TestFixture]
    public class NFLMatchesTests
    {
        private NFLMatches pageModel;
        private ILogger<NFLMatches> testLogger;

        [SetUp]
        public void Setup()
        {
            // Initialize logger
            testLogger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<NFLMatches>();

            // Instantiate NFLMatches
            pageModel = new NFLMatches(null, testLogger);
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
            var gamesProperty = typeof(NFLMatches).GetProperty("Games", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (gamesProperty != null)
            {
                // Set the value if `Games` is a property
                gamesProperty.SetValue(pageModel, mockGames);
            }
            else
            {
                // Attempt to find the `Games` field (likely an auto-property backing field)
                var gamesField = typeof(NFLMatches).GetField("<Games>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
                Assert.That(gamesField, Is.Not.Null, "Games property or backing field not found.");

                // Set the value if `Games` is a field
                gamesField.SetValue(pageModel, mockGames);
            }

            // Assert that Games has been set correctly
            Assert.That(pageModel.Games, Is.Not.Null);
            Assert.That(pageModel.Games.Count, Is.EqualTo(mockGames.Count));
            Assert.That(pageModel.Games[0].Teams.Home.Name, Is.EqualTo("Team A"));
            Assert.That(pageModel.Games[1].Teams.Home.Name, Is.EqualTo("Team C"));
        }
    }
}
