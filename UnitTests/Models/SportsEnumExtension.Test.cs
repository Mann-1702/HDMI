using NUnit.Framework;
using ContosoCrafts.WebSite.Models;

namespace UnitTests.Models
{
    public class SportsEnumTests
    {
        #region ToDisplayString Tests

        [Test]
        public void ToDisplayString_NFL_Should_Return_NFL()
        {
            // Arrange
            var sport = SportsEnum.NFL;

            // Act
            var result = sport.ToDisplayString();

            // Assert
            Assert.That(result, Is.EqualTo("NFL"));
        }

        [Test]
        public void ToDisplayString_NBA_Should_Return_NBA()
        {
            // Arrange
            var sport = SportsEnum.NBA;

            // Act
            var result = sport.ToDisplayString();

            // Assert
            Assert.That(result, Is.EqualTo("NBA"));
        }

        [Test]
        public void ToDisplayString_Soccer_Should_Return_Soccer()
        {
            // Arrange
            var sport = SportsEnum.Soccer;

            // Act
            var result = sport.ToDisplayString();

            // Assert
            Assert.That(result, Is.EqualTo("Soccer"));
        }

        [Test]
        public void ToDisplayString_Undefined_Should_Return_Undefined()
        {
            // Arrange
            var sport = SportsEnum.Undefined;

            // Act
            var result = sport.ToDisplayString();

            // Assert
            Assert.That(result, Is.EqualTo("Undefined"));
        }

        [Test]
        public void ToDisplayString_Invalid_Value_Should_Return_Undefined()
        {
            // Arrange
            var sport = (SportsEnum)999; // Invalid enum value

            // Act
            var result = sport.ToDisplayString();

            // Assert
            Assert.That(result, Is.EqualTo("Undefined"));
        }

        #endregion ToDisplayString Tests
    }
}
