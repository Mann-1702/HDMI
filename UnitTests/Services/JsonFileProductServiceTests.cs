using System.Linq;

using Microsoft.AspNetCore.Mvc;

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
        //[Test]
        //public void AddRating_InValid_....()
        //{
        //    // Arrange

        //    // Act
        //    //var result = TestHelper.ProductService.AddRating(null, 1);

        //    // Assert
        //    //Assert.AreEqual(false, result);
        //}

        // ....

        [Test]
        public void AddRating_InValid_Product_Null_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating(null, 1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddRating_InValid_Product_Empty_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating("", 1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddRating_Valid_Product_Rating_5_Should_Return_True()
        {
            // Arrange

            // Get the First data item
            var data = TestHelper.ProductService.GetAllData().First();
            var countOriginal = data.Ratings.Length;

            // Act
            var result = TestHelper.ProductService.AddRating(data.Id, 5);
            var dataNewList = TestHelper.ProductService.GetAllData().First();

            // Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(dataNewList.Ratings.Length, Is.EqualTo(countOriginal + 1));
            Assert.That(dataNewList.Ratings.Last(), Is.EqualTo(5));
        }
        #endregion AddRating

        #region GetFilteredData
        [Test]
        public void GetFilteredData_Invalid_ProductTypeFilter()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.GetFilteredData("NonExistingProductType");

            // Assert
            Assert.That(result, Is.EqualTo(null));
        }


        [Test]
        public void GetFilteredData_Invalid_SportFilter()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.GetFilteredData(null, "NotExistingSport");

            // Assert
            Assert.That(result, Is.EqualTo(null));
        }


        [Test]
        public void GetFilteredData_Sport_ProductTypeFilter_Should_Return_Sport_Products()
        {
            // Arrange

            // Act
            var data = TestHelper.ProductService.GetAllData();
            data = data.Where(p => p.ProductType.ToString() == "Sport");

            var result = TestHelper.ProductService.GetFilteredData("Sport");

            // Assert
            Assert.That(result.ToString(), Is.EqualTo(data.ToString()));
        }

        [Test]
        public void GetFilteredData_NFL_SportFilter_Should_Return_NFL_Teams()
        {
            // Arrange
            string SportFilter = "NFL";

            // Act
            var data = TestHelper.ProductService.GetAllData();
            data = data.Where(p => p.Sport == SportFilter);

            var result = TestHelper.ProductService.GetFilteredData(null, SportFilter);

            // Assert
            Assert.That(result.ToString(), Is.EqualTo(data.ToString()));
        }
        [Test]
        public void GetFilteredData_NullFilters_Should_Return_AllData()
        {
            // Arrange

            // Act
            var data = TestHelper.ProductService.GetAllData();

            var result = TestHelper.ProductService.GetFilteredData();

            // Assert
            Assert.That(result.ToString(), Is.EqualTo(data.ToString()));
        }

        #endregion AddRating


    }
}