using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class DeleteSportModel : PageModel
    {
        private readonly JsonFileProductService _productService;

        public DeleteSportModel(JsonFileProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public ProductModel Product { get; set; }

        public IActionResult OnGet(string id)
        {
            // Fetch the product by ID
            Product = _productService.GetAllData().FirstOrDefault(p => p.Id == id && p.ProductType == ProductTypeEnum.Sport);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost(string id)
        {
            // Ensure product exists before attempting to delete
            var productToDelete = _productService.GetAllData().FirstOrDefault(p => p.Id == id && p.ProductType == ProductTypeEnum.Sport);

            if (productToDelete == null)
            {
                return NotFound();
            }

            // Delete the product using the service
            _productService.DeleteData(id);

            // Redirect back to the index page
            return RedirectToPage("Index");
        }
    }
}