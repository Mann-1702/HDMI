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
    public class UpdateTests
    {
        private UpdateModel pageModel;
        private Mock<IWebHostEnvironment> mockWebHostEnvironment;
        private JsonFileProductService productService;

        [SetUp]
        public void TestInitialize()
        {

            // IWebHostEnvironment
            mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            // JsonFileProductService
            productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            // UpdateModel
            pageModel = new UpdateModel(productService);
        }

        [Test]
        public void OnGet_ValidId_Should_Return_Product()
        {

            // Arrange
            string validProductId = "jenlooper-survival"; 

            // Act
            pageModel.OnGet(validProductId);

            // Assert
            Assert.That(pageModel.Product, Is.Not.Null);
            Assert.That(pageModel.Product.Id, Is.EqualTo(validProductId));
        }

        [Test]
        public void OnGet_InvalidId_Should_Return_Null()
        {

            // Arrange
            string invalidProductId = "invalid_id"; 

            // Act
            pageModel.OnGet(invalidProductId);

            // Assert
            Assert.That(pageModel.Product, Is.Null);
        }

        [Test]
        public void OnPost_InvalidModelState_Should_Return_Page()
        {

            // Arrange - 
            pageModel.ModelState.AddModelError("Error", "Invalid Model State");

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        [Test]
        public void OnPost_ValidModelState_Should_Update_Product_And_Redirect()
        {

            // Arrange -
            pageModel.Product = new ProductModel { Id = "jenlooper-survival", Title = "Updated Product", Description = "Updated Description" };

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());

            // 检查是否已更新产品
            var updatedProduct = productService.GetAllData().FirstOrDefault(p => p.Id == "jenlooper-survival");
            Assert.That(updatedProduct.Title, Is.EqualTo("Updated Product"));
            Assert.That(updatedProduct.Description, Is.EqualTo("Updated Description"));
        }

    }

}