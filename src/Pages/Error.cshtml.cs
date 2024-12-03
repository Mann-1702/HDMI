using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Diagnostics;
using System;

namespace ContosoCrafts.WebSite.Pages
{
    // The ResponseCache attribute specifies no caching for the error page response.
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        // RequestId property to store the ID of the current HTTP request for logging or diagnostic purposes.
        public string RequestId { get; set; }

        // ShowRequestId property to check if the RequestId is not empty and should be displayed.
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        // Private logger field to log error messages.
        private readonly ILogger<ErrorModel> _logger;

        // Property to store the user-friendly error message based on the error code.
        public string ErrorMessage { get; set; }

        // Property to store the HTTP error code (e.g., 404, 500).
        public int ErrorCode { get; set; }

        // Property to store the exception message if an exception occurred.
        public string ExceptionMessage { get; set; }

        // Property to store the stack trace of the exception for debugging purposes.
        public string StackTrace { get; set; }

        /// <summary>
        /// Constructor to inject the logger for recording error details.
        /// </summary>
        /// <param name="logger">Logger instance to log error information.</param>
        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Method to handle GET requests to the error page and gather error details.
        /// </summary>
        /// <param name="statusCode">Optional HTTP status code, default is 0 if not provided.</param>
        public void OnGet(int? statusCode = 0)
        {
            // Set the RequestId for the current request, which is used for tracking errors.
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            // Reset ExceptionMessage and StackTrace in case of no exception.
            ExceptionMessage = null;
            StackTrace = null;

            // Obtain the error code from the method parameter (defaults to 0 if null).
            ErrorCode = statusCode.GetValueOrDefault();

            // Retrieve the error message based on the error code.
            ErrorMessage = ErrorCodeHandler.GetErrorMessage(ErrorCode);

            // Check for any exceptions that might have been thrown during the request.
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            // If an exception exists, extract its details.
            if (exceptionFeature?.Error is Exception exception)
            {
                exception = exceptionFeature.Error;
                ExceptionMessage = exception.Message; // Store the exception message.
                StackTrace = exception.StackTrace;   // Store the stack trace for debugging.

                // Clear the default error message if there's an exception.
                ErrorMessage = null;

                // Log the exception details along with the RequestId for troubleshooting.
                _logger.LogError(exception, "Unhandled exception occurred: {RequestId}", RequestId);
            }
        }
    }
}