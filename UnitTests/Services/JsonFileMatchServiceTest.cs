using System.Linq;

using Microsoft.AspNetCore.Mvc;

using NUnit.Framework;

using ContosoCrafts.WebSite.Models;
using System;
using System.Threading;
using System.ComponentModel.DataAnnotations;

namespace UnitTests.Services
{
    public class JsonFileMatchServiceTests
    {
        #region TestSetup

        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup

        #region GetAllData
        [Test]
        public void GetAllData_Should_Return_48_Matches()
        {
            // Arrange

            // Act
            var result = TestHelper.MatchService.GetAllData();
            int count = result.Count();

            // Assert
            Assert.That(count, Is.EqualTo(48));
        }
        #endregion GetAllData


        #region SwapTeam1Team2
        [Test]
        public void SwapTeam1Team2_Invalid_Null_Input_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.MatchService.SwapTeam1Team2(null);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void SwapTeam1Team2_Invalid_Empty_Match_Should_Return_False()
        {
            // Arrange
            MatchModel testMatch = new MatchModel();

            // Act
            var result = TestHelper.MatchService.SwapTeam1Team2(testMatch);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void SwapTeam1Team2_Invalid_Team1_Should_Return_False()
        {
            // Arrange
            MatchModel testMatch = new MatchModel();

            // Invalid testMatch.team1 - unassigned
            testMatch.Team2 = "Team2";

            // Act
            var result = TestHelper.MatchService.SwapTeam1Team2(testMatch);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void SwapTeam1Team2_Invalid_Team2_Should_Return_False()
        {
            // Arrange
            MatchModel testMatch = new MatchModel();

            // Invalid testMatch.team2 - unassigned
            testMatch.Team1 = "Team1";

            // Act
            var result = TestHelper.MatchService.SwapTeam1Team2(testMatch);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void SwapTeam1Team2_Invalid_Negative_Team1_Score_Should_Return_False()
        {
            // Arrange
            MatchModel testMatch = new MatchModel();
            testMatch.Team1 = "Team1";
            testMatch.Team2 = "Team2";
            testMatch.Team1_Score = -1;

            // Act
            var result = TestHelper.MatchService.SwapTeam1Team2(testMatch);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void SwapTeam1Team2_Invalid_Negative_Team2_Score_Should_Return_False()
        {
            // Arrange
            MatchModel testMatch = new MatchModel();
            testMatch.Team1 = "Team1";
            testMatch.Team2 = "Team2";
            testMatch.Team2_Score = -1;

            // Act
            var result = TestHelper.MatchService.SwapTeam1Team2(testMatch);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void SwapTeam1Team2_Valid_Match_Should_Return_True()
        {
            // Arrange
            MatchModel testMatch = new MatchModel();
            testMatch.Team1 = "Team1";
            testMatch.Team2 = "Team2";


            // Act
            var result = TestHelper.MatchService.SwapTeam1Team2(testMatch);

            // Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(testMatch.Team1, Is.EqualTo("Team2"));
            Assert.That(testMatch.Team2, Is.EqualTo("Team1"));
        }
        #endregion SwapTeam1Team2

        #region IsValidMatch
        [Test]
        public void IsValidMatch_Invalid_Null_Input_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.MatchService.IsValidMatch(null);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void IsValidMatch_Invalid_Null_Team1_Should_Return_False()
        {
            // Arrange
            MatchModel testMatch = new MatchModel();
            testMatch.Team1 = null;
            testMatch.Team2 = "Team2";

            // Act
            var result = TestHelper.MatchService.IsValidMatch(testMatch);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void IsValidMatch_Invalid_Null_Team2_Should_Return_False()
        {
            // Arrange
            MatchModel testMatch = new MatchModel();
            testMatch.Team1 = "Team2";
            testMatch.Team2 = null;

            // Act
            var result = TestHelper.MatchService.IsValidMatch(testMatch);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void IsValidMatch_Invalid_Negative_Team1_Score_Should_Return_False()
        {
            // Arrange
            MatchModel testMatch = new MatchModel();
            testMatch.Team1 = "Team1";
            testMatch.Team2 = "Team2";
            testMatch.Team1_Score = -1;
            testMatch.Team2_Score = 0;

            // Act
            var result = TestHelper.MatchService.IsValidMatch(testMatch);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void IsValidMatch_Invalid_Negative_Team2_Score_Should_Return_False()
        {
            // Arrange
            MatchModel testMatch = new MatchModel();
            testMatch.Team1 = "Team1";
            testMatch.Team2 = "Team2";
            testMatch.Team1_Score = 0;
            testMatch.Team2_Score = -1;

            // Act
            var result = TestHelper.MatchService.IsValidMatch(testMatch);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void IsValidMatch_Valid_Match_Should_Return_True()
        {
            // Arrange
            MatchModel testMatch = new MatchModel();
            testMatch.Team1 = "Team1";
            testMatch.Team2 = "Team2";
            testMatch.Team1_Score = 1;
            testMatch.Team2_Score = 2;

            // Act
            var result = TestHelper.MatchService.IsValidMatch(testMatch);

            // Assert
            Assert.That(result, Is.EqualTo(true));
        }
        #endregion IsValidMatch
    }
}