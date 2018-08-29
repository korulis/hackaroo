using RabbitHunter;
using Xunit;

namespace RabbitHunterTests
{
    public class DecrypterAcceptanceTests
    {
        private readonly Decrypter _sut;
        private const string Anagram = "poultry outwits ants";

        public DecrypterAcceptanceTests()
        {
            _sut = new Decrypter();
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