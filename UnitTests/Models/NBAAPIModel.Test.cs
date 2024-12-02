using NUnit.Framework;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UnitTests.Models
{
    [TestFixture]
    public class NbaGameResponseTests
    {
        private NbaGameResponse _nbaGameResponse;

        [SetUp]
        public void Setup()
        {
            _nbaGameResponse = new NbaGameResponse
            {
                Id = 101,
                League = "NBA",
                Season = 2024,
                Date = new NbaGameDate
                {
                    Start = "2024-12-01T19:00:00Z",
                    End = "2024-12-01T21:30:00Z",
                    Duration = "2:30"
                },
                Stage = "Final",
                Status = new NbaStatus
                {
                    Short = "FT",
                    Long = "Full Time",
                    Clock = "00:00",
                    Halftime = true
                },
                Teams = new NbaTeams
                {
                    Home = new NbaTeam
                    {
                        Id = 1,
                        Name = "Los Angeles Lakers",
                        Nickname = "Lakers",
                        Code = "LAL",
                        Logo = "https://example.com/lakers_logo.png"
                    },
                    Visitors = new NbaTeam
                    {
                        Id = 2,
                        Name = "Golden State Warriors",
                        Nickname = "Warriors",
                        Code = "GSW",
                        Logo = "https://example.com/warriors_logo.png"
                    }
                },
                Scores = new NbaScores
                {
                    Home = new NbaScoreDetails
                    {
                        Win = 15,
                        Loss = 5,
                        Points = 120,
                        Linescore = new List<string> { "30", "28", "32", "30" },
                        Series = new SeriesRecord { Win = 1, Loss = 0 }
                    },
                    Visitors = new NbaScoreDetails
                    {
                        Win = 14,
                        Loss = 6,
                        Points = 118,
                        Linescore = new List<string> { "28", "30", "30", "30" },
                        Series = new SeriesRecord { Win = 0, Loss = 1 }
                    }
                }
            };
        }

        [Test]
        public void NbaGameResponse_SerializationDeserialization_ShouldMatchOriginalObject()
        {
            // Act
            string json = JsonConvert.SerializeObject(_nbaGameResponse);
            var deserialized = JsonConvert.DeserializeObject<NbaGameResponse>(json);

            // Assert
            Assert.That(deserialized, Is.Not.Null);

            // NbaGameResponse Properties
            Assert.That(deserialized.Id, Is.EqualTo(_nbaGameResponse.Id));
            Assert.That(deserialized.League, Is.EqualTo(_nbaGameResponse.League));
            Assert.That(deserialized.Season, Is.EqualTo(_nbaGameResponse.Season));

            // Date
            Assert.That(deserialized.Date.Start, Is.EqualTo(_nbaGameResponse.Date.Start));
            Assert.That(deserialized.Date.End, Is.EqualTo(_nbaGameResponse.Date.End));
            Assert.That(deserialized.Date.Duration, Is.EqualTo(_nbaGameResponse.Date.Duration));

            // Stage
            Assert.That(deserialized.Stage, Is.EqualTo(_nbaGameResponse.Stage));

            // Status
            Assert.That(deserialized.Status.Short, Is.EqualTo(_nbaGameResponse.Status.Short));
            Assert.That(deserialized.Status.Long, Is.EqualTo(_nbaGameResponse.Status.Long));
            Assert.That(deserialized.Status.Clock, Is.EqualTo(_nbaGameResponse.Status.Clock));
            Assert.That(deserialized.Status.Halftime, Is.EqualTo(_nbaGameResponse.Status.Halftime));

            // Teams
            Assert.That(deserialized.Teams.Home.Name, Is.EqualTo(_nbaGameResponse.Teams.Home.Name));
            Assert.That(deserialized.Teams.Home.Code, Is.EqualTo(_nbaGameResponse.Teams.Home.Code));
            Assert.That(deserialized.Teams.Visitors.Name, Is.EqualTo(_nbaGameResponse.Teams.Visitors.Name));
            Assert.That(deserialized.Teams.Visitors.Code, Is.EqualTo(_nbaGameResponse.Teams.Visitors.Code));

            // Scores
            Assert.That(deserialized.Scores.Home.Points, Is.EqualTo(_nbaGameResponse.Scores.Home.Points));
            Assert.That(deserialized.Scores.Home.Linescore, Is.EqualTo(_nbaGameResponse.Scores.Home.Linescore));
            Assert.That(deserialized.Scores.Home.Series.Win, Is.EqualTo(_nbaGameResponse.Scores.Home.Series.Win));

            Assert.That(deserialized.Scores.Visitors.Points, Is.EqualTo(_nbaGameResponse.Scores.Visitors.Points));
            Assert.That(deserialized.Scores.Visitors.Linescore, Is.EqualTo(_nbaGameResponse.Scores.Visitors.Linescore));
            Assert.That(deserialized.Scores.Visitors.Series.Win, Is.EqualTo(_nbaGameResponse.Scores.Visitors.Series.Win));
        }
    }
}