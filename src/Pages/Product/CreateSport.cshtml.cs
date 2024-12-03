using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ContosoCrafts.WebSite.Models;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class CreateSportModel : PageModel
    {
        private readonly JsonFileProductService _productService;

        [ActivatorUtilitiesConstructor]
        public CreateSportModel(JsonFileProductService productService)
        {
            _productService = productService;

            // Initialize predefined list of sports using SportsEnum values
            HardcodedSports = Enum.GetValues(typeof(SportsEnum))
                .Cast<SportsEnum>()
                .Where(sport => sport != SportsEnum.Undefined) // Exclude Undefined
                .ToList();
        }

        /// <summary>
        /// The product model for binding form data.
        /// </summary>
        [BindProperty]
        public ProductModel Product { get; set; }

        /// <summary>
        /// List of predefined sports for the dropdown.
        /// </summary>
        public List<SportsEnum> HardcodedSports { get; }

        /// <summary>
        /// List of existing sports from the database.
        /// </summary>
        public List<SportsEnum> ExistingSports { get; private set; }

        /// <summary>
        /// Initializes the page and loads existing sports.
        /// </summary>
        public IActionResult OnGet()
        {
            Product = new ProductModel();

            // Load existing sports from the database
            ReinitializeExistingSportsList();

            return Page();
        }

        /// <summary>
        /// Handles the form submission to create a new sport.
        /// </summary>
        public IActionResult OnPost()
        {
            // Reload the existing sports list
            ReinitializeExistingSportsList();

            // Check if the form state is valid
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Ensure the user selected a sport
            if (Product.Sport == SportsEnum.Undefined)
            {
                ModelState.AddModelError(string.Empty, "Please select a valid sport.");
                return Page();
            }

            // Validate if the sport already exists
            if (ExistingSports.Contains(Product.Sport))
            {
                ModelState.AddModelError(string.Empty, $"The sport '{Product.Sport.ToDisplayString()}' already exists.");
                return Page();
            }

            // Set the product type to Sport
            Product.ProductType = ProductTypeEnum.Sport;

            // Save the new sport to the database
            _productService.CreateData(Product);

            // Refresh the sports list after successful creation
            ReinitializeExistingSportsList();

            // Redirect to the index page
            return RedirectToPage("/Index");
        }

        /// <summary>
        /// Reinitializes the list of existing sports from the database.
        /// </summary>
        private void ReinitializeExistingSportsList()
        {
            ExistingSports = _productService.GetAllData()
                .Where(p => Enum.TryParse<SportsEnum>(p.Sport.ToString(), out _)) // Validate enum values
                .Select(p => Enum.Parse<SportsEnum>(p.Sport.ToString())) // Convert to SportsEnum
                .Distinct()
                .ToList();
        }
    }
}