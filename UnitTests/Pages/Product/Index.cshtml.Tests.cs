using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Services;

namespace UnitTests.Pages.Product
{
    public class IndexTests
    {
        #region TestSetup
        public static IUrlHelperFactory urlHelperFactory;
        public static DefaultHttpContext httpContextDefault;
        public static IWebHostEnvironment webHostEnvironment;
        public static ModelStateDictionary modelState;
        public static ActionContext actionContext;
        public static EmptyModelMetadataProvider modelMetadataProvider;
        public static ViewDataDictionary viewData;
        public static TempDataDictionary tempData;
        public static PageContext pageContext;
        public static IndexModel pageModel;
        JsonFileProductService productService;


        [SetUp]
        public void TestInitialize()
        {
            httpContextDefault = new DefaultHttpContext()
            {

                //RequestServices = serviceProviderMock.Object,
            };

            modelState = new ModelStateDictionary();

            actionContext = new ActionContext(httpContextDefault, httpContextDefault.GetRouteData(), new PageActionDescriptor(), modelState);

            modelMetadataProvider = new EmptyModelMetadataProvider();
            viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            tempData = new TempDataDictionary(httpContextDefault, Mock.Of<ITempDataProvider>());

            pageContext = new PageContext(actionContext)
            {
                ViewData = viewData,
            };

            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/bin/Debug/net7.0/wwwroot");
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("./data/");

            var MockLoggerDirect = Mock.Of<ILogger<IndexModel>>();

            productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            pageModel = new IndexModel(productService)
            {
            };

        }

        #endregion TestSetup


        #region OnGet
        [Test]
        public void OnGet_Valid_Should_Return_Products()
        {

            // Arrange

            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(pageModel.Products.ToList().Count, Is.EqualTo(21));
        }


        [Test]
        public void OnGet_Search_Valid_SearchTerm_Should_Return_Filtered_Page()
        {

            // Arrange
            IndexModel validSearchPageModel = new IndexModel(productService)
            {
                SearchTerm = "Seahawks"
            };

            // Act
            validSearchPageModel.OnGet();

            // Assert
            Assert.That(validSearchPageModel.Products.Count, Is.EqualTo(1));
        }
        #endregion OnGet
    }

}