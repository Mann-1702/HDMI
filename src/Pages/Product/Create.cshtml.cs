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
        // Service for managing product data (e.g., CRUD operations)
        private readonly JsonFileProductService _productService;

        // Service for verifying team-related constraints
        private readonly TeamVerifier _teamVerifier;

        /// <summary>
        /// Constructor to initialize the CreateModel with required services.
        /// The [ActivatorUtilitiesConstructor] attribute allows dependency injection.
        /// </summary>
        /// <param name="productService">Service to handle product data.</param>
        /// <param name="teamVerifier">Service to validate team data.</param>
        [ActivatorUtilitiesConstructor]
        public CreateModel(JsonFileProductService productService, TeamVerifier teamVerifier)
        {
            _productService = productService;
            _teamVerifier = teamVerifier;
        }

        /// <summary>
        /// Holds the data for the product (team) being created.
        /// This property is bound to the form input data.
        /// </summary>
        [BindProperty]
        public ProductModel Product { get; set; }

        /// <summary>
        /// A list of unique sports types derived from the SportsEnum.
        /// Used to populate dropdowns or other UI elements.
        /// </summary>
        public List<string> Sports { get; set; }

        /// <summary>
        /// Handles GET requests to initialize the page with an empty product model and a list of sports.
        /// </summary>
        /// <returns>The current page with initialized data.</returns>
        public IActionResult OnGet()
        {
            // Initialize a new empty product for the form
            Product = new ProductModel();

            // Populate the list of sports using the SportsEnum
            Sports = System.Enum.GetNames(typeof(SportsEnum)).ToList();

            return Page();
        }

        /// <summary>
        /// Handles form submission to create a new product (team).
        /// Validates the form input and saves the new product if valid.
        /// </summary>
        /// <returns>
        /// The current page with validation errors if input is invalid, or redirects to the Index page upon success.
        /// </returns>
        public IActionResult OnPost()
        {
            // Check if the form input is valid
            if (!ModelState.IsValid)
            {
                // Reinitialize the sports list for re-rendering the form
                Sports = System.Enum.GetNames(typeof(SportsEnum)).ToList();
                return Page();
            }

            // Check if the team verifier service is available
            if (_teamVerifier == null)
            {
                ModelState.AddModelError(string.Empty, "Team validation is unavailable. Please contact support.");
                Sports = System.Enum.GetNames(typeof(SportsEnum)).ToList(); // Reinitialize Sports list
                return Page();
            }

            // Check if the team already exists
            if (_productService.IsDuplicateTeam(Product.Title))
            {
                Sports = System.Enum.GetNames(typeof(SportsEnum)).ToList(); // Reinitialize Sports list
                ModelState.AddModelError(string.Empty, $"Team '{Product.Title}' already exists.");
                return Page();
            }

            // Validate the team name and sport combination
            if (!_teamVerifier.IsValidName(Product.Sport.ToString(), Product.Title))
            {
                Sports = System.Enum.GetNames(typeof(SportsEnum)).ToList(); // Reinitialize Sports list
                ModelState.AddModelError(string.Empty, $"Invalid team name '{Product.Title}' for sport '{Product.Sport}'.");
                return Page();
            }

            // Set the product type to 'Team' as this form handles team creation
            Product.ProductType = ProductTypeEnum.Team;

            // Save the new product data
            _productService.CreateData(Product);

            // Redirect to the Index page after successful creation
            return RedirectToPage("/Index");
        }
    }
}