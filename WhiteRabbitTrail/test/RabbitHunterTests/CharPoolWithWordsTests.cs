using System.Collections.Generic;
using Cp = RabbitHunter.CharPoolWithWords;
using Xunit;

namespace RabbitHunterTests
{
    public class CharPoolWithWordsTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void Tests(List<string> wordList, List<Cp> expected)
        {
            var actual = Cp.GetCharPools(wordList);

            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < actual.Count; i++)
            {
                Assert.Equal(expected[i].Value, actual[i].Value);
                Assert.Equal(expected[i].Words.Count, actual[i].Words.Count);
                for (int j = 0; j < actual[i].Words.Count; j++)
                {
                    Assert.Equal(expected[i].Words[j], actual[i].Words[j]);
                }
            }
        }

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] {
                    new List<string> {"a"},
                    new List<Cp> {new Cp("a",new List<string> {"a"})}, },

                new object[] {
                    new List<string> {"ba"},
                    new List<Cp> {new Cp("ab",new List<string> {"ba"})}, },

                new object[] {
                    new List<string> {"a", "ba"},
                    new List<Cp>
                    {
                        new Cp("a",new List<string> {"a"}),
                        new Cp("ab",new List<string> {"ba"})
                    }, },

                new object[] {
                    new List<string> {"ab", "ba"},
                    new List<Cp>
                    {
                        new Cp("ab",new List<string> {"ab","ba"})
                    }, },

                new object[] {
                    new List<string> {"ab", "ba", "bca","cab"},
                    new List<Cp>
                    {
                        new Cp("ab",new List<string> {"ab","ba"}),
                        new Cp("abc",new List<string> {"bca","cab"})
                    }, },

            };
    }
}