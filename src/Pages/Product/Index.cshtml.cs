using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="productService"></param>
        public IndexModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        // Data Service
        public JsonFileProductService ProductService { get; }

        // Collection of the Data
        public IEnumerable<ProductModel> Products { get; private set; }

        // Property to capture the search term
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        /// <summary>
        /// REST OnGet
        /// Return all data or filter based on SearchTerm
        /// </summary>
        public void OnGet()
        {
            Products = ProductService.GetAllData();

            // Filter products if SearchTerm is provided
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Products = Products.Where(p =>
                    p.Title.Contains(SearchTerm, System.StringComparison.OrdinalIgnoreCase));
            }


        }
    }
}