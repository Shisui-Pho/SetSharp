using SetsLibrary.Utility;
using Xunit;
namespace Tests.Utilities
{
    public class BraveEvaluatorTests
    {
        // Individual tests for clarity
        [Fact]
        public void Test_ValidBraces_ReturnsTrue()
        {
            // Arrange
            string expression = "{1, 2, {3, 4}}";

            // Act
            bool result = BraceEvaluator.AreBracesCorrect(expression, out _);

            // Assert
            Assert.True(result);
        }//Test_ValidBraces_ReturnsTrue

        [Fact]
        public void Test_InvalidOpeningBrace_ReturnsFalse()
        {
            // Arrange
            string expression = "1, 2, {3, 4}";

            // Act
            bool result = BraceEvaluator.AreBracesCorrect(expression, out _);

            // Assert
            Assert.False(result);
        }//Test_InvalidOpeningBrace_ReturnsFalse

        [Fact]
        public void Test_InvalidClosingBrace_ReturnsFalse()
        {
            // Arrange
            string expression = "{1, 2, 3, 4}}";

            // Act
            bool result = BraceEvaluator.AreBracesCorrect(expression, out _);

            // Assert
            Assert.False(result);
        }//Test_InvalidClosingBrace_ReturnsFalse

        [Fact]
        public void Test_UnmatchedBraces_ReturnsFalse()
        {
            // Arrange
            string expression = "{1, 2, {3, 4}";

            // Act
            bool result = BraceEvaluator.AreBracesCorrect(expression, out _);

            // Assert
            Assert.False(result);
        }//Test_UnmatchedBraces_ReturnsFalse

        // Parameterized tests using Theory
        [Theory]
        [InlineData("{{1, 2}, {3, 4}}", true)]           // Nested valid braces
        [InlineData("{}}", false)]                        // Extra closing brace
        [InlineData("{ { } }", true)]                     // Nested empty braces
        [InlineData("{}", true)]                          // Single pair of braces
        [InlineData("{a, b, {c, d}}", true)]             // Valid with variables
        [InlineData("{a, b, {c, d}}}", false)]           // Extra closing brace at the end
        [InlineData("{a, b, c, d", false)]               // Unmatched opening brace with content
        public void Test_BraceEvaluation(string expression, bool expected)
        {
            // Act
            bool result = BraceEvaluator.AreBracesCorrect(expression, out _);

            // Assert
            Assert.Equal(expected, result);
        }//Test_BraceEvaluation
        [Fact]
        public void Test_LongValidBraces_ReturnsTrue()
        {
            // Arrange
            string expression = "{1, 2, {3, 4, {5, 6, {7, 8}, 9}, 10}, 11}";

            // Act
            bool result = BraceEvaluator.AreBracesCorrect(expression, out _);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Test_LongInvalidBraces_ReturnsFalse()
        {
            // Arrange
            string expression = "{1, 2, {3, 4, {5, 6, {7, 8}, 9}, 10}, 11"; // Missing closing brace

            // Act
            bool result = BraceEvaluator.AreBracesCorrect(expression, out _);

            // Assert
            Assert.False(result);
        }//Test_LongInvalidBraces_ReturnsFalse

        [Theory]
        [InlineData("{1, 2, {3, {4, {5, {6, 7}}}}}", true)] // Valid: 6 levels
        [InlineData("{1, 2, {3, 4, {5, 6}}, 7}", true)] // Valid: 4 levels
        [InlineData("{1, {2, {3, {4, {5, 6}}}}, 7}", true)] // Valid: 5 levels
        [InlineData("{1, 2, {3, 4, {5, 6, {7, 8}}}}}", false)] // Valid: 6 levels
        [InlineData("{1, 2, {3, 4, {5, 6}, 7, 8}", false)] // Invalid: Missing closing brace
        [InlineData("{1, 2, {3, 4, {5, 6}, 7}, 8}}", false)] // Invalid: Extra closing brace
        [InlineData("{1, {2, {3, {4, 5}, 6}, 7, {8}}}", true)] // Invalid: Missing closing brace
        public void Test_LongBraceTheory(string expression, bool expected)
        {
            // Act
            bool result = BraceEvaluator.AreBracesCorrect(expression, out _ );

            // Assert
            Assert.Equal(expected, result);
        }//Test_LongBraceTheory
    }//class
}//namespace