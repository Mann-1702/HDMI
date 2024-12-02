using System.Linq;
using NUnit.Framework;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace UnitTests.Services
{
    public class ErrorCodeHandlerTests
    {
        #region TestSetup

        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup

        #region Error
        [Test]
        public void GetErrorMessage_Unknown_ErrorCode_Should_Return_Default_ErrorMessage()
        {
            // Arrange
            int unknownErrorCode = 999999;
            string defaultErrorMessage = "An unexpected error occurred. Please try again.";

            // Act
            var result = ErrorCodeHandler.GetErrorMessage(unknownErrorCode);

            // Assert
            Assert.That(result, Is.EqualTo(defaultErrorMessage));
        }

        [Test]
        public void GetErrorMessage_ErrorCode_404_Should_Return_Page_Not_Found_Message()
        {
            // Arrange
            int unknownErrorCode = 404;
            string errorMessage = "The page you are looking for could not be found.";

            // Act
            var result = ErrorCodeHandler.GetErrorMessage(unknownErrorCode);

            // Assert
            Assert.That(result, Is.EqualTo(errorMessage));
        }

        [Test]
        public void GetErrorMessage_ErrorCode_401_Should_Return_Invalid_Authorization_Message()
        {
            // Arrange
            int unknownErrorCode = 401;
            string errorMessage = "You are not authorized to view this page. Please log in.";

            // Act
            var result = ErrorCodeHandler.GetErrorMessage(unknownErrorCode);

            // Assert
            Assert.That(result, Is.EqualTo(errorMessage));
        }
        
        #endregion Error

    }

}