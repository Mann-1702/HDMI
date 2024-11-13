using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnitTests.Pages
{
    [TestFixture]
    public class EPLMatchesTests
    {
        private EPLMatches pageModel;
        private EPLMatches invalidPageModel;

        private ILogger<SportsApiClient> logger;
        private ILogger<EPLMatches> testLogger;

        private SportsApiClient sportsApiClient;

        [SetUp]
        public void Setup()
        {

            // Initialize logger

            logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<SportsApiClient>();
            testLogger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<EPLMatches>();


            var apiKey = "51272e96672158d36c1baffaefc7f223";
            sportsApiClient = new SportsApiClient(apiKey, logger);


            // Instantiate NFLMatches
            pageModel = new EPLMatches(sportsApiClient, testLogger);
        }


        [Test]
        public void OnGet_Should_Fetch_EPL_Games()
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
            invalidPageModel = new EPLMatches(null, testLogger);

            // Act
            invalidPageModel.OnGet();

            // Assert
            Assert.That(invalidPageModel.Games, Is.Empty);
        }
    }

}