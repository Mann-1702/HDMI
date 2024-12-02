using Moq;
using NUnit.Framework;
using ContosoCrafts.WebSite.Pages;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Collections.Generic;

namespace UnitTests.Pages
{
    public class CompareTeamsTests
    {
        #region TestSetup

        public static CompareTeamsModel pageModel;
        private Mock<IWebHostEnvironment> mockWebHostEnvironment;
        private JsonFileProductService productService;

        [SetUp]
        public void TestInitialize()
        {
            mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            productService = new JsonFileProductService(mockWebHostEnvironment.Object);


            pageModel = new CompareTeamsModel();
        }

        #endregion TestSetup

        #region OnGet

        [Test]
        public void OnGet_Valid_Should_Return_Valid_Page()
        {

            // Arrange

            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
        }

        #endregion OnGet

        #region OnPostCompare

        [Test]
        public void OnPostCompare_Valid_Teams_Should_Return_Valid_Page()
        {

            // Arrange

            // Act
            pageModel.OnGet();
            pageModel.SelectedSport = "NFL";
            pageModel.Team1 = "Patriots";
            pageModel.Team2 = "49ers";
            pageModel.OnPostCompare();

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
        }

        [Test]
        public void OnPostCompare_Invalid_Duplicate_Teams_Should_Return_Valid_Page()
        {

            // Arrange
            string errorMessage = "Please select two different teams to compare.";

            // Act
            pageModel.OnGet();
            pageModel.SelectedSport = "NFL";
            pageModel.Team1 = "Patriots";
            pageModel.Team2 = "Patriots";
            pageModel.OnPostCompare();

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(pageModel.ComparisonResult, Is.EqualTo(errorMessage));
        }

        [Test]
        public void OnPostCompare_Invalid_Null_Team_Should_Return_Valid_Page()
        {

            // Arrange
            string errorMessage = "Please select two different teams to compare.";

            // Act
            pageModel.OnGet();
            pageModel.SelectedSport = "NFL";
            pageModel.Team1 = null;
            pageModel.Team2 = "Patriots";
            pageModel.OnPostCompare();

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(pageModel.ComparisonResult, Is.EqualTo(errorMessage));
        }
        #endregion OnPostCompare

        #region OnGetTeamsBySport
        [Test]
        public void OnGetTeamsBySport_Valid_Sport_Should_Return_List_Of_Teams()
        {

            // Arrange
            string sport = "NFL";
            var expectedTeams = new List<string> { "Patriots", "49ers", "Cowboys", "Packers" };

            // Act
            pageModel.OnGet();
            var result = pageModel.OnGetTeamsBySport(sport).Value as List<string>;


            // Assert
            Assert.That(result, Is.EqualTo(expectedTeams));
        }

        [Test]
        public void OnGetTeamsBySport_InValid_Sport_Should_Return_Empty_List()
        {

            // Arrange
            string invalidSport = "Invalid Sport";
            var expectedTeams = new List<string> { };

            // Act
            pageModel.OnGet();
            var result = pageModel.OnGetTeamsBySport(invalidSport).Value as List<string>;


            // Assert
            Assert.That(result, Is.EqualTo(expectedTeams));
        }

        #endregion OnGetTeamsBySport

        #region Sports

        [Test]
        public void Sports_Property_Should_Initialize_With_Correct_Values()
        {

            // Arrange
            var expectedSports = new List<string> { "NFL", "NBA", "Soccer" };

            // Act
            var result = pageModel.Sports;

            // Assert
            Assert.That(result, Is.EqualTo(expectedSports));
        }

        [Test]
        public void Sports_Setter_Should_Update_The_Sports_List()
        {

            // Arrange
            var newSports = new List<string> { "Baseball", "Hockey", "Tennis" };

            // Act
            pageModel.Sports = newSports;  // Setting the new list

            // Assert
            Assert.That(pageModel.Sports, Is.EqualTo(newSports));  // Verify the new value
        }

        #endregion Sports

        #region Teams
        [Test]
        public void Teams_Getter_Should_Return_Initial_Teams_List()
        {

            // Arrange
            var expectedTeams = new Dictionary<string, List<string>>
                {
                    { "NFL", new List<string> { "Patriots", "49ers", "Cowboys", "Packers" } },
                    { "NBA", new List<string> { "Lakers", "Warriors", "Celtics", "Nets" } },
                    { "Soccer", new List<string> { "Barcelona", "Real Madrid", "Liverpool", "Manchester United" } }
                };

            // Act
            var result = pageModel.Teams;

            // Assert
            Assert.That(result, Is.EqualTo(expectedTeams));
        }

        [Test]
        public void Teams_Setter_Should_Update_Teams_List()
        {

            // Arrange
            var newTeams = new Dictionary<string, List<string>>
                {
                    { "NFL", new List<string> { "Giants", "Eagles", "Dolphins" } },
                    { "NBA", new List<string> { "Heat", "Bucks", "Bulls" } },
                    { "Soccer", new List<string> { "Juventus", "PSG", "Bayern Munich" } }
                };

            // Act
            pageModel.Teams = newTeams;

            // Assert
            Assert.That(pageModel.Teams, Is.EqualTo(newTeams));
        }

        #endregion Teams
    }

}