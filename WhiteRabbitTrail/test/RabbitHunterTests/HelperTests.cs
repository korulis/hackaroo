using RabbitHunter;
using Xunit;

namespace RabbitHunterTests
{
    public class HelperTests
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

        [Theory]
        [InlineData("a", "a")]
        [InlineData("a ", "a")]
        [InlineData("ab", "ab")]
        [InlineData("a b", "ab")]
        [InlineData("ab a","aab")]
        public void Alphabetize(string input, string expected)
        {
            var actual = Decrypter.Alphabetize(input);

            Assert.Equal(expected, actual);
        }

    }
}