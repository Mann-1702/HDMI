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

        // Property to bind the sport name entered by the user
        [BindProperty]
        public string Sport { get; set; }

        // List of existing sports
        public List<string> ExistingSports { get; set; }

        /// <summary> Initialize an empty sport entry for the form </summary>
        public IActionResult OnGet()
        {
            // Retrieve unique sports from the product data
            ExistingSports = _productService.GetAllData()
                .Where(p => !string.IsNullOrEmpty(p.Sport))
                .Select(p => p.Sport)
                .Distinct()
                .ToList();

            return Page();
        }

        /// <summary> Handles form submission to create a new sport </summary>
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // Reinitialize the existing sports list before returning the page
                ExistingSports = _productService.GetAllData()
                    .Where(p => !string.IsNullOrEmpty(p.Sport))
                    .Select(p => p.Sport)
                    .Distinct()
                    .ToList();

                return Page();
            }

            if (string.IsNullOrWhiteSpace(Sport))
            {
                ModelState.AddModelError(string.Empty, "Sport name cannot be empty.");
                ExistingSports = _productService.GetAllData()
                    .Where(p => !string.IsNullOrEmpty(p.Sport))
                    .Select(p => p.Sport)
                    .Distinct()
                    .ToList();

                return Page();
            }

            if (ExistingSports.Contains(Sport))
            {
                ModelState.AddModelError(string.Empty, $"The sport '{Sport}' already exists.");
                ExistingSports = _productService.GetAllData()
                    .Where(p => !string.IsNullOrEmpty(p.Sport))
                    .Select(p => p.Sport)
                    .Distinct()
                    .ToList();

                return Page();
            }

            // Create a placeholder ProductModel to associate the new sport
            var newSportProduct = new ProductModel
            {
                Sport = Sport,
                Title = $"{Sport} (New Sport)", // Default title for the new sport entry
                ProductType = ProductTypeEnum.Sport
            };

            // Save the new sport in the product data
            _productService.CreateData(newSportProduct);

            return RedirectToPage("/Index");
        }
    }
}
