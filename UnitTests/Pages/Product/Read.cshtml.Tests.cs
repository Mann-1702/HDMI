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

namespace UnitTests.Pages.Product
{
    public class ReadTests
    {
        private ReadModel pageModel;
        private Mock<IWebHostEnvironment> mockWebHostEnvironment;
        private JsonFileProductService productService;

        [SetUp]
        public void TestInitialize()
        {
            
            mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns(Directory.GetCurrentDirectory());

    
            productService = new JsonFileProductService(mockWebHostEnvironment.Object);

         
            pageModel = new ReadModel(productService);
        }

        [Test]
        public void OnGet_ValidId_Should_Return_Product()
        {
            // Arrange
            string validProductId = "jenlooper-survival"; 

            // Act
            var result = pageModel.OnGet(validProductId);

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.Product, Is.Not.Null);
            Assert.That(pageModel.Product.Id, Is.EqualTo(validProductId));
        }



    }
}
