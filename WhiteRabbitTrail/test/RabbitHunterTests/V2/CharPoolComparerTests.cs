using RabbitHunter;
using Xunit;

namespace RabbitHunterTests.V2
{
    public class CharPoolComparerTests
    {
        [Theory]
        [InlineData("", "", 0)]
        [InlineData(null, null, 0)]
        [InlineData("a", "a", 0)]
        [InlineData("a", "b", -1)]
        [InlineData("b", "a", 1)]
        [InlineData("ab", "aa", 1)]
        [InlineData(null, "", -1)]
        [InlineData("", "a", -1)]
        [InlineData("ab", "aaa", -1)]
        [InlineData("aaa", "ab", 1)]
        public void Compares(string x, string y, int expected)
        {
            var sut = new CharPoolComparer();

            var actual = sut.Compare(x, y);
            Assert.Equal(expected, actual);
        }
    }
}
