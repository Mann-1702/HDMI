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
            Console.WriteLine($"Loading JSON from: {JsonFileName}");
        }


        public IWebHostEnvironment WebHostEnvironment { get; }

        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json"); }
        }

        public IEnumerable<ProductModel> GetAllData()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                var data = JsonSerializer.Deserialize<ProductModel[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });



                return data;
            }
        }


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

        public bool AddRating(string productId, int rating)
        {
            if (string.IsNullOrEmpty(productId) || rating < 0 || rating > 5)
            {
                return false;
            }

            var products = GetAllData();
            var product = products.FirstOrDefault(x => x.Id.Equals(productId));

            if (product == null)
            {
                return false;
            }

            if (product.Ratings == null)
            {
                product.Ratings = new int[] { };
            }

            product.Ratings = product.Ratings.Append(rating).ToArray();
            SaveData(products);

            return true;
        }

        public ProductModel UpdateData(ProductModel data)
        {
            var products = GetAllData();
            var productData = products.FirstOrDefault(x => x.Id.Equals(data.Id));

            if (productData == null)
            {
                return null;
            }

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

        private void SaveData(IEnumerable<ProductModel> products)
        {
            using (var outputStream = File.Create(JsonFileName))
            {
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

        public ProductModel CreateData(ProductModel product)
        {
            product.Id = Guid.NewGuid().ToString();

            var dataSet = GetAllData().ToList();
            dataSet.Add(product);

            SaveData(dataSet);

            return product;
        }

        public void DeleteData(string productId)
        {
            var products = GetAllData().ToList();
            var productToRemove = products.FirstOrDefault(p => p.Id == productId);

            if (productToRemove == null)
            {
                return;
            }

            if (productToRemove.ProductType == ProductTypeEnum.Sport)
            {
                var sport = productToRemove.Sport;

                var teamsToRemove = products
                    .Where(p => p.ProductType == ProductTypeEnum.Team && p.Sport == sport)
                    .ToList();

                foreach (var team in teamsToRemove)
                {
                    products.Remove(team);
                }
            }

            products.Remove(productToRemove);
            SaveData(products);
        }

        public Dictionary<SportsEnum, IEnumerable<ProductModel>> GetTopTeamsByTrophies(int topCount = 3)
        {
            var allProducts = GetAllData();
            var teams = allProducts.Where(p => p.ProductType == ProductTypeEnum.Team);

            return teams
                .GroupBy(t => t.Sport)
                .ToDictionary(
                    group => group.Key,
                    group => group.OrderByDescending(t => t.Trophies).Take(topCount)
                );
        }

        public bool IsDuplicateTeam(string teamName)
        {
            return GetAllData()
                .Where(p => p.ProductType == ProductTypeEnum.Team)
                .Any(p => p.Title.Equals(teamName, StringComparison.OrdinalIgnoreCase));
        }

        public bool IsDuplicateSport(SportsEnum sport)
        {
            return GetAllData()
                .Any(p => p.Sport == sport && p.ProductType == ProductTypeEnum.Sport);
        }
    }
}
