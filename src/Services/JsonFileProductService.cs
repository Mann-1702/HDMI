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
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json"); }
        }

        public IEnumerable<ProductModel> GetAllData()
        {
            using(var jsonFileReader = File.OpenText(JsonFileName))
            {
                return JsonSerializer.Deserialize<ProductModel[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
        }

        public IEnumerable<ProductModel> GetFilteredData(string ProductTypeFilter = null, string SportFilter = null)
        {
            IEnumerable<ProductModel> filteredProducts = GetAllData();

            string[] validProductTypes = { "Sport", "Team" };
            string[] validSports = {"NFL", "NBA", "Soccer"};

            // ProductTypeFilter is not null
            if (!string.IsNullOrEmpty(ProductTypeFilter))
            {
                // Checks for valid ProductType
                if (!Array.Exists(validProductTypes, element => element == ProductTypeFilter))
                {
                    return null;
                }

                // Filters products by ProductType using ProductTypeFilter
                filteredProducts = filteredProducts.Where(p => p.ProductType.ToString() == ProductTypeFilter);

            }

            // SportFilter is not null
            if (!string.IsNullOrEmpty(SportFilter))
            {
                // Checks for valid Sport
                if (!Array.Exists(validSports, element => element == SportFilter))
                {
                    return null;
                }

                // Filters products by Sport using SportFilter
                filteredProducts = filteredProducts.Where(p => p.Sport == SportFilter);

            }

            return filteredProducts;
        }



        /// <summary>
        /// Add Rating
        /// 
        /// Take in the product ID and the rating
        /// If the rating does not exist, add it
        /// Save the update
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="rating"></param>
        public bool AddRating(string productId, int rating)
        {
            // If the ProductID is invalid, return
            if (string.IsNullOrEmpty(productId))
            {
                return false;
            }

            var products = GetAllData();

            // Look up the product, if it does not exist, return
            var data = products.FirstOrDefault(x => x.Id.Equals(productId));
            if (data == null)
            {
                return false;
            }

            // Check Rating for boundaries, do not allow ratings below 0
            if (rating < 0)
            {
                return false;
            }

            // Check Rating for boundaries, do not allow ratings above 5
            if (rating > 5)
            {
                return false;
            }

            // Check to see if the rating exist, if there are none, then create the array
            if(data.Ratings == null)
            {
                data.Ratings = new int[] { };
            }

            // Add the Rating to the Array
            var ratings = data.Ratings.ToList();
            ratings.Add(rating);
            data.Ratings = ratings.ToArray();

            // Save the data back to the data store
            SaveData(products);

            return true;
        }

        /// <summary>
        /// Find the data record
        /// Update the fields
        /// Save to the data store
        /// </summary>
        /// <param name="data"></param>
        public ProductModel UpdateData(ProductModel data)
        {
            var products = GetAllData();
            var productData = products.FirstOrDefault(x => x.Id.Equals(data.Id));

            if (productData == null)
            {
                return null;
            }

            // Update the data to the new passed in values
            productData.Title = data.Title;
            productData.Description = data.Description.Trim();
            productData.Url = data.Url;
            productData.Image = data.Image;

            productData.FoundingYear = data.FoundingYear;
            productData.Trophies = data.Trophies;

            productData.CommentList = data.CommentList;

            SaveData(products);

            return productData;
        }

        /// <summary>
        /// Save All products data to storage
        /// </summary>
        private void SaveData(IEnumerable<ProductModel> products)
        {

            using (var outputStream = File.Create(JsonFileName))
            {
                JsonSerializer.Serialize<IEnumerable<ProductModel>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                    products
                );
            }
        }

        /// <summary>
        /// Create a new product using default values
        /// After create the user can update to set values
        /// </summary>
        /// <returns></returns>
        public ProductModel CreateData()
        {
            var data = new ProductModel()
            {
                Id = System.Guid.NewGuid().ToString(),
                Title = "Enter Title",
                Description = "Enter Description",
                Url = "Enter URL",
                Image = "",
            };

            // Get the current set, and append the new record to it because IEnumerable does not have Add
            var dataSet = GetAllData();
            dataSet = dataSet.Append(data);

            SaveData(dataSet);

            return data;
        }


        /// <summary>
        /// Create a new product using default values
        /// After create the user can update to set values
        /// </summary>
        /// <returns></returns>
        public ProductModel CreateData(ProductModel product)
        {
            product.Id = System.Guid.NewGuid().ToString(); 

            var dataSet = GetAllData().ToList();
            dataSet.Add(product);

            SaveData(dataSet);

            return product;
        }


        /// <summary>
        /// Remove the item from the system
        /// </summary>
        /// <returns></returns>
        public void DeleteData(string productId)
        {
            var products = GetAllData().ToList();
            var productToRemove = products.FirstOrDefault(p => p.Id == productId);

            if (productToRemove != null)
            {
                products.Remove(productToRemove);

                // Saves data to json file
                SaveData(products); 
            }
        }



    }
}