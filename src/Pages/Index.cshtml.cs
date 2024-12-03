using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    /// Model class for the Index page, responsible for handling requests and providing data to the Razor page.
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Logger instance for logging information, warnings, and errors.
        /// </summary>
        private readonly ILogger<IndexModel> _logger;

        /// <summary>
        /// Constructor to initialize the IndexModel with the necessary dependencies.
        /// </summary>
        /// <param name="logger">Logger service for logging messages.</param>
        /// <param name="productService">Service to handle product data operations.</param>
        public IndexModel(ILogger<IndexModel> logger, JsonFileProductService productService)
        {
            _logger = logger;
            ProductService = productService;
        }

        /// <summary>
        /// Service to manage product data, such as retrieving and manipulating product information.
        /// </summary>
        public JsonFileProductService ProductService { get; }

        /// <summary>
        /// A collection of ProductModel instances representing the product data to be displayed on the page.
        /// </summary>
        public IEnumerable<ProductModel> Products { get; private set; }

        /// <summary>
        /// Handles GET requests to the Index page.
        /// Retrieves all product data from the ProductService and assigns it to the Products property.
        /// </summary>
        public void OnGet()
        {
            // Fetch all product data and store it in the Products property for display on the page.
            Products = ProductService.GetAllData();
        }
    }
}