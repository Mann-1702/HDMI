using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class DeleteSportModel : PageModel
    {
        // Service for managing product data, such as retrieving and deleting sports
        private readonly JsonFileProductService _productService;

        /// <summary>
        /// Constructor to initialize DeleteSportModel with the required service.
        /// </summary>
        /// <param name="productService">Service to handle product data operations.</param>
        public DeleteSportModel(JsonFileProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Holds the sport data to be deleted.
        /// This property is bound to form input data for confirmation.
        /// </summary>
        [BindProperty]
        public ProductModel Product { get; set; }

        /// <summary>
        /// Handles the GET request to fetch and display sport details for deletion.
        /// </summary>
        /// <param name="id">The ID of the sport to be deleted.</param>
        /// <returns>
        /// The delete confirmation page if the sport is found; otherwise, a NotFound result.
        /// </returns>
        public IActionResult OnGet(string id)
        {
            // Fetch the product with the specified ID and ensure it is of type Sport
            Product = _productService.GetAllData().FirstOrDefault(p => p.Id == id && p.ProductType == ProductTypeEnum.Sport);

            // If no matching sport is found, return a NotFound result
            if (Product == null)
            {
                return NotFound();
            }

            // Render the delete confirmation page with the sport details
            return Page();
        }

        /// <summary>
        /// Handles the POST request to delete the specified sport.
        /// </summary>
        /// <param name="id">The ID of the sport to be deleted.</param>
        /// <returns>
        /// Redirects to the Index page upon successful deletion; otherwise, a NotFound result if the sport does not exist.
        /// </returns>
        public IActionResult OnPost(string id)
        {
            // Fetch the product to ensure it exists and is of type Sport
            var productToDelete = _productService.GetAllData().FirstOrDefault(p => p.Id == id && p.ProductType == ProductTypeEnum.Sport);

            // If the product is not found, return a NotFound result
            if (productToDelete == null)
            {
                return NotFound();
            }

            // Delete the sport from the data source using the service
            _productService.DeleteData(id);

            // Redirect to the Index page after successful deletion
            return RedirectToPage("Index");
        }
    }
}