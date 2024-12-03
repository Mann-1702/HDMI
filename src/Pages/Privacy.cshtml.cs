using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ContosoCrafts.WebSite.Pages
{
    public class PrivacyModel : PageModel
    {
        // Logger instance used to log important information or errors related to this page.
        private readonly ILogger<PrivacyModel> _logger;

        /// <summary>
        /// Constructor to initialize the logger for the Privacy page model.
        /// </summary>
        /// <param name="logger">ILogger to log any issues or important actions related to the Privacy page.</param>
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handles the GET request for the Privacy page.
        /// This method currently doesn't perform any specific actions, but can be extended in the future.
        /// </summary>
        public void OnGet()
        {
            // No logic is required for the GET request currently.
        }
    }
}