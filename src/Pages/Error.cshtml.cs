using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Diagnostics;
using System;

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

        public string ExceptionMessage { get; set; }

        public string StackTrace { get; set; }

        /// <summary>
        /// Instantiates a logger to record error data
        /// </summary>
        /// <param name="logger"></param>
        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(int? statusCode = 0)
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            ExceptionMessage = null;
            StackTrace = null;

            // Obtains error code (defaults to 0 if null)
            ErrorCode = statusCode.GetValueOrDefault();

            // Handle specific status codes
            ErrorMessage = ErrorCodeHandler.GetErrorMessage(ErrorCode);

            // Checks for exceptions
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (exceptionFeature?.Error is Exception exception)
            {
                exception = exceptionFeature.Error;
                ExceptionMessage = exception.Message;
                StackTrace = exception.StackTrace;

                ErrorMessage = null;

                // Log the exception
                _logger.LogError(exception, "Unhandled exception occurred: {RequestId}", RequestId);
            }


        }

    }

}