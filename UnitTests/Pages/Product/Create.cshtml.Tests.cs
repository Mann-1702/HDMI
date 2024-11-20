using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;
using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Services;
using ContosoCrafts.WebSite.Models;
using System.IO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Linq;
using System;

namespace UnitTests.Pages.Product
{
    public class CreateTests
    {
        private CreateModel pageModel;
        private Mock<IWebHostEnvironment> mockWebHostEnvironment;
        private JsonFileProductService productService;
        private TeamVerifier teamVerifier;

        [SetUp]
        public void TestInitialize()
        {
            mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

            // Set up mock web host environment paths
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            // Initialize product service
            productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            // Resolve the JSON file path (names.json should exist in wwwroot/data)
            var jsonFilePath = Path.Combine(mockWebHostEnvironment.Object.WebRootPath, "data", "names.json");

            // Assert that the file exists to avoid runtime errors
            Assert.That(File.Exists(jsonFilePath), Is.True, $"The required JSON file was not found at: {jsonFilePath}");

            // Initialize TeamVerifier with the correct JSON file path
            teamVerifier = new TeamVerifier(jsonFilePath);

            // Initialize the CreateModel page with the required dependencies
            pageModel = new CreateModel(productService, teamVerifier);
        }

        #region OnGet Tests

        [Test]
        public void OnGet_Should_Return_Valid_Create_Page()
        {
            // Act
            var result = pageModel.OnGet();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.Product, Is.Not.Null);
        }

        [Test]
        public void OnGet_Should_Fetch_Valid_Sports()
        {
            // Act
            var result = pageModel.OnGet();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.Sports, Does.Contain("NFL"));
            Assert.That(pageModel.Sports, Does.Contain("NBA"));
            Assert.That(pageModel.Sports, Does.Contain("Soccer"));
        }

        #endregion OnGet Tests

        #region OnPost Tests

        [Test]
        public void OnPost_Invalid_ModelState_Should_Return_CreatePage()
        {
            // Arrange
            pageModel.OnGet();
            pageModel.ModelState.AddModelError("Product.Title", "Title is required");

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

       
        [Test]
        public void OnPost_Invalid_Team_Should_Return_Page_With_Error()
        {
            // Arrange
            pageModel.OnGet();
            pageModel.Product = new ProductModel
            {
                Title = "Invalid Team",
                Sport = "NBA",
                Description = "An invalid NBA team.",
                Url = "http://example.com",
                Image = "http://example.com/image.jpg",
                FoundingYear = 1990,
                Trophies = 3
            };

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.ModelState.IsValid, Is.False);
            Assert.That(pageModel.ModelState[""].Errors.First().ErrorMessage,
                        Is.EqualTo("Invalid team name 'Invalid Team' for sport 'NBA'."));
        }

        [Test]
        public void OnPost_TeamVerifier_Unavailable_Should_Return_Page_With_Error()
        {
            // Arrange
            pageModel = new CreateModel(productService); // TeamVerifier not injected
            pageModel.OnGet();
            pageModel.Product = new ProductModel
            {
                Title = "Any Team",
                Sport = "NBA",
                Description = "A team with unavailable verifier.",
                Url = "http://example.com",
                Image = "http://example.com/image.jpg",
                FoundingYear = 1990,
                Trophies = 3
            };

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.ModelState.IsValid, Is.False);
            Assert.That(pageModel.ModelState[""].Errors.First().ErrorMessage,
                        Is.EqualTo("Team validation is unavailable. Please contact support."));
        }

        [Test]
        public void OnPost_Duplicate_Team_Should_Add_Error_And_Return_Page()
        {
            // Arrange
            pageModel.OnGet();

            // Get a duplicate team product
            pageModel.Product = productService.GetAllData().FirstOrDefault(p => p.ProductType == ProductTypeEnum.Team);

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.ModelState.IsValid, Is.False);
            Assert.That(pageModel.ModelState[""].Errors.First().ErrorMessage,
                        Is.EqualTo($"Team '{pageModel.Product.Title}' already exists."));
        }

        [Test]
        public void OnPost_Valid_Team_Should_Add_Team_And_Return_To_Index()
        {
            // Arrange
            pageModel = new CreateModel(productService, teamVerifier);
            pageModel.OnGet();

            // Get a duplicate team product
            pageModel.Product = productService.GetAllData().LastOrDefault(p => p.ProductType == ProductTypeEnum.Team);
            var productTitle = pageModel.Product.Title;

            // Delete Product
            productService.DeleteData(pageModel.Product.Id);

            // Act
            var result = pageModel.OnPost();
            var products = productService.GetAllData();

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.True);
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            Assert.That(products.Any(p => p.Title == productTitle));
        }

        #endregion OnPost Tests
    }
}
