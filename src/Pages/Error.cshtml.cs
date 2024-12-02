using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;
        public string ErrorMessage { get; set; }

        public int ErrorCode { get; set; }

        /// <summary>
        /// Instantiates a logger to record error data
        /// </summary>
        /// <param name="logger"></param>
        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(int? statusCode = null)
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            // Default message for unexpected errors
            ErrorMessage = "An unexpected error occurred.";

            ErrorCode = statusCode.GetValueOrDefault();

            // Handle specific status codes
            if (statusCode.HasValue)
            {
                ErrorMessage = ErrorCodeHandler.GetErrorMessage(ErrorCode);
            }
        }

    }

}