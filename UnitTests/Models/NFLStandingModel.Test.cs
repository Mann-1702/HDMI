using Newtonsoft.Json;
using NUnit.Framework.Legacy;
using NUnit.Framework;

namespace UnitTests.Models
{
    [TestFixture]
    public class TeamStandingTests
    {
        [Test]
        public void Deserialize_ValidJson_Should_MapToTeamStanding()
        {
            // Arrange
            string json = @"
        {
            ""Conference"": ""Eastern"",
            ""Division"": ""Atlantic"",
            ""Position"": 1,
            ""team"": {
                ""Id"": 101,
                ""Name"": ""Team A"",
                ""Logo"": ""https://example.com/logo.png""
            },
            ""Won"": 15,
            ""Lost"": 5,
            ""Ties"": 0,
            ""Points"": {
                ""For"": 1200,
                ""Against"": 1100,
                ""Difference"": 100
            },
            ""Records"": {
                ""Home"": ""10-2"",
                ""Road"": ""5-3"",
                ""Conference"": ""8-2"",
                ""Division"": ""4-1""
            },
            ""Streak"": ""W3""
        }";

            // Act
            var teamStanding = JsonConvert.DeserializeObject<TeamStanding>(json);

            // Assert
            Assert.That(teamStanding, Is.Not.Null, "Deserialization resulted in a null object.");
            Assert.That(teamStanding.Conference, Is.EqualTo("Eastern"));
            Assert.That(teamStanding.Division, Is.EqualTo("Atlantic"));
            Assert.That(teamStanding.Position, Is.EqualTo(1));
            Assert.That(teamStanding.Team1, Is.Not.Null, "Team object was not deserialized.");
            Assert.That(teamStanding.Team1.Id, Is.EqualTo(101));
            Assert.That(teamStanding.Team1.Name, Is.EqualTo("Team A"));
            Assert.That(teamStanding.Team1.Logo, Is.EqualTo("https://example.com/logo.png"));
            Assert.That(teamStanding.Won, Is.EqualTo(15));
            Assert.That(teamStanding.Lost, Is.EqualTo(5));
            Assert.That(teamStanding.Ties, Is.EqualTo(0));
            Assert.That(teamStanding.Points, Is.Not.Null, "Points object was not deserialized.");
            Assert.That(teamStanding.Points.For, Is.EqualTo(1200));
            Assert.That(teamStanding.Points.Against, Is.EqualTo(1100));
            Assert.That(teamStanding.Points.Difference, Is.EqualTo(100));
            Assert.That(teamStanding.Records, Is.Not.Null, "Records object was not deserialized.");
            Assert.That(teamStanding.Records.Home, Is.EqualTo("10-2"));
            Assert.That(teamStanding.Records.Road, Is.EqualTo("5-3"));
            Assert.That(teamStanding.Records.Conference, Is.EqualTo("8-2"));
            Assert.That(teamStanding.Records.Division, Is.EqualTo("4-1"));
            Assert.That(teamStanding.Streak, Is.EqualTo("W3"));
        }

       
        [Test]
        public void Serialize_TeamStanding_Should_ProduceValidJson()
        {
            // Arrange
            var teamStanding = new TeamStanding
            {
                Conference = "Western",
                Division = "Pacific",
                Position = 2,
                Team1 = new Team1
                {
                    Id = 102,
                    Name = "Team B",
                    Logo = "https://example.com/team_b_logo.png"
                },
                Won = 10,
                Lost = 8,
                Ties = 2,
                Points = new Points
                {
                    For = 950,
                    Against = 900,
                    Difference = 50
                },
                Records = new Records
                {
                    Home = "5-3",
                    Road = "5-5",
                    Conference = "3-2",
                    Division = "2-1"
                },
                Streak = "L2"
            };

            // Act
            string json = JsonConvert.SerializeObject(teamStanding, Formatting.Indented);

            // Assert
            StringAssert.Contains(@"""Conference"": ""Western""", json);
            StringAssert.Contains(@"""Division"": ""Pacific""", json);
            StringAssert.Contains(@"""Position"": 2", json);
            StringAssert.Contains(@"""Id"": 102", json);
            StringAssert.Contains(@"""Name"": ""Team B""", json);
            StringAssert.Contains(@"""Logo"": ""https://example.com/team_b_logo.png""", json);
            StringAssert.Contains(@"""Won"": 10", json);
            StringAssert.Contains(@"""Lost"": 8", json);
            StringAssert.Contains(@"""Ties"": 2", json);
            StringAssert.Contains(@"""For"": 950", json);
            StringAssert.Contains(@"""Against"": 900", json);
            StringAssert.Contains(@"""Difference"": 50", json);
            StringAssert.Contains(@"""Home"": ""5-3""", json);
            StringAssert.Contains(@"""Road"": ""5-5""", json);
            StringAssert.Contains(@"""Conference"": ""3-2""", json);
            StringAssert.Contains(@"""Division"": ""2-1""", json);
            StringAssert.Contains(@"""Streak"": ""L2""", json);
        }
    }
}