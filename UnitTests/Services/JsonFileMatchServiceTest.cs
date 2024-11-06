using System.Linq;

using Microsoft.AspNetCore.Mvc;

using NUnit.Framework;

using ContosoCrafts.WebSite.Models;
using System;
using System.Threading;



namespace UnitTests.Services
{
    public class JsonFileMatchServiceTests
    {
        #region TestSetup

        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup

        #region GetAllData
        [Test]
        public void GetAllData_Should_Return_48_Matches()
        {
            // Arrange

            // Act
            var result = TestHelper.MatchService.GetAllData();
            int count = result.Count();

            // Assert
            Assert.That(count, Is.EqualTo(48));
        }
        #endregion GetAllData




    }
}