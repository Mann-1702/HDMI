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
    public class CreateSportTests
    {
        private CreateSportModel pageModel;
        private Mock<IWebHostEnvironment> mockWebHostEnvironment;
        private JsonFileProductService productService;

        [SetUp]
        public void TestInitialize()
        {
            mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            pageModel = new CreateSportModel(productService);
        }

        [Test]
        public void OnGet_Should_Initialize_Product_And_ExistingSports()
        {
            // Arrange
            var mockProducts = new List<ProductModel>
            {
                new ProductModel { Sport = SportsEnum.NFL },
                new ProductModel { Sport = SportsEnum.NBA }
            };

            productService.CreateData(mockProducts[0]);
            productService.CreateData(mockProducts[1]);

            // Act
            var result = pageModel.OnGet();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.Product, Is.Not.Null);
            Assert.That(pageModel.ExistingSports, Is.Not.Null);
            Assert.That(pageModel.ExistingSports.Contains(SportsEnum.NFL), Is.True);
            Assert.That(pageModel.ExistingSports.Contains(SportsEnum.NBA), Is.True);
        }

        [Test]
        public void OnPost_Invalid_Model_Should_Return_Page()
        {
            // Arrange
            pageModel.ModelState.AddModelError("Sport", "Required");

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        [Test]
        public void OnPost_Duplicate_Sport_Should_Return_Page_With_Error()
        {
            // Arrange
            var existingSport = SportsEnum.NFL;
            productService.CreateData(new ProductModel { Sport = existingSport });

            pageModel.Product = new ProductModel { Sport = existingSport };

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.ModelState[string.Empty].Errors.First().ErrorMessage, Is.EqualTo($"The sport '{existingSport.ToDisplayString()}' already exists."));
        }

        [Test]
        public void OnPost_Undefined_Sport_Should_Return_Page_With_Error()
        {
            // Arrange
            pageModel.Product = new ProductModel { Sport = SportsEnum.Undefined };

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.ModelState[string.Empty].Errors.First().ErrorMessage, Is.EqualTo("Please select a valid sport."));
        }

        
        [Test]
        public void OnPost_Empty_Product_Should_Return_Page_With_Error()
        {
            // Arrange
            pageModel.Product = new ProductModel(); // Empty product

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.ModelState.IsValid, Is.False);
        }
       

        [Test]
        public void OnPost_Product_With_Existing_Sport_Should_Return_Page_With_Duplicate_Error()
        {
            // Arrange
            var existingSport = SportsEnum.NBA;
            productService.CreateData(new ProductModel { Sport = existingSport });

            pageModel.Product = new ProductModel { Sport = existingSport }; // Duplicate sport

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.ModelState[string.Empty].Errors.First().ErrorMessage, Is.EqualTo($"The sport '{existingSport.ToDisplayString()}' already exists."));
        }
        [Test]
        public void HardcodedSports_Should_Contain_All_Valid_Sports_Except_Undefined()
        {
            // Act
            var hardcodedSports = pageModel.HardcodedSports;

            // Assert
            Assert.That(hardcodedSports, Is.Not.Null);
            Assert.That(hardcodedSports.Contains(SportsEnum.Undefined), Is.False);
            Assert.That(hardcodedSports.Contains(SportsEnum.NFL), Is.True);
            Assert.That(hardcodedSports.Contains(SportsEnum.NBA), Is.True);
            Assert.That(hardcodedSports.Contains(SportsEnum.Soccer), Is.True);
        }
        [Test]
        public void OnPost_Duplicate_Sport_Should_Add_ModelState_Error_And_Return_Page()
        {
            // Arrange
            var duplicateSport = SportsEnum.NFL;
            productService.CreateData(new ProductModel { Sport = duplicateSport }); // Mock existing sport

            pageModel.Product = new ProductModel { Sport = duplicateSport }; // Attempt to create the same sport

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.ModelState[string.Empty].Errors.Count, Is.EqualTo(1));
            Assert.That(pageModel.ModelState[string.Empty].Errors.First().ErrorMessage, Is.EqualTo($"The sport '{duplicateSport.ToDisplayString()}' already exists."));
        }
        

        [Test]
        public void OnPost_With_ModelState_Errors_Should_Return_Page()
        {
            // Arrange
            pageModel.ModelState.AddModelError("Sport", "Sport is required");
            pageModel.ModelState.AddModelError("ProductType", "Product type is invalid");

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.ModelState.IsValid, Is.False);
            Assert.That(pageModel.ModelState["Sport"].Errors.First().ErrorMessage, Is.EqualTo("Sport is required"));
            Assert.That(pageModel.ModelState["ProductType"].Errors.First().ErrorMessage, Is.EqualTo("Product type is invalid"));
        }
        [Test]
        public void OnPost_Invalid_SportsEnum_Value_Should_Return_Page()
        {
            // Arrange
            pageModel.Product = new ProductModel { Sport = (SportsEnum)999 }; // Invalid enum value

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.ModelState[string.Empty].Errors.Any(), Is.True);
        }
       

        [Test]
        public void OnPost_Invalid_Enum_Value_Should_Return_Page()
        {
            // Arrange
            pageModel.Product = new ProductModel { Sport = (SportsEnum)999 }; // Invalid enum value

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.ModelState[string.Empty].Errors.Any(), Is.True);
            Assert.That(pageModel.ModelState[string.Empty].Errors.First().ErrorMessage, Is.EqualTo("Please select a valid sport."));
        }

    }
}
