using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class CreateSportModel : PageModel
    {
        private readonly JsonFileProductService _productService;

        [ActivatorUtilitiesConstructor]
        public CreateSportModel(JsonFileProductService productService)
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

        /// <summary> Handles the form submission to create a new sport </summary>
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // Reinitialize Sports list before returning the page
                Sports = _productService.GetAllData()
                    .Where(p => !string.IsNullOrEmpty(p.Sport))
                    .Select(p => p.Sport)
                    .Distinct()
                    .ToList();

                return Page();
            }

            if (string.IsNullOrWhiteSpace(Product.Title))
            {
                ModelState.AddModelError(string.Empty, "Sport title cannot be empty.");
                ReinitializeSportsList();
                return Page();
            }

            if (_productService.IsDuplicateSport(Product.Title))
            {
                ModelState.AddModelError(string.Empty, $"The sport '{Product.Title}' already exists.");
                ReinitializeSportsList();
                return Page();
            }

            // Set the ProductType to Sport
            Product.ProductType = ProductTypeEnum.Sport;

            // Set the 'Sport' field to the value of the 'Title' field
            Product.Sport = Product.Title;  // **This is the key change**

            // Save the new sport in the product data
            _productService.CreateData(Product);

            return RedirectToPage("/Index");
        }

        /// <summary> Reinitializes the Sports list for the page </summary>
        private void ReinitializeSportsList()
        {
            Sports = _productService.GetAllData()
                .Where(p => !string.IsNullOrEmpty(p.Sport))
                .Select(p => p.Sport)
                .Distinct()
                .ToList();
        }
    }
}
