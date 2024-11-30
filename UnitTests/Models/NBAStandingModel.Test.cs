using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Models
{
    [TestFixture]
    public class NBAStandingTests
    {
        private NBAStanding _nbaStanding;

        [SetUp]
        public void Setup()
        {
            _nbaStanding = new NBAStanding
            {
                League = "NBA",
                Season = 2024,
                Team2 = new Team2
                {
                    Id = 1,
                    Name = "Los Angeles Lakers",
                    Nickname = "Lakers",
                    Code = "LAL",
                    Logo = "https://example.com/lakers_logo.png"
                },
                Conference = new Conference
                {
                    Name = "Western",
                    Rank = 1,
                    Win = 50,
                    Loss = 30
                },
                Division = new Division
                {
                    Name = "Pacific",
                    Rank = 1,
                    Win = 30,
                    Loss = 10,
                    GamesBehind = "0.5"
                },
                Win = new WinLoss
                {
                    Home = 25,
                    Away = 25,
                    Total = 50,
                    Percentage = "0.625",
                    LastTen = 7
                },
                Loss = new WinLoss
                {
                    Home = 10,
                    Away = 20,
                    Total = 30,
                    Percentage = "0.375",
                    LastTen = 3
                },
                GamesBehind = "0.5",
                Streak = 3,
                WinStreak = true,
                TieBreakerPoints = "0"
            };
        }

        [Test]
        public void NBAStanding_SerializationDeserialization_ShouldMatchOriginalObject()
        {
            // Act: Serialize the NBAStanding object to JSON string
            string json = JsonConvert.SerializeObject(_nbaStanding);

            // Act: Deserialize the JSON string back to an NBAStanding object
            var deserialized = JsonConvert.DeserializeObject<NBAStanding>(json);

            // Assert: Check if the deserialized object matches the original object
            Assert.That(deserialized, Is.Not.Null);

            // Check that all properties match
            Assert.That(deserialized.League, Is.EqualTo(_nbaStanding.League));
            Assert.That(deserialized.Season, Is.EqualTo(_nbaStanding.Season));

            // Team2
            Assert.That(deserialized.Team2.Name, Is.EqualTo(_nbaStanding.Team2.Name));
            Assert.That(deserialized.Team2.Code, Is.EqualTo(_nbaStanding.Team2.Code));
            Assert.That(deserialized.Team2.Logo, Is.EqualTo(_nbaStanding.Team2.Logo));

            // Conference
            Assert.That(deserialized.Conference.Name, Is.EqualTo(_nbaStanding.Conference.Name));
            Assert.That(deserialized.Conference.Win, Is.EqualTo(_nbaStanding.Conference.Win));
            Assert.That(deserialized.Conference.Loss, Is.EqualTo(_nbaStanding.Conference.Loss));

            // Division
            Assert.That(deserialized.Division.Name, Is.EqualTo(_nbaStanding.Division.Name));
            Assert.That(deserialized.Division.Win, Is.EqualTo(_nbaStanding.Division.Win));
            Assert.That(deserialized.Division.Loss, Is.EqualTo(_nbaStanding.Division.Loss));
            Assert.That(deserialized.Division.GamesBehind, Is.EqualTo(_nbaStanding.Division.GamesBehind));

            // Win
            Assert.That(deserialized.Win.Total, Is.EqualTo(_nbaStanding.Win.Total));
            Assert.That(deserialized.Win.Percentage, Is.EqualTo(_nbaStanding.Win.Percentage));
            Assert.That(deserialized.Win.LastTen, Is.EqualTo(_nbaStanding.Win.LastTen));

            // Loss
            Assert.That(deserialized.Loss.Total, Is.EqualTo(_nbaStanding.Loss.Total));
            Assert.That(deserialized.Loss.Percentage, Is.EqualTo(_nbaStanding.Loss.Percentage));
            Assert.That(deserialized.Loss.LastTen, Is.EqualTo(_nbaStanding.Loss.LastTen));

            // Other properties
            Assert.That(deserialized.GamesBehind, Is.EqualTo(_nbaStanding.GamesBehind));
            Assert.That(deserialized.Streak, Is.EqualTo(_nbaStanding.Streak));
            Assert.That(deserialized.WinStreak, Is.EqualTo(_nbaStanding.WinStreak));
            Assert.That(deserialized.TieBreakerPoints, Is.EqualTo(_nbaStanding.TieBreakerPoints));
        }

        [Test]
        public void NBAStanding_Deserialization_WithMissingFields_ShouldHandleGracefully()
        {
            // Create JSON with missing 'TieBreakerPoints' and 'Streak'
            var jsonWithMissingFields = @"
            {
                'League': 'NBA',
                'Season': 2024,
                'team': {
                    'Id': 1,
                    'Name': 'Los Angeles Lakers',
                    'Nickname': 'Lakers',
                    'Code': 'LAL',
                    'Logo': 'https://example.com/lakers_logo.png'
                },
                'Conference': {
                    'Name': 'Western',
                    'Rank': 1,
                    'Win': 50,
                    'Loss': 30
                },
                'Division': {
                    'Name': 'Pacific',
                    'Rank': 1,
                    'Win': 30,
                    'Loss': 10,
                    'GamesBehind': '0.5'
                },
                'Win': {
                    'Home': 25,
                    'Away': 25,
                    'Total': 50,
                    'Percentage': '0.625',
                    'LastTen': 7
                },
                'Loss': {
                    'Home': 10,
                    'Away': 20,
                    'Total': 30,
                    'Percentage': '0.375',
                    'LastTen': 3
                },
                'GamesBehind': '0.5'
            }";

            // Act: Deserialize the JSON string
            var deserialized = JsonConvert.DeserializeObject<NBAStanding>(jsonWithMissingFields);

            // Assert: Ensure that default values are applied for missing fields
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized.TieBreakerPoints, Is.Null);
            Assert.That(deserialized.Streak, Is.EqualTo(0)); // Default value for int
        }
    }
}
