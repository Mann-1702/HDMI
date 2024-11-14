using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.Reflection;

namespace UnitTests.Pages
{
    [TestFixture]
    public class NBAMatchesTests
    {
        private NBAMatchesModel pageModel;
        private NBAMatchesModel invalidPageModel;

        private ILogger<SportsApiClient> logger;
        private ILogger<NBAMatchesModel> testLogger;

        private SportsApiClient sportsApiClient;

        [SetUp]
        public void Setup()
        {

            // Initialize logger

            logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<SportsApiClient>();
            testLogger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<NBAMatchesModel>();


            var apiKey = "b4ed364047f61b7a0ae7699c69c7ad57";
            sportsApiClient = new SportsApiClient(apiKey, logger);


            // Instantiate NFLMatches
            pageModel = new NBAMatchesModel(sportsApiClient, testLogger);
        }



        [Test]
        public void OnGet_Should_Fetch_2024_NBA_Games()
        {
            // Arrange

            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.Games, Is.Not.Empty);
        }

        [Test]
        public void OnGet_Invalid_SportsApiClient_Should_Return_Valid_Page_With_No_Games()
        {
            // Arrange
            invalidPageModel = new NBAMatchesModel(null, testLogger);

            // Act
            invalidPageModel.OnGet();

            // Assert
            Assert.That(invalidPageModel.Games, Is.Empty);
        }
    }

}