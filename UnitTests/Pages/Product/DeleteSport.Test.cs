using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;
using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Services;
using ContosoCrafts.WebSite.Models;
using System.IO;
using Moq;


namespace UnitTests.Pages.Product
{
    public class DeleteSportTests
    {
        private DeleteSportModel pageModel;
        private Mock<IWebHostEnvironment> mockWebHostEnvironment;
        private JsonFileProductService productService;

        [SetUp]
        public void TestInitialize()
        {
            mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            pageModel = new DeleteSportModel(productService);

            // Prepopulate the product service with mock data
            productService.CreateData(new ProductModel { Id = "1", Title = "Basketball", ProductType = ProductTypeEnum.Sport });
            productService.CreateData(new ProductModel { Id = "2", Title = "Soccer", ProductType = ProductTypeEnum.Sport });
            
        }

       

        [Test]
        public void OnGet_Invalid_Id_Should_Return_NotFound()
        {
            // Arrange
            var invalidId = "999";

            // Act
            var result = pageModel.OnGet(invalidId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void OnGet_NonSport_Product_Should_Return_NotFound()
        {
            // Arrange
            var nonSportId = "3"; // Chess Board is not a Sport

            // Act
            var result = pageModel.OnGet(nonSportId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
            Assert.That(pageModel.Product, Is.Null);
        }

       

        [Test]
        public void OnPost_Invalid_Id_Should_Return_NotFound()
        {
            // Arrange
            var invalidId = "999";

            // Act
            var result = pageModel.OnPost(invalidId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

       

        
    }
}
