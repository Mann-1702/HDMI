using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

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

        // Bind the ProductModel to receive form data
        [BindProperty]
        public ProductModel Product { get; set; }

        // List to store unique sports types retrieved from SportsEnum
        public List<string> Sports { get; set; }

        /// <summary> Initialize an empty Product model for the form </summary>
        public IActionResult OnGet()
        {
            Product = new ProductModel();

            // Retrieve unique sports from the SportsEnum
            Sports = System.Enum.GetNames(typeof(SportsEnum)).ToList();

            return Page();
        }

        /// <summary> Handles the form submission to create a new product </summary>
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Sports = System.Enum.GetNames(typeof(SportsEnum)).ToList(); // Reinitialize Sports list
                return Page();
            }

            if (_teamVerifier == null)
            {
                ModelState.AddModelError(string.Empty, "Team validation is unavailable. Please contact support.");
                Sports = System.Enum.GetNames(typeof(SportsEnum)).ToList(); // Reinitialize Sports list
                return Page();
            }

            if (_productService.IsDuplicateTeam(Product.Title))
            {
                Sports = System.Enum.GetNames(typeof(SportsEnum)).ToList(); // Reinitialize Sports list
                ModelState.AddModelError(string.Empty, $"Team '{Product.Title}' already exists.");
                return Page();
            }

            if (!_teamVerifier.IsValidName(Product.Sport.ToString(), Product.Title))
            {
                Sports = System.Enum.GetNames(typeof(SportsEnum)).ToList(); // Reinitialize Sports list
                ModelState.AddModelError(string.Empty, $"Invalid team name '{Product.Title}' for sport '{Product.Sport}'.");
                return Page();
            }

            // Set the ProductType to Team
            Product.ProductType = ProductTypeEnum.Team;

            // Save the new team data
            _productService.CreateData(Product);

            return RedirectToPage("/Index");
        }
    }
}
