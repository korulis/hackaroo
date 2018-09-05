using RabbitHunter;
using Xunit;

namespace RabbitHunterTests
{
    public class HelperTests
    {

        [Theory]
        [InlineData("a", "a", "")]
        [InlineData("aa", "aa", "")]
        [InlineData("aa", "", "aa")]
        [InlineData("aa", "ab", null)]
        [InlineData("ab", "aa", null)]
        [InlineData("aa", "a", "a")]
        [InlineData("dcba", "bc", "ad")] // subtract from unsorted
        [InlineData("abcd", "cb", "ad")] // subtract unsorted
        [InlineData("dcba", "cb", "ad")] // subtract unsorted from unsorted
        [InlineData("dcba ", "cb", "ad")] // can subtract from sentence
        [InlineData("dcba", "c b", null)] // can not subtract sentence
        [InlineData("d cba", "c b", null)]
        public void Subtract(string initial, string input, string expected)
        {
            var actual = initial.SubtractChars(input);

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
            var actual = SortedStringExtensions.Alphabetize(input);

            Assert.Equal(expected, actual);
        }

    }
}