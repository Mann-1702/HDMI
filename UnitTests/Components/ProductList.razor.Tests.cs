using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using Bunit;
using NUnit.Framework;

using ContosoCrafts.WebSite.Components;
using ContosoCrafts.WebSite.Services;
using System;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using ContosoCrafts.WebSite.Models;

namespace UnitTests.Components
{
    public class ProductListTests : BunitTestContext
    {
        FakeNavigationManager navManager;
        #region TestSetup

        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup

        [Test]
        public void ProductList_Valid_Default_Should_Return_Content()
        {

            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            // Get the Cards retrned
            var result = page.Markup;

            // Assert
            Assert.That(result.Contains("NFL"), Is.EqualTo(true));
        }

        [Test]
        public void PictureButton_Valid_ID_NFL_Should_Return_Content()
        {

            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var buttonId = "PictureButton_NFL";
            var page = RenderComponent<ProductList>();

            // Find the Buttons (more info)
            var buttonList = page.FindAll("Button");
            var button = buttonList.First(m => m.OuterHtml.Contains(buttonId));

            // Act
            button.Click();

            // Get the Cards retrned
            var pageMarkup = page.Markup;

            // Assert
            Assert.That(pageMarkup.Contains("Seahawks"), Is.EqualTo(true));
            Assert.That(pageMarkup.Contains("Chelsea"), Is.EqualTo(false));
        }

        [Test]
        public void MoreInfoButton_Valid_ID_NFL_Should_Return_Content()
        {

            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var buttonId = "MoreInfoButton_NFL";
            var page = RenderComponent<ProductList>();

            // Find the Buttons (more info)
            var buttonList = page.FindAll("Button");
            var button = buttonList.First(m => m.OuterHtml.Contains(buttonId));

            // Act
            button.Click();

            // Get the Cards retrned
            var pageMarkup = page.Markup;

            // Assert
            Assert.That(pageMarkup.Contains("American football is a popular sport primarily played in the United States."), Is.EqualTo(true));
            Assert.That(pageMarkup.Contains("Chelsea"), Is.EqualTo(false));
        }

        [Test]
        public void ViewTeamsButton_Valid_ID_NFL_Should_Return_Content()
        {

            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var buttonId = "ViewTeamsButton_NFL";
            var page = RenderComponent<ProductList>();

            // Find the Buttons (more info)
            var buttonList = page.FindAll("Button");
            var button = buttonList.First(m => m.OuterHtml.Contains(buttonId));

            // Act
            button.Click();

            // Get the Cards retrned
            var pageMarkup = page.Markup;

            // Assert
            Assert.That(pageMarkup.Contains("Seahawks"), Is.EqualTo(true));
            Assert.That(pageMarkup.Contains("Chelsea"), Is.EqualTo(false));
        }

        [Test]
        public void ReadMoreButton_Valid_ID_SeattleSeahawks_Should_Redirect_To_Read_Page()
        {

            // Arrange

            // Getting Services
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var navigationManager = Services.GetService<NavigationManager>();


            // Setting up button id and page
            var buttonId_ViewTeams = "ViewTeamsButton_NFL";
            var buttonId_ReadMore = "ReadMoreButton_Seattle Seahawks";
            var page = RenderComponent<ProductList>();

            // View Teams Button
            var buttonList = page.FindAll("Button");
            var button = buttonList.First(m => m.OuterHtml.Contains(buttonId_ViewTeams));
            button.Click();

            // Assert View Teams button worked
            var pageMarkup = page.Markup;
            Assert.That(pageMarkup.Contains("Seahawks"), Is.EqualTo(true));

            // Find Read More Button
            buttonList = page.FindAll("Button");
            button = buttonList.First(m => m.OuterHtml.Contains(buttonId_ReadMore));

            // Act
            button.Click();

            // Assert
            Assert.That(navigationManager.Uri.Contains("Product/Read/Seattle Seahawks"));
        }

        [Test]
        public void ShowSportsButton_Should_Show_All_Sports_Cards()
        {

            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var buttonId = "ShowSportsButton";
            var page = RenderComponent<ProductList>();

            // Find the Buttons (more info)
            var buttonList = page.FindAll("Button");
            var button = buttonList.First(m => m.OuterHtml.Contains(buttonId));

            // Act
            button.Click();

            // Get the Cards retrned
            var pageMarkup = page.Markup;

            // Assert
            Assert.That(pageMarkup.Contains("NFL"), Is.EqualTo(true));
            Assert.That(pageMarkup.Contains("NBA"), Is.EqualTo(true));
            Assert.That(pageMarkup.Contains("Soccer"), Is.EqualTo(true));
        }

        [Test]
        public void ReadMoreTopTeamButton_Valid_ID_SanFrancisco49ers_Should_Redirect_To_Read_Page()
        {

            // Arrange

            // Getting Services
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var navigationManager = Services.GetService<NavigationManager>();


            // Setting up button id and page
            var buttonId = "ReadMoreTopTeamButton_San Francisco 49ers";
            var page = RenderComponent<ProductList>();


            // Find Read More Button
            var buttonList = page.FindAll("Button");
            var button = buttonList.First(m => m.OuterHtml.Contains(buttonId));


            // Act
            button.Click();

            // Assert
            Assert.That(navigationManager.Uri.Contains("Product/Read/San Francisco 49ers"));
        }

    }

}