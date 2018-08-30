using System.Collections.Generic;
using RabbitHunter;
using Xunit;

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
            _sut = new Decrypter(new List<string> {"fix later"}, ecrypter);
        }

        [Theory]
        [InlineData("e4820b45d2277f3844eac66c903e84be", "printout stout yawls")]
        [InlineData("23170acc097c24edb98fc5488ab033fe", "ty outlaws printouts")]
        public void Decrypt_ReturnsCorrectPhrases_ForRabbitHunt(string hash, string expectedPhrase)
        {
            var actualPhrase = _sut.GetDecryptedPhrase(hash, Anagram);

            Assert.Equal(expectedPhrase, actualPhrase);
        }
    }
}