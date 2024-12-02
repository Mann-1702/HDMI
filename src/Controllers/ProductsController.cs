using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Controllers
{
    // Marks the class as an API controller, enabling API-specific behaviors like model validation and error responses.
    [ApiController]
    // Specifies the route pattern for this controller: "Products".
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        /// <summary>
        /// Constructor to initialize the ProductsController.
        /// Accepts a JsonFileProductService instance for managing product-related operations.
        /// </summary>
        /// <param name="productService">Service to handle product data operations.</param>
        public ProductsController(JsonFileProductService productService)
        {
            ProductService = productService; // Assigns the injected service to a property for use in the controller.
        }

        /// <summary>
        /// Property to hold the instance of JsonFileProductService.
        /// This service provides methods to retrieve and manipulate product data stored in a JSON file.
        /// </summary>
        public JsonFileProductService ProductService { get; }

        /// <summary>
        /// HTTP GET method to retrieve all product data.
        /// </summary>
        /// <returns>
        /// An IEnumerable of ProductModel containing all the products.
        /// </returns>
        [HttpGet]
        public IEnumerable<ProductModel> Get()
        {
            // Fetches all product data using the ProductService and returns it.
            return ProductService.GetAllData();
        }

        /// <summary>
        /// HTTP PATCH method to update the rating of a product.
        /// </summary>
        /// <param name="request">An object containing the product ID and the new rating value.</param>
        /// <returns>
        /// An ActionResult indicating the status of the operation (200 OK if successful).
        /// </returns>
        [HttpPatch]
        public ActionResult Patch([FromBody] RatingRequest request)
        {
            // Adds the provided rating to the product identified by ProductId.
            ProductService.AddRating(request.ProductId, request.Rating);

            // Returns an HTTP 200 OK response indicating the operation succeeded.
            return Ok();
        }

        /// <summary>
        /// Class to represent a request for updating product ratings.
        /// </summary>
        public class RatingRequest
        {
            /// <summary>
            /// Gets or sets the ID of the product to be rated.
            /// </summary>
            public string ProductId { get; set; }

            /// <summary>
            /// Gets or sets the rating value to be added to the product.
            /// </summary>
            public int Rating { get; set; }
        }
    }
}