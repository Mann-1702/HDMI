using ContosoCrafts.WebSite.Models;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json;

namespace UnitTests.Models
{
    [TestFixture]
    public class MatchModelTests
    {
        private MatchModel _matchModel;

        [SetUp]
        public void Setup()
        {
            _matchModel = new MatchModel
            {
                Match = "Championship Game",
                Date = new DateTime(2024, 12, 25),
                Location = "City Stadium",
                Team1 = "Team A",
                Team2 = "Team B",
                Team1_Score = 3,
                Team2_Score = 2
            };
        }

        [Test]
        public void MatchModel_AllProperties_ShouldBeValid()
        {
            // Arrange - no changes needed as Setup initializes properties

            // Act
            var validationResults = ValidateModel(_matchModel);

            // Assert - ensure there are no validation errors
            Assert.That(validationResults, Is.Empty);
        }

        [Test]
        public void MatchModel_MissingRequiredFields_ShouldReturnValidationErrors()
        {
            // Arrange
            _matchModel.Match = null; // Required field missing
            _matchModel.Location = null; // Required field missing

            // Act
            var validationResults = ValidateModel(_matchModel);

            // Assert
            Assert.That(validationResults, Is.Not.Empty);
            Assert.That(validationResults.Count, Is.EqualTo(2)); // Two validation errors expected
            Assert.That(validationResults[0].ErrorMessage, Does.Contain("The Match field is required"));
            Assert.That(validationResults[1].ErrorMessage, Does.Contain("The Location field is required"));
        }

        [Test]
        public void MatchModel_LocationLengthOutOfRange_ShouldReturnValidationError()
        {
            // Arrange - Location length less than minimum
            _matchModel.Location = "AB";

            // Act
            var validationResults = ValidateModel(_matchModel);

            // Assert
            Assert.That(validationResults, Is.Not.Empty);
            Assert.That(validationResults[0].ErrorMessage, Does.Contain("The Location should have a length of more than 3 and less than 100"));
        }

        [Test]
        public void MatchModel_ToString_ShouldReturnValidJson()
        {
            // Act
            var json = _matchModel.ToString();

            // Assert
            Assert.That(json, Is.Not.Null);
            var deserialized = JsonSerializer.Deserialize<MatchModel>(json);
            Assert.That(deserialized.Match, Is.EqualTo(_matchModel.Match));
            Assert.That(deserialized.Date, Is.EqualTo(_matchModel.Date));
            Assert.That(deserialized.Location, Is.EqualTo(_matchModel.Location));
            Assert.That(deserialized.Team1, Is.EqualTo(_matchModel.Team1));
            Assert.That(deserialized.Team2, Is.EqualTo(_matchModel.Team2));
            Assert.That(deserialized.Team1_Score, Is.EqualTo(_matchModel.Team1_Score));
            Assert.That(deserialized.Team2_Score, Is.EqualTo(_matchModel.Team2_Score));
        }

        // Helper validate model
        private static IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}