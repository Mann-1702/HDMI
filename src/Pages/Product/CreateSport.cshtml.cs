using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class CreateSportModel : PageModel
    {
        private readonly JsonFileProductService _productService;

        [ActivatorUtilitiesConstructor]
        public CreateSportModel(JsonFileProductService productService)
        {
            _productService = productService;

            // Predefined list of sports options
            HardcodedSports = new List<string> { "Soccer", "NBA", "NFL" };
        }

        // Bind ProductModel to receive form data
        [BindProperty]
        public ProductModel Product { get; set; }

        // Fixed list of sports for the dropdown
        public List<string> HardcodedSports { get; }

        // List of existing sports from the database
        public List<string> ExistingSports { get; private set; }

        /// <summary>
        /// Initializes the page and loads the list of existing sports.
        /// </summary>
        public IActionResult OnGet()
        {
            Product = new ProductModel();

            // Load the existing sports from the database
            ExistingSports = _productService.GetAllData()
                .Where(p => !string.IsNullOrEmpty(p.Sport))
                .Select(p => p.Sport)
                .Distinct()
                .ToList();

            return Page();
        }

        /// <summary>
        /// Handles form submission to create a new sport.
        /// </summary>

        public IActionResult OnPost()
        {
            // Refresh the list of existing sports from the database
            ReinitializeExistingSportsList();

            // Check if the form state is valid
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Ensure the user selected a sport
            if (string.IsNullOrWhiteSpace(Product.Title))
            {
                ModelState.AddModelError(string.Empty, "Please select a sport from the dropdown.");
                return Page();
            }

            // Validate if the selected sport already exists in the database
            if (ExistingSports.Any(sport => sport.Equals(Product.Title, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError(string.Empty, $"The sport '{Product.Title}' already exists.");
                return Page();
            }

            // Set the ProductType to Sport
            Product.ProductType = ProductTypeEnum.Sport;

            // Assign the selected sport to the 'Sport' field
            Product.Sport = Product.Title;

            // Save the new sport to the database
            _productService.CreateData(Product);

            // Refresh the list of existing sports to ensure the new sport is included
            ReinitializeExistingSportsList();

            // Redirect to the index page after successful creation
            return RedirectToPage("/Index");
        }


        /// <summary>
        /// Reloads the list of existing sports for the page.
        /// </summary>
        private void ReinitializeExistingSportsList()
        {
            ExistingSports = _productService.GetAllData()
                .Where(p => !string.IsNullOrEmpty(p.Sport)) // Exclude empty or null values
                .Select(p => p.Sport.Trim()) // Trim whitespace for consistency
                .Distinct() // Remove duplicates
                .ToList();
        }

    }
}
