using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using NUnit.Framework;
using ContosoCrafts.WebSite.Models;

namespace UnitTests.Models
{
    [TestFixture]
    public class ProductTypeEnumExtensionsTests
    {
        private ProductModel _product;

        [SetUp]
        public void Setup()
        {
            _product = new ProductModel
            {
                Id = "1",
                Image = "https://example.com/image.jpg",
                Url = "https://example.com",
                Title = "Sample Product",
                Description = "This is a sample product description.",
                Ratings = new int[] { 5, 4, 3 },
                ProductType = ProductTypeEnum.Sport,
                Sport = SportsEnum.Soccer,
                FoundingYear = 1990,
                Trophies = 5,
                CommentList = new List<CommentModel>
                {
                    new CommentModel { Id = "1", Comment = "Great product!" },
                    new CommentModel { Id = "2", Comment = "Not bad." }
                }

            };

        }

        [Test]
        public void DisplayName_Sport_Should_Return_Valid_Name()
        {
            // Arrange
            _product.ProductType = ProductTypeEnum.Sport;
            var expectedDisplayName = "Sport / Sport League";

            // Act
            var result = _product.ProductType.DisplayName();

            // Assert
            Assert.That(result, Is.EqualTo(expectedDisplayName));
        }

        [Test]
        public void DisplayName_Team_Should_Return_Valid_Name()
        {
            // Arrange
            _product.ProductType = ProductTypeEnum.Team;
            var expectedDisplayName = "Sport Team";

            // Act
            var result = _product.ProductType.DisplayName();

            // Assert
            Assert.That(result, Is.EqualTo(expectedDisplayName));
        }

        [Test]
        public void DisplayName_Undefined_Should_Return_Empty_String()
        {
            // Arrange
            _product.ProductType = ProductTypeEnum.Undefined;
            var expectedDisplayName = "";

            // Act
            var result = _product.ProductType.DisplayName();

            // Assert
            Assert.That(result, Is.EqualTo(expectedDisplayName));
        }

    }
}