using System;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Moq;
using ContosoCrafts.WebSite.Pages;
using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace UnitTests.Pages.Error
{
    public class ErrorTests
    {
        #region TestSetup
        public static ErrorModel pageModel;

        [SetUp]
        public void TestInitialize()
        {
            var MockLoggerDirect = Mock.Of<ILogger<ErrorModel>>();

            pageModel = new ErrorModel(MockLoggerDirect)
            {
                PageContext = TestHelper.PageContext,
                TempData = TestHelper.TempData,
            };


        }

        #endregion TestSetup

        #region OnGet
        [Test]
        public void OnGet_Valid_Activity_Set_Should_Return_RequestId()
        {

            // Arrange
            Activity activity = new Activity("activity");
            activity.Start();

            // Act
            pageModel.OnGet();

            // Reset
            activity.Stop();

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(pageModel.RequestId, Is.EqualTo(activity.Id));
        }

        [Test]
        public void OnGet_InValid_Activity_Null_Should_Return_TraceIdentifier()
        {

            // Arrange

            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(pageModel.ShowRequestId, Is.EqualTo(true));
            Assert.That(pageModel.RequestId, Is.EqualTo("trace"));
        }

        #endregion OnGet

        #region ErrorMessage
        [Test]
        public void ErrorMessage_Should_Store_and_Retreive_Error_Message()
        {
            // Arrange
            int errorCode = 404;
            string expectedErrorMessage = "The page you are looking for could not be found.";

            // Act
            pageModel.OnGet(errorCode);
            var errorMessage = pageModel.ErrorMessage;

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(errorMessage, Is.EqualTo(expectedErrorMessage));
        }
        #endregion ErrorMessage

        #region ErrorCode
        [Test]
        public void ErrorCode_Should_Store_and_Retreive_Error_Code()
        {
            // Arrange
            int expectedErrorCode = 404;

            // Act
            pageModel.OnGet(expectedErrorCode);
            var errorCode = pageModel.ErrorCode;

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(errorCode, Is.EqualTo(expectedErrorCode));
        }

        #endregion ErrorCode

        #region ExceptionMessage
        [Test]
        public void ExceptionMessage_ErrorCode404_Should_Store_and_Return_Null()
        {
            // Arrange
            int ErrorCode = 404;

            // Act
            pageModel.OnGet(ErrorCode);
            var exceptionMessage = pageModel.ExceptionMessage;

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(exceptionMessage, Is.EqualTo(null));
        }

        [Test]
        public void OnGet_Exception_Should_Set_ExceptionMessage_And_StackTrace()
        {
            // Arrange
            var logger = new LoggerFactory().CreateLogger<ErrorModel>();
            pageModel = new ErrorModel(logger);

            var exception = new InvalidOperationException("Test exception occurred");
            var httpContext = new DefaultHttpContext();

            // Add an exception feature to the HttpContext
            httpContext.Features.Set<IExceptionHandlerFeature>(new ExceptionHandlerFeature
            {
                Error = exception
            });

            // Assign HttpContext to PageContext
            pageModel.PageContext.HttpContext = httpContext;

            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(pageModel.ExceptionMessage, Is.EqualTo(exception.Message));
            Assert.That(pageModel.StackTrace, Is.EqualTo(exception.StackTrace));
            Assert.That(pageModel.ErrorMessage, Is.Null);
        }

        #endregion ExceptionMessage
    }

}