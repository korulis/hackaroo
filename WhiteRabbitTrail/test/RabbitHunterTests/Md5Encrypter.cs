using System.Security.Cryptography;
using System.Text;

namespace RabbitHunterTests
{
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
}