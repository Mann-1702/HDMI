using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Collections.Generic;
using System.Linq;

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

        // List to store unique sports types retrieved from existing product data
        public List<string> Sports { get; set; }

        /// <summary> Initialize an empty Product model for the form </summary>
        public IActionResult OnGet()
        {
            Product = new ProductModel();

            // Retrieve unique sports from existing product data
            Sports = _productService.GetAllData()
                .Where(p => !string.IsNullOrEmpty(p.Sport)) // Ensure Sport field is not empty
                .Select(p => p.Sport)
                .Distinct()
                .ToList();

            return Page();
        }

        /// <summary> Handles the form submission to create a new product </summary>
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Product.ProductType = ProductTypeEnum.Team;


            _productService.CreateData(Product);

          
            return RedirectToPage("/Index");
        }

    }
}
