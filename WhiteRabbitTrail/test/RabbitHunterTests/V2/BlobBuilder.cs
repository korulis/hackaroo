using System.Collections.Generic;
using RabbitHunter.V2;

namespace RabbitHunterTests.V2
{
    public class BlobBuilder
    {
        public static Blob BuildSingletonBlob(string charPool)
        {
            return new Blob(charPool, new List<string> { charPool });
        }
    }
}