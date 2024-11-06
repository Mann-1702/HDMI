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

namespace UnitTests.Pages.Product
{
    public class DeleteTests
    {
        private DeleteModel pageModel;
        private Mock<IWebHostEnvironment> mockWebHostEnvironment;
        private JsonFileProductService productService;

        [SetUp]
        public void TestInitialize()
        {

            mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns(Directory.GetCurrentDirectory());


            productService = new JsonFileProductService(mockWebHostEnvironment.Object);


            pageModel = new DeleteModel(productService);
        }

        [Test]
        public void OnGet_Valid_ProductId_Should_Return_Delete_Page()
        {
            // Arrange
            string validId = productService.GetAllData().First().Id;

            // Act
            var result = pageModel.OnGet(validId);

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.Product, Is.Not.Null);
            Assert.That(pageModel.Product.Id, Is.EqualTo(validId));
        }

        [Test]
        public void OnGet_InValid_ProductId_Should_Return_Index_Page()
        {
            // Arrange
            string invalidId = "Invalid ID";

            // Act
            var result = pageModel.OnGet(invalidId); 
            var redirectResult = result as RedirectToPageResult;

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            Assert.That(redirectResult.PageName, Is.EqualTo("Index"));

        }


        [Test]
        public void OnPost_Valid_Product_Should_Delete_And_Redirect()
        {
            // Arrange
            string validId = productService.GetAllData().First().Id;

            // Adding the product to the service

            // Act
            pageModel.OnGet(validId);
            var result = pageModel.OnPost();
            var data = productService.GetAllData();

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            var redirectResult = result as RedirectToPageResult;
            Assert.That(redirectResult.PageName, Is.EqualTo("Index"));
            Assert.That(data.Any(p => p.Id == validId), Is.False);
        }

        [Test]
        public void OnPost_Invalid_Null_Product_Should_Redirect_To_Index()
        {
            // Arrange
            string validId = productService.GetAllData().First().Id;

            // Adding the product to the service

            // Act
            var initialCount = productService.GetAllData().Count();
            pageModel.OnGet(validId);
            pageModel.Product.Id = null;

            var result = pageModel.OnPost();
            var endCount = productService.GetAllData().Count();

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            var redirectResult = result as RedirectToPageResult;
            Assert.That(redirectResult.PageName, Is.EqualTo("Index"));
            Assert.That(initialCount, Is.EqualTo(endCount));
        }
    }
}