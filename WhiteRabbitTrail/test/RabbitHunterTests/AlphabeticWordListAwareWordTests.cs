using System.Collections.Generic;
using Aw = RabbitHunter.AlphabeticListAwareWords;
using Xunit;

namespace RabbitHunterTests
{
    public class AlphabeticWordListAwareWordTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void Tests(List<string> wordList, List<Aw> expected)
        {
            var actual = Aw.GetAlphabeticListAwareWords(wordList);

            Assert.Equal(actual.Count, expected.Count);
            for (int i = 0; i < actual.Count; i++)
            {
                Assert.Equal(expected[i].Value, actual[i].Value);
                Assert.Equal(expected[i].WordsAhead, actual[i].WordsAhead);
            }
        }

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] {
                    new List<string> {"a"},
                    new List<Aw> {new Aw("a",0)}, },

                new object[] {
                    new List<string> {"a","b"},
                    new List<Aw> {new Aw("a",0), new Aw("b",0)}},

                new object[] {
                    new List<string> {"a","ab"},
                    new List<Aw> {new Aw("a",1), new Aw("ab",0)}},

                new object[] {
                    new List<string> {"a", "ab", "ba"},
                    new List<Aw> {new Aw("a",2), new Aw("ab",1), new Aw("ba",0)}},

                new object[] {
                    new List<string> {"a", "ab", "ac"},
                    new List<Aw> {new Aw("a",2), new Aw("ab",0), new Aw("ac",0)}},

                new object[] {
                    new List<string> {"a", "ab", "bc"},
                    new List<Aw> {new Aw("a",1), new Aw("ab",0), new Aw("bc",0)}},
            };
    }
}