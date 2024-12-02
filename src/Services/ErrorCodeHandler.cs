using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace ContosoCrafts.WebSite.Services
{
    /// <summary>
    /// Service class responsible for handling error code information.
    /// Provides methods to retrieve error messages.
    /// </summary>
    public class ErrorCodeHandler
    {
        /// <summary>
        /// Returns a error message correlating to a specific error code
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static string GetErrorMessage(int errorCode)
        {
            // Define custom messages for various status codes
            Dictionary<int, string> errorMessages = new Dictionary<int, string>
            {
                { 404, "The page you are looking for could not be found." },
                { 500, "An internal server error occurred. Please try again later." },
                { 403, "You do not have permission to access this resource." },
                { 400, "The request was invalid. Please check your input and try again." },
                { 401, "You are not authorized to view this page. Please log in." }
            };

            string defaultErrorMessage = "An unexpected error occurred. Please try again.";

            // Return the error message if it exists in dictionary
            if (errorMessages.ContainsKey(errorCode))
            {
                return errorMessages[errorCode];
            }

            // Returns default error message
            return defaultErrorMessage;
        }
    }
}
