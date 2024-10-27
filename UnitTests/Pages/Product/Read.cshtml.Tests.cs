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
            // 设置 Mock 的 IWebHostEnvironment 并提供路径
            mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            // 实例化 JsonFileProductService
            productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            // 创建 ReadModel 实例并传入产品服务
            pageModel = new ReadModel(productService);
        }
        [Test]
        public void OnGet_ValidId_Should_Return_Product()
        {
            // Arrange
            string validProductId = "jenlooper-survival"; // 假设这个 ID 在 products.json 中存在

            // Act
            var result = pageModel.OnGet(validProductId);

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.Product, Is.Not.Null);
            Assert.That(pageModel.Product.Id, Is.EqualTo(validProductId));
        }

        [Test]
        public void OnGet_InvalidId_Should_Return_NotFound()
        {
            // Arrange
            string invalidProductId = "invalid_id"; // 假设这个 ID 不存在

            // Act
            var result = pageModel.OnGet(invalidProductId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
            Assert.That(pageModel.Product, Is.Null);
        }
    }
}
