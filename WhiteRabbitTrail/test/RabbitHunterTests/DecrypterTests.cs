using System.Collections.Generic;
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
                "mail",
                "wail",
                "was",
            };

        public DecrypterTests()
        {
            _sut = new Decrypter();
        }

        [Theory]
        [InlineData("hail was", "4baa9bd4e256f2040183789f31f486a1", "has mail")]
        //[InlineData("hail was cool mail", "cc7855c00fed581afd264fe2458884d1", "wail has cool mail")]
        //[InlineData("hail was cool mail", "fc99cc261fa078ac422a8214e0242ae5", "mail was cool hail")]
        public void Decrypt_ReturnsCorrectPhrases(string anagram, string hash, string expectedPhrase)
        {
            var actualPhrase = _sut.GetDecryptedPhrase(hash, anagram);

            Assert.Equal(expectedPhrase, actualPhrase);
        }

    }
}
