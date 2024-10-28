using System.Linq;
using NUnit.Framework;
using ContosoCrafts.WebSite.Models;
using System;

namespace UnitTests.Pages.Product.AddRating
{
    public class JsonFileProductServiceTests
    {
        #region TestSetup

        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup

        #region AddRating

        [Test]
        public void AddRating_Null_ProductId_Should_Return_False()
        {
            // Act
            var result = TestHelper.ProductService.AddRating(null, 1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddRating_Empty_ProductId_Should_Return_False()
        {
            // Act
            var result = TestHelper.ProductService.AddRating("", 1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddRating_Valid_ProductId_And_Rating_Should_Return_True_And_Add_Rating()
        {
            // Arrange
            var data = TestHelper.ProductService.GetAllData().First();
            var originalRatingCount = data.Ratings.Length;

            // Act
            var result = TestHelper.ProductService.AddRating(data.Id, 5);
            var updatedData = TestHelper.ProductService.GetAllData().First();

            // Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(updatedData.Ratings.Length, Is.EqualTo(originalRatingCount + 1));
            Assert.That(updatedData.Ratings.Last(), Is.EqualTo(5));
        }

        #endregion AddRating

        #region GetFilteredData

        [Test]
        public void GetFilteredData_Invalid_ProductType_Should_Return_Null()
        {
            // Act
            var result = TestHelper.ProductService.GetFilteredData("NonExistingProductType");

            // Assert
            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public void GetFilteredData_Invalid_Sport_Should_Return_Null()
        {
            // Act
            var result = TestHelper.ProductService.GetFilteredData(null, "NonExistingSport");

            // Assert
            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public void GetFilteredData_Valid_ProductType_Should_Return_Expected_Products()
        {
            // Arrange
            var expectedProducts = TestHelper.ProductService.GetAllData().Where(p => p.ProductType.ToString() == "Sport");

            // Act
            var result = TestHelper.ProductService.GetFilteredData("Sport");

            // Assert
            Assert.That(result.Select(p => p.Id), Is.EquivalentTo(expectedProducts.Select(p => p.Id)));
        }

        [Test]
        public void GetFilteredData_Valid_Sport_Should_Return_Expected_Teams()
        {
            // Arrange
            var expectedTeams = TestHelper.ProductService.GetAllData().Where(p => p.Sport == "NFL");

            // Act
            var result = TestHelper.ProductService.GetFilteredData(null, "NFL");

            // Assert
            Assert.That(result.Select(p => p.Id), Is.EquivalentTo(expectedTeams.Select(p => p.Id)));
        }

        [Test]
        public void GetFilteredData_Null_Filters_Should_Return_All_Data()
        {
            // Act
            var expectedData = TestHelper.ProductService.GetAllData();
            var result = TestHelper.ProductService.GetFilteredData();

            // Assert
            Assert.That(result.Select(p => p.Id), Is.EquivalentTo(expectedData.Select(p => p.Id)));
        }

        #endregion GetFilteredData
    }
}
