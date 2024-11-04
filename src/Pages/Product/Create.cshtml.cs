using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class CreateModel : PageModel
    {
        private readonly JsonFileProductService _productService;

        public CreateModel(JsonFileProductService productService)
        {
            _productService = productService;
        }

        // Bind the ProductModel to receive form data
        [BindProperty]
        public ProductModel Product { get; set; }

        /// <summary> Initialize an empty Product model for the form </summary>
        public IActionResult OnGet()
        {
            Product = new ProductModel();
            return Page();
        }

        /// <summary> Handles the form submission to create a new product </summary>
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Call the service to create a new product with default values
            var newProduct = _productService.CreateData();

            // Update the new product with the form data
            newProduct.Title = Product.Title;
            newProduct.Description = Product.Description;
            newProduct.Url = Product.Url;
            newProduct.Image = Product.Image;
            newProduct.FoundingYear = Product.FoundingYear;
            newProduct.Trophies = Product.Trophies;

            // Save the updated product data back to storage
            _productService.UpdateData(newProduct);

            // Redirect to the Index page after successful creation
            return RedirectToPage("/Index");
        }
    }
}
