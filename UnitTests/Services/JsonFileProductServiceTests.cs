using System.Linq;
using NUnit.Framework;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Moq;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace UnitTests.Pages.Product
{
    public class JsonFileProductServiceTests
    {
        private JsonFileProductService productService;

        [SetUp]
        public void TestInitialize()
        {
            // Mock WebHostEnvironment to simulate web environment
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

            // Set up mock paths
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            // Initialize JsonFileProductService with the mock environment
            productService = new JsonFileProductService(mockWebHostEnvironment.Object);
        }

        #region AddRating Tests

        [Test]
        public void AddRating_Null_ProductId_Should_Return_False()
        {
            // Act
            var result = productService.AddRating(null, 1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddRating_Empty_ProductId_Should_Return_False()
        {
            // Act
            var result = productService.AddRating("", 1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddRating_Valid_ProductId_And_Rating_Should_Return_True_And_Add_Rating()
        {
            // Arrange
            var data = productService.GetAllData().First();
            var originalRatingCount = data.Ratings.Length;

            // Act
            var result = productService.AddRating(data.Id, 5);
            var updatedData = productService.GetAllData().First();

            // Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(updatedData.Ratings.Length, Is.EqualTo(originalRatingCount + 1));
            Assert.That(updatedData.Ratings.Last(), Is.EqualTo(5));
        }

        [Test]
        public void AddRating_Rating_Below_0_Should_Return_False()
        {
            // Arrange
            var data = productService.GetAllData().First();

            // Act
            var result = productService.AddRating(data.Id, -1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddRating_Rating_Above_5_Should_Return_False()
        {
            // Arrange
            var data = productService.GetAllData().First();

            // Act
            var result = productService.AddRating(data.Id, 6);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddRating_Should_Add_Rating_To_Product_Without_Existing_Ratings()
        {
            // Arrange
            var newProduct = productService.CreateData(new ProductModel
            {
                Title = "Test Product",
                ProductType = ProductTypeEnum.Sport,
                Sport = SportsEnum.NFL
            });
            newProduct.Ratings = null;

            // Act
            var result = productService.AddRating(newProduct.Id, 4);
            var updatedProduct = productService.GetAllData().First(p => p.Id == newProduct.Id);

            // Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(updatedProduct.Ratings.Length, Is.EqualTo(1));
            Assert.That(updatedProduct.Ratings.First(), Is.EqualTo(4));
        }

        #endregion AddRating Tests

        #region GetFilteredData Tests

        [Test]
        public void GetFilteredData_Invalid_ProductType_Should_Return_Null()
        {
            // Act
            var result = productService.GetFilteredData(ProductTypeEnum.Undefined);

            // Assert
            Assert.That(result, Is.Null, "Expected null result for invalid ProductType.");
        }


        [Test]
        public void GetFilteredData_Valid_ProductType_Should_Return_Expected_Products()
        {
            // Arrange
            var expectedProducts = productService.GetAllData()
                .Where(p => p.ProductType == ProductTypeEnum.Sport);

            // Act
            var result = productService.GetFilteredData(ProductTypeEnum.Sport);

            // Assert
            Assert.That(result.Select(p => p.Id), Is.EquivalentTo(expectedProducts.Select(p => p.Id)));
        }

        [Test]
        public void GetFilteredData_Valid_Sport_Should_Return_Expected_Teams()
        {
            // Arrange
            var expectedTeams = productService.GetAllData()
                .Where(p => p.Sport == SportsEnum.NFL);

            // Act
            var result = productService.GetFilteredData(null, SportsEnum.NFL);

            // Assert
            Assert.That(result.Select(p => p.Id), Is.EquivalentTo(expectedTeams.Select(p => p.Id)));
        }


        [Test]
        public void GetFilteredData_Null_Filters_Should_Return_All_Data()
        {
            // Act
            var expectedData = productService.GetAllData();
            var result = productService.GetFilteredData();

            // Assert
            Assert.That(result.Select(p => p.Id), Is.EquivalentTo(expectedData.Select(p => p.Id)));
        }

        [Test]
        public void GetFilteredData_Valid_ProductType_And_Sport_Should_Return_Filtered_Data()
        {
            // Arrange
            var expectedData = productService.GetAllData()
                .Where(p => p.ProductType == ProductTypeEnum.Sport && p.Sport == SportsEnum.NFL);

            // Act
            var result = productService.GetFilteredData(ProductTypeEnum.Sport, SportsEnum.NFL);

            // Assert
            Assert.That(result.Select(p => p.Id), Is.EquivalentTo(expectedData.Select(p => p.Id)),
                "Filtered data does not match expected results.");
        }


        #endregion GetFilteredData Tests

        #region CreateData Tests

        [Test]
        public void CreateData_Should_Return_Product_With_NonNull_Id()
        {
            // Act
            var result = productService.CreateData(new ProductModel
            {
                Title = "Test Product",
                ProductType = ProductTypeEnum.Team,
                Sport = SportsEnum.Soccer
            });

            // Assert
            Assert.That(result.Id, Is.Not.Null);
        }

        [Test]
        public void CreateData_Should_Call_SaveData_With_New_Product()
        {
            // Arrange
            var initialData = productService.GetAllData();

            // Act
            var result = productService.CreateData(new ProductModel
            {
                Title = "Test Product",
                ProductType = ProductTypeEnum.Sport,
                Sport = SportsEnum.NFL
            });

            var updatedData = productService.GetAllData();

            // Assert
            Assert.That(updatedData.Select(p => p.Id), Does.Contain(result.Id));
        }

        #endregion CreateData Tests
    }
}
