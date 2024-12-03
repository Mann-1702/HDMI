using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Services;
using ContosoCrafts.WebSite.Models;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class DeleteModel : PageModel
    {
        // Service for managing product data (e.g., retrieval and deletion)
        private readonly JsonFileProductService _productService;

        /// <summary>
        /// Constructor to initialize DeleteModel with the required services.
        /// </summary>
        /// <param name="productService">Service to handle product data operations.</param>
        public DeleteModel(JsonFileProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Holds the product data to be deleted.
        /// This property is bound to the form input data.
        /// </summary>
        [BindProperty]
        public ProductModel Product { get; set; }

        /// <summary>
        /// Handles the GET request to load the delete confirmation page.
        /// Fetches the product details based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the product to be deleted.</param>
        /// <returns>The delete confirmation page, or redirects to the Index page if the product is not found.</returns>
        public IActionResult OnGet(string id)
        {
            // Attempt to retrieve the product by the given ID
            Product = _productService.GetAllData().FirstOrDefault(p => p.Id == id);

            // If the product is not found, redirect to the Index page
            if (Product == null)
            {
                return RedirectToPage("Index");
            }

            // Render the confirmation page with the product details
            return Page();
        }

        /// <summary>
        /// Handles the POST request to delete a product.
        /// Validates the product existence before deletion.
        /// </summary>
        /// <returns>
        /// Redirects to the Index page upon successful deletion or if the product does not exist.
        /// </returns>
        public IActionResult OnPost()
        {
            // Retrieve the product from the service using the provided ID
            var productToDelete = _productService.GetAllData().FirstOrDefault(p => p.Id == Product.Id);

            // If the product is not found, redirect to the Index page
            if (productToDelete == null)
            {
                return RedirectToPage("Index");
            }

            // Perform the deletion using the product service
            _productService.DeleteData(Product.Id);

            // Redirect to the Index page after successful deletion
            return RedirectToPage("Index");
        }
    }
}