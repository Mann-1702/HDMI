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
    public class CreateTests
    { 
        private CreateModel pageModel;
        private Mock<IWebHostEnvironment> mockWebHostEnvironment;
        private JsonFileProductService productService;

        [SetUp]
        public void TestInitialize()
        {

            mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns(Directory.GetCurrentDirectory());


            productService = new JsonFileProductService(mockWebHostEnvironment.Object);


            pageModel = new CreateModel(productService);
        }

        #region OnGet
        [Test]
        public void OnGet_Should_Return_Valid_Create_Page()
        {

            // Arrange

            // Act
            var result = pageModel.OnGet();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.Product, Is.Not.Null);
        }

        [Test]
        public void OnGet_Should_Fetch_Valid_Sports()
        {
            // Arrange

            // Act
            var result = pageModel.OnGet();


            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.Sports, Does.Contain("NFL"));
            Assert.That(pageModel.Sports, Does.Contain("Soccer"));
            Assert.That(pageModel.Sports, Does.Contain("NBA"));
        }
        #endregion OnGet

        #region OnPost
        [Test]
        public void OnPost_Valid_Product_Should_Create_and_Store_New_Product()
        {

            // Arrange
            ProductModel testProduct = new ProductModel();
            testProduct.Title = "Test Title";
            testProduct.Description = "Test Description";
            testProduct.Url = "google.com";
            testProduct.Image = "google.com";
            testProduct.FoundingYear = 2000;
            testProduct.Trophies = 0;
            testProduct.Sport = "NFL";

            // Act
            var initialCount = productService.GetAllData().ToList().Count();

            pageModel.OnGet();
            pageModel.Product = testProduct;

            var result = pageModel.OnPost();
            var newCount = productService.GetAllData().ToList().Count();

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            Assert.That(initialCount, Is.Not.EqualTo(newCount));
        }


        [Test]
        public void OnPost_Valid_Product_Should_Redirect_To_Index()
        {

            // Arrange
            ProductModel testProduct = new ProductModel();
            testProduct.Title = "Test Title";
            testProduct.Description = "Test Description";
            testProduct.Url = "google.com";
            testProduct.Image = "google.com";
            testProduct.FoundingYear = 2000;
            testProduct.Trophies = 0;
            testProduct.Sport = "NFL";

            // Act
            var initialCount = productService.GetAllData().ToList().Count();

            pageModel.OnGet();
            pageModel.Product = testProduct;

            var result = pageModel.OnPost();
            var newCount = productService.GetAllData().ToList().Count();

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());

            var redirectResult = result as RedirectToPageResult;
            Assert.That(redirectResult.PageName, Is.EqualTo("/Index"));
        }



        [Test]
        public void OnPost_Invalid_ModelState_Should_Return__CreatePage_Page_Result()
        {

            // Arrange

            // Act
            pageModel.OnGet();
            pageModel.ModelState.AddModelError("Product.Title", "Title is required");

            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
        }
        #endregion OnPost

    }
}