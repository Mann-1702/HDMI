using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;
using ContosoCrafts.WebSite.Controllers;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace UnitTests.Controllers
{
    [TestFixture]
    public class MatchControllerTests
    {
        private MatchController _controller;
        private JsonFileMatchService _matchService;
        private string _testWebRootPath;

        [SetUp]
        public void Setup()
        {
            // Create a temporary directory as WebRootPath
            _testWebRootPath = Path.Combine(Path.GetTempPath(), "TestWebRoot");
            Directory.CreateDirectory(_testWebRootPath);

            // Create data directory and sample data file
            string dataDirectory = Path.Combine(_testWebRootPath, "data");
            Directory.CreateDirectory(dataDirectory);

            string matchesJsonPath = Path.Combine(dataDirectory, "matches.json");
            File.WriteAllText(matchesJsonPath, @"
            [
                {
                    ""Id"": ""1"",
                    ""Match"": ""Match 1"",
                    ""Date"": ""2023-11-01"",
                    ""Location"": ""Stadium 1"",
                    ""Team1"": ""Team A"",
                    ""Team2"": ""Team B"",
                    ""Team1_Score"": 1,
                    ""Team2_Score"": 2
                },
                {
                    ""Id"": ""2"",
                    ""Match"": ""Match 2"",
                    ""Date"": ""2023-11-02"",
                    ""Location"": ""Stadium 2"",
                    ""Team1"": ""Team C"",
                    ""Team2"": ""Team D"",
                    ""Team1_Score"": 3,
                    ""Team2_Score"": 3
                }
            ]");

            // Mock IWebHostEnvironment to return the test WebRootPath
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            mockEnvironment.Setup(m => m.WebRootPath).Returns(_testWebRootPath);

            // Initialize JsonFileMatchService
            _matchService = new JsonFileMatchService(mockEnvironment.Object);

            // Initialize controller with matchService
            _controller = new MatchController(_matchService);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up the temporary directory after tests
            if (Directory.Exists(_testWebRootPath))
            {
                Directory.Delete(_testWebRootPath, true);
            }
        }

        [Test]
        public void Get_Should_Return_All_Matches()
        {
            // Act
            var result = _controller.Get();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2), "Expected Get() to return two matches.");
        }
    }
}
