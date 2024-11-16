﻿using NUnit.Framework;
using Newtonsoft.Json;

namespace UnitTests.Models
{
    [TestFixture]
    public class GameResponseTests
    {
        private GameResponse _gameResponse;

        [SetUp]
        public void Setup()
        {
            _gameResponse = new GameResponse
            {
                Game = new GameDetails
                {
                    Id = 101,
                    Stage = "Final",
                    Week = "Week 12",
                    Date = new GameDate
                    {
                        Timezone = "UTC",
                        Date = "2024-12-01",
                        Time = "19:00",
                        Timestamp = 1738465200,
                        Start = "2024-12-01T19:00:00Z",
                        End = "2024-12-01T21:30:00Z",
                        Duration = "2:30"
                    },
                    Venue = new Venue
                    {
                        Name = "Stadium A",
                        City = "City X",
                        State = "State Y",
                        Country = "Country Z"
                    },
                    Status = new Status
                    {
                        Short = "FT",
                        Long = "Full Time",
                        Timer = "00:00",
                        Clock = "90:00",
                        HalfTime = true
                    },
                    Periods = new Period
                    {
                        Current = 4,
                        Total = 4,
                        EndOfPeriod = true
                    }
                },
                League = new League
                {
                    Id = 201,
                    Name = "Premier League",
                    Season = 2024,
                    Logo = "https://example.com/league_logo.png",
                    Country = new Country
                    {
                        Name = "England",
                        Code = "ENG",
                        Flag = "https://example.com/england_flag.png"
                    }
                },
                Teams = new Teams
                {
                    Home = new Team
                    {
                        Id = 1,
                        Name = "Team A",
                        Logo = "https://example.com/team_a_logo.png",
                        Nickname = "The Aces",
                        Code = "A"
                    },
                    Away = new Team
                    {
                        Id = 2,
                        Name = "Team B",
                        Logo = "https://example.com/team_b_logo.png",
                        Nickname = "The Braves",
                        Code = "B"
                    },
                    Visitors = new Team
                    {
                        Id = 3,
                        Name = "Team C",
                        Logo = "https://example.com/team_c_logo.png",
                        Nickname = "The Chargers",
                        Code = "C"
                    }
                },
                Scores = new Scores
                {
                    Home = new ScoreDetails
                    {
                        Quarter_1 = 20,
                        Quarter_2 = 15,
                        Quarter_3 = 25,
                        Quarter_4 = 30,
                        Overtime = 10,
                        Total = 100
                    },
                    Away = new ScoreDetails
                    {
                        Quarter_1 = 15,
                        Quarter_2 = 20,
                        Quarter_3 = 20,
                        Quarter_4 = 25,
                        Overtime = 5,
                        Total = 85
                    },
                    Visitors = new ScoreDetails
                    {
                        Quarter_1 = 10,
                        Quarter_2 = 15,
                        Quarter_3 = 15,
                        Quarter_4 = 20,
                        Overtime = null,
                        Total = 60
                    }
                }
            };
        }

        [Test]
        public void GameResponse_SerializationDeserialization_ShouldMatchOriginalObject()
        {
            // Act
            string json = JsonConvert.SerializeObject(_gameResponse);
            var deserialized = JsonConvert.DeserializeObject<GameResponse>(json);

            // Assert
            Assert.That(deserialized, Is.Not.Null);

            // Game Details
            Assert.That(deserialized.Game.Id, Is.EqualTo(_gameResponse.Game.Id));
            Assert.That(deserialized.Game.Stage, Is.EqualTo(_gameResponse.Game.Stage));
            Assert.That(deserialized.Game.Week, Is.EqualTo(_gameResponse.Game.Week));

            // Game Date
            Assert.That(deserialized.Game.Date.Timezone, Is.EqualTo(_gameResponse.Game.Date.Timezone));
            Assert.That(deserialized.Game.Date.Date, Is.EqualTo(_gameResponse.Game.Date.Date));
            Assert.That(deserialized.Game.Date.Time, Is.EqualTo(_gameResponse.Game.Date.Time));
            Assert.That(deserialized.Game.Date.Timestamp, Is.EqualTo(_gameResponse.Game.Date.Timestamp));
            Assert.That(deserialized.Game.Date.Start, Is.EqualTo(_gameResponse.Game.Date.Start));
            Assert.That(deserialized.Game.Date.End, Is.EqualTo(_gameResponse.Game.Date.End));

            // Venue
            Assert.That(deserialized.Game.Venue.Name, Is.EqualTo(_gameResponse.Game.Venue.Name));
            Assert.That(deserialized.Game.Venue.City, Is.EqualTo(_gameResponse.Game.Venue.City));
            Assert.That(deserialized.Game.Venue.Country, Is.EqualTo(_gameResponse.Game.Venue.Country));

            // League
            Assert.That(deserialized.League.Name, Is.EqualTo(_gameResponse.League.Name));
            Assert.That(deserialized.League.Season, Is.EqualTo(_gameResponse.League.Season));
            Assert.That(deserialized.League.Country.Name, Is.EqualTo(_gameResponse.League.Country.Name));

            // Teams
            Assert.That(deserialized.Teams.Home.Name, Is.EqualTo(_gameResponse.Teams.Home.Name));
            Assert.That(deserialized.Teams.Away.Name, Is.EqualTo(_gameResponse.Teams.Away.Name));
            Assert.That(deserialized.Teams.Visitors.Name, Is.EqualTo(_gameResponse.Teams.Visitors.Name));

            // Scores
            Assert.That(deserialized.Scores.Home.Total, Is.EqualTo(_gameResponse.Scores.Home.Total));
            Assert.That(deserialized.Scores.Away.Total, Is.EqualTo(_gameResponse.Scores.Away.Total));
            Assert.That(deserialized.Scores.Visitors.Total, Is.EqualTo(_gameResponse.Scores.Visitors.Total));
        }
    }
}
