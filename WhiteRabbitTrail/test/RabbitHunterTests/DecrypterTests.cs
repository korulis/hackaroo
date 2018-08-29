using RabbitHunter;
using Xunit;

namespace RabbitHunterTests
{

    public class DecrypterTests
    {
        private Decrypter _sut;

        public DecrypterTests()
        {
            _sut = new Decrypter();
        }


        [Theory]
        [InlineData("e4820b45d2277f3844eac66c903e84be", "printout stout yawls")]
        [InlineData("23170acc097c24edb98fc5488ab033fe", "ty outlaws printouts")]
        public void Decrypt_ReturnsCorrectAnagrams(string hash, string expectedAnagram)
        {
            var actualAnagram = _sut.DecryptAnagram(hash);

            Assert.Equal(expectedAnagram, actualAnagram);
        }
    }
}
