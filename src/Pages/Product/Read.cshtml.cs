using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class ReadModel : PageModel
    {
        private readonly JsonFileProductService _productService;

        public ReadModel(JsonFileProductService productService)
        {
            _productService = productService;
        }

        public ProductModel Product { get; set; }

        public IActionResult OnGet(string teamTitle)
        {
            Product = _productService.GetAllData().FirstOrDefault(p => p.Title == teamTitle);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
