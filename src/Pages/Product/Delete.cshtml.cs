using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Services;
using ContosoCrafts.WebSite.Models;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class DeleteModel : PageModel
    {
        private readonly JsonFileProductService _productService;

        public DeleteModel(JsonFileProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public ProductModel Product { get; set; }

        // Fetches the product details based on ID for confirmation page
        public IActionResult OnGet(string id)
        {
            // Attempt to find the product by the given ID
            Product = _productService.GetAllData().FirstOrDefault(p => p.Id == id);

            // If product not found, redirect to Index
            if (Product == null)
            {
                return RedirectToPage("Index");
            }

            return Page();
        }

        // Handles the deletion upon form submission
        public IActionResult OnPost()
        {
            // Check if the product is valid
            if (!ModelState.IsValid || Product == null)
            {
                return Page();
            }

            // Call the delete method from the service
            _productService.DeleteData(Product.Id);

            // Redirect to the Index page after deletion
            return RedirectToPage("Index");
        }
    }
}
