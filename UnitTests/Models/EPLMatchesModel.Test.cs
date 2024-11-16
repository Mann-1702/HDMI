using Newtonsoft.Json;
using NUnit.Framework;
using System;

namespace UnitTests.Models
{
    [TestFixture]
    public class FixtureResponseTests
    {
        private FixtureResponse _fixtureResponse;

        [SetUp]
        public void Setup()
        {
            _fixtureResponse = new FixtureResponse
            {
                League = new LeagueInfo
                {
                    Id = 1,
                    Name = "Premier League",
                    Country = "England"
                },
                Season = 2024,
                Teams = new FixtureTeams
                {
                    Home = new TeamE
                    {
                        Id = 101,
                        Name = "Manchester United",
                        City = "Manchester",
                        Code = "MUFC",
                        Logo = "https://example.com/mu_logo.png"
                    },
                    Visitors = new TeamE
                    {
                        Id = 102,
                        Name = "Liverpool",
                        City = "Liverpool",
                        Code = "LFC",
                        Logo = "https://example.com/lfc_logo.png"
                    }
                },
                Scores = new FixtureScores
                {
                    Halftime = new ScoreDetails1 { Home = 1, Away = 2 },
                    Fulltime = new ScoreDetails1 { Home = 3, Away = 2 },
                    ExtraTime = new ScoreDetails1 { Home = null, Away = null },
                    Penalty = new ScoreDetails1 { Home = 5, Away = 4 }
                },
                Date = new FixtureDate
                {
                    Start = "2024-12-01T18:00:00Z",
                    Timezone = "UTC",
                    DateTime = new DateTimeOffset(2024, 12, 1, 18, 0, 0, TimeSpan.Zero),
                    Timestamp = 1738461600
                },
                Status = new FixtureStatus
                {
                    Short = "FT",
                    Long = "Full Time",
                    Clock = "90:00",
                    Halftime = true
                },
                Stage = "Final"
            };
        }

        [Test]
        public void FixtureResponse_SerializationDeserialization_ShouldMatchOriginalObject()
        {
            // Act
            string json = JsonConvert.SerializeObject(_fixtureResponse);
            var deserialized = JsonConvert.DeserializeObject<FixtureResponse>(json);

            // Assert
            Assert.That(deserialized, Is.Not.Null);

            // League Info
            Assert.That(deserialized.League.Id, Is.EqualTo(_fixtureResponse.League.Id));
            Assert.That(deserialized.League.Name, Is.EqualTo(_fixtureResponse.League.Name));
            Assert.That(deserialized.League.Country, Is.EqualTo(_fixtureResponse.League.Country));

            // Season
            Assert.That(deserialized.Season, Is.EqualTo(_fixtureResponse.Season));

            // Teams
            Assert.That(deserialized.Teams.Home.Id, Is.EqualTo(_fixtureResponse.Teams.Home.Id));
            Assert.That(deserialized.Teams.Home.Name, Is.EqualTo(_fixtureResponse.Teams.Home.Name));
            Assert.That(deserialized.Teams.Home.City, Is.EqualTo(_fixtureResponse.Teams.Home.City));
            Assert.That(deserialized.Teams.Home.Code, Is.EqualTo(_fixtureResponse.Teams.Home.Code));
            Assert.That(deserialized.Teams.Home.Logo, Is.EqualTo(_fixtureResponse.Teams.Home.Logo));

            Assert.That(deserialized.Teams.Visitors.Id, Is.EqualTo(_fixtureResponse.Teams.Visitors.Id));
            Assert.That(deserialized.Teams.Visitors.Name, Is.EqualTo(_fixtureResponse.Teams.Visitors.Name));
            Assert.That(deserialized.Teams.Visitors.City, Is.EqualTo(_fixtureResponse.Teams.Visitors.City));
            Assert.That(deserialized.Teams.Visitors.Code, Is.EqualTo(_fixtureResponse.Teams.Visitors.Code));
            Assert.That(deserialized.Teams.Visitors.Logo, Is.EqualTo(_fixtureResponse.Teams.Visitors.Logo));

            // Scores
            Assert.That(deserialized.Scores.Halftime.Home, Is.EqualTo(_fixtureResponse.Scores.Halftime.Home));
            Assert.That(deserialized.Scores.Halftime.Away, Is.EqualTo(_fixtureResponse.Scores.Halftime.Away));
            Assert.That(deserialized.Scores.Fulltime.Home, Is.EqualTo(_fixtureResponse.Scores.Fulltime.Home));
            Assert.That(deserialized.Scores.Fulltime.Away, Is.EqualTo(_fixtureResponse.Scores.Fulltime.Away));
            Assert.That(deserialized.Scores.Penalty.Home, Is.EqualTo(_fixtureResponse.Scores.Penalty.Home));
            Assert.That(deserialized.Scores.Penalty.Away, Is.EqualTo(_fixtureResponse.Scores.Penalty.Away));

            // Date
            Assert.That(deserialized.Date.Start, Is.EqualTo(_fixtureResponse.Date.Start));
            Assert.That(deserialized.Date.Timezone, Is.EqualTo(_fixtureResponse.Date.Timezone));
            Assert.That(deserialized.Date.DateTime, Is.EqualTo(_fixtureResponse.Date.DateTime));
            Assert.That(deserialized.Date.Timestamp, Is.EqualTo(_fixtureResponse.Date.Timestamp));

            // Status
            Assert.That(deserialized.Status.Short, Is.EqualTo(_fixtureResponse.Status.Short));
            Assert.That(deserialized.Status.Long, Is.EqualTo(_fixtureResponse.Status.Long));
            Assert.That(deserialized.Status.Clock, Is.EqualTo(_fixtureResponse.Status.Clock));
            Assert.That(deserialized.Status.Halftime, Is.EqualTo(_fixtureResponse.Status.Halftime));

            // Stage
            Assert.That(deserialized.Stage, Is.EqualTo(_fixtureResponse.Stage));
        }
    }
}
