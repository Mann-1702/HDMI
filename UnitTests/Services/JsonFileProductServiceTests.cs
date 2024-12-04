using System.Linq;
using NUnit.Framework;
using ContosoCrafts.WebSite.Models;
using ProtoBuf.WellKnownTypes;

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
        #region TestSetup

        [SetUp]
        public void TestInitialize1()
        {
            // Initialize the TestHelper to provide mock data for ProductService.
            TestHelper.ProductService.CreateData(new ProductModel
            {
                Id = "1",
                Sport = SportsEnum.NFL,
                ProductType = ProductTypeEnum.Sport
            });

            TestHelper.ProductService.CreateData(new ProductModel
            {
                Id = "2",
                Sport = SportsEnum.NBA,
                ProductType = ProductTypeEnum.Sport
            });

            TestHelper.ProductService.CreateData(new ProductModel
            {
                Id = "3",
                Sport = SportsEnum.Soccer,
                ProductType = ProductTypeEnum.Sport // Different product type
            });
        }

        #endregion TestSetup

        [Test]
        public void IsDuplicateSport_Existing_Sport_Should_Return_True()
        {
            // Arrange
            var existingSport = SportsEnum.NFL;

            // Act
            var result = TestHelper.ProductService.IsDuplicateSport(existingSport);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsDuplicateSport_Null_Input_Should_Return_False()
        {
            // Arrange
            var invalidSport = (SportsEnum)(-1); // Invalid enum value

            // Act
            var result = TestHelper.ProductService.IsDuplicateSport(invalidSport);

            // Assert
            Assert.That(result, Is.False);
        }

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
            var product = new ProductModel()
            {
                Id = System.Guid.NewGuid().ToString(),
                Title = "Enter Title",
                Description = "Enter Description",
                Url = "Enter URL",
                Image = "",
            };

            // Create a fresh product
            var newProduct = TestHelper.ProductService.CreateData(product);

            // Ensure the product has no ratings
            newProduct.Ratings = null;

            // Act
            var result = TestHelper.ProductService.AddRating(newProduct.Id, 4);

            // Fetch the updated product by its ID
            var updatedProduct = TestHelper.ProductService.GetAllData().First(p => p.Id == newProduct.Id);

            // Assert
            Assert.That(result, Is.EqualTo(true));

            // Ensure only one rating is added
            Assert.That(updatedProduct.Ratings.Length, Is.EqualTo(1));

            // Ensure the rating is 4
            Assert.That(updatedProduct.Ratings.First(), Is.EqualTo(4));
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
        public void GetFilteredData_Undefined_ProductType_Should_Return_Null()
        {

            // Act
            var result = TestHelper.ProductService.GetFilteredData(ProductTypeEnum.Undefined);

            // Assert
            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public void GetFilteredData_Undefined_SportEnum_Should_Return_Null()
        {

            // Act
            var result = TestHelper.ProductService.GetFilteredData(sportFilter: SportsEnum.Undefined);

            // Assert
            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public void GetFilteredData_Valid_ProductType_Should_Return_Expected_Products()
        {

            // Arrange
            var expectedProducts = TestHelper.ProductService.GetAllData().Where(p => p.ProductType == ProductTypeEnum.Sport);

            // Act
            var result = TestHelper.ProductService.GetFilteredData(ProductTypeEnum.Sport);

            // Assert
            Assert.That(result.Select(p => p.Id), Is.EquivalentTo(expectedProducts.Select(p => p.Id)));
        }

        [Test]
        public void GetFilteredData_Valid_Sport_Should_Return_Expected_Teams()
        {

            // Arrange
            var expectedTeams = TestHelper.ProductService.GetAllData().Where(p => p.Sport == SportsEnum.NFL);

            // Act
            var result = TestHelper.ProductService.GetFilteredData(null, SportsEnum.NFL);

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

        [Test]
        public void GetFilteredData_Valid_ProductType_And_Sport_Should_Return_Filtered_Data()
        {

            // Arrange
            var expectedData = TestHelper.ProductService.GetAllData()
                .Where(p => p.ProductType.ToString() == "Sport" && p.Sport == SportsEnum.NFL);

            // Act
            var result = TestHelper.ProductService.GetFilteredData(ProductTypeEnum.Sport, SportsEnum.NFL);

            // Assert
            Assert.That(result.Select(p => p.Id), Is.EquivalentTo(expectedData.Select(p => p.Id)));
        }

        #endregion GetFilteredData


        #region CreateData
        [Test]
        public void CreateData_Should_Return_Product_With_NonNull_Id()
        {
            // Arrange
            var product = new ProductModel()
            {
                Id = System.Guid.NewGuid().ToString(),
                Title = "Enter Title",
                Description = "Enter Description",
                Url = "Enter URL",
                Image = "",
            };

            // Act
            var result = TestHelper.ProductService.CreateData(product);

            // Assert
            Assert.That(result.Id, Is.Not.Null);
        }

        [Test]
        public void CreateData_Should_Return_Product_With_Expected_Title()
        {

            // Act
            var product = new ProductModel()
            {
                Id = System.Guid.NewGuid().ToString(),
                Title = "Enter Title",
                Description = "Enter Description",
                Url = "Enter URL",
                Image = "",
            };

            var result = TestHelper.ProductService.CreateData(product);

            // Assert
            Assert.That(result.Title, Is.EqualTo("Enter Title"));
        }

        [Test]
        public void CreateData_Should_Call_SaveData_With_New_Product()
        {

            // Arrange
            var product = new ProductModel()
            {
                Id = System.Guid.NewGuid().ToString(),
                Title = "Enter Title",
                Description = "Enter Description",
                Url = "Enter URL",
                Image = "",
            };

            var initialData = TestHelper.ProductService.GetAllData();

            // Act
            var result = TestHelper.ProductService.CreateData(product);

            // Get updated data after the call to SaveData
            var updatedData = TestHelper.ProductService.GetAllData();

            // Assert
            Assert.That(updatedData.Select(p => p.Id), Is.SupersetOf(initialData.Select(p => p.Id).Concat(new[] { result.Id })));
        }

        [Test]
        public void CreateData_Should_Return_Product_With_Expected_Description()
        {

            // Act
            var product = new ProductModel()
            {
                Id = System.Guid.NewGuid().ToString(),
                Title = "Enter Title",
                Description = "Enter Description",
                Url = "Enter URL",
                Image = "",
            };

            var result = TestHelper.ProductService.CreateData(product);

            // Assert
            Assert.That(result.Description, Is.EqualTo("Enter Description"));
        }

        [Test]
        public void CreateData_Should_Add_Product_To_Data_Set()
        {

            // Act
            var product = new ProductModel()
            {
                Id = System.Guid.NewGuid().ToString(),
                Title = "Enter Title",
                Description = "Enter Description",
                Url = "Enter URL",
                Image = "",
            };

            var result = TestHelper.ProductService.CreateData(product);
            var updatedData = TestHelper.ProductService.GetAllData();

            // Assert
            Assert.That(updatedData.Any(p => p.Id == result.Id), Is.True);
        }

        [Test]
        public void CreateData_Valid_Product_Input_Should_Add_Product_To_Data_Set()
        {

            // Arrange
            ProductModel testProduct = new ProductModel();
            testProduct.Title = "Test Title";
            testProduct.Description = "Test Description";
            testProduct.Url = "Test URL";
            testProduct.Image = "Test Image URL";
            testProduct.FoundingYear = 2000;
            testProduct.Trophies = 10;
            testProduct.Sport = SportsEnum.NFL;

            // Act
            TestHelper.ProductService.CreateData(testProduct);
            var data = TestHelper.ProductService.GetAllData();
            var addedProduct = data.Where(p => p.Id == testProduct.Id).First();

            // Assert
            Assert.That(addedProduct.Id, Is.EqualTo(testProduct.Id));
            Assert.That(addedProduct.Title, Is.EqualTo(testProduct.Title));
            Assert.That(addedProduct.Description, Is.EqualTo(testProduct.Description));
            Assert.That(addedProduct.Url, Is.EqualTo(testProduct.Url));
            Assert.That(addedProduct.Image, Is.EqualTo(testProduct.Image));
            Assert.That(addedProduct.FoundingYear, Is.EqualTo(testProduct.FoundingYear));
            Assert.That(addedProduct.Trophies, Is.EqualTo(testProduct.Trophies));
            Assert.That(addedProduct.Sport, Is.EqualTo(testProduct.Sport));
        }

        #endregion CreateData

        #region DeleteData
        [Test]
        public void DeleteData_Should_Remove_Product_With_Given_Id()
        {

            // Arrange
            var product = new ProductModel()
            {
                Id = System.Guid.NewGuid().ToString(),
                Title = "Enter Title",
                Description = "Enter Description",
                Url = "Enter URL",
                Image = "",
            };

            // Create a product and get its ID
            var productToDelete = TestHelper.ProductService.CreateData(product); 
            var initialData = TestHelper.ProductService.GetAllData();
            var initialCount = initialData.Count();

            // Act
            TestHelper.ProductService.DeleteData(productToDelete.Id);
            var updatedData = TestHelper.ProductService.GetAllData();

            // Assert
            Assert.That(updatedData.Any(p => p.Id == productToDelete.Id), Is.False);

            // Ensure the count decreases after deletion
            Assert.That(updatedData.Count(), Is.EqualTo(initialCount - 1));
        }

        [Test]
        public void DeleteData_Should_Not_Modify_Data_If_Id_Not_Found()
        {

            // Arrange
            // Assuming this ID does not exist
            var nonExistentId = "non-existent-id"; 
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


        [Test]
        public void DeleteData_Deleting_Item_Should_Remove_Item_From_Json_File()
        {

            // Arrange
            var productToDelete = TestHelper.ProductService.GetAllData().Last();

            // Act
            TestHelper.ProductService.DeleteData(productToDelete.Id);
            var result = TestHelper.ProductService.GetAllData();

            // Assert
            Assert.That(result.Any(p => p.Id == productToDelete.Id), Is.False);
        }

        #endregion DeleteData
        #region SaveData

        [Test]
        public void SaveData_Should_Persist_Products()
        {

            // Arrange
            var product = new ProductModel()
            {
                Id = System.Guid.NewGuid().ToString(),
                Title = "Enter Title",
                Description = "Enter Description",
                Url = "Enter URL",
                Image = "",
            };

            var products = TestHelper.ProductService.GetAllData();
            var originalCount = products.Count();

            // Act
            var newProduct = TestHelper.ProductService.CreateData(product);
            var updatedProducts = TestHelper.ProductService.GetAllData();

            // Assert
            Assert.That(updatedProducts.Count(), Is.EqualTo(originalCount + 1));
        }

        #endregion SaveData
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
                // Assuming this ID does not exist
                Id = "999",
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

        [Test]
        public void IsDuplicateTeam_Invalid_Null_Input_Should_Return_False()
        {
            // Arrange
            string nullString = null;

            // Act
            var result = TestHelper.ProductService.IsDuplicateTeam(nullString);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

    }

}