using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class CreateModel : PageModel
    {
        private readonly JsonFileProductService _productService;

        private readonly TeamVerifier _teamVerifier;

        [ActivatorUtilitiesConstructor]
        public CreateModel(JsonFileProductService productService, TeamVerifier teamVerifier)
        {
            _productService = productService;
            _teamVerifier = teamVerifier;
        }

        // Overloaded Constructor: Takes only ProductService
        public CreateModel(JsonFileProductService productService)
        {
            _productService = productService;
            _teamVerifier = null; // Set to null when not needed
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

            if (_teamVerifier == null)
            {
                ModelState.AddModelError(string.Empty, "Team validation is unavailable. Please contact support.");
                return Page();
            }

            if (_productService.IsDuplicateTeam(Product.Title))
            {

                Sports = _productService.GetAllData()
                    .Where(p => !string.IsNullOrEmpty(p.Sport))
                    .Select(p => p.Sport)
                    .Distinct()
                    .ToList();

                ModelState.AddModelError(string.Empty, $"Team '{Product.Title}' already exists.");
                return Page();
            }

            if (!_teamVerifier.IsValidName(Product.Sport, Product.Title))
            {
                // Reinitialize Sports list before returning the page
                Sports = _productService.GetAllData()
                    .Where(p => !string.IsNullOrEmpty(p.Sport))
                    .Select(p => p.Sport)
                    .Distinct()
                    .ToList();

                ModelState.AddModelError(string.Empty, $"Invalid team name '{Product.Title}' for sport '{Product.Sport}'.");
                return Page();
            }

            if (!Regex.IsMatch(Product.Url, @"^https?:\/\/(www\.)?[a-zA-Z0-9\-\.]+\.com$"))
            {
                Sports = _productService.GetAllData()
                   .Where(p => !string.IsNullOrEmpty(p.Sport))
                   .Select(p => p.Sport)
                   .Distinct()
                   .ToList();
                ModelState.AddModelError("Product.Url", "Invalid URL format. Please ensure the URL starts with http:// or https:// and ends with .com.");
                return Page();
            }


            Product.ProductType = ProductTypeEnum.Team;


            _productService.CreateData(Product);

          
            return RedirectToPage("/Index");
        }

    }

}