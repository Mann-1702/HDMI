using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Linq;
using System.Text.RegularExpressions;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class UpdateModel : PageModel
    {
        /// <summary>
        /// Service for managing product data, including updating existing entries.
        /// </summary>
        public JsonFileProductService ProductService { get; }

        /// <summary>
        /// Constructor to initialize UpdateModel with the required service.
        /// </summary>
        /// <param name="productService">Service to handle product data operations.</param>
        public UpdateModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        /// <summary>
        /// Holds the product data to be updated.
        /// This property is bound to the form input data during POST requests.
        /// </summary>
        [BindProperty]
        public ProductModel Product { get; set; }

        /// <summary>
        /// Handles the GET request to load the data for the product to be updated.
        /// </summary>
        /// <param name="id">The ID of the product to be retrieved for update.</param>
        public void OnGet(string id)
        {
            // Retrieve the product by its ID from the service
            Product = ProductService.GetAllData().FirstOrDefault(m => m.Id.Equals(id));
        }

        /// <summary>
        /// Handles the POST request to update the product with the new data.
        /// Validates the model state before saving the updated product.
        /// </summary>
        /// <returns>
        /// Returns the page with validation errors if the input is invalid, or redirects to the Index page upon successful update.
        /// </returns>
        public IActionResult OnPost()
        {
            // Check if the form input data is valid
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Example of validating a URL field (uncomment if needed)
            /*
            if (!Regex.IsMatch(Product.Url, @"^https?:\/\/(www\.)?[a-zA-Z0-9\-\.]+\.[a-z]{2,}$"))
            {
                ModelState.AddModelError("Product.Url", "Invalid URL format. Please ensure the URL starts with http:// or https://.");
                return Page();
            }
            */

            // Save the updated product data using the service
            ProductService.UpdateData(Product);

            // Redirect to the Index page upon successful update
            return RedirectToPage("./Index");
        }
    }
}