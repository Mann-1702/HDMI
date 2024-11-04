using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using NUnit.Framework;
using ContosoCrafts.WebSite.Models;

namespace UnitTests.Models
{
    [TestFixture]
    public class ProductModelTests
    {
        private ProductModel _product;

        [SetUp]
        public void Setup()
        {
            _product = new ProductModel
            {
                Id = "1",
                Maker = "Maker",
                Image = "https://example.com/image.jpg",
                Url = "https://example.com",
                Title = "Sample Product",
                Description = "This is a sample product description.",
                Ratings = new int[] { 5, 4, 3 },
                ProductType = ProductTypeEnum.Sport,
                Sport = "Soccer",
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
        public void ProductModel_Should_Have_Valid_Properties()
        {
            Assert.That(_product.Id, Is.EqualTo("1"));
            Assert.That(_product.Maker, Is.EqualTo("Maker"));
            Assert.That(_product.Image, Is.EqualTo("https://example.com/image.jpg"));
            Assert.That(_product.Url, Is.EqualTo("https://example.com"));
            Assert.That(_product.Title, Is.EqualTo("Sample Product"));
            Assert.That(_product.Description, Is.EqualTo("This is a sample product description."));
            Assert.That(_product.Ratings, Is.EqualTo(new int[] { 5, 4, 3 }));
            Assert.That(_product.ProductType, Is.EqualTo(ProductTypeEnum.Sport));
            Assert.That(_product.Sport, Is.EqualTo("Soccer"));
            Assert.That(_product.FoundingYear, Is.EqualTo(1990));
            Assert.That(_product.Trophies, Is.EqualTo(5));
            Assert.That(_product.CommentList.Count, Is.EqualTo(2));
        }

        [Test]
        public void ToString_Should_Return_Valid_Json()
        {
            var jsonString = _product.ToString();
            Assert.That(() => JsonSerializer.Deserialize<ProductModel>(jsonString), Throws.Nothing);
        }

        [Test]
        public void Title_Should_Fail_Validation_When_Empty()
        {
            _product.Title = string.Empty;
            var validationResults = ValidateModel(_product);
            Assert.That(validationResults, Has.Some.Matches<ValidationResult>(v => v.ErrorMessage.Contains("Title is required")));
        }

        [Test]
        public void Title_Should_Fail_Validation_When_Too_Long()
        {
            _product.Title = new string('A', 34); // Exceeds 33 characters
            var validationResults = ValidateModel(_product);
            Assert.That(validationResults, Has.Some.Matches<ValidationResult>(v => v.ErrorMessage.Contains("The Title should have a length of more than")));
        }

        [Test]
        public void FoundingYear_Should_Fail_Validation_When_Out_Of_Range()
        {
            _product.FoundingYear = 101; // Exceeds 100
            var validationResults = ValidateModel(_product);
            Assert.That(validationResults, Has.Some.Matches<ValidationResult>(v => v.ErrorMessage.Contains("Value for FoundingYear must be between")));
        }

        [Test]
        public void Image_Should_Fail_Validation_When_Invalid_Url()
        {
            _product.Image = "invalid_url";
            var validationResults = ValidateModel(_product);
            Assert.That(validationResults, Has.Some.Matches<ValidationResult>(v => v.ErrorMessage.Contains("Please enter a valid image URL")));
        }

        [Test]
        public void Url_Should_Fail_Validation_When_Invalid_Url()
        {
            _product.Url = "invalid_url";
            var validationResults = ValidateModel(_product);
            Assert.That(validationResults, Has.Some.Matches<ValidationResult>(v => v.ErrorMessage.Contains("Please enter a valid URL")));
        }

        private static List<ValidationResult> ValidateModel(ProductModel model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}
