using System.Collections.Generic;
using System.IO;
using System.Linq;
using RabbitHunter;
using Xunit;

namespace RabbitHunterTests
{
    public class BenchmarkTests
    {

        [Theory]
        [InlineData("underworld al", "1a2220030900a307be8b72b571986f50", "underworld al")]
        [InlineData("underworld repeated al", "d0c88c88d585e1d3b756909a74d1850e", "underworld repeated al")]
        public void Tests(string anagram, string hash, string expectedPhrase)
        {
            var ecrypter = new Md5Encrypter();
            var words = File.ReadLines("fractionOfWords.txt").ToList();
            var sut = new Decrypter(words, ecrypter);

            var actualPhrase = sut.GetDecryptedPhrase(hash, anagram);

            Assert.Equal(expectedPhrase, actualPhrase);
        }

        [Fact(Skip = "For manual generation only")]
        public void TheTenth()
        {
            var words = File.ReadLines("wordlist.txt").ToList();
            var tenthOfWords = new List<string>();

            var i = 0;
            foreach (var word in words)
            {
                i++;
                if (i == 10)
                {
                    i = 0;
                    tenthOfWords.Add(word);
                }
            }

            File.WriteAllLines("fractionOfWords2.txt", tenthOfWords);

        }

    }
}