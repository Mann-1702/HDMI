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
using System.Threading.Tasks;

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

        [Test]
        public void SubmitRating_Valid_ID_Click_1Star_Unstared_Should_Increment_Count_And_Check_Star()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            var buttonId_ViewTeams = "ViewTeamsButton_NFL";
            var buttonId_MoreInfo = "MoreInfoButton_Seattle Seahawks";

            var page = RenderComponent<ProductList>();

            // Find the Buttons (more info)
            var buttonList = page.FindAll("Button");

            // Click on View Teams
            var button = buttonList.First(m => m.OuterHtml.Contains(buttonId_ViewTeams));
            button.Click();

            // Click on More Info (Seahawks)
            buttonList = page.FindAll("Button");
            button = buttonList.First(m => m.OuterHtml.Contains(buttonId_MoreInfo));
            button.Click();

            var buttonMarkup = page.Markup;



            // Get the Star Buttons
            var starButtonList = page.FindAll("span");

            // Get the Vote Count, the List should have 7 elements, element 2 is the string for the count
            var preVoteCountSpan = starButtonList[1];
            var preVoteCountString = preVoteCountSpan.OuterHtml;

            // Get the First star item from the list, it should not be checked
            var starButton = starButtonList.First(m => !string.IsNullOrEmpty(m.ClassName) && m.ClassName.Contains("fa fa-star"));

            // Save the html for it to compare after the click
            var preStarChange = starButton.OuterHtml;



            // Act
            starButton.Click();

            // Get the markup to use for the assert
            buttonMarkup = page.Markup;

            // Get the Star Buttons
            starButtonList = page.FindAll("span");

            // Get the Vote Count, the List should have 7 elements, element 2 is the string for the count
            var postVoteCountSpan = starButtonList[1];
            var postVoteCountString = postVoteCountSpan.OuterHtml;

            // Get the Last stared item from the list
            starButton = starButtonList.First(m => !string.IsNullOrEmpty(m.ClassName) && m.ClassName.Contains("fa fa-star"));

            // Save the html for it to compare after the click
            var postStarChange = starButton.OuterHtml;

            // Assert

            // Confirm the record had no votes to start, and 1 vote after:
            Assert.That(preVoteCountString.Contains("Be the first to vote!"), Is.EqualTo(true));
            Assert.That(postVoteCountString.Contains("1 Vote"), Is.EqualTo(true));
            Assert.That(preVoteCountString, Is.Not.EqualTo(postVoteCountString));
        }

        [Test]
        public void SubmitRating_Valid_ID_Click_5Star_1Star_Unstared_Should_Increment_Count_And_Check_Star()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            var buttonId_ViewTeams = "ViewTeamsButton_NFL";
            var buttonId_MoreInfo = "MoreInfoButton_Seattle Seahawks";

            var page = RenderComponent<ProductList>();

            // Find the Buttons (more info)
            var buttonList = page.FindAll("Button");

            // Click on View Teams
            var button = buttonList.First(m => m.OuterHtml.Contains(buttonId_ViewTeams));
            button.Click();

            // Click on More Info (Seahawks)
            buttonList = page.FindAll("Button");
            button = buttonList.First(m => m.OuterHtml.Contains(buttonId_MoreInfo));
            button.Click();

            var buttonMarkup = page.Markup;



            // Get the Star Buttons
            var starButtonList = page.FindAll("span");

            // Get the Vote Count, the List should have 7 elements, element 2 is the string for the count
            var preVoteCountSpan = starButtonList[1];
            var preVoteCountString = preVoteCountSpan.OuterHtml;

            // Get the First star item from the list, it should not be checked
            var starButton5Star = starButtonList.Where(m => !string.IsNullOrEmpty(m.ClassName) && m.ClassName.Contains("fa fa-star")).ToList()[4];

            // Save the html for it to compare after the click
            var preStarChange = starButton5Star.OuterHtml;

            // Act

            // Vote 5 Star
            starButton5Star.Click();

            // Get the Star Buttons and Vote 1 Star
            starButtonList = page.FindAll("span");
            var starButton1Star = starButtonList.Where(m => !string.IsNullOrEmpty(m.ClassName) && m.ClassName.Contains("fa fa-star")).ToList()[1];

            starButton1Star.Click();


            // Get the markup to use for the assert
            buttonMarkup = page.Markup;

            // Get the Star Buttons
            starButtonList = page.FindAll("span");

            // Get the Vote Count, the List should have 7 elements, element 2 is the string for the count
            var postVoteCountSpan = starButtonList[1];
            var postVoteCountString = postVoteCountSpan.OuterHtml;

            // Get the Last stared item from the list
            var starButton = starButtonList.Last(m => !string.IsNullOrEmpty(m.ClassName) && m.ClassName.Contains("fa fa-star"));

            // Save the html for it to compare after the click
            var postStarChange = starButton.OuterHtml;

            // Assert

            // Confirm the record had no votes to start, and 1 vote after:
            Assert.That(preVoteCountString.Contains("Be the first to vote!"), Is.EqualTo(false));
            Assert.That(preVoteCountString.Contains("1 Vote"), Is.EqualTo(true));
            Assert.That(postVoteCountString.Contains("3 Vote"), Is.EqualTo(true));

            Assert.That(preVoteCountString, Is.Not.EqualTo(postVoteCountString));
        }

        [Test]
        public async Task TopTeamsByTrophies_Should_DisplayLoadingMessage_When_Null()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var page = RenderComponent<ProductList>();

            // Access the component instance
            var componentInstance = page.Instance;

            // Access the private field `topTeamsByTrophies` using reflection
            var fieldInfo = componentInstance.GetType()
                .GetField("topTeamsByTrophies", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Ensure the field was found
            Assert.That(fieldInfo, Is.Not.Null, "The topTeamsByTrophies field was not found.");

            // Use InvokeAsync to set the field value and trigger rendering
            await page.InvokeAsync(() =>
            {
                // Set the value of the field to null
                fieldInfo.SetValue(componentInstance, null);

                // Trigger a re-render manually
                var stateHasChangedMethod = componentInstance.GetType()
                    .GetMethod("StateHasChanged", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                // Ensure the StateHasChanged method was found
                Assert.That(stateHasChangedMethod, Is.Not.Null, "The StateHasChanged method was not found.");

                // Invoke StateHasChanged to re-render
                stateHasChangedMethod.Invoke(componentInstance, null);
            });

            // Act
            var pageMarkup = page.Markup;

            // Assert
            Assert.That(pageMarkup.Contains("Loading top teams by trophies..."), Is.EqualTo(true));
        }



    }

}