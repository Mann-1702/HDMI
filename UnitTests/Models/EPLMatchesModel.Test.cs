using Newtonsoft.Json;
using NUnit.Framework;
using System;

namespace UnitTests.Models
{
    /// <summary>
    /// Unit tests for the FixtureResponse model to ensure proper serialization and deserialization.
    /// </summary>
    [TestFixture]
    public class FixtureResponseTests
    {
        private FixtureResponse _fixtureResponse;

        /// <summary>
        /// Sets up the test environment by initializing a sample FixtureResponse object with test data.
        /// </summary>
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

        /// <summary>
        /// Tests if the FixtureResponse object can be serialized and deserialized while retaining its original data.
        /// </summary>
        [Test]
        public void FixtureResponse_SerializationDeserialization_ShouldMatchOriginalObject()
        {
            // Act
            string json = JsonConvert.SerializeObject(_fixtureResponse);
            var deserialized = JsonConvert.DeserializeObject<FixtureResponse>(json);

            // Assert
            Assert.That(deserialized, Is.Not.Null, "Deserialization should return a non-null object.");

            // Validate League Information
            Assert.That(deserialized.League.Id, Is.EqualTo(_fixtureResponse.League.Id), "League ID mismatch.");
            Assert.That(deserialized.League.Name, Is.EqualTo(_fixtureResponse.League.Name), "League name mismatch.");
            Assert.That(deserialized.League.Country, Is.EqualTo(_fixtureResponse.League.Country), "League country mismatch.");

            // Validate Season
            Assert.That(deserialized.Season, Is.EqualTo(_fixtureResponse.Season), "Season mismatch.");

            // Validate Teams Information
            Assert.That(deserialized.Teams.Home.Id, Is.EqualTo(_fixtureResponse.Teams.Home.Id), "Home team ID mismatch.");
            Assert.That(deserialized.Teams.Home.Name, Is.EqualTo(_fixtureResponse.Teams.Home.Name), "Home team name mismatch.");
            Assert.That(deserialized.Teams.Visitors.Id, Is.EqualTo(_fixtureResponse.Teams.Visitors.Id), "Visitor team ID mismatch.");
            Assert.That(deserialized.Teams.Visitors.Name, Is.EqualTo(_fixtureResponse.Teams.Visitors.Name), "Visitor team name mismatch.");

            // Validate Scores
            Assert.That(deserialized.Scores.Halftime.Home, Is.EqualTo(_fixtureResponse.Scores.Halftime.Home), "Halftime home score mismatch.");
            Assert.That(deserialized.Scores.Halftime.Away, Is.EqualTo(_fixtureResponse.Scores.Halftime.Away), "Halftime away score mismatch.");
            Assert.That(deserialized.Scores.Fulltime.Home, Is.EqualTo(_fixtureResponse.Scores.Fulltime.Home), "Fulltime home score mismatch.");
            Assert.That(deserialized.Scores.Fulltime.Away, Is.EqualTo(_fixtureResponse.Scores.Fulltime.Away), "Fulltime away score mismatch.");
            Assert.That(deserialized.Scores.Penalty.Home, Is.EqualTo(_fixtureResponse.Scores.Penalty.Home), "Penalty home score mismatch.");
            Assert.That(deserialized.Scores.Penalty.Away, Is.EqualTo(_fixtureResponse.Scores.Penalty.Away), "Penalty away score mismatch.");

            // Validate Date Information
            Assert.That(deserialized.Date.Start, Is.EqualTo(_fixtureResponse.Date.Start), "Date start mismatch.");
            Assert.That(deserialized.Date.Timezone, Is.EqualTo(_fixtureResponse.Date.Timezone), "Timezone mismatch.");
            Assert.That(deserialized.Date.DateTime, Is.EqualTo(_fixtureResponse.Date.DateTime), "DateTime mismatch.");
            Assert.That(deserialized.Date.Timestamp, Is.EqualTo(_fixtureResponse.Date.Timestamp), "Timestamp mismatch.");

            // Validate Fixture Status
            Assert.That(deserialized.Status.Short, Is.EqualTo(_fixtureResponse.Status.Short), "Status short mismatch.");
            Assert.That(deserialized.Status.Long, Is.EqualTo(_fixtureResponse.Status.Long), "Status long mismatch.");
            Assert.That(deserialized.Status.Clock, Is.EqualTo(_fixtureResponse.Status.Clock), "Clock time mismatch.");
            Assert.That(deserialized.Status.Halftime, Is.EqualTo(_fixtureResponse.Status.Halftime), "Halftime status mismatch.");

            // Validate Stage
            Assert.That(deserialized.Stage, Is.EqualTo(_fixtureResponse.Stage), "Stage mismatch.");
        }
    }
}
