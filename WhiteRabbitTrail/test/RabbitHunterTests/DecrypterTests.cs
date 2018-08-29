﻿using System.Collections.Generic;
using RabbitHunter;
using Xunit;

namespace RabbitHunterTests
{
    public class DecrypterTests
    {
        private readonly Decrypter _sut;
        private readonly List<string> _alphabeticWordPool = new List<string>
            {
                "cool",
                "hail",
                "has",
                "lima",
                "liam",
                "mail",
                "mails",
                "mass",
                "wail",
                "was",
            };

        public DecrypterTests()
        {
            _sut = new Decrypter();
        }

        [Theory]
        [InlineData("mail", "6742923575546471370cc028f289db40", "lima")]
        [InlineData("mail", "6d8c4d103f90154cc06dd75a71d061be", "liam")]
        public void Decrypt_ReturnsCorrectPhrases_SingleWord(string anagram, string hash, string expectedPhrase)
        {
            var actualPhrase = _sut.GetDecryptedPhrase(hash, anagram);

            Assert.Equal(expectedPhrase, actualPhrase);
        }

        [Theory]
        [InlineData("hail was", "4baa9bd4e256f2040183789f31f486a1", "has mail")]
        [InlineData("hail mass", "b3df493553df242256dac2d37511ec64", "has mails")]
        public void Decrypt_ReturnsCorrectPhrases_TwoWords(string anagram, string hash, string expectedPhrase)
        {
            var actualPhrase = _sut.GetDecryptedPhrase(hash, anagram);

            Assert.Equal(expectedPhrase, actualPhrase);
        }

        [Theory]
        [InlineData("hail was cool mail", "cc7855c00fed581afd264fe2458884d1", "wail has cool mail")]
        [InlineData("hail was cool mail", "fc99cc261fa078ac422a8214e0242ae5", "mail was cool hail")]
        public void Decrypt_ReturnsCorrectPhrases_ThreeWords(string anagram, string hash, string expectedPhrase)
        {
            var actualPhrase = _sut.GetDecryptedPhrase(hash, anagram);

            Assert.Equal(expectedPhrase, actualPhrase);
        }

    }
}
