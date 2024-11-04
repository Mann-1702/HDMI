using System.Linq;
using NUnit.Framework;
using ContosoCrafts.WebSite.Models;
using System;
using Moq;
using System.Collections.Generic;

namespace UnitTests.Pages.Product
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
        [Test]
        public void AddRating_Rating_Below_0_Should_Return_False()
        {
            // Arrange
            var data = TestHelper.ProductService.GetAllData().First();

            // Act
            var result = TestHelper.ProductService.AddRating(data.Id, -1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }
        [Test]
        public void AddRating_Rating_Above_5_Should_Return_False()
        {
            // Arrange
            var data = TestHelper.ProductService.GetAllData().First();

            // Act
            var result = TestHelper.ProductService.AddRating(data.Id, 6);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }
        [Test]
        public void AddRating_Should_Add_Rating_To_Product_Without_Existing_Ratings()
        {
            // Arrange
            var newProduct = TestHelper.ProductService.CreateData(); // Create a fresh product
            newProduct.Ratings = null; // Ensure the product has no ratings

            // Act
            var result = TestHelper.ProductService.AddRating(newProduct.Id, 4);

            // Fetch the updated product by its ID
            var updatedProduct = TestHelper.ProductService.GetAllData().First(p => p.Id == newProduct.Id);

            // Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(updatedProduct.Ratings.Length, Is.EqualTo(1)); // Ensure only one rating is added
            Assert.That(updatedProduct.Ratings.First(), Is.EqualTo(4)); // Ensure the rating is 4
        }

        [Test]
        public void AddRating_Invalid_ProductId_Should_Return_False()
        {
            // Act
            var result = TestHelper.ProductService.AddRating("non-existent-id", 3);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddRating_Rating_Of_0_Should_Add_Successfully()
        {
            // Arrange
            var data = TestHelper.ProductService.GetAllData().First();
            var originalRatingCount = data.Ratings.Length;

            // Act
            var result = TestHelper.ProductService.AddRating(data.Id, 0);
            var updatedData = TestHelper.ProductService.GetAllData().First();

            // Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(updatedData.Ratings.Length, Is.EqualTo(originalRatingCount + 1));
            Assert.That(updatedData.Ratings.Last(), Is.EqualTo(0));
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


        #region CreateData
        [Test]
        public void CreateData_Should_Return_Product_With_NonNull_Id()
        {
            // Act
            var result = TestHelper.ProductService.CreateData();

            // Assert
            Assert.That(result.Id, Is.Not.Null);
        }
        [Test]
        public void CreateData_Should_Return_Product_With_Expected_Title()
        {
            // Act
            var result = TestHelper.ProductService.CreateData();

            // Assert
            Assert.That(result.Title, Is.EqualTo("Enter Title"));
        }
        [Test]
        public void CreateData_Should_Call_SaveData_With_New_Product()
        {
            // Arrange
            var initialData = TestHelper.ProductService.GetAllData();

            // Act
            var result = TestHelper.ProductService.CreateData();

            // Get updated data after the call to SaveData
            var updatedData = TestHelper.ProductService.GetAllData();

            // Assert
            Assert.That(updatedData.Select(p => p.Id), Is.SupersetOf(initialData.Select(p => p.Id).Concat(new[] { result.Id })));
        }
        [Test]
        public void CreateData_Should_Return_Product_With_Expected_Description()
        {
            // Act
            var result = TestHelper.ProductService.CreateData();

            // Assert
            Assert.That(result.Description, Is.EqualTo("Enter Description"));
        }
        [Test]
        public void CreateData_Should_Add_Product_To_Data_Set()
        {
            // Act
            var result = TestHelper.ProductService.CreateData();
            var updatedData = TestHelper.ProductService.GetAllData();

            // Assert
            Assert.That(updatedData.Any(p => p.Id == result.Id), Is.True);
        }
        #endregion CreateData

        #region DeleteData
        [Test]
        public void DeleteData_Should_Remove_Product_With_Given_Id()
        {
            // Arrange
            var idToDelete = "123";
            var initialData = TestHelper.ProductService.GetAllData();

            // Act
            TestHelper.ProductService.DeleteData(idToDelete);
            var updatedData = TestHelper.ProductService.GetAllData();

            // Assert
            Assert.That(updatedData.Any(p => p.Id == idToDelete), Is.False);
        }
        
        [Test]
        public void DeleteData_Should_Not_Modify_Data_If_Id_Not_Found()
        {
            // Arrange
            var nonExistentId = "999";  // Assuming this ID does not exist
            var initialData = TestHelper.ProductService.GetAllData();

            // Act
            TestHelper.ProductService.DeleteData(nonExistentId);
            var updatedData = TestHelper.ProductService.GetAllData();

            // Assert
            Assert.That(updatedData.Select(p => p.Id), Is.EquivalentTo(initialData.Select(p => p.Id)));
        }


        [Test]
        public void DeleteData_Should_Not_Throw_Exception_If_Product_Not_Found()
        {
            // Arrange
            var nonExistentId = "999";

            // Act & Assert
            Assert.DoesNotThrow(() => TestHelper.ProductService.DeleteData(nonExistentId),
                                "DeleteData should not throw an exception if the product does not exist.");
        }

        #endregion DeleteData

        #region UpdateData

        [Test]
        public void UpdateData_Should_Update_Existing_Product()
        {
            // Arrange
            var existingProduct = TestHelper.ProductService.GetAllData().First();
            var updatedProduct = new ProductModel
            {
                Id = existingProduct.Id,
                Title = "Updated Title",
                Description = "Updated Description",
                Url = "updated-url.com",
                Image = "updated-image.jpg",
                FoundingYear = 2020,
                Trophies = 5
            };

            // Act
            var result = TestHelper.ProductService.UpdateData(updatedProduct);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Title, Is.EqualTo("Updated Title"));
                Assert.That(result.Description, Is.EqualTo("Updated Description"));
                Assert.That(result.Url, Is.EqualTo("updated-url.com"));
                Assert.That(result.Image, Is.EqualTo("updated-image.jpg"));
                Assert.That(result.FoundingYear, Is.EqualTo(2020));
                Assert.That(result.Trophies, Is.EqualTo(5));
            });
        }
        [Test]
        public void UpdateData_Should_Return_Null_If_Product_Not_Found()
        {
            // Arrange
            var nonExistentProduct = new ProductModel
            {
                Id = "999",  // Assuming this ID does not exist
                Title = "Non-existent Product"
            };

            // Act
            var result = TestHelper.ProductService.UpdateData(nonExistentProduct);

            // Assert
            Assert.That(result, Is.Null);
        }
        [Test]
        public void UpdateData_Should_Trim_Description()
        {
            // Arrange
            var existingProduct = TestHelper.ProductService.GetAllData().First();
            var updatedProduct = new ProductModel
            {
                Id = existingProduct.Id,
                Title = "Updated Title",
                Description = "  Updated Description with spaces  "
            };

            // Act
            var result = TestHelper.ProductService.UpdateData(updatedProduct);

            // Assert
            Assert.That(result.Description, Is.EqualTo("Updated Description with spaces"));
        }

        
       



        #endregion UpdateData
    }
}