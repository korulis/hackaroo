using System.Collections.Generic;
using System.Linq;
using RabbitHunter;
using RabbitHunter.V1;
using Xunit;

namespace RabbitHunterTests
{
    public class CharPoolWithWordsTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void Tests(List<string> wordList, List<WordEquivalencyClass> expected)
        {
            var actual = WordEquivalencyClass.GetDictionary(wordList).Select(x => new WordEquivalencyClass(x.Key, x.Value)).ToList();

            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < actual.Count; i++)
            {
                Assert.Equal(expected[i].CharPool, actual[i].CharPool);
                Assert.Equal(expected[i].Words.Count, actual[i].Words.Count);
                var expectedWords = expected[i].Words.ToArray();
                var actualWords = actual[i].Words.ToArray();
                for (int j = 0; j < actual[i].Words.Count; j++)
                {
                    Assert.Equal(expectedWords[j], actualWords[j]);
                }
            }
        }

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] {
                    new List<string> {"a"},
                    new List<WordEquivalencyClass> {new WordEquivalencyClass("a",new List<string> {"a"})}, },

                new object[] {
                    new List<string> {"ba"},
                    new List<WordEquivalencyClass> {new WordEquivalencyClass("ab",new List<string> {"ba"})}, },

                new object[] {
                    new List<string> {"a", "ba"},
                    new List<WordEquivalencyClass>
                    {
                        new WordEquivalencyClass("a",new List<string> {"a"}),
                        new WordEquivalencyClass("ab",new List<string> {"ba"})
                    }, },

                new object[] {
                    new List<string> {"ab", "ba"},
                    new List<WordEquivalencyClass>
                    {
                        new WordEquivalencyClass("ab",new List<string> {"ab","ba"})
                    }, },

                new object[] {
                    new List<string> {"ab", "ba", "bca","cab"},
                    new List<WordEquivalencyClass>
                    {
                        new WordEquivalencyClass("ab",new List<string> {"ab","ba"}),
                        new WordEquivalencyClass("abc",new List<string> {"bca","cab"})
                    }, },

            };
    }
}