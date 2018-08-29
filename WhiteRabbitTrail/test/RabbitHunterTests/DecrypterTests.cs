using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RabbitHunterTests
{

    public class DecrypterTests
    {
        public DecrypterTests()
        {
            
        }


        [Theory]
        [InlineData("e4820b45d2277f3844eac66c903e84be", "printout stout yawls")]
        [InlineData("23170acc097c24edb98fc5488ab033fe", "ty outlaws printouts")]
        public void Decrypt_ReturnsCorrectAnagrams(string hash, string expectedAnagram)
        {

        }
    }
}
