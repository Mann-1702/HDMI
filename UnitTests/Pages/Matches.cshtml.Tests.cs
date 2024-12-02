using System.Linq;
using Moq;
using NUnit.Framework;
using ContosoCrafts.WebSite.Pages;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace UnitTests.Pages
{
    public class MatchesTests
    {
        #region TestSetup

        public static MatchesModel pageModel;
        private Mock<IWebHostEnvironment> mockWebHostEnvironment;
        private JsonFileMatchService matchService;

        [SetUp]
        public void TestInitialize()
        {
            mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            matchService = new JsonFileMatchService(mockWebHostEnvironment.Object);

            pageModel = new MatchesModel(matchService);
        }

        #endregion TestSetup

        #region OnGet
        [Test]
        public void OnGet_Valid_Null_Input_Should_Fetch_All_Matches()
        {

            // Arrange

            // Act
            pageModel.OnGet(null);
            var allMatches = matchService.GetAllData();

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(pageModel.Matches.Count(), Is.EqualTo(allMatches.Count()));
        }

        [Test]
        public void OnGet_Valid_Team_Input_Should_Fetch_All_Matches_For_Only_That_Team()
        {

            // Arrange
            string validTeamName = "Kansas City Chiefs";

            // Act
            pageModel.OnGet(validTeamName);
            var allMatches = matchService.GetAllData().Where(m => m.Team1 == validTeamName || m.Team2 == validTeamName);

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(pageModel.Matches.Count(), Is.EqualTo(allMatches.Count()));

            // Checks each match to see if it's a Kansas City Chiefs's match
            foreach (var match in pageModel.Matches)
            {
                Assert.That(match.Team1, Is.EqualTo(validTeamName));
            }

        }

        #endregion OnGet
    }

}