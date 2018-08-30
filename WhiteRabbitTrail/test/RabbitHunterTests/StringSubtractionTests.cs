using RabbitHunter;
using Xunit;

namespace RabbitHunterTests
{
    public class StringSubtractionTests
    {

        [Theory]
        [InlineData("a", "a", "")]
        [InlineData("aa", "aa", "")]
        [InlineData("aa", "ab", null)]
        [InlineData("ab", "aa", null)]
        [InlineData("aa", "a", "a")]
        public void Subtract(string initial, string input, string expected)
        {
            var actual = initial.Subtract(input);

            Assert.Equal(expected, actual);
        }

    }
}