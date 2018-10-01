using System.IO;
using System.Linq;
using RabbitHunter;
using Xunit;
using Xunit.Sdk;

namespace RabbitHunterTests
{
    public class DecrypterAcceptanceTests
    {
        private class DummyEncrypter : Encrypter
        {
            public string Hash(string phrase)
            {
                throw new System.NotImplementedException();
            }
        }

        private readonly Decrypter _sut;
        private const string Anagram = "poultry outwits ants";

        public DecrypterAcceptanceTests()
        {
            // todo replate with real implementation later.
            var ecrypter = new DummyEncrypter();

            var words = File.ReadLines("wordlist.txt").ToList();

            _sut = new Decrypter(words, ecrypter);
        }


        [Theory/*(Skip = "not ready")*/]
        [InlineData("e4820b45d2277f3844eac66c903e84be", "printout stout yawls")]
        [InlineData("23170acc097c24edb98fc5488ab033fe", "ty outlaws printouts")]
        public void Decrypt_ReturnsCorrectPhrases_ForRabbitHunt(string hash, string expectedPhrase)
        {
            var actualPhrase = _sut.GetDecryptedPhrase(hash, Anagram);

            Assert.Equal(expectedPhrase, actualPhrase);
        }
    }
}