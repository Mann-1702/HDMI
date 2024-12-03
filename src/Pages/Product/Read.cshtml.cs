using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class ReadModel : PageModel
    {
        // Service for managing product data, including retrieval operations
        private readonly JsonFileProductService _productService;

        /// <summary>
        /// Constructor to initialize ReadModel with the required service.
        /// </summary>
        /// <param name="productService">Service to handle product data operations.</param>
        public ReadModel(JsonFileProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Holds the product (team) data retrieved for display.
        /// </summary>
        public ProductModel Product { get; set; }

        /// <summary>
        /// Handles the GET request to retrieve and display a product (team) by its title.
        /// </summary>
        /// <param name="teamTitle">The title of the team to be retrieved.</param>
        /// <returns>
        /// The page displaying the team details if found; otherwise, a NotFound result.
        /// </returns>
        public IActionResult OnGet(string teamTitle)
        {
            // Attempt to retrieve the product (team) with the specified title
            Product = _productService.GetAllData().FirstOrDefault(p => p.Title == teamTitle);

            // If the team is not found, return a NotFound result
            if (Product == null)
            {
                return NotFound();
            }

            // Render the page displaying the team details
            return Page();
        }
    }
}