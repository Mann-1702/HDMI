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

namespace UnitTests.Pages.Product.Read

{
    public class ReadTests
    {s

        private ReadModel pageModel;
        private Mock<JsonFileProductService> mockProductService;

        [SetUp]
        public void TestInitialize()
        {
        
            mockProductService = new Mock<JsonFileProductService>();
            pageModel = new ReadModel(mockProductService.Object);
        }
    }
}
