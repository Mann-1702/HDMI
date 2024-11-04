using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class CreateModel : PageModel
    {
        // Data service to interact with product data
        public JsonFileProductService ProductService { get; }

        /// <summary> Default Constructor </summary>
        public CreateModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        // The product model bound to the form data
        [BindProperty]
        public ProductModel Product { get; set; }

        /// <summary> REST Get request to initialize the form with an empty product </summary>
        public void OnGet()
        {
            // Initialize an empty Product model to bind to the form
            Product = new ProductModel();
        }

        /// <summary> Handles the form submission to create a new product </summary>
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Generate a unique ID for the new product
            Product.Id = System.Guid.NewGuid().ToString();

            // Call the service to add the new product to the data source
            ProductService.CreateData(Product);

            // Redirect to the index page after successful creation
            return RedirectToPage("./Index");
        }
    }
}
