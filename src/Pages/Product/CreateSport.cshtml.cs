using System;
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
        // Service for managing product data (e.g., CRUD operations)
        private readonly JsonFileProductService _productService;

        /// <summary>
        /// Constructor to initialize CreateSportModel with required services.
        /// Uses [ActivatorUtilitiesConstructor] for dependency injection.
        /// Also initializes the predefined list of sports.
        /// </summary>
        /// <param name="productService">Service to handle product data.</param>
        [ActivatorUtilitiesConstructor]
        public CreateSportModel(JsonFileProductService productService)
        {
            _productService = productService;

            // Initialize predefined list of sports excluding the Undefined value
            HardcodedSports = Enum.GetValues(typeof(SportsEnum))
                .Cast<SportsEnum>()
                .Where(sport => sport != SportsEnum.Undefined) // Exclude Undefined from the list
                .ToList();
        }

        /// <summary>
        /// The product model for binding form data (e.g., new sport creation).
        /// </summary>
        [BindProperty]
        public ProductModel Product { get; set; }

        /// <summary>
        /// Predefined list of sports available for selection, derived from SportsEnum.
        /// Used to populate dropdowns in the UI.
        /// </summary>
        public List<SportsEnum> HardcodedSports { get; }

        /// <summary>
        /// List of sports currently existing in the database.
        /// This is dynamically populated based on saved data.
        /// </summary>
        public List<SportsEnum> ExistingSports { get; private set; }

        /// <summary>
        /// Handles the GET request to initialize the page with necessary data.
        /// Loads an empty product model and populates the list of existing sports.
        /// </summary>
        /// <returns>The current page with initialized data.</returns>
        public IActionResult OnGet()
        {
            // Initialize a new product model for the form
            Product = new ProductModel();

            // Populate the list of existing sports
            ReinitializeExistingSportsList();

            return Page();
        }

        /// <summary>
        /// Handles the POST request to create a new sport.
        /// Validates the form data, checks for duplicates, and saves the new sport.
        /// </summary>
        /// <returns>
        /// The current page with validation errors if the input is invalid, or redirects to the Index page upon success.
        /// </returns>
        public IActionResult OnPost()
        {
            // Refresh the list of existing sports from the database
            ReinitializeExistingSportsList();

            // Check if the form input is valid
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Validate that the user has selected a valid sport
            if (Product.Sport == SportsEnum.Undefined)
            {
                ModelState.AddModelError(string.Empty, "Please select a valid sport.");
                return Page();
            }

            // Check if the selected sport already exists in the database
            if (ExistingSports.Contains(Product.Sport))
            {
                ModelState.AddModelError(string.Empty, $"The sport '{Product.Sport.ToDisplayString()}' already exists.");
                return Page();
            }

            // Set the product type to Sport for this entry
            Product.ProductType = ProductTypeEnum.Sport;

            // Save the new sport to the database using the product service
            _productService.CreateData(Product);

            // Update the existing sports list after successful creation
            ReinitializeExistingSportsList();

            // Redirect to the Index page upon successful submission
            return RedirectToPage("/Index");
        }

        /// <summary>
        /// Reinitializes the list of existing sports by fetching data from the database.
        /// Filters and converts the sports data into a list of distinct SportsEnum values.
        /// </summary>
        private void ReinitializeExistingSportsList()
        {
            ExistingSports = _productService.GetAllData()
                .Where(p => Enum.TryParse<SportsEnum>(p.Sport.ToString(), out _)) // Ensure valid enum values
                .Select(p => Enum.Parse<SportsEnum>(p.Sport.ToString())) // Convert string to SportsEnum
                .Distinct() // Remove duplicates
                .ToList();
        }
    }
}