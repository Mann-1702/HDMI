using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Hosting;

namespace ContosoCrafts.WebSite.Services
{
    public class JsonFileProductService
    {
        // Constructor that initializes the JsonFileProductService and sets up the environment for file access.
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
            Console.WriteLine($"Loading JSON from: {JsonFileName}");
        }

        // Property for accessing the hosting environment in which the application is running (used for file paths).
        public IWebHostEnvironment WebHostEnvironment { get; }

        // Property to get the path to the JSON file that stores product data.
        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json"); }
        }

        // Method to retrieve all product data from the JSON file.
        public IEnumerable<ProductModel> GetAllData()
        {
            // Open the JSON file and read its contents
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                // Deserialize the JSON content into a list of ProductModel objects
                var data = JsonSerializer.Deserialize<ProductModel[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return data;
            }
        }

        // Method to retrieve filtered product data based on the given product type and/or sport filter.
        public IEnumerable<ProductModel> GetFilteredData(ProductTypeEnum? productTypeFilter = null, SportsEnum? sportFilter = null)
        {
            var filteredProducts = GetAllData();

            // Apply ProductType filter if provided
            if (productTypeFilter.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => p.ProductType == productTypeFilter.Value);
            }

            // Apply Sport filter if provided
            if (sportFilter.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => p.Sport == sportFilter.Value);
            }

            return filteredProducts;
        }

        // Method to add a rating to a specific product.
        public bool AddRating(string productId, int rating)
        {
            // Validate the input: productId should not be empty, and rating should be between 0 and 5
            if (string.IsNullOrEmpty(productId) || rating < 0 || rating > 5)
            {
                return false;
            }

            // Get all product data
            var products = GetAllData();
            // Find the product by its ID
            var product = products.FirstOrDefault(x => x.Id.Equals(productId));

            // If the product doesn't exist, return false
            if (product == null)
            {
                return false;
            }

            // Initialize Ratings if it is null
            if (product.Ratings == null)
            {
                product.Ratings = new int[] { };
            }

            // Append the new rating to the product's rating list
            product.Ratings = product.Ratings.Append(rating).ToArray();
            // Save the updated product list
            SaveData(products);

            return true;
        }

        // Method to update an existing product's data.
        public ProductModel UpdateData(ProductModel data)
        {
            // Get all product data
            var products = GetAllData();
            // Find the existing product by its ID
            var productData = products.FirstOrDefault(x => x.Id.Equals(data.Id));

            // If the product doesn't exist, return null
            if (productData == null)
            {
                return null;
            }

            // Update the properties of the existing product with the new data
            productData.Title = data.Title;
            productData.Description = data.Description.Trim();
            productData.Url = data.Url;
            productData.Image = data.Image;
            productData.FoundingYear = data.FoundingYear;
            productData.Trophies = data.Trophies;
            productData.CommentList = data.CommentList;

            // Save the updated product data
            SaveData(products);
            return productData;
        }

        // Method to save updated product data to the JSON file.
        private void SaveData(IEnumerable<ProductModel> products)
        {
            // Open the file for writing
            using (var outputStream = File.Create(JsonFileName))
            {
                // Serialize the product data to the JSON file
                JsonSerializer.Serialize(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                    products
                );
            }
        }

        // Method to create and add a new product.
        public ProductModel CreateData(ProductModel product)
        {
            // Generate a new ID for the product
            product.Id = Guid.NewGuid().ToString();

            // Get the current list of products
            var dataSet = GetAllData().ToList();
            // Add the new product to the list
            dataSet.Add(product);

            // Save the updated product list
            SaveData(dataSet);

            return product;
        }

        // Method to delete a product by its ID.
        public void DeleteData(string productId)
        {
            // Get all products
            var products = GetAllData().ToList();
            // Find the product to be removed by its ID
            var productToRemove = products.FirstOrDefault(p => p.Id == productId);

            // If the product doesn't exist, exit the method
            if (productToRemove == null)
            {
                return;
            }

            // If the product is of type "Sport", also remove associated teams
            if (productToRemove.ProductType == ProductTypeEnum.Sport)
            {
                var sport = productToRemove.Sport;

                // Find all teams associated with the sport and remove them
                var teamsToRemove = products
                    .Where(p => p.ProductType == ProductTypeEnum.Team && p.Sport == sport)
                    .ToList();

                foreach (var team in teamsToRemove)
                {
                    products.Remove(team);
                }
            }

            // Remove the product from the list
            products.Remove(productToRemove);
            // Save the updated product list
            SaveData(products);
        }

        // Method to retrieve the top teams based on their trophy count.
        public Dictionary<SportsEnum, IEnumerable<ProductModel>> GetTopTeamsByTrophies(int topCount = 3)
        {
            // Get all products
            var allProducts = GetAllData();
            // Filter for products that are teams
            var teams = allProducts.Where(p => p.ProductType == ProductTypeEnum.Team);

            // Group teams by sport and select top teams based on their trophy count
            return teams
                .GroupBy(t => t.Sport)
                .ToDictionary(
                    group => group.Key,
                    group => group.OrderByDescending(t => t.Trophies).Take(topCount)
                );
        }

        // Method to check if a team already exists (based on the team name).
        public bool IsDuplicateTeam(string teamName)
        {
            // Check if a team with the given name already exists in the list
            return GetAllData()
                .Where(p => p.ProductType == ProductTypeEnum.Team)
                .Any(p => p.Title.Equals(teamName, StringComparison.OrdinalIgnoreCase));
        }

        // Method to check if a sport already exists.
        public bool IsDuplicateSport(SportsEnum sport)
        {
            // Check if a sport already exists in the product data
            return GetAllData()
                .Any(p => p.Sport == sport && p.ProductType == ProductTypeEnum.Sport);
        }
    }
}