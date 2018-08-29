using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
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
        public void Decrypt_ReturnsCorrectPhrases_ForRabbitHunt(string hash, string expectedPhrase)
        {
            var actualPhrase = _sut.GetDecryptedPhrase(hash);

            Assert.Equal(expectedPhrase, actualPhrase);
        }

        [Theory]
        [InlineData("hail was cool mail", "cc7855c00fed581afd264fe2458884d1", "wail has cool mail")]
        [InlineData("hail was cool mail", "fc99cc261fa078ac422a8214e0242ae5", "mail was cool hail")]
        public void Decrypt_ReturnsCorrectPhrases(string anagram, string hash, string expectedPhrase)
        {
            //var testHash = new Md5Encrypter().Hash(expectedPhrase);

            var alphabeticWordPool = new List<string>
            {
                "cool",
                "hail",
                "has",
                "mail",
                "wail",
                "was",
            };

            var sut = new Decrypter();

            var actualPhrase = sut.GetDecryptedPhrase(hash);

            Assert.Equal(expectedPhrase, actualPhrase);
        }


    }

    public class Md5Encrypter : Encrypter
    {
        public string Hash(string phrase)
        {
            byte[] hashBytes;
            using (var md5 = MD5.Create())
            {
                hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(phrase));
            }

            var sb = new StringBuilder();
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            var hash = sb.ToString();
            return hash;
        }
    }

    public interface Encrypter
    {
        string Hash(string phrase);
    }
}
